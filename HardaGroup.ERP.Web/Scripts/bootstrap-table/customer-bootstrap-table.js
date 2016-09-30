$(function () {

    $('#collapseSearch').on('show.bs.collapse', function () {
        // 执行一些动作...
        $(".search-collapse >i").removeClass("fa-chevron-up");
        $(".search-collapse >i").addClass("fa-chevron-down");

    });
    $('#collapseSearch').on('hide.bs.collapse', function () {
        // 执行一些动作...
        $(".search-collapse >i").removeClass("fa-chevron-down");
        $(".search-collapse >i").addClass("fa-chevron-up");
    });

    $(".search-empty").click(function () {
        $("#collapseSearch input[type='text']").val('');
    });

});