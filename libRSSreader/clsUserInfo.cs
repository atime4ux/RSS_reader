using System;
using System.Collections.Generic;
using System.Text;

namespace libRSSreader
{
    public class clsUserInfo
    {
        public string user_id;
        public string email;
        public string passwd;

        public clsUserInfo(string user_id, string email, string passwd)
        {
            this.user_id = user_id;
            this.email = email;
            this.passwd = passwd;
        }

        public string getCols()
        {
            return "user_id|email|passwd";
        }

        public string getVals()
        {
            return user_id + "|" + email + "|" + passwd;
        }

        public string getICols()
        {
            return getCols() + "|reg_id|update_id";
        }

        public string getIVals(string login_id)
        {
            return getVals() + "|" + login_id + "|" + login_id;
        }

        public string getUCols()
        {
            return getCols() + "|update_id|update_date";
        }

        public string getUVals(string login_id)
        {
            return getVals() + "|" + login_id + "|" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "";
        }

        /// <summary>
        /// 탑섹션 HTML 스트링
        /// </summary>
        /// <param name="compName"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        /// 
        public string getTopInfoString(string user_id)
        {
            StringBuilder strBuilder = new StringBuilder();

            strBuilder.AppendLine("<table class=\"top_loginInfo\">");
            strBuilder.AppendLine(" <tr>");
            strBuilder.AppendLine("     <td class=\"top_loginInfo_hello\"><span class=\"top_loginInfo_user_name\">&nbsp;&nbsp;" + user_id + "</span>님께서 로그인하셨습니다.</td>");
            strBuilder.AppendLine("     <td class=\"top_loginInfo_logout\"><a href=\"javascript:logout();\" id=\"logout\" name=\"logout\">[Logout]</a></td>");
            strBuilder.AppendLine(" </tr>");
            strBuilder.AppendLine("</table>");

            return strBuilder.ToString();
        }

    }
}