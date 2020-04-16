<%@ Page Language="C#" %>

<%
    libCommon.clsWebFunc objWebFunc = new libCommon.clsWebFunc();
    libCommon.clsDB objDB = new libCommon.clsDB();
    libCommon.clsUtil objUtil = new libCommon.clsUtil();

    
    libRSSreader.clsCmnDB objCmnDB = new libRSSreader.clsCmnDB();
    libRSSreader.clsRSSinfo objRSSInfo = new libRSSreader.clsRSSinfo();
    
    System.Data.SqlClient.SqlConnection dbCon;

    System.Data.DataSet DS = new System.Data.DataSet();

    System.Data.SqlClient.SqlTransaction TRX;
   
    
    string PGMID;
    string FUNCNAME;

    string RSS_url;
    string idx;
        
    string Result = "FAIL";
    
    string[] R_url;
    int i;
   
    PGMID = objWebFunc.getRequest(Request, "PGMID");
    FUNCNAME = objWebFunc.getRequest(Request, "FUNCNAME");
   
    RSS_url = objWebFunc.getRequest(Request, "RSS_url");
    idx = objWebFunc.getRequest(Request, "idx");
    
    objUtil.writeLog("ACC_PROC : " + PGMID + ":" + FUNCNAME);

    if (FUNCNAME.Equals("RSSsite"))
    {
        libRSSreader.clsUser objUser = new libRSSreader.clsUser();

        dbCon = objDB.GetConnection();

        Result = objRSSInfo.insertRSSsite(RSS_url, (string)Session["user_id"]);

        dbCon.Close();

        Response.Clear();
        Response.Write(Result);
        Response.End();
    }
    else if (FUNCNAME.Equals("DELDATA"))
    {

        libRSSreader.clsUser objUser = new libRSSreader.clsUser();
        
        R_url = objUtil.Split(RSS_url, "|");

        dbCon = objDB.GetConnection();

        TRX = dbCon.BeginTransaction();

        for (i = 0; i < R_url.Length; i++)
        {
            objRSSInfo.deleteRSSsite(dbCon, TRX, (string)Session["user_id"], R_url[i]);
        }

        Result = "SUCCESS";
        
        if (Result.Equals("FAIL"))
        {
            TRX.Rollback();
            objUtil.writeLog(string.Format("FAIL deleteRSSsite HISTORY : {0}-{1}({2})", (string)Session["user_id"], RSS_url));
        }
        else
        {
            TRX.Commit();
        }        
        
        
        dbCon.Close();

        Response.Clear();
        Response.Write(Result);
        Response.End();
    }

    Response.Clear();
    Response.Write(Result);
    Response.End();
%>