//$(function () {
//    $('[data-toggle="popover"]').popover()
//})

$(document).ready(function () {
    $("[data-toggle='popover']").popover();
});

$(document).ready(function () {
    $('ul.nav > li').click(function (e) {
        e.preventDefault();
        $('ul.nav > li').removeClass('active');
        $(this).addClass('active');
    });
});