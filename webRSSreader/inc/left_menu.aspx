<%
    string menu_state = "";

    string login_user_name = (string)Session["user_name"];
    string login_comp_name = (string)Session["comp_name"];
    string login_part_name = (string)Session["part_name"];

    libRSSreader.clsLeft objLeft = new libRSSreader.clsLeft();
    
    //로그인 했을때
    if((string)Session["user_id"] != "Not login")
    {
        dbCon = objDB.GetConnection();
        
        DS = objLeft.LeftMenu(dbCon, (string)Session["user_id"]);

        dbCon.Close();
        
        menu_state = objLeft.DS2Left(DS, (string)Session["user_id"]);
    }
    else{
        menu_state = "<a href='javascript:gologin();'>프로그램</a>";
    }
%>
<%=menu_state %>