// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var lastPageUrl;

window.onbeforeunload = function () {
    var scrollPosition;
    if (typeof window.pageYOffset != 'undefined') {
        scrollPosition = window.pageYOffset;
    }
    else if (typeof document.compatMode != 'undefined' && document.compatMode != 'BackCompat') {
        scrollPosition = document.documentElement.scrollTop;
    }
    else if (typeof document.body != 'undefined') {
        scrollPosition = document.body.scrollTop;
    }
    document.cookie = "scrollTopPos=" + scrollPosition;
    document.cookie = "lastPageUrl=" + window.location;
}
window.onload = function () {
    lastPageUrl = document.cookie.match(/lastPageUrl=([^;]+)(;|$)/);
    document.cookie = "lastPageUrlFromReload=" + lastPageUrl[1];
    document.cookie = "windowLocation=" + window.location;
    if (lastPageUrl[1] == window.location) {
        console.log("lastPageUrl[1] == window.location is true");
        var arr = document.cookie.match(/scrollTopPos=([^;]+)(;|$)/);
        if (arr != null) {
            console.log("arr: " + arr);
            console.log("document.cookie: " + document.cookie);
            console.log("window.location: " + window.location);
            document.documentElement.scrollTop = parseInt(arr[1]);
            document.body.scrollTop = parseInt(arr[1]);
        }
    }
}