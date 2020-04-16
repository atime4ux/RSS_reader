using System;
using System.Collections.Generic;
using System.Text;

namespace libRSSreader
{
    public class clsUser
    {
        libCommon.clsDB objDB = new libCommon.clsDB();
        libCommon.clsUtil objUtil = new libCommon.clsUtil();
        libRSSreader.clsCmnDB objCmnDB = new clsCmnDB();

        /// <summary>
        /// 오류는 FAIL, 중복은 DUPE 리턴
        /// </summary>
        public string insertUser(System.Data.SqlClient.SqlConnection dbCon, clsUserInfo objUserInfo, string loginID)
        {
            System.Data.SqlClient.SqlTransaction TRX;

            string tb_Name = "tb_user";
            string Result = "";

            TRX = dbCon.BeginTransaction();

            Result = chkUserID(dbCon, TRX, objUserInfo.user_id);
            if (Result.Equals("OK"))
            {
                Result = objCmnDB.INSERT_DB(dbCon, TRX, tb_Name, objUserInfo.getICols(), objUserInfo.getIVals(loginID));

                if (Result.Equals("FAIL"))
                {
                    TRX.Rollback();
                    objUtil.writeLog("FAIL INSERT USER : " + objUserInfo.user_id);
                }
                else
                {
                    TRX.Commit();
                }
            }
            else
            {
                TRX.Rollback();
            }

            return Result;
        }

        //ID중복 검사
        private string chkUserID(System.Data.SqlClient.SqlConnection dbCon, System.Data.SqlClient.SqlTransaction TRX, string user_id)
        {
            System.Text.StringBuilder strBuilder = new StringBuilder();
            System.Data.DataSet DS = new System.Data.DataSet();

            string Result = "";
            
            try
            {
                Result = objCmnDB.chk_duplicate(dbCon, TRX, "user_id", "tb_user", "user_id", user_id);
            }
            catch (Exception ex)
            {
                Result = "FAIL";
                objUtil.writeLog("FAIL CHECK USER ID : " + user_id);
            }

            return Result;
        }

        /// <summary>
        /// 로그인 정보 일치하면 SUCCESS, 실패나 오류는 FAIL 리턴
        /// </summary>
        public string userAuth(System.Data.SqlClient.SqlConnection dbCon, string user_id, string passwd)
        {
            System.Data.DataSet DS = new System.Data.DataSet();

            string QUERY = "SELECT COUNT(*) FROM tb_user WHERE user_id='" + user_id + "' AND passwd='" + passwd + "'";
            string Result = "FAIL";

            try
            {
                DS = objDB.ExecuteDSQuery(dbCon, QUERY);
                if (DS.Tables[0].Rows[0][0].ToString().Equals("1"))
                {
                    Result = "SUCCESS";
                }
            }
            catch (Exception e)
            {
                Result = "FAIL";
                objUtil.writeLog("FAIL USER AUTHENTICATION : " + user_id);
            }

            return Result;
        }

        //RSS URL 추가
        public string insertURL(System.Data.SqlClient.SqlConnection dbCon, string URL, string loginID)
        {
            System.Data.SqlClient.SqlTransaction TRX;

            string tb_Name = "tb_RSSsite";
            string Result = "";
            int idx;
            string Cols = "user_id|RSS_name|RSS_state|reg_date|update_date";
            string Vals = loginID + "||" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "|" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            TRX = dbCon.BeginTransaction();

            Result = objCmnDB.INSERT_DB(dbCon, TRX, tb_Name, Cols, Vals);

            if (Result.Equals("FAIL"))
            {
                TRX.Rollback();
                objUtil.writeLog("FAIL INSERT URL : " + URL);
            }
            else
            {
                TRX.Commit();
            }


            return Result;
        }

        //getUSERRssSite(RSS002.aspx)
        public System.Data.DataSet userRSSList(System.Data.SqlClient.SqlConnection dbCon, string user_id, int pageNum, int pageCnt, int totalCnt)
        {
            System.Data.DataSet DS = new System.Data.DataSet();
            StringBuilder strBuilder = new StringBuilder();

            strBuilder.Append("SELECT ");

            if ((pageCnt * pageNum) > 0)
            {
                if (totalCnt == 0)
                {
                    strBuilder.Append(" TOP " + (pageNum * pageCnt).ToString());
                }

                strBuilder.Append(" idx, RSS_name, RSS_url from tb_RSSsite WHERE user_id ='" + user_id + "' AND RSS_state = 'AA'" );

            }
            else
            {
                strBuilder.Append(" count(*) from tb_RSSsite WHERE user_id ='" + user_id + "'");
            }

            string Result = "FAIL";

            try
            {
                DS = objDB.ExecuteDSQuery(dbCon, strBuilder.ToString());
                //리스트 출력시 로그파일 기록
                objUtil.writeLog("GET userRSSList QUERY : " + strBuilder.ToString());

            }
            catch (Exception e)
            {
                Result = "FAIL";
                objUtil.writeLog("FAIL USER userRSSList : " + user_id);
            }

            return DS;
        }

        //RSSList 출력(구독관리 화면)
        public string RSSList_DS2Table(System.Data.DataSet DS, int pageCnt, int pageNum)
        {

            int i;
            int j;
            int k = 2;
            string idx;
            string RSS_name;
            string RSS_url;

            StringBuilder strBuilder = new StringBuilder();
            if (DS.Tables[0].Rows.Count > 0)
            {
                strBuilder.AppendLine("<table style='width:996px;'>");
                strBuilder.AppendLine("    <colgroup>");
                strBuilder.AppendLine("        <col style='width:60px;' />");
                strBuilder.AppendLine("        <col style='width:500px;' />");
                strBuilder.AppendLine("        <col style='width:100px;' />");
                strBuilder.AppendLine("    </colgroup>");
                /*

strBuilder.AppendLine("    <thead>");
strBuilder.AppendLine("        <tr class = 'row1'>");

//제목줄 출력
for (j = 0; j < DS.Tables[0].Columns.Count; j++)
{
    strBuilder.AppendLine("        <th>");
    strBuilder.AppendLine(DS.Tables[0].Columns[j].ColumnName);
    strBuilder.AppendLine("        </th>");
}
strBuilder.AppendLine("        </tr>");
strBuilder.AppendLine("    </thead>");

*/

                strBuilder.AppendLine("    <tbody>");

                for (i = 0; i < DS.Tables[0].Rows.Count; i++)
                {
                    //table 새로운 행이 시작 될때(tr)
                    if (k % 2 == 1)
                    {
                        strBuilder.AppendLine("        <tr class = 'row2'>");
                        k++;
                    }
                    else
                    {
                        strBuilder.AppendLine("        <tr class = 'row3'>");
                        k++;
                    }

                    //열 출력(td)
                    for (j = 0; j < DS.Tables[0].Columns.Count; j++)
                    {
                        idx = DS.Tables[0].Rows[i][0].ToString();
                        RSS_name = DS.Tables[0].Rows[i][1].ToString();
                        RSS_url = DS.Tables[0].Rows[i][2].ToString();

                        if (j == 0)
                        {
                            strBuilder.AppendLine("        <td class='c_align'>");
                            strBuilder.AppendLine("            <input type='checkbox' name='check' value='" + RSS_url + "'>");
                            strBuilder.AppendLine("            </input>");
                            strBuilder.AppendLine("        </td>");
                        }
                        else if (j == DS.Tables[0].Columns.Count - 1)
                        {
                            strBuilder.AppendLine("        <td class='c_align'>");
                           // strBuilder.AppendLine("            <a href=\"javascript:delRSSList('" + idx + "');\" >");
                            strBuilder.AppendLine("                 <input type='button' onclick=\"javascript:delRSSList('" + RSS_url + "');\" style='height:20px' value='구독취소'/>");
                            //strBuilder.AppendLine("            </a>");
                            strBuilder.AppendLine("        </td>");
                        }
                        else
                        {
                            strBuilder.AppendLine("        <td class = l_align>");
                            //값 클릭시 get방식으로 값 넘기기
                            strBuilder.AppendLine("<span class='RSS_name'>" + RSS_name + "</span></br>");
                            strBuilder.AppendLine("<span>" + RSS_url + "</span>");
                            strBuilder.AppendLine("        </td>");
                        }

                    }
                    strBuilder.AppendLine("        </tr>");
                }
                strBuilder.AppendLine("    </tbody>");
                strBuilder.AppendLine("</table>");
            }
            else
            {
                strBuilder.AppendLine("");
            }


            return strBuilder.ToString();
        }

    }
}
