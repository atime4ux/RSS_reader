<%@ Page Language="C#" %>

<%
    libCommon.clsWebFunc objWebFunc = new libCommon.clsWebFunc();
    libCommon.clsDB objDB = new libCommon.clsDB();
    libCommon.clsUtil objUtil = new libCommon.clsUtil();

    
    libRSSreader.clsCmnDB objCmnDB = new libRSSreader.clsCmnDB();
    libRSSreader.clsUserInfo objUserInfo = new libRSSreader.clsUserInfo("","","");
    
    System.Data.SqlClient.SqlConnection dbCon;

    System.Data.DataSet DS = new System.Data.DataSet();
    
    string PGMID;
    string FUNCNAME;

    string user_id;
    string user_pass;
    
    string Result = "FAIL";
    
    string[] SessionInfo;
    
    PGMID = objWebFunc.getRequest(Request, "PGMID");
    FUNCNAME = objWebFunc.getRequest(Request, "FUNCNAME");
   
    user_id = objWebFunc.getRequest(Request, "user_id");
    user_pass = objWebFunc.getRequest(Request, "user_pass");
    

    objUtil.writeLog("ACC_PROC : " + PGMID + ":" + FUNCNAME);

    if (FUNCNAME.Equals("LOGIN"))
    {
        libRSSreader.clsUser objUser = new libRSSreader.clsUser();

        dbCon = objDB.GetConnection();

        Result = objUser.userAuth(dbCon, user_id, user_pass);

        dbCon.Close();

        SessionInfo = objUtil.Split(Result, "|");

        if (SessionInfo[0].Equals("SUCCESS"))
        {
            Result = "SUCCESS";

            Session["user_id"] = user_id;
            
        }

        Response.Clear();
        Response.Write(Result);
        Response.End();
    }

    //로그아웃
    if (FUNCNAME.Equals("LOGOUT"))
    {
        Session.Clear();
        Session.Abandon();

        if (Session.Count == 0)
        {
            Result = "SUCCESS";
        }
        else
        {
            Result = "FAIL";
        }

        Response.Clear();
        Response.Write(Result);
        Response.End();
    }

    Response.Clear();
    Response.Write(Result);
    Response.End();
%>