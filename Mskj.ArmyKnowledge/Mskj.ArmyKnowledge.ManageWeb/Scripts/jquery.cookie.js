function setCookie(key, val, time,path) {
    var date = new Date();    var expiresDays = time;    date.setTime(date.getTime() + expiresDays * 24 * 3600 * 1000);    document.cookie = key + "=" + val + ";expires=" + date.toGMTString() + ";path=" + path;
}

function getCookie(key) {
    var getCookie = document.cookie.replace(/[ ]/g, "");  //��ȡcookie�����ҽ���õ�cookie��ʽ����ȥ���ո��ַ�    var arrCookie = getCookie.split(";")    var tips;    for (var i = 0; i < arrCookie.length; i++) {
        var arr = arrCookie[i].split("=");        if (key == arr[0]) {
            tips = arr[1];            break;
        }
    }    return tips;
}

function delCookie(key,path) {
    this.setCookie(key, '', -1,path);
}
