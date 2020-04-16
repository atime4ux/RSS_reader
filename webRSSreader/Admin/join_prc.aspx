<%@ Page Language="C#" %>

<%
    libCommon.clsWebFunc objWebFunc = new libCommon.clsWebFunc();
    libCommon.clsDB objDB = new libCommon.clsDB();
    libCommon.clsUtil objUtil = new libCommon.clsUtil();

    libRSSreader.clsCmnDB objCmnDB = new libRSSreader.clsCmnDB();
    libRSSreader.clsUser objUser = new libRSSreader.clsUser();
    libRSSreader.clsUserInfo objUserInfo = new libRSSreader.clsUserInfo();
    
    
    System.Data.SqlClient.SqlConnection dbCon;

    System.Data.DataSet DS = new System.Data.DataSet();
    
    string PGMID;
    string FUNCNAME;

    string user_id;
    string user_email;
    string user_pass;
    
    string Result = "FAIL";
    
    string[] SessionInfo;
    
    PGMID = objWebFunc.getRequest(Request, "PGMID");
    FUNCNAME = objWebFunc.getRequest(Request, "FUNCNAME");
   
    user_id = objWebFunc.getRequest(Request, "user_id");
    user_email = objWebFunc.getRequest(Request, "user_email");
    user_pass = objWebFunc.getRequest(Request, "user_pass");

    objUserInfo.user_id = user_id;
    objUserInfo.email = user_email;
    objUserInfo.passwd = user_pass;
    

    objUtil.writeLog("ACC_PROC : " + PGMID + ":" + FUNCNAME);

    if (FUNCNAME.Equals("USERJOIN"))
    {
        dbCon = objDB.GetConnection();

        Result = objUser.insertUser(dbCon, objUserInfo, (string)Session["user_id"]);

        dbCon.Close();

        SessionInfo = objUtil.Split(Result, "|");

        if (SessionInfo[0].Equals("1"))
        {
            Result = "SUCCESS";
            
            dbCon.Close();
            
        }

        Response.Clear();
        Response.Write(Result);
        Response.End();
    }


%>