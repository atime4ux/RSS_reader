function joinMember() {

    var user_id = $("#fuser_id").val();
    var user_email = $("#fuser_email").val();
    var user_pass = $("#fuser_pass").val();
    var confirm_passsworkd = $("#fuser_pass_R").val();

    //validate
    /*
    $("#m_join").vaildate({
        rules: {
            user_id: "required",

            user_pass: {
                required: true,
                minlength: 8
            },

            confirm_passsworkd: {
                required: true,
                equalTo: "#user_pass"
            }
        }

    });
    */
    if (isNull(user_id) == true) {
        alert("이름을 입력해 주세요. 필수항목 입니다.");
        m_join.fuser_id.focus();
        return false;
    }
    if (isNull(user_email) == true) {
        alert("이메일을 입력해 주세요. 필수항목 입니다.");
        m_join.fuser_email.focus();
        return false;
    }
    if (isNull(user_pass) == true) {
        alert("password를 입력해 주세요. 필수항목 입니다.");
        m_join.fuser_pass.focus();
        return false;
    }
    if (isNull(confirm_passsworkd) == true) {
        alert("password를 확인해 주세요. 필수항목 입니다.");
        m_join.fuser_pass_R.focus();
        return false;
    }

    
    if (isVaildEmail(user_email) == false) {
        alert("email형식으로 넣어주세요");
        m_join.fuser_email.focus();
        return false;
    }
   

    if (user_pass != confirm_passsworkd) {
        alert("password를 확인해 주세요.");
        m_join.fuser_pass_R.focus();
        return false;
    }



    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        url: './join_prc.aspx',
        data: 'PGMID=JOIN&FUNCNAME=USERJOIN&user_email= ' + user_email + '&user_pass= ' + user_pass + '&user_id= ' + user_id,
        success: function (data) {

            if (data == "SUCCESS") {
                alert("가입완료되었습니다. 로그인 해 주세요");
                document.location = "../Admin/login.aspx";

            }
        },
        error: function (data) {
            errProc("ERR : " + data);
        }
    });

}