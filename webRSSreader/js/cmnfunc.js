//공백과 null에 대한 유효성 검사
function isNull(input) {

    if (input == null | input == "") {
        return true;
    }
    return false;
}

//email
function isVaildEmail(input) {

    if (!input == "") {
        var format = /^[0-9a-zA-Z-_\.]*@[0-9a-zA-Z]([-_\.]?[0-9a-zA-Z])*\.[0-zA-Z]{2,3}$/i;
        return isValidFormat(input, format);
    }

}

//공백과 숫자에 대한 유효성 검사
function isNumber(input) {
    if (isNull(input) == false) {
        var chars = "0123456789";
        return containsCharsOnly(input, chars);
    }
    return true;
}



//핸드폰번호에 대한 유효성 검사
function isValidPhone(input) {
    var format = /^(\d+)-(\d+)-(\d+)$/;
    return isValidFormat(input, format);
}



//핸드폰번호에 대한 유효성 검사(null 검사도 포함)
function isValidPhone_u(input) {
    if (isNull(input) == false) {
        var format = /^(\d+)-(\d+)-(\d+)$/;
        return isValidFormat(input, format);
    }
    return true;
}




function isValidFormat(input, format) {
    if (isNull(input) == false) {

        if (input.search(format) != -1) {
        
            return true;
        }
        return false;
    }
    return false;
}




function containsCharsOnly(input, chars) {
    var inx;
    if (input.length == 10) {
        for (inx = 0; inx < input.length; inx++) {
            if (chars.indexOf(input.charAt(inx)) != -1) {
                return true;
            }
            return false;
        }
    }
    return false;
}



//pageNum click시 다음 페이지에 넘겨야 할 값을 얻어와 submit
function goPage(cPage, pageCnt) {
    var pageNum;
    var pageCnt;

    pageNum = cPage;
    pageCnt = pageCnt;

    document.getElementById("pageNum").value = pageNum;
    document.getElementById("pageCnt").value = pageCnt;

    document.frmSrch.submit();
}



function errProc(msg) {
    //에러를 핸들링한다.
    alert(msg);
}



//html의 input tag의 value값들을 가져오는 function
//ajax에서 사용할 postData 생성
function getPostData() {

    var postData = "";
    //alert($(':input').size());
    /*
    $(':input').each(function (index) {
        if (postData.length > 0) {
            postData += '&';
        }
        if ($(this).val() != null) {
            if ($(this).val().length > 0) {
                postData += $(this).attr('name') + '=' + encodeURIComponent($(this).val());
            }
        }
    }); 
    */

    $(':input').each(function (index) {
        if ($(this).val() != null) {
            if ($(this).val().length > 0) {
                postData += $(this).attr('name') + '=' + encodeURIComponent($(this).val());
                postData += '&';
            }
        }
    }); 
    return postData;
}




function callAjax(target_url, postData, success_url, fail_msg) {
    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        url: target_url,
        data: postData,
        success: function (data) {
            if (data == "SUCCESS") {
                document.location = success_url;
            }
            else {
                alert(fail_msg);
            }
        },
        error: function (data) {
            errProc("ERR : " + data);
        }
    });
}


//popup창에서 값 넘기기
function setPopVal(inputid, val) {
    opener.document.getElementById(inputid).value = val;
    self.close();
}

//체크박스 전체 클릭i
function checkbox(field) {
    if (field[0].checked == true) {
        for (i = 0; i < field.length; i++) {
            field[i].checked = true;
        }
    } else {
        for (i = 0; i < field.length; i++) {
            field[i].checked = false;
        }
    }
}



//enter키 이벤트
function enterPress(e, event_id) {
    //IE를 제외한 것들
    var keyF;
    var keyIE;
    var keyCR;
    
    /*
    alert("keycode : " + window.event.keyCode);
    alert("charCode : " + window.event.charCode);
    alert("e.which : " + e.which);
    */

    var elem = document.getElementById(event_id);
    if (window.event) {
        keyIE = window.event.keyCode;
        keyCR = window.event.charCode;
    }
    else {
        keyF = e.which;
    }

    if (keyF == 13 || keyCR == 13) {
        eventFirefox(event_id);
    }
    else if (keyIE == 13) {
        document.getElementById(event_id).click();
    }
}



function eventFirefox(event_id) {

    if (event_id == "comp_search") {
        return comp_search();
    }
    else if (event_id == "part_search") {
        return part_search();
    }
    else if (event_id == "user_search") {
        return user_search();
    }
    else if (event_id == "loginF") {
        return login();
    }
    else if (event_id == "mNum_search") {
        return mNum_search();
    }
    else if (event_id == "sNum_search") {
        return sNum_search();
    }
    else if (event_id == "goSearch") {
        return goSearch();
    }
    else if (event_id == "sNum_pop_search") {
        return gosNum_Search();
    }
    else if (event_id == "mNum_pop_search") {
        return gomNum_Search();
    }
    else if (event_id == "comp_pop_search") {
        return comp_pop_search();
    }
    else if (event_id == "part_pop_Search") {
        return part_pop_Search();
    }
    else if (event_id == "user_pop_search") {
        return user_pop_Search();
    }
}



function popupOpen(prcURL, openURL, field, name, width, height) {
    var winOpts = "toolbar=no, location=no, directories=no, status=no, menubar=no, width=" + width + "," + "height=" + height;
    if (name == "URL") {
        var postData = getPostData();
    } else
    {
        var postData = getPostData() + "&popupOpen=POPUPCOUNT";
    }
    var popupData;

    var f_action = field.action;
    var f_method = field.method;
    var f_target = field.target;

    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        url: prcURL,
        data: postData,
        success: function (data) {
            if (data.length == undefined) {
                data = "";
            }
            popupData = data.split("|");

            if (popupData[0] == "0") {
                alert("검색 결과가 없습니다.");
            }
            else if (popupData[0] == "1") {
                setOpenerField(data);
            }
            else {
                window.open("", name, winOpts);
                field.action = openURL;
                field.method = "post";
                field.target = name;
                field.submit();

                //부모창 값 원래대로
                field.action = f_action;
                field.method = f_method;
                field.target = f_target;
            }
        },
        error: function (data) {
            errProc("ERR : " + data);
        }
    });
}

function getCheckboxData(input1, input2, input3) {
    var arr_tmp = "";
    var checkboxData = "";
    var pageName = input1;
    var PGMID = input2;
    var input3Data = "";

    if (pageName == PGMID + ".aspx") {
        $("input:checkbox[name=check]:checked").each(function (i) {
            input3Data += $(this).val() + '|';
        });

    }
    else if (pageName == PGMID + "_edt.aspx") {
        input3Data = $("#" +input3).val();
    }

    postData = 'PGMID=' + PGMID + '&FUNCNAME=DELDATA&' + input3 + '=' + input3Data;

    return postData + "||" + input3Data;

}

function msg(input) {
    if (input == "del") {

        return "삭제 하시겠습니까??";

    }
}


//백스페이스 스킵
function skipBackSpace() {
    //alert("You pressed a following key : " + window.event.keyCode);

    // back-space 누를 때 
    if (window.event.keyCode == 8) {
        if (window.event.srcElement.readOnly || window.event.srcElement.disabled) {
            // readOnly나 disabled인 경우 작동하지 않도록
            alert("수정할 수 없는 값입니다.");
            window.event.returnValue = false;
            return;
        }
    }

    event.returnValue = true;
}

function showImgWin(what) {
    var imgwin = window.open("", 'WIN', 'scrollbars=no,status=no,toolbar=no,resizable=1,location=no, menu=no,width=10,height=10');
    imgwin.focus();
    imgwin.document.open();
    imgwin.document.write("<!DOCTYPE HTML PUBLIC '-//W3C//DTD HTML 4.01 Transitional//EN'>\n");
    imgwin.document.write("<html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' />\n"); // euc-kr? utf-8?
    imgwin.document.write("<title>크게보기</title>\n");   // 새창으로 페이지 제목
    imgwin.document.write("<sc" + "ript>\n");
    imgwin.document.write("function resize() {\n");
    imgwin.document.write("pic = document.il;\n");
    //imgwin.document.write("alert(eval(pic).height);\n"); 
    imgwin.document.write("if (eval(pic).height) { var name = navigator.appName\n");
    imgwin.document.write("  if (name == 'Microsoft Internet Explorer') { myHeight = eval(pic).height + 60;  myWidth = eval(pic).width + 10;\n");
    imgwin.document.write("  } else { myHeight = eval(pic).height + 56; myWidth = eval(pic).width + 8; }\n");
    imgwin.document.write("  //clearTimeout();\n");
    imgwin.document.write("  var height = screen.height;\n");
    imgwin.document.write("  var width = screen.width;\n");
    imgwin.document.write("  var leftpos = width / 2 - myWidth / 2;\n"); //hjhj
    imgwin.document.write("  var toppos = height / 2 - myHeight / 2; \n");
    imgwin.document.write("  self.moveTo(leftpos, toppos);\n");
    imgwin.document.write("  self.resizeTo(myWidth, myHeight);\n");
    imgwin.document.write("}else setTimeOut(resize(), 100);}\n");
    imgwin.document.write("</sc" + "ript>\n");
    imgwin.document.write("</head><body style='margin:0px;padding:0px'>\n");
    imgwin.document.write("<img border=0 src=" + what + " xwidth='100' xheight='9' name='il' onload='resize();' alt='이미지를 클릭하시면 창이 닫힙니다.' title='이미지를 클릭하시면 창이 닫힙니다.' onclick='javascript:window.close()' />\n");
    imgwin.document.write("</body></html>\n");
    imgwin.document.close();
}
function go_Excel(option) {

    var PGMID = $("#PGMID").val();
    var comp_id = $("#comp_id").val();
    var user_id = $("#user_id").val();

    var folder = PGMID.substring(0, 3);


    if (folder == "CMN") {

        document.location = "../common/" + PGMID + ".aspx?EXCEL=" + option + "&" + getPostData();

    } else if (folder == "MEM") {
        if (PGMID == "MEM004") {
            document.location = "../account/" + PGMID + "_edt.aspx?EXCEL=" + option + "&fcomp_id=" + comp_id + "&user_id=" + user_id;
        } else if (PGMID == "MEM005") {

            document.location = "../account/" + PGMID + ".aspx?EXCEL=" + option + "&fcomp_id=" + comp_id;
        }
        else {

            document.location = "../account/" + PGMID + ".aspx?EXCEL=" + option + "&" + getPostData();
        }

    } else if (folder == "MOM") {

        document.location = "../MO/" + PGMID + ".aspx?EXCEL=" + option + "&" + getPostData();

    } else if (folder == "MTM") {

        document.location = "../MTM/" + PGMID + ".aspx?EXCEL=" + option + "&" + getPostData();

    } else if (folder == "MTS") {

        document.location = "../MTS/" + PGMID + ".aspx?EXCEL=" + option + "&" + getPostData();

    }
    else if (folder == "EVT") {

        document.location = "../EVT/" + PGMID + ".aspx?EXCEL=" + option + "&" + getPostData();
    }

    else if (folder == "STA") {

        document.location = "../STA/" + PGMID + ".aspx?EXCEL=" + option + "&" + getPostData();
    }
}

//FAQ 숨김/보이기
function change_disp(id) {

    var change_obj = document.getElementById(id);
    var displayNoneID = "";

    displayNoneID =  $("#display").val();

    if (change_obj.style.display == "none") {

        change_obj.style.display = "block";
        if (displayNoneID != "" && displayNoneID != undefined) {
            document.getElementById(displayNoneID).style.display = "none";
        }
        //$("#" + displayNoneID).css("display", "none");

    } else {

        change_obj.style.display = "none";
    }

    $("#display").val(id);


}

/////////////////////////툴팁용 js//////////////////////////////////////////////////
function hideN(L) {
    if (document.all) {
        var a = "D" + L; // Ex) id : D + 1 => D1
        document.getElementById(a).style.visibility = "hidden";

        //document.all[a].style.visibility = "hidden";  Exploror만 호환되는 문장(all)    }
    } else {
        var a = "D" + L; // Ex) id : D + 1 => D1
        document.getElementById(a).style.visibility = "hidden";
    }
}

function showN(L) {

    var uAgent = navigator.userAgent.toLowerCase();
    /*
    var mobilePhones = new Array('iphone', 'ipod', 'android', 'blackberry', 'windows ce',
    'nokia', 'webos', 'opera mini', 'sonyericsson', 'opera mobi', 'iemobile');
    */
    if (uAgent.indexOf("iphone") == -1 || uAgent.indexOf("android") == -1) {
        if (document.all) {
            var a = "D" + L;
            document.getElementById(a).style.visibility = "visible";

            //document.all[a].style.visibility = "visible";  Exploror만 호환되는 문장(all)
        } else {
            var a = "D" + L;
            document.getElementById(a).style.visibility = "visible";
        }
    }
}

function find_Obj(L, i, e) {

    var uAgent = navigator.userAgent.toLowerCase();
    /*
    var mobilePhones = new Array('iphone', 'ipod', 'android', 'blackberry', 'windows ce',
    'nokia', 'webos', 'opera mini', 'sonyericsson', 'opera mobi', 'iemobile');
    */

    if (uAgent.indexOf("iphone") == -1 || uAgent.indexOf("android") == -1) {

        var i;
        var j = 0;
        var a = "D" + L;
        var barron = document.getElementById(a); // document.all[a];
        var MSG = $("#MSG" + i).val();
        var MSG1 = "";
        var msgLength;

        if (document.all) {

            barron.style.pixelLeft = event.clientX + document.body.scrollLeft - 10; //event 객체를 사용하여 뜨게될 좌표 위치를 setting
            barron.style.pixelTop = event.clientY + document.body.scrollTop + 3;

        } else {
            barron.style.left = e.pageX + "px";
            barron.style.top = e.pageY + "px";
        }

        document.getElementById("STM").innerHTML = MSG; // 위에서 설정한 span의 id인 STM을 호출
        barron.style.visibility = "visible"; // 보이게 속성을 변경
        //barron.style.width = "800px";

    }
}

//left menu click
function go_ListPage() {

}


//스크롤 위치 기억
function set_scroll_position(element) {
    if (element.length == 0) {
        $(window).scroll(function () {
            $.cookie('scroll_loc', $(this).scrollTop());
        });
    }
    else {
            $.cookie('scroll_loc', $("#" + element).scrollTop());
    }
}

//스크롤 위치 불러오기
function get_scroll_position(element) {
    if (element.length == 0) {
        if ($.cookie('scroll_loc')) {
            $(window).scrollTop($.cookie('scroll_loc'));
        }
    }
    else {
        if ($.cookie('scroll_loc')) {
            $("#"+element).scrollTop($.cookie('scroll_loc'));
        }
    }
}