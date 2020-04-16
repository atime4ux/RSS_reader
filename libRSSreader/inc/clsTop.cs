using System;
using System.Collections.Generic;
using System.Text;

namespace libRSSreader
{
    public class clsTop
    {
        public string TopMenuString(System.Web.SessionState.HttpSessionState Session)
        {
            StringBuilder login_strBuilder = new StringBuilder();

            if (!((string)Session["user_id"]).Equals("Not login"))
            {
                libRSSreader.clsUserInfo TopUserInfo = new libRSSreader.clsUserInfo("", "", "");

                login_strBuilder.AppendLine(TopUserInfo.getTopInfoString((string)Session["user_id"]));
            }
            else
            {
                login_strBuilder.AppendLine("<a href=\"../Admin/login.aspx\" id=\"loginF\" name=\"loginF\">&nbsp;&nbsp;&nbsp;<img src=\"../img/event_pop_img4.gif\" alt=\"로그인\" height=\"30px\"></a>");
            }

            return login_strBuilder.ToString();
        }
    }
}
