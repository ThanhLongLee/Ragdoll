
$(function () {
    $("#frm-employee").submit(function () {
        if ($(this).valid()) {
            extend.loading("body", true, true);
        }
    });

})

var frmChangePassword = (function () {
    var cache = {
        $modal: $("#modalChangePassword"),

    }
    var onBegin = function () {
        extend.loading(cache.$modal.find(".modal-content"), true, false);

    }

    var onSuccess = function (data) {
        if (checkAjax_result(data)) {
            if (data.result === true) {
                var html = '<div class="alert alert-success m-3" role="alert">' + data.content + '</div >';
                cache.$modal.find(".modal-content").html(html);
            }
        } else {
            location.reload();
        }
        extend.loading(cache.$modal.find(".modal-content"), false, false);
    }

    return {
        OnBegin: onBegin,
        OnSuccess: onSuccess
    }

})();


