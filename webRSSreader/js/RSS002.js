function addURL() {
    var RSS_url = $("#URL").val();

    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        url: '../RSS/RSS002_prc.aspx',
        data: 'FUNCNAME=RSSsite&RSS_url=' + encodeURIComponent(RSS_url),
        success: function (data) {
            if (data.length == undefined) {
                data = "";
            }

            if (data != "FAIL") {
                alert("추가되었습니다");
                document.location = "../RSS/RSS002.aspx";
            }
            else {
                alert("유효한 경로가 아닙니다.");
            }
        },
        error: function (data) {
            errProc("ERR : " + data);
        }
    });
}


function delRSSList(RSS_url) {

    var message = "구독취소 하시겠습니까??";
    var PGMID = $("#PGMID").val();
    var postData = getPostData();

    var RSS_url;

    if (RSS_url == "") {

        $("input:checkbox[name=check]:checked").each(function (i) {
            RSS_url += $(this).val() + "|";
        })
    }


    //pageName이 MEM001.aspx이면서 자료를 선택하지 않았으면 alert
    if (RSS_url.length == 0) {
        alert("삭제할 자료를 선택하세요.");
    }
    else {
        if (confirm(message)) {

            callAjax("../RSS/" + PGMID + "_prc.aspx?PGMID=" + PGMID + "&FUNCNAME=DELDATA" + "&RSS_url=" + encodeURIComponent(RSS_url)
            , postData
            , "../RSS/" + PGMID + ".aspx?" + postData
            , "삭제 실패했습니다.");


        } else{
            for (i = 0; i < document.frmSrch.check.length; i++) {
                document.frmSrch.check[i].checked = false;
            }
        }
    }
}

