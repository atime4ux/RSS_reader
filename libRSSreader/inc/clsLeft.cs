using System;
using System.Collections.Generic;
using System.Text;

namespace libRSSreader
{
    public class clsLeft
    {
        libCommon.clsDB objDB = new libCommon.clsDB();
        libCommon.clsUtil objUtil = new libCommon.clsUtil();
        libRSSreader.clsCmnDB objCmnDB = new clsCmnDB();

        // 추가 by 이현석(2011-07-27)
        public string LeftMenuString(System.Web.SessionState.HttpSessionState Session, clsFeed objFeed)
        {
            System.Data.SqlClient.SqlConnection dbCon;
            System.Data.DataSet siteList_DS = new System.Data.DataSet();

            string menu_state = "";
            
            string login_user_name = (string)Session["user_name"];
            string login_comp_name = (string)Session["comp_name"];
            string login_part_name = (string)Session["part_name"];

            libRSSreader.clsLeft objLeft = new libRSSreader.clsLeft();

            //로그인 했을때
            if ((string)Session["user_id"] != "Not login")
            {
                dbCon = objDB.GetConnection();
                siteList_DS = objLeft.LeftMenu(dbCon, (string)Session["user_id"]);
                dbCon.Close();

                menu_state = objLeft.DS2Left(siteList_DS, (string)Session["user_id"], objFeed);
            }
            else
            {
                menu_state = "<a href='javascript:gologin();'>프로그램</a>";
            }

            return menu_state;
        }

        public System.Data.DataSet LeftMenu(System.Data.SqlClient.SqlConnection dbCon, string user_id)
        {
            System.Data.DataSet DS = new System.Data.DataSet();
            StringBuilder strBuilder = new StringBuilder();

            string Result = "FAIL";

            strBuilder.Append("SELECT ");

            strBuilder.Append(" RSS_name,RSS_url, idx from tb_RSSsite WHERE user_id ='" + user_id + "' AND RSS_state='AA'");


            try
            {
                DS = objDB.ExecuteDSQuery(dbCon, strBuilder.ToString());
                //리스트 출력시 로그파일 기록
                objUtil.writeLog("GET LeftMenu QUERY : " + strBuilder.ToString());

            }
            catch (Exception e)
            {
                Result = "FAIL";
                objUtil.writeLog("FAIL LeftMenu : " + user_id);
            }

            return DS;
        }

        //left
        // 읽지 않은 아이템 수 추가 by 이현석(2011-07-27)
        public string DS2Left(System.Data.DataSet DS, string user_id, clsFeed objFeed)
        {
            clsRSS objRSS = new clsRSS();
            StringBuilder strBuilder = new StringBuilder();

            string idx;

            int i;

            if (objCmnDB.validateDS(DS))
            {
                //전체 항목
                strBuilder.Append("     <ul>");
                //전체 항목 인자값 넣어주기
                strBuilder.Append("         <a href='javascript:go_AllList();' >");
                strBuilder.Append("             <span id='AllList' name='AllList' class='MenuName'>");
                strBuilder.Append("전체항목(" + objFeed.UnreadItems().Count + ")");
                strBuilder.Append("             </span>");
                strBuilder.Append("         </a>");
                
                strBuilder.Append("     </ul>");

                //관심항목
                strBuilder.Append("     <ul>");
                //관심항목 인자값 넣어주기
                strBuilder.Append("         <a href='javascript:go_FavorPage();' >");
                strBuilder.Append("             <span id='selectFavorItem' name='selectFavorItem' class='MenuName'>");

                strBuilder.Append("관심항목(" + objFeed.FavorItems().UnreadItems().Count + ")");
                strBuilder.Append("             </span>");
                strBuilder.Append("         </a>");
                strBuilder.Append("     </ul>");

                //구독 LIST 출력
                for (i = 0; i < DS.Tables[0].Rows.Count; i++)
                {
                    strBuilder.Append("     <ul>");
                    strBuilder.Append("         <a href=\"javascript:go_ListPage('" + DS.Tables[0].Rows[i][1].ToString() + "', '" + DS.Tables[0].Rows[i][2].ToString() + "');\">");
                    strBuilder.Append("             <span id='List" + DS.Tables[0].Rows[i][2].ToString() + "' name='List" + DS.Tables[0].Rows[i][2].ToString() + "' class='MenuName'>");

                    idx = DS.Tables[0].Rows[i][2].ToString();

                    strBuilder.Append("   " + DS.Tables[0].Rows[i][0] + "(" + objFeed.extractItems(idx).UnreadItems().Count + ")");
                    strBuilder.Append("             </span>");
                    strBuilder.Append("         </a>");
                    strBuilder.Append("     </ul>");

                }

                //구독관리(rss002.aspx)
                strBuilder.Append("     <ul>");
                //관심항목 인자값 넣어주기
                strBuilder.Append("         <a href='../RSS/RSS002.aspx' >");
                strBuilder.Append("             <span id='manage' name='manage' class='MenuName'>");
                strBuilder.Append("구독관리");
                strBuilder.Append("             </span>");
                strBuilder.Append("         </a>");
                strBuilder.Append("     </ul>");

            }

            return strBuilder.ToString();
        }


    }
}
