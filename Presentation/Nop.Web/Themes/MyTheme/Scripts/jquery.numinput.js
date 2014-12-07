(function ($) {
    $.extend($.fn, {
        numinput:
		function (setting) {

		    var default_settings = {
		        min: 1,
		        max: 9999,
		        callback: null
		    };

		    var proxy = $.proxy;
		    var ps = $.extend(default_settings, setting);
		    var that = this;
		    that.hide();

		    var controlId = $(that).attr('id');
		    var wrapper = $('<div class="num-container"></div>');
		    var btnreduce = $('<span data-direction="reduce" class="num-reduce num-handle disabled"></span>');
		    var inputtext = $('<em class="num-input">1</em>');
		    var btnadd = $('<span data-direction="add" class="num-add num-handle "></span>');

		    that.before(wrapper.append(btnreduce)
				.append(inputtext)
				.append(btnadd));


		    wrapper.on('click', '.num-handle', proxy(changeNum, that, ps.callback));

		    init();

		    function init() {

		        var currentValue = $('#' + controlId).val();
		        if (currentValue == ps.min) {
		            btnreduce.addClass('disabled');
		        } else {
		            btnreduce.removeClass('disabled');
		        }
		        if (currentValue == ps.max) {
		            btnadd.addClass('disabled');
		        } else {
		            btnadd.removeClass('disabled');
		        }
		        inputtext.text(currentValue);
		    }

		    function changeNum(callback, e) {
		        e.preventDefault();
		        var target = $(e.currentTarget);

		        var direction = target.data('direction');
		        var currentValue = $('#' + controlId).val();
		        if (target.hasClass('disabled')) {
		            return false;
		        } else {
		            if (direction == "add") {
		                currentValue++;
		            } else {
		                currentValue--;
		            }
		        }
		        if (currentValue == ps.min) {
		            btnreduce.addClass('disabled');
		        } else {
		            btnreduce.removeClass('disabled');
		        }
		        if (currentValue == ps.max) {
		            btnadd.addClass('disabled');
		        } else {
		            btnadd.removeClass('disabled');
		        }
		        $('#' + controlId).val(currentValue);
		        inputtext.text(currentValue);
		        if (callback) {
		            callback();
		        }
		    }
		}
    });
})(jQuery)

