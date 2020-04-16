<%@ Page Language="C#"%>
<%
    string PGMID = "LOGIN";
%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
	<head>
        <!--<meta http-equiv="Content-Type" content="text/html; charset=euc-kr" />-->
		<title>로그인</title>
		<link href="../css/style.css" type="text/css" rel="stylesheet"/>

        <link href="../css/ui-lightness/jquery-ui-1.8.13.custom.css" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="../js/jquery-1.5.1.min.js"></script>
        <script type="text/javascript" src="../js/jquery-ui-1.8.13.custom.min.js"></script>
        <script type="text/javascript" src="../js/jquery.ui.datepicker-ko.js"></script>  

        <script type="text/javascript" src="../js/login.js"></script>
        <script type="text/javascript" src="../js/cmnfunc.js"></script>

	</head>
	
    <body>
	    <div id="wrap">
            <div class="loginTop">
		    </div>

		    <div id="body">
                <div id="left" style="background-color:White">
			    </div>
                <div id="right">
            <table style="width:500px;"  >
                <tr>
                <td class="loginSection">
                    <form name="frmLogin" method="post" action="javascript:login();" style="width:190px; float:right">
                        <span class="loginTitle">회원 로그인</span>
                        <br /><br /><br /><br />

                        <input type="hidden" name="PGMID" id="PGMID" value="<%=PGMID %>" />
                        <input type="hidden" name="FUNCNAME" id="FUNCNAME" value="LOGIN" />
                        아이디&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type="text" id="fuser_id" name="fuser_id" style="width:120px"  onkeypress="enterPress(event,'loginF')" value="" /><br /><br />
                        비밀번호&nbsp;&nbsp;<input type="password" id="fuser_pass" name="fuser_pass" style="width:120px"  onkeypress="enterPress(event,'loginF')" value="" /><br /><br />
                        <a href="./join.aspx" class="loginF">회원가입</a>&nbsp;&nbsp;
                        <!--<img src="../img/event_pop_img4.gif" alt="로그인" height="30px"  />-->
                        <a href="javascript:login();" id="loginF" name="loginF" >로그인</a>&nbsp;&nbsp;
                    </form>
                </td>
                </tr>
			</table>
                </div>
            </div>
            <div class="loginBottom">
            </div>
        </div>
	</body>
</html>
					
		            