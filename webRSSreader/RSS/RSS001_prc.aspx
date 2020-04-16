<%@ Page Language="C#" %>

<%
    libCommon.clsWebFunc objWebFunc = new libCommon.clsWebFunc();
    libCommon.clsDB objDB = new libCommon.clsDB();
    libCommon.clsUtil objUtil = new libCommon.clsUtil();

    
    libRSSreader.clsCmnDB objCmnDB = new libRSSreader.clsCmnDB();
    libRSSreader.clsRSS objRSS = new libRSSreader.clsRSS();
    
    System.Data.SqlClient.SqlConnection dbCon;

    System.Data.DataSet DS = new System.Data.DataSet();

    System.Data.SqlClient.SqlTransaction TRX;
    
    
    string PGMID;
    string FUNCNAME;

    string RSS_idx;
    string Item_title;
    string Item_url;
    string url;
    string Item_desc;
    string Item_date;
    string user_id;
        
    string Result = "FAIL";

    string flag = "";
   
    PGMID = objWebFunc.getRequest(Request, "PGMID");
    FUNCNAME = objWebFunc.getRequest(Request, "FUNCNAME");

    RSS_idx = objWebFunc.getRequest(Request, "idx");
    Item_title = objWebFunc.getRequest(Request, "title");
    //idx 찾기용 url
    url = objWebFunc.getRequest(Request, "url");
    //진짜 url
    Item_url = objWebFunc.getRequest(Request, "Item_url");
    Item_desc = objWebFunc.getRequest(Request, "desc");
    Item_date = objWebFunc.getRequest(Request, "date");
    user_id = objWebFunc.getRequest(Request, "user_id");
    
    objUtil.writeLog("ACC_PROC : " + PGMID + ":" + FUNCNAME);

    if (FUNCNAME.Equals("favorite"))
    {
        libRSSreader.clsUser objUser = new libRSSreader.clsUser();

        dbCon = objDB.GetConnection();
        TRX = dbCon.BeginTransaction();
        
        flag = objRSS.isDupeItem(dbCon, TRX, user_id, Item_url, "tb_favorite");

        //관심항목 추가
        if (flag.Equals("OK"))
        {

            RSS_idx = objRSS.getRSS_Idx(user_id, url);
            Result = objRSS.insertFavorItem(RSS_idx, Item_title, Item_url, Item_desc, Item_date, (string)Session["user_id"]);
        }
        //관심항목 삭제
        else if (flag.Equals("DUPE"))
        {
            Result = objRSS.toggleFavorState(dbCon, TRX, Item_url, (string)Session["user_id"], "");
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

        Response.Clear();
        Response.Write(Result);
        Response.End();
    }
    else if (FUNCNAME.Equals("PastItem"))
    {
        libRSSreader.clsUser objUser = new libRSSreader.clsUser();

        dbCon = objDB.GetConnection();

        RSS_idx = objRSS.getRSS_Idx((string)Session["user_id"], url);
        Result = objRSS.insertPastItem(RSS_idx, Item_url, Item_date, (string)Session["user_id"]);

        dbCon.Close();

        Response.Clear();
        Response.Write(Result);
        Response.End();

    }

    Response.Clear();
    Response.Write(Result);
    Response.End();
%>