using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;

namespace libRSSreader
{
    public class clsRSSinfo
    {
        libCommon.clsDB objDB = new libCommon.clsDB();
        libCommon.clsUtil objUtil = new libCommon.clsUtil();
        clsCmnDB objCmnDB = new clsCmnDB();

        public string idx;
        public string siteTitle;
        public string siteURL;

        public clsRSSinfo()
        {
            siteTitle = "";
            siteURL = "";
        }

        public clsRSSinfo(string title, string url)
        {
            siteTitle = title;
            siteURL = url;
        }

        /// <summary>
        /// RSS사이트 인서트, 실패/오류시 FAIL 리턴
        /// </summary>
        public string insertRSSsite(string URL, string user_id)
        {
            System.Data.SqlClient.SqlConnection dbCon;
            System.Data.SqlClient.SqlTransaction TRX;

            string Cols;
            string Vals;

            string flag;
            string Result = "FAIL";

            setSiteInfo(URL);

            if (siteURL.Trim().Length > 0 && siteTitle.Trim().Length > 0)
            {
                Cols = "user_id|RSS_name|RSS_url";
                Vals = user_id + "|" + siteTitle + "|" + siteURL;

                dbCon = objDB.GetConnection();
                TRX = dbCon.BeginTransaction();
                flag = isDupeSite(dbCon, TRX, user_id, URL);
                if (flag.Equals("OK"))
                {
                    Result = objCmnDB.INSERT_DB(dbCon, TRX, "tb_RSSsite", Cols, Vals);
                }
                else if (flag.Equals("DUPE"))
                {
                    Result = toggleSiteState(dbCon, TRX, user_id, URL, "AA");
                }

                if (Result.Equals("FAIL"))
                {
                    TRX.Rollback();
                    objUtil.writeLog(string.Format("FAIL INSERT RSS SITE INFO : {0}-{1}({2})", user_id, siteTitle, siteURL));
                }
                else
                {
                    TRX.Commit();
                }
                dbCon.Close();

            }

            return Result;
        }

        /// <summary>
        /// RSS사이트의 idx를 설정(title, url필요)
        /// </summary>
        /// <param name="user_id"></param>
        public void setSiteIdx(string user_id)
        {
            System.Data.DataSet DS = new System.Data.DataSet();
            System.Data.SqlClient.SqlConnection dbCon;

            string QUERY = "SELECT idx FROM tb_RSSsite WHERE user_id='" + user_id + "' AND RSS_name='" + objUtil.toDb(siteTitle) + "' AND RSS_url='" + siteURL + "' AND RSS_state='AA'";

            dbCon = objDB.GetConnection();
            DS = objDB.ExecuteDSQuery(dbCon, QUERY);
            dbCon.Close();

            if (objCmnDB.validateDS(DS))
            {
                idx = DS.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                idx = "";
            }
        }

        /// <summary>
        /// RSS사이트 정보 확인
        /// </summary>
        private void setSiteInfo(string URL)
        {
            XmlNodeType element = XmlNodeType.Element;

            clsRSS objRSS = new clsRSS();

            objRSS.Create_XML_Reader(URL);

            if (objRSS.MoveCursor(element, "channel"))
            {
                if (objRSS.MoveCursor(element, "title"))
                {
                    siteTitle = objRSS.reader.ReadElementString();
                    siteURL = URL;
                }
            }
        }

        /// <summary>
        /// RSS사이트 정보 삭제
        /// </summary>
        /// <param name="dbCon"></param>
        /// <param name="TRX"></param>
        /// <param name="user_id"></param>
        /// <param name="url"></param>
        public void deleteRSSsite(System.Data.SqlClient.SqlConnection dbCon, System.Data.SqlClient.SqlTransaction TRX, string user_id, string url)
        {
            toggleSiteState(dbCon, TRX, user_id, url, "ZZ");
        }

        /// <summary>
        /// RSS사이트 state 변경
        /// </summary>
        /// <param name="state">설정:AA, 해제:ZZ</param>
        /// <returns></returns>
        private string toggleSiteState(System.Data.SqlClient.SqlConnection dbCon, System.Data.SqlClient.SqlTransaction TRX, string user_id, string url, string state)
        {
            string wCols;
            string wVals;
            string Result;

            wCols = "user_id|RSS_url";
            wVals = user_id + "|" + url;

            Result = objCmnDB.UPDATE_DB(dbCon, TRX, "tb_RSSsite", "RSS_state", state, wCols, wVals);

            if (Result.Equals("FAIL"))
            {
                objUtil.writeLog(string.Format("FAIL UPDATE STATE : {0} - {1}", url, state));
            }

            return Result;
        }

        /// <summary>
        /// RSS사이트 중복 검사
        /// 중복값 없으면 OK, 중복이면 DUPE, 에러는 FAIL 리턴
        /// </summary>
        /// <param name="tbName">"tb_history" or "tb_favorite"</param>
        private string isDupeSite(System.Data.SqlClient.SqlConnection dbCon, System.Data.SqlClient.SqlTransaction TRX, string user_id, string url)
        {
            string wCols = "user_id|RSS_url";
            string wVals = user_id + "|" + url;
            
            return objCmnDB.chk_duplicate(dbCon, TRX, "*", "tb_RSSsite", wCols, wVals);
        }
    }
}
