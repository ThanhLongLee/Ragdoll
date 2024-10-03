$(function () {

    KeepAlive();
    ActiveMenu();
});

var KeepAlive = function () {
    APP.Data.Main.keepAlive(function (data) {
        if (checkAjax_result(data)) {
           // console.log(data.dateTime);
        }
        setTimeout(KeepAlive, 1000 * 300); // function refers to itself
    });
};

function checkAjax_result(data) {
    if (data.status != null) {
        if (data.status !== false) {
            return true;
        }
        if (data.status === false) {
            alert(data.message);
            return false;
        }
    }
    return true;
}

var extend = (function () {

    var loading = function (wraper, enable = true, fullpage = false) {
        if (enable) {
            if (fullpage) {
                $(wraper).addClass("loading loading-full-screen");
            } else {
                $(wraper).addClass("loading");
            }
        } else {
            $(wraper).removeClass("loading loading-full-screen");
        }

    }

    var alert = function (alterStyle, $target, message) {
        return $target.html('<div class="alert alert-' + alterStyle + '" role="alert">' + message + ' </div>');
    }

    return {
        loading: loading,
        alert: alert
    }
})();


function inputCurrency(element) {
    $(element).mask("#,##0", { reverse: true });
}

var showPassword = function ($this) {

    $($this).on('click', function () {
        $(this).find("i").toggleClass("fa-eye fa-eye-slash");
        var inputTarget = $(this).attr("data-target");
        var input = $(inputTarget);
        if (input.attr("type") === "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password");
        }
    });
}

function ActiveMenu() {
    if ($(".nav-treeview").length > 0) {
        var contrainActive = $(".nav-treeview .item-list").attr("class");
        var checkActive = contrainActive.includes("active") ? "true" : "false";

        $(".nav-treeview .item-list.active")
            .closest("ul").attr("style", "display: block;");

        $(".nav-treeview .item-list.active").closest("ul")
            .closest(".nav-item.menu-item").attr("class", "nav-item menu-item menu-is-opening menu-open");
    }

}

function isEmpty(value) {
    return (value == null || (typeof value === "string" && value.trim().length === 0));
}

const isObjectEmpty = (objectName) => {
    for (let prop in objectName) {
        if (objectName.hasOwnProperty(prop)) {
            return false;
        }
    }
    return true;
};

function isNullOrUndefined(value) {
    return value === undefined || value === null;
}