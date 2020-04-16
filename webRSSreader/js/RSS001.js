//체크 이벤트 일어날때마다 실행(관심항목 체크하기)
function favorite(title, url, j, date, user_id) {

    var PGMID = $("#PGMID").val();
    var Item_url = url;
    var tmp = url.split("/");
    var url = tmp[1] + tmp[2];
    var hiddenN = "#desc" + j;
    var desc = $(hiddenN).text();
    $("#goURL").val("")
    var goURL = $("#goURL").val();
    //desc = desc.text();
    

    var postData = 'PGMID=RSS001&FUNCNAME=favorite&title=' + encodeURIComponent(title) + "&url=" + encodeURIComponent(url) + "&desc=" + desc + "&date=" + encodeURIComponent(date) + "&user_id=" + user_id + "&Item_url=" + encodeURIComponent(Item_url);

    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        url: '../RSS/RSS001_prc.aspx',
        data: postData,
        success: function (data) {
            if (data.length == undefined) {
                data = "";
            }

            if (data == "SUCCESS") {
                document.location = "../RSS/RSS001.aspx?goURL=" + encodeURIComponent(goURL);
            }
        },
        error: function (data) {
            errProc("ERR : " + data);
        }
    });
    /*
    callAjax('../RSS/RSS001_prc.aspx'
                , postData
                , '../RSS/RSS001.aspx'
                , "실패했습니다.");
*/
}

//전체 보기
function go_AllList() {

   // var postData = getPostData();
    var PGMID = $("#PGMID").val();

    //document.location = "../RSS/" + PGMID + ".aspx?goURL=ALL&" + postData;
    document.location = "../RSS/RSS001.aspx";


}

//관심 항목 페이지 출력
function go_FavorPage() {

    //var postData = getPostData();
    var PGMID = $("#PGMID").val();

    //document.location = "../RSS/" + PGMID + ".aspx?goURL=selectFavorItem&" + postData;
    document.location = "../RSS/RSS001.aspx?goURL=selectFavorItem&";

}

//등록된 rss url별 page 출력
function go_ListPage(goURL, idx) {

    //var postData = getPostData();
    var PGMID = $("#PGMID").val();

    //document.location = "../RSS/" + PGMID + ".aspx?goURL=" + encodeURIComponent(goURL) + "&idx="+ idx + "&" + postData;
    document.location = "../RSS/RSS001.aspx?goURL=List" + idx + "&idx=" + idx;
}

function PastItem(url, date) {

    var PGMID = $("#PGMID").val();
    var Item_url = url;
    var tmp = url.split("/");
    var url = tmp[1] + tmp[2];
    var goURL = $("#goURL").val();

    var postData = 'PGMID=RSS001&FUNCNAME=PastItem&url=' + encodeURIComponent(url) + '&date=' + encodeURIComponent(date) + '&Item_url=' + encodeURIComponent(Item_url);

    open(Item_url, "", "");

    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        url: '../RSS/RSS001_prc.aspx',
        data: postData,
        success: function (data) {
            if (data.length == undefined) {
                data = "";
            }

            if (data == "SUCCESS") {
                //document.location = "../RSS/RSS001.aspx";
                document.location = "../RSS/RSS001.aspx?goURL=" + encodeURIComponent(goURL);

            }
        },
        error: function (data) {
            errProc("ERR : " + data);
        }
    });
    /*

    callAjax('../RSS/RSS001_prc.aspx'
                , postData
                , '../RSS/RSS001.aspx'
                , "실패했습니다.");
    */

}

//desc display
function toggle(j) {
    //alert($("#desc4").text());

    var descDisplay = "#descDisplay" + j;
    
    var flip = 0;
    
    //$(".desc").toggle(flip++ % 2 == 0);
    $(descDisplay).toggle();
    //$(date).toggle();
}

//테스트용
function cntItem() {
    var items;
    items = document.getElementsByName("check");
    alert(items.length);
}

//10개 기사 더보기
function selectMore10(moreCnt) {

    var goURL = $("#goURL").val();
    var idx = goURL.replace("List", "");

    set_scroll_position("result");

    document.location = "../RSS/RSS001.aspx?goURL=" + encodeURIComponent(goURL) + "&moreCnt=" + moreCnt + "&idx=" + idx;

}