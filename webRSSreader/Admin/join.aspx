<%@ Page Language="C#"%>
<%
    //페이지간 데이터 얻기 위한 lib
    libCommon.clsWebFunc objWebFunc = new libCommon.clsWebFunc();
    
    string PGMID = "JOIN";
    
%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
		<meta http-equiv="Content-Type" content="text/html; charset=euc-kr" />
		<title>whatEat</title>
		<link href="../css/style.css" type="text/css" rel="stylesheet"/>
        
    <script type="text/javascript" src="../js/jquery-1.4.1-vsdoc.js"></script>
    <script type="text/javascript" src="../js/jquery-1.5.1.min.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.8.13.custom.min.js"></script>
    <script type="text/javascript" src="../js/jquery.ui.datepicker-ko.js"></script>   

     
        <script type="text/javascript" src="../js/cmnfunc.js"> </script>
        <script type="text/javascript" src="../js/join.js"> </script>
        <script type="text/javascript" src="../js/jquery.form.js"> </script>
	</head>
    <body>

        <div id="wrap">
	        <div id="top">
		    </div>
			
		    <div id="body">
		        <div id="right">
                    <div id="titleN">
                        회원가입
                    </div>

                    <div id="joinT">
                        <form name = "m_join" method = "post" action = "../Admin/join.aspx" >
                           <input type="hidden" name="PGMID" id="PGMID" value="<%=PGMID %>" />
                           <input type="hidden" name="FUNCNAME" id="FUNCNAME" value="MEMBERJOIN" />

                            <table>
                                <tr>
                                    <td class="l_align"><b>*ID</b></td>
                                    <td class="l_align"><input type="text" id="fuser_id" name="fuser_id" /></td>
                                </tr>
                                <tr>
                                    <td class="l_align"><b>*EMAIL</b></td>
                                    <td class="l_align"><input type="text" id="fuser_email" name="fuser_email" />&nbsp;&nbsp;이메일 형식으로 입력하세요</td>
                                </tr>
                                <tr>
                                    <td class="l_align"><b>*PASSWORD</b></td>
                                    <td class="l_align"><input type="password" id="fuser_pass" name="fuser_pass" />&nbsp;&nbsp;8~12 글자로 입력하세요</td>
                                </tr>
                                <tr>
                                    <td class="l_align"><b>*PASSWORD</b></td>
                                    <td class="l_align"><input type="password" id="fuser_pass_R" name="fuser_pass_R" />&nbsp;&nbsp;패스워드를 한 번더 입력하세요.</td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td class="l_align"><b>*는 필수 입력사항입니다.</b></td>
                                </tr>
                            </table>
                              
                            <input type="button" id="send" name="sendB" class="c_align" value="가입" onclick="javascript:joinMember();" />
            
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </body>
</html>
