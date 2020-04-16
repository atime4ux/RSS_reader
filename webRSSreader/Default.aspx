<%@ Page Language="C#" %>
<%
    string PGMID = "RSS001";

    if ((string)Session["user_id"] == "Not login")
    {
        Response.Redirect("../Admin/login.aspx");
    }        
    //로그인 및 할당된 페이지 검사
   // prcSession.prcSession(Response, Session, PGMID);
    

    libCommon.clsDB objDB = new libCommon.clsDB();
    libCommon.clsUtil objUtil = new libCommon.clsUtil();
    libCommon.clsWebFunc objWebFunc = new libCommon.clsWebFunc();

    libRSSreader.clsCmnDB objCmnDB = new libRSSreader.clsCmnDB();

    libRSSreader.clsRSS objRSS = new libRSSreader.clsRSS();
    
    System.Data.DataSet DS = new System.Data.DataSet();

    libRSSreader.clsFeed objFeed = new libRSSreader.clsFeed();

    System.Data.SqlClient.SqlConnection dbCon;

    string result = "";

    string NavigationString = "";

    int pageNum;
    int pageCnt;
    int totalCnt = 0;

    string login = (string)Session["user_id"];
    string user_id = (string)Session["user_id"];
    
    string goURL;
    string idx;
    
    //PGMID로 프로그램 이름 가져오기
    dbCon = objDB.GetConnection();
    //libRSSreader.clsProgInfo objProgInfo = new libRSSreader.clsProgInfo(dbCon, PGMID);
    dbCon.Close();

    //네비게이션 스트링
   // NavigationString = objUserInfo_navi.getNaviString(login_comp_id, login, PGMID);
    if (NavigationString.Equals("FAIL"))
    {
        NavigationString = "";
    }

    
    dbCon = objDB.GetConnection();
    
    dbCon.Close();


    goURL = objWebFunc.getRequest(Request, "goURL");
    idx = objWebFunc.getRequest(Request, "idx");

    if (objWebFunc.getRequest(Request, "pageNum").Trim().ToString().Length > 0)
    {
        pageNum = objUtil.ToInt32(objWebFunc.getRequest(Request, "pageNum"));
    }
    else
    {
        pageNum = 1;
    }

    if (objWebFunc.getRequest(Request, "pageCnt").Trim().ToString().Length > 0)
    {
        pageCnt = objUtil.ToInt32(objWebFunc.getRequest(Request, "pageCnt"));
    }
    else
    {
        pageCnt = 10;
    }    

    dbCon = objDB.GetConnection();

    if (goURL != "")
    {
        //idx설정
        
        //관심 항목 list보기
        if (goURL == "selectFavorItem")
        {
            DS = objRSS.selectFavorItem(user_id);
            objFeed = objRSS.TOobjFeed(DS);
        }
        //전체 보기
        else if (goURL == "ALL")
        {
            objFeed = objRSS.showAllFeed(user_id);

        }
        //구독 url
        else
        {

            objFeed = objRSS.getRSSfeed(idx,goURL);

        }
        
        objRSS.prcFavorItem(objFeed, user_id);
        objRSS.prcPastItem(objFeed, user_id);

        
    }

    dbCon.Close();

    result = objRSS.DS2Table(objFeed,user_id);

%>






<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>EvtFrame</title>
    
    <link type="text/css" href="../css/style.css" rel="stylesheet"/>
    <link href="../css/ui-lightness/jquery-ui-1.8.13.custom.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../js/jquery-1.5.1.min.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.8.13.custom.min.js"></script>
	<script type="text/javascript" src="../js/jquery.ui.datepicker-ko.js"></script>  
     
    <script type="text/javascript" src="../js/cmnfunc.js"></script>

    <script type="text/javascript" src="../js/<%=PGMID %>.js"></script>
    <script type="text/javascript" src="../js/login.js"></script>

    
    <script type="text/javascript">
        //페이지가 로딩 되는 시점에 코드가 수행되도록 함.
        $(document).ready(
        function () {

        });
    </script>
</head>


<body>
	<div id="wrap">
            <div id="top">
			     <!--#include file="../inc/top.aspx"-->
		    </div>
		    <div id="body">
                <div id="left">
			     <!--#include file="../inc/left_menu.aspx"-->
			    </div>
            <div id="right">
				<div id="navi">
				</div>
                    
                <form name="frmSrch" method="post" action="<%=PGMID %>.aspx">
                        <!-- 값 유지할 히든 항목들 -->
                    <input type="hidden" id="pageName" name="pageName" value="<%=PGMID %>.aspx" />
                    <input type="hidden" id="PGMID" name="PGMID" value="<%=PGMID %>" />
                    <input type="hidden" id="pageNum" name="pageNum" value="<%=pageNum %>" />
                    <input type="hidden" id="pageCnt" name="pageCnt" value="<%=pageCnt %>" />

                    <input type="hidden" id="FUNCNAME" name="FUNCNAME" value="" />
                        
                    <div id="result">
                            <%=result %>
                    </div>
                </form>

            </div>
            <!--right끝-->

	        </div><!--body끝-->

    </div>
    <div id="bottom">
	</div>
</body>
</html>