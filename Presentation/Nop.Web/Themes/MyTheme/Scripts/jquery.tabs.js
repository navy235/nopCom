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

      that.on('click', 'li > a[data-toggle="tab"]', changeTabs)

      var tabIds = $.map(that.find('li > a[data-toggle="tab"]'),function (item) {
        return $(item).attr('href');
      })

      function changeTabs(e) {
        e.preventDefault();
        var target = $(e.currentTarget);
        $.each(tabIds, function (index, item) {
          $(item).hide();
        })
        var tabId = target.attr('href');
        $(tabId).show();
        that.children('li').removeClass('selected');
        target.parents('li').addClass('selected');
      }
    }
  });
})(jQuery)

$(function () {
  $('[data-role="tab"]').tabs();
})