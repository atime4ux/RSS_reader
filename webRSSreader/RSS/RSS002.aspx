<%@ Page Language="C#" %>
<%
    string PGMID = "RSS002";

    if ((string)Session["user_id"] == "Not login")
    {
        Response.Redirect("../Admin/login.aspx");
    }     
        
    libCommon.clsDB objDB = new libCommon.clsDB();
    libCommon.clsUtil objUtil = new libCommon.clsUtil();
    libCommon.clsWebFunc objWebFunc = new libCommon.clsWebFunc();

    libRSSreader.clsRSS objRSS = new libRSSreader.clsRSS();
    libRSSreader.clsCmnDB objCmnDB = new libRSSreader.clsCmnDB();
    libRSSreader.clsUser objUser = new libRSSreader.clsUser();
    libRSSreader.clsPaging objPaging = new libRSSreader.clsPaging();
    
    System.Data.DataSet DS = new System.Data.DataSet();

    System.Data.SqlClient.SqlConnection dbCon;

    libRSSreader.clsFeed objFeed = new libRSSreader.clsFeed();
   

    string result = "";

    string NavigationString;

    int pageNum;
    int pageCnt;
    int totalCnt = 0;
    string pageString = "";

    string login = (string)Session["user_id"];
    string user_id = (string)Session["user_id"];

    /*
    //PGMID로 프로그램 이름 가져오기
    dbCon = objDB.GetConnection();
    libRSSreader.clsProgInfo objProgInfo = new libRSSreader.clsProgInfo(dbCon, PGMID);
    dbCon.Close();

    //네비게이션 스트링
    NavigationString = objUserInfo_navi.getNaviString(login_comp_id, login, PGMID);
    if (NavigationString.Equals("FAIL"))
    {
        NavigationString = "";
    }

    */
    dbCon = objDB.GetConnection();
    
    dbCon.Close();
        

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

    
    //전체 리스트 생성    
    objFeed = objRSS.showAllFeed(user_id);

    //추가 by 이현석(2011-07-27)
    //탑, 레프트 메뉴 스트링 생성
    libRSSreader.clsLeft objLeft = new libRSSreader.clsLeft();
    libRSSreader.clsTop objTop = new libRSSreader.clsTop();
    string TopMenuString = "";
    string LeftMenuString = "";
    TopMenuString = objTop.TopMenuString(Session);
    LeftMenuString = objLeft.LeftMenuString(Session, objFeed);


    DS = objUser.userRSSList(dbCon, user_id, 0, 0, totalCnt);
    totalCnt = objUtil.ToInt32(DS.Tables[0].Rows[0][0].ToString());
    DS = objUser.userRSSList(dbCon, user_id, pageNum, pageCnt, 0);

    dbCon.Close();

    if (totalCnt != 0)
    {
        objPaging.cutDS(DS, pageNum, pageCnt);
        pageString = objPaging.getPageString(pageNum, totalCnt, pageCnt);
       
        result = objUser.RSSList_DS2Table(DS, pageNum, pageCnt);
    }
    
%>






<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>RSS Reader</title>
    
    <link type="text/css" href="../css/style.css" rel="stylesheet"/>
    <link href="../css/ui-lightness/jquery-ui-1.8.13.custom.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../js/jquery-1.5.1.min.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.8.13.custom.min.js"></script>
	<script type="text/javascript" src="../js/jquery.ui.datepicker-ko.js"></script>  
     
    <script type="text/javascript" src="../js/cmnfunc.js"></script>
    <script type="text/javascript" src="../js/<%=PGMID %>.js"></script>
    <script type="text/javascript" src="../js/RSS001.js"></script>
    <script type="text/javascript" src="../js/login.js"></script>

    
    <script type="text/javascript">
        //페이지가 로딩 되는 시점에 코드가 수행되도록 함.
        $(document).ready(
        function () {
            document.getElementById("manage").style.fontWeight = "bold";
        });
    </script>
</head>


<body>
	<div id="wrap">
            <div id="top">
                <div id="loginInfo">
                    <%=TopMenuString %>
                </div>
		    </div>
		    <div id="body">
                <div id="left">
                    <%=LeftMenuString %>
			    </div>

            <div id="right">
                <form name="frmSrch" method="post" action="<%=PGMID %>.aspx">
                        <!-- 값 유지할 히든 항목들 -->
                    <input type="hidden" id="pageName" name="pageName" value="<%=PGMID %>.aspx" />
                    <input type="hidden" id="PGMID" name="PGMID" value="<%=PGMID %>" />
                    <input type="hidden" id="pageNum" name="pageNum" value="<%=pageNum %>" />
                    <input type="hidden" id="pageCnt" name="pageCnt" value="<%=pageCnt %>" />

                    <input type="hidden" id="FUNCNAME" name="FUNCNAME" value="" />
                        
                    <input type="text" id="URL" name="URL" value="" style="width:400px; margin-left:5px" />
                    <input type="button" onclick="javascript:addURL();" style="height:20px" value="RSS URL 추가" />
                    <input type="button" onclick="javascript:delRSSList('');" style="height:20px" value="구독 취소" />
                    <div id="result">
                            <%=result %>
                        <div id="page">
                            <%=pageString %>
                        </div>

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