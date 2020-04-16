function login() {
    var user_id = $("#fuser_id").val();
    var user_email = $("#fuser_email").val();
    var user_pass = $("#fuser_pass").val();
    var result;

    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        url: '../Admin/login_prc.aspx',
        data: 'FUNCNAME=LOGIN&user_id=' + user_id + '&user_email=' + user_email + '&user_pass=' + user_pass,
        success: function (data) {
            if (data.length == undefined) {
                data = "";
            }

            if (data == "SUCCESS") {
                document.location = "../RSS/RSS001.aspx";
            }
            else {
                alert("ID와 패스워드를 확인하세요");
                $("#fuser_id").val("");
                $("#fuser_pass").val("");
                frmLogin.fuser_id.focus();
            }
        },
        error: function (data) {
            errProc("ERR : " + data);
        }
    });
}

function logout() {


    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        url: '../Admin/login_prc.aspx',
        data: 'FUNCNAME=LOGOUT',
        success: function (data) {
            if (data == "SUCCESS") {

                document.location = "../Admin/login.aspx?";

            }
            else {
                alert("로그아웃 실패");
            }
        },
        error: function (data) {
            errProc("ERR : " + data);
        }
    });
}

//메누를 눌렀을때 로그인이 안된 상태
function gologin() {

    var message = "login 후 이용 가능한 page입니다. \nlogin하시 겠습니까?";
    
    if(confirm(message)){

        document.location = "../Admin/login.aspx";
    }

}

