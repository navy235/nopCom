$(function () {
  $('.address-inner').hover(function () {
    $(this).addClass('hover')
  }, function () {
    $(this).removeClass('hover')
  })

  $('.address-inner').on('click', function () {
    $('.address-inner').removeClass('current');
    $(this).addClass('current');
    $(this).find('[name="addressId"]').prop("checked", true);
  })

  $('.mt-checkbox').on('click', function () {
    var input = $(this).find('input[type="checkbox"]');
    var checked = input.prop("checked");
    input.prop("checked", !checked);
    if (checked) {
      $(this).find('.ui-checkbox').removeClass('checked');
    } else {
      $(this).find('.ui-checkbox').addClass('checked');
    }
  })

  $('.mt-radio').on('click', function () {
    var input = $(this).find('input[type="radio"]');
    var checked = input.prop("checked", true);
    $(this).find('.ui-radio').addClass('checked');
    var payitem = $(this).parents('.payment-item');
    payitem.addClass('selected');
    payitem.siblings().removeClass('selected');
    $.each(payitem.siblings(), function (index, item) {
      $(this).find('.ui-radio').removeClass('checked');
    })
  })
})