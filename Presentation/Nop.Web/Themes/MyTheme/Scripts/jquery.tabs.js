(function ($) {
    $.extend($.fn, {
        tabs:
        function (setting) {
            if (!setting) {
                setting = {};
            }
            var ps = $.extend({
            }, setting);
            var that = this;

            that.on('click', 'li > a[data-toggle="tab"]', change)

            var tabIds = $.map(that.find('li > a[data-toggle="tab"]'), function (item) {
                return $(item).attr('href');
            })

            function change(e) {
                e.preventDefault();
                var target = $(e.currentTarget);
                var tabId = target.attr('href');
                changeTab(tabId);
            }

            function changeTab(tabId) {
                $.each(tabIds, function (index, item) {
                    $(item).hide();
                })
                $(tabId).show();
                that.children('li').removeClass('selected');
                var li = that.find('a[href="' + tabId + '"]').parents("li");
                li.addClass('selected');
            }

            function init() {
                var initId = that.find('li.selected > a[data-toggle="tab"]').attr('href');
                changeTab(initId);
            }

            init();
        }
    });
})(jQuery)

$(function () {
    $('[data-role="tab"]').tabs();
})