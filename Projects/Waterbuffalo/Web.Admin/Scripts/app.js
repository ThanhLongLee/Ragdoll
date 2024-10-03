APP.Data.Ajax = (function () {
    function requestAjax(url, data, type, callback) {
        $.ajax({
            url: location.origin + "/admin/" + url,
            data: addForgeryToken(data),
            type: type,
            success: function (response) {
                callback(response);
            },
            error: function (error) {
                console.log('Error: ' + error.statusText);
                //location.reload();
            }
        });
    }

    function addForgeryToken(data) {
        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        data.__RequestVerificationToken = token;

        return data;

    };
    return {
        request: requestAjax
    }
})();

APP.Data.Main = (function () {
    var keepAlive = function (callback) {
        var data = {};
        APP.Data.Ajax.request("keepAlive", data, "POST", callback);
    }


    return {
        keepAlive: keepAlive,
    };

})();


APP.Data.Employee = (function () {
    var search = function (data, callback) {
        APP.Data.Ajax.request("employee/search", data, "POST", callback);
    }

    return {
        search: search
    };

})();