using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;

namespace libRSSreader
{
    public class clsRSS
    {
        libCommon.clsDB objDB = new libCommon.clsDB();
        libCommon.clsUtil objUtil = new libCommon.clsUtil();
        clsCmnDB objCmnDB = new clsCmnDB();

        public XmlReader reader;

        /// <summary>
        /// 사용자의 모든 피드 리스트
        /// </summary>
        public clsFeed showAllFeed(string user_id)
        {
            System.Data.DataSet RSSsite_DS = new System.Data.DataSet();
            clsFeed objFeed = new clsFeed();
            clsFeed objFeed_tmp = new clsFeed();
            string URL;
            string RSS_idx;
            int i;
            int j;

            RSSsite_DS = selectRSSsite(user_id);

            if (objCmnDB.validateDS(RSSsite_DS))
            {
                for (i = 0; i < RSSsite_DS.Tables[0].Rows.Count; i++)
                {
                    RSS_idx = RSSsite_DS.Tables[0].Rows[i][0].ToString();
                    URL = RSSsite_DS.Tables[0].Rows[i][2].ToString();
                    objFeed_tmp = getRSSfeed(RSS_idx, URL, user_id);
                    for (j = 0; j < objFeed_tmp.Count; j++)
                    {
                        objFeed.addFeed(objFeed_tmp[j]);
                    }
                }
            }

            return objFeed;
        }

        /// <summary>
        /// 사용자의 관심항목 피드 리스트
        /// </summary>
        public clsFeed showFavorFeed(string user_id)
        {
            System.Data.DataSet DS = new System.Data.DataSet();
            clsFeed objFeed = new clsFeed();

            string idx;
            string title;
            string url;
            string desc;
            bool isRead;
            DateTime date = new DateTime();

            int i;

            DS = selectFavorItem(user_id);

            if (objCmnDB.validateDS(DS))
            {
                for (i = 0; i < DS.Tables[0].Rows.Count; i++)
                {
                    idx = DS.Tables[0].Rows[i][0].ToString();
                    title = DS.Tables[0].Rows[i][1].ToString();
                    url = DS.Tables[0].Rows[i][2].ToString();
                    desc = DS.Tables[0].Rows[i][3].ToString();
                    date = Convert.ToDateTime(DS.Tables[0].Rows[i][4]);
                    if (DS.Tables[0].Rows[i]["Read"].ToString().Equals("AA"))
                    {
                        isRead = true;
                    }
                    else
                    {
                        isRead = false;
                    }
                    objFeed.addFeed(idx, title, url, desc, date, true, isRead);
                }

                prcPastItem(objFeed, user_id);
            }

            return objFeed; 
        }

        /// <summary>
        /// 사용자의 RSS사이트 조회
        /// </summary>
        public System.Data.DataSet selectRSSsite(string user_id)
        {
            System.Data.DataSet DS = new System.Data.DataSet();
            System.Data.SqlClient.SqlConnection dbCon;

            string QUERY = "SELECT idx, RSS_name, RSS_url FROM tb_RSSsite WHERE user_id='" + user_id + "' AND RSS_state='AA'";

            dbCon = objDB.GetConnection();
            DS = objDB.ExecuteDSQuery(dbCon, QUERY);
            dbCon.Close();

            return DS;
        }

        /// <summary>
        /// RSS정보 읽어와서 파싱
        /// </summary>
        public clsFeed getRSSfeed(string RSS_idx, string URL, string user_id)
        {
            clsFeed objFeed = new clsFeed();
            
            Create_XML_Reader(URL);

            if (reader == null)
            {
                return new clsFeed();
            }

            string title = "";
            string link = "";
            string desc = "";
            DateTime date = new DateTime();

            try
            {
                if (MoveCursor(XmlNodeType.Element, "channel"))
                {
                    if (MoveCursor(XmlNodeType.Element, "title"))
                    {
                        objFeed.siteTitle = reader.ReadElementString();
                        objFeed.siteURL = URL;
                    }
                }

                while (MoveCursor(XmlNodeType.Element, "item"))
                {
                    if (MoveCursor(XmlNodeType.Element, "title"))
                    {
                        title = reader.ReadString();
                    }

                    if (MoveCursor(XmlNodeType.Element, "link"))
                    {
                        link = reader.ReadString();
                    }

                    if (MoveCursor(XmlNodeType.Element, "description"))
                    {
                        desc = reader.ReadString();
                    }

                    if (MoveCursor(XmlNodeType.Element, "pubDate"))
                    {
                        date = Convert.ToDateTime(reader.ReadString());
                    }

                    objFeed.addFeed(RSS_idx, title, link, desc, date, false, false);
                }
            }
            catch(Exception ex)
            {
                objUtil.writeLog("ERR PARSING XML : " + URL);
                return new clsFeed();
            }

            prcFavorItem(objFeed, user_id);
            prcPastItem(objFeed, user_id);

            return objFeed;
        }

        /// <summary>
        /// 관심 항목 처리
        /// </summary>
        private void prcFavorItem(clsFeed objFeed, string user_id)
        {
            System.Data.DataSet favorItem_DS = new System.Data.DataSet();

            string itemURL;
            int i;
            int j;

            favorItem_DS = selectFavorItem(user_id);

            if (objCmnDB.validateDS(favorItem_DS))
            {
                for (i = 0; i < favorItem_DS.Tables[0].Rows.Count; i++)
                {
                    for (j = 0; j < objFeed.Count; j++)
                    {
                        itemURL = favorItem_DS.Tables[0].Rows[i][2].ToString();

                        if (itemURL.Equals(objFeed[j].Item_url))
                        {
                            objFeed[j].isFavor = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 읽은 목록 처리
        /// </summary>
        private void prcPastItem(clsFeed objFeed, string user_id)
        {
            System.Data.DataSet pastItem_DS = new System.Data.DataSet();

            string itemURL;
            int i;
            int j;

            pastItem_DS = selectPastItem(objFeed.siteTitle, objFeed.siteURL, user_id, objFeed[objFeed.Count-1].Item_date);

            if (objCmnDB.validateDS(pastItem_DS))
            {
                for (i = 0; i < pastItem_DS.Tables[0].Rows.Count; i++)
                {
                    for (j = 0; j < objFeed.Count; j++)
                    {
                        itemURL = pastItem_DS.Tables[0].Rows[i][0].ToString();

                        if (itemURL.Equals(objFeed[j].Item_url))
                        {
                            objFeed[j].isRead = true;
                        }
                    }
                }
            }
        }

        //읽은 목록 가져오기
        private System.Data.DataSet selectPastItem(string siteTitle, string siteURL, string user_id, DateTime date)
        {
            System.Data.SqlClient.SqlConnection dbCon;
            System.Data.DataSet DS = new System.Data.DataSet();
            clsRSSinfo objRSSinfo = new clsRSSinfo(siteTitle, siteURL);
            
            string QUERY;

            objRSSinfo.setSiteIdx(user_id);
            QUERY = "SELECT Item_url FROM tb_history WHERE state='AA' AND user_id='" + user_id + "' AND RSS_idx='" + objRSSinfo.idx + "' AND CONVERT(varchar(10), Item_date, 20) BETWEEN '" + date.ToString("yyyy-MM-dd") + "' AND CONVERT(varchar(10), GETDATE(), 20) ORDER BY Item_date ASC";

            dbCon = objDB.GetConnection();
            DS = objDB.ExecuteDSQuery(dbCon, QUERY);
            dbCon.Close();

            return DS;
        }

        //관심 항목 가져오기
        public System.Data.DataSet selectFavorItem(string user_id)
        {
            System.Data.SqlClient.SqlConnection dbCon;
            System.Data.DataSet DS = new System.Data.DataSet();

            string QUERY;

            QUERY = "SELECT A.idx, A.Item_title, A.Item_url, A.Item_desc, A.Item_date, ISNULL(B.state, '') AS 'Read' FROM tb_favorite A LEFT OUTER JOIN tb_history B ON A.Item_url=B.Item_url WHERE A.state='AA' AND A.user_id='" + user_id + "' AND A.state='AA'  ORDER BY A.Item_date ASC";

            dbCon = objDB.GetConnection();
            DS = objDB.ExecuteDSQuery(dbCon, QUERY);
            dbCon.Close();

            return DS;
        }

        /// <summary>
        /// URL에서 XML리더 생성하여 리턴
        /// </summary>
        public void Create_XML_Reader(string URL)
        {
            XmlReaderSettings setting = new XmlReaderSettings();
            setting.IgnoreWhitespace = true;

            try
            {
                reader = XmlReader.Create(URL, setting);
            }
            catch(Exception ex)
            {
                objUtil.writeLog("ERR CREATE XML READER : " + URL);
                reader = null;
            }
        }

        /// <summary>
        /// 노드타입과 이름이 같은곳에 도달했는지 알아보기위한 메서드
        /// </summary>
        public bool MoveCursor(XmlNodeType type, string name)
        {
            while (reader.Read())
            {
                if (reader.NodeType.Equals(type) && reader.Name.Equals(name))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 읽은 기사 인서트 실패/오류시 FAIL 리턴
        /// </summary>
        /// <param name="RSS_idx">RSS사이트 idx</param>
        /// <param name="Item_date">yyyy-MM-dd HH:mm:ss</param>
        public string insertPastItem(string RSS_idx, string Item_url, string Item_date, string user_id)
        {
            System.Data.SqlClient.SqlConnection dbCon;
            System.Data.SqlClient.SqlTransaction TRX;

            string strDate;
            string Cols;
            string Vals;

            string Result = "FAIL";
            string flag;

            strDate = Convert.ToDateTime(Item_date).ToString("yyyy-MM-dd HH:mm:ss");

            Cols = "RSS_idx|user_id|Item_url|Item_date";
            Vals = RSS_idx + "|" + user_id + "|" + Item_url + "|" + strDate;

            dbCon = objDB.GetConnection();
            TRX = dbCon.BeginTransaction();
            flag = isDupeItem(dbCon, TRX, user_id, Item_url, "tb_history");
            if (flag.Equals("OK"))
            {
                Result = objCmnDB.INSERT_DB(dbCon, TRX, "tb_history", Cols, Vals);
            }
            else if (flag.Equals("DUPE"))
            {
                Result = toggleReadItemState(dbCon, TRX, Item_url, user_id, "AA");
            }

            if (Result.Equals("FAIL"))
            {
                TRX.Rollback();
                objUtil.writeLog(string.Format("FAIL INSERT HISTORY : {0}-{1}({2})", user_id, RSS_idx, Item_url));
            }
            else
            {
                TRX.Commit();
            }
            dbCon.Close();

            return Result;
        }

        /// <summary>
        /// 관심기사 인서트 실패/오류시 FAIL 리턴
        /// </summary>
        /// <param name="RSS_idx">RSS사이트 idx</param>
        /// <param name="Item_desc">요약정보</param>
        /// <param name="Item_date">yyyy-MM-dd HH:mm:ss</param>
        public string insertFavorItem(string RSS_idx, string Item_title, string Item_url, string Item_desc, string Item_date, string user_id)
        {
            System.Data.SqlClient.SqlConnection dbCon;
            System.Data.SqlClient.SqlTransaction TRX;
            
            string strDate;
            string Cols;
            string Vals;

            string Result = "FAIL";
            string flag;

            strDate = Convert.ToDateTime(Item_date).ToString("yyyy-MM-dd HH:mm:ss");

            Cols = "RSS_idx|user_id|Item_title|Item_url|Item_desc|Item_date";
            Vals = RSS_idx + "|" + user_id + "|" + Item_title.Replace("|", "") + "|" + Item_url + "|" + Item_desc.Replace("|", "") + "|" + strDate;

            dbCon = objDB.GetConnection();
            TRX = dbCon.BeginTransaction();
            flag = isDupeItem(dbCon, TRX, user_id, Item_url, "tb_favorite");
            if (flag.Equals("OK"))
            {
                Result = objCmnDB.INSERT_DB(dbCon, TRX, "tb_favorite", Cols, Vals);
            }
            else if (flag.Equals("DUPE"))
            {
                Result = toggleFavorState(dbCon, TRX, Item_url, user_id, "AA");
            }

            if (Result.Equals("FAIL"))
            {
                TRX.Rollback();
                objUtil.writeLog(string.Format("FAIL INSERT FAVORITE : {0}-{1}({2})", user_id, RSS_idx, Item_url));
            }
            else
            {
                TRX.Commit();
            }
            dbCon.Close();

            return Result;
        }

        /// <summary>
        /// state 컬럼 값 변경, 실패시 FAIL 리턴
        /// </summary>
        private string updateState(System.Data.SqlClient.SqlConnection dbCon, System.Data.SqlClient.SqlTransaction TRX, string tbName, string wCols, string wVals, string state)
        {
            string Result;

            Result = objCmnDB.UPDATE_DB(dbCon, TRX, tbName, "state", state, wCols, wVals);
            
            if (Result.Equals("FAIL"))
            {
                objUtil.writeLog(string.Format("FAIL UPDATE STATE : {0} - {1}", tbName, state));
            }

            return Result;
        }

        /// <summary>
        /// 존재하는 관심항목 설정/해제, 실패시 FAIL 리턴
        /// </summary>
        /// <param name="state">설정:AA, 해제:ZZ</param>
        public string toggleFavorState(System.Data.SqlClient.SqlConnection dbCon, System.Data.SqlClient.SqlTransaction TRX, string Item_url, string user_id, string state)
        {
            string Cols = "Item_url|user_id";
            string Vals = Item_url + "|" + user_id;
            string tbName = "tb_favorite";

            return updateState(dbCon, TRX, tbName, Cols, Vals, "ZZ");
        }

        /// <summary>
        /// 존재하는 읽은항목 설정/해제, 실패시 FAIL 리턴
        /// </summary>
        /// <param name="state">읽음:AA, 읽지 않음:ZZ</param>
        public string toggleReadItemState(System.Data.SqlClient.SqlConnection dbCon, System.Data.SqlClient.SqlTransaction TRX, string Item_url, string user_id, string state)
        {
            string Cols = "Item_url|user_id";
            string Vals = Item_url + "|" + user_id;
            string tbName = "tb_history";

            return updateState(dbCon, TRX, tbName, Cols, Vals, state);
        }

        /// <summary>
        /// 관심항목, 읽은기사 중복 검사
        /// 중복값 없으면 OK, 중복이면 DUPE, 에러는 FAIL 리턴
        /// </summary>
        /// <param name="tbName">"tb_history" or "tb_favorite"</param>
        public string isDupeItem(System.Data.SqlClient.SqlConnection dbCon, System.Data.SqlClient.SqlTransaction TRX, string user_id, string Item_url, string tbName)
        {
            string Cols;
            string Vals;

            Cols = "user_id|Item_url";
            Vals = user_id + "|" + Item_url;
            return objCmnDB.chk_duplicate(dbCon, TRX, "*", tbName, Cols, Vals);
        }
        
        
        public string DS2Table(clsFeed objFeed, string user_id, int moreCnt)
        {
            int i;
            int j;
            int k = 2;
            //int moreCnt = 15;

            string title;
            string url;
            string desc;
            DateTime date;
            bool favor;
            bool read;
            
            StringBuilder strBuilder = new StringBuilder();

            if (objFeed.Count == 0)
            {
                return "피드가 없습니다.";
            }

            /*
            strBuilder.AppendLine("<table>");
            strBuilder.AppendLine("    <colgroup>");
            strBuilder.AppendLine("        <col style='width:30px;' />");
            strBuilder.AppendLine("        <col style='width:190px;' />");
            strBuilder.AppendLine("        <col style='width:190px;' />");
            strBuilder.AppendLine("        <col style='width:190px;' />");
            strBuilder.AppendLine("    </colgroup>");
            strBuilder.AppendLine("    <tbody>");
            */

            if (objFeed.Count <= moreCnt)
            {
                moreCnt = objFeed.Count;
            }

            if (objFeed.Count >= moreCnt)
            {
                for (j = 0; j < moreCnt; j++)
                //기존의 것
                //for (j = 0; j < objFeed.Count; j++)
                {
                    title = objFeed[j].Item_title.ToString();
                    url = objFeed[j].Item_url.ToString();
                    desc = objFeed[j].Item_desc.ToString();
                    //desc = objUtil.ToJavascript(desc);
                    date = objFeed[j].Item_date;
                    favor = objFeed[j].isFavor;
                    read = objFeed[j].isRead;

                    //strBuilder.AppendLine("        <input type='hidden' id='desc" + j + "' name='desc" + j + "' value='" + desc + "'/>");

                    //제목줄
                    if (read == true)
                    {
                        strBuilder.AppendLine("        <div class='read'>");
                    }
                    else
                    {
                        strBuilder.AppendLine("        <div>");
                    }
                    strBuilder.AppendLine("        <div id='titleDiv'>");

                    if (favor == true)
                    {
                        strBuilder.AppendLine("            <input type=checkbox name='check' value='' checked='checked'  onfocus=\"javascript:favorite('" + title + "', '" + url + "', '" + j + "', '" + date + "', '" + user_id + "');\"/>");

                    }
                    else
                    {
                        strBuilder.AppendLine("            <input type=checkbox name='check' value='' onfocus=\"javascript:favorite('" + title + "', '" + url + "', '" + j + "', '" + date + "', '" + user_id + "');\"/>");
                    }

                    strBuilder.AppendLine("         <a class='title' href=\"javascript:toggle('" + j + "');\" >");
                    strBuilder.AppendLine(title);
                    strBuilder.AppendLine("             <span class='date'>");
                    strBuilder.AppendLine(date.ToString());
                    strBuilder.AppendLine("             </span>");
                    strBuilder.AppendLine("         </a>");
                    strBuilder.AppendLine("     </div>");

                    //date &desc(내부에 table이 있음)
                    strBuilder.AppendLine("     <div id='descDisplay" + j + "' name='descDisplay" + j + "' class='desc' style='display:none'>");
                    strBuilder.AppendLine("         <a href=\"javascript:PastItem('" + url + "', '" + date + "');\">");
                    strBuilder.AppendLine("<span class='more'>more</span>");
                    strBuilder.AppendLine("         </a>");
                    strBuilder.AppendLine("<span id='desc" + j + "'>");
                    strBuilder.AppendLine(desc);
                    strBuilder.AppendLine("</span>");
                    strBuilder.AppendLine("     </div>");


                    strBuilder.AppendLine("</div>");

                }

            }

            if (moreCnt < objFeed.Count)
            {
                moreCnt += 10;
                strBuilder.AppendLine("</br>");
                strBuilder.AppendLine("<div id='more10' name='more10'>");
                strBuilder.AppendLine("         <a class='more10' href=\"javascript:selectMore10('" + moreCnt + "');\">");
                //strBuilder.AppendLine("<span class='fontColor'>(10)</span>항목 더보기&nbsp;&gt;&nbsp;&gt;");
                strBuilder.AppendLine("항목 더보기&nbsp;&gt;&nbsp;&gt;");
                strBuilder.AppendLine("         </a>");
                strBuilder.AppendLine("</div>");
            }

            return strBuilder.ToString();

        }

        public string getRSS_Idx(string user_id, string RSS_url)
        {
            System.Data.SqlClient.SqlConnection dbCon;
            System.Data.DataSet DS = new System.Data.DataSet();

            string result;

            string QUERY = "SELECT idx FROM tb_RSSsite WHERE user_id='" + user_id + "' AND RSS_url LIKE '%" + RSS_url + "%' AND RSS_state='AA'";
            objUtil.writeLog("getRSS_Idx:" + QUERY);

            dbCon = objDB.GetConnection();
            DS = objDB.ExecuteDSQuery(dbCon, QUERY);
            dbCon.Close();

            result = DS.Tables[0].Rows[0][0].ToString();

            return result;

        }
        //관심항목 select ds ---> objfeed
        public clsFeed TOobjFeed(System.Data.DataSet DS)
        {
            string idx = "";
            string title = "";
            string url = "";
            string desc = "";
            DateTime date = new DateTime();

            int i;

            libCommon.clsUtil objUtil = new libCommon.clsUtil();
            clsFeed objFeed = new clsFeed();


            if (DS.Tables[0].Rows.Count > 0)
            {

                for (i = 0; i < DS.Tables[0].Rows.Count; i++)
                {
                    idx = DS.Tables[0].Rows[i][0].ToString();
                    title = DS.Tables[0].Rows[i][1].ToString();
                    url = DS.Tables[0].Rows[i][2].ToString();
                    desc = DS.Tables[0].Rows[i][3].ToString();
                    date = Convert.ToDateTime(DS.Tables[0].Rows[i][4]);

                    objFeed.addFeed(idx, title, url, desc, date, true, false);

                }

            }

            return objFeed;
        }

    }
}
