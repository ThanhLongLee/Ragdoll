
$(function () {
    history.pushState("", document.title, window.location.pathname);

})



var getListEmployee = function () {
    this.PageNo = 1;
    this.cache = {
        $fillter: $("#filter-data"),
        $tbl: $("#tbl-data"),
        $pagination: $("#pagination"),
        prevHeader: "#tbl-data-prev",
        nextHeader: "#tbl-data-next",
        controller: "Employee",
        action: "search",
    }

    this.init();
}

getListEmployee.prototype = {
    init: function () {
        // Check has PageNo on Url
        if (window.location.href.indexOf("#page") > -1) {
            var indexPage = window.location.href.indexOf("#page");
            var str = window.location.href.substring(indexPage, window.location.href.length);
            var pagehref = str.split('-').pop();
            if ($.isNumeric(pagehref)) {
                this.PageNo = parseInt(pagehref);
            }
        }
        //this.datimeRange_init();
        this.bindEvents();
        this.initSorting();
        this.getTableData();
        this.btnPaginationHeaderClick();
    },


    bindEvents: function () {
        var $parentThis = this;
        this.cache.$fillter.on('change', "select", $parentThis.fieldFilterChange.bind(this, "change"));
        this.cache.$fillter.on('click', "#btnKeyword", $parentThis.fieldFilterChange.bind(this, "click"));
        //  this.on('click', "#btn-delete-employee", $parentThis.deleteEmployeeClick.bind(this, "click"));


        this.cache.$fillter.find('input#Keyword').keydown(function (e) {
            if (e.keyCode == 13) {
                $parentThis.fieldFilterChange();
            }
        });
        this.cache.$tbl.on('click', "thead th[data-orderby]", $parentThis.sorting.bind(this, "click"));
    },

    deleteEmployeeClick: function (userId) {
        var $parentThis = this;
        var keyword = this.cache.$fillter.find("input[name=Keyword]").val() || "";
        var pageNo = this.PageNo;
        $.confirm({
            title: "Ngừng hoạt động",
            type: "red",
            content: "<label>Bạn có muốn xác nhận không?</label>",
            buttons: {
                confirm: {
                    btnClass: "btn-warning",
                    text: "Xác nhận",
                    action: function () {
                        extend.loading($parentThis.cache.$tbl, true, false);
                        $.ajax({
                            url: location.origin + "/employee/delete?id=" + userId + "&keyword=" + keyword + "&pageNo=" + pageNo,
                            type: "POST",
                            success: function (data) {
                                console.log(data);
                                $parentThis.cache.$tbl.find("tbody").html(data.view);
                                $parentThis.intitPagination('#pagination', data.totalPage);
                                $parentThis.cache.$pagination.pagination('drawPage', $parentThis.PageNo);
                                $parentThis.checkBtnPaginationHeader(data.totalPage)

                                var html = '<div class="alert alert-success m-3" role="alert">' + data.content + '</div >';
                                $(".alter-status").html(html);   

                                extend.loading($parentThis.cache.$tbl, false, false)
                            },
                            error: function (error) {
                                console.log('Error: ' + error.statusText);
                                extend.loading($parentThis.cache.$tbl, false, false)
                            }
                        });
                        ;
                    }
                },
                cancel: {
                    text: 'Đóng'
                }
            }
        })
    },


    fieldFilterChange: function () {
        this.PageNo = 1;
        this.getTableData();
    },


    datimeRange_init: function () {
        var $parentThis = this;

        // date picker
        var dateRangeStart = moment().subtract(6, 'days');
        var dateRangeEnd = moment();
        this.initPluginDatepickerRange(this.cache.$fillter.find("#rangeDate"), dateRangeStart, dateRangeEnd, this.renderDatePicker);
        this.renderDatePicker(dateRangeStart, dateRangeEnd);

        // Date Range change
        this.cache.$fillter.find("#rangeDate").on('apply.daterangepicker', function (ev, picker) {
            $parentThis.fieldFilterChange();
        });
    },


    renderDatePicker: function (start, end) {
        $("#rangeDate").find("span").html(start.format('DD/MM/YYYY') + ' - ' + end.format('DD/MM/YYYY'));
        $("#rangeDate").find("input#StartDate").val(start.format('DD/MM/YYYY'));
        $("#rangeDate").find("input#EndDate").val(end.format('DD/MM/YYYY'));
    },


    // moment.min.js, daterangepicker.js
    initPluginDatepickerRange: function (element, start, end, callback) {
        $(element).daterangepicker({
            showDropdowns: true,
            locale: {
                format: "DD/MM/YYYY",
                separator: " - ",
                applyLabel: "Áp dụng",
                cancelLabel: "Đóng",
                fromLabel: "từ",
                toLabel: "đến",
                customRangeLabel: "Tùy chỉnh",
                weekLabel: "W",
                daysOfWeek: [
                    "CN",
                    "T2",
                    "T3",
                    "T4",
                    "T5",
                    "T6",
                    "T7"
                ],
                monthNames: [
                    "Tháng 1",
                    "Tháng 2",
                    "Tháng 3",
                    "Tháng 4",
                    "Tháng 5",
                    "Tháng 6",
                    "Tháng 7",
                    "Tháng 8",
                    "Tháng 9",
                    "Tháng 10",
                    "Tháng 11",
                    "Tháng 12"
                ],
                "Ngày đầu tiên": 1
            },
            startDate: start,
            endDate: end,
            opens: "left",
            ranges: {
                'Tất cả': [moment('20180101'), moment()],
                'Hôm nay': [moment(), moment()],
                'Hôm qua': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                '7 ngày qua': [moment().subtract(6, 'days'), moment()],
                '30 ngày trước': [moment().subtract(29, 'days'), moment()]
            }
        }, callback);
    },


    getTableData: function () {
        var $parentThis = this;

        $(this.cache.prevHeader).prop("disabled", true);
        $(this.cache.nextHeader).prop("disabled", true);

        extend.loading(this.cache.$tbl, true, false);

        var datas = this.getDataFilter();
        datas.pageNo = this.PageNo;


        $.extend(datas, this.getDataSorting());

        APP.Data[this.cache.controller][this.cache.action](datas, function (data) {
            if (checkAjax_result(data)) {
                $parentThis.cache.$tbl.find("tbody").html("");
                if (data.view != null) {
                    $parentThis.cache.$tbl.find("tbody").html(data.view);
                    $parentThis.intitPagination('#pagination', data.totalPage);
                    $parentThis.cache.$pagination.pagination('drawPage', $parentThis.PageNo);
                    $parentThis.checkBtnPaginationHeader(data.totalPage)
                }
            }

            extend.loading($parentThis.cache.$tbl, false, false);
        });
    },


    getDataFilter: function () {
        var nameField = {
            keyword: "Keyword",
            startDate: "StartDate",
            endDate: "EndDate",
        }

        var datas = {};
        datas.keyword = this.cache.$fillter.find("input[name=" + nameField.keyword + "]").val() || "";
        //datas.status = parseInt(this.cache.$fillter.find("#dropStatus > li.active").attr("data-value"));
        //datas.startDate = this.cache.$fillter.find("input[name=" + nameField.startDate + "]").val() || "";
        //datas.endDate = this.cache.$fillter.find("input[name=" + nameField.endDate + "]").val() || "";

        return datas;
    },


    intitPagination: function (element, totalPages, callback) {
        var $parentThis = this;

        $(element).pagination({
            items: totalPages,
            itemOnPage: 8,
            currentPage: 1,
            cssStyle: '',
            prevText: '<span aria-hidden="true">&laquo;</span>',
            nextText: '<span aria-hidden="true">&raquo;</span>',
            onInit: function () {
                // fire first page loading
            },
            onPageClick: function (page, evt) {
                $parentThis.PageNo = page;
                $parentThis.getTableData();
            }
        });
    },


    initSorting: function () {
        this.cache.$tbl.find("thead").find("th[data-orderby]").each(function (index, element) {
            var $element = $(element);
            $element.attr("data-sort-active", "false");
            $element.attr("data-order-expression", "asc");
            $element.addClass("sorting");
        });
    },


    sorting: function (target, e) {
        var $thisColumn = $(e.currentTarget);
        if ($thisColumn.attr("data-orderby").length > 0) {
            if ($thisColumn.attr("data-sort-active").length > 0 && $thisColumn.attr("data-sort-active") == "true") {
                var orderExpression = $thisColumn.attr("data-order-expression");
                $thisColumn.attr("data-order-expression", orderExpression == "asc" ? "desc" : "asc");
                $thisColumn.removeClass("sorting-" + orderExpression);
                var expressionClass = orderExpression == "asc" ? "desc" : "asc";
                $thisColumn.addClass("sorting-" + expressionClass);
            } else {
                // refresh All sorting
                this.cache.$tbl.find("thead").find("th[data-orderby]").not($thisColumn).each(function (index, element) {
                    var $element = $(element);
                    if ($element.attr("data-orderby").length > 0) {
                        $element.attr("data-sort-active", "false");
                        $element.attr("data-order-expression", "asc");
                        $element.removeClass("sorting-desc");
                        $element.removeClass("sorting-asc");
                        $element.addClass("sorting");
                    }
                });
                $thisColumn.attr("data-sort-active", "true");
                $thisColumn.removeClass("sorting");
                $thisColumn.addClass("sorting-asc");
            }
        }

        this.fieldFilterChange();
    },


    getDataSorting: function () {
        var datas = {
            sortByDate: false
        }

        // reset value sorting
        $.each(this.getListOrderBy(), function (index, value) {
            datas[value] = false;
        });

        var $sortThis = this.cache.$tbl.find("thead").find("th[data-sort-active='true']");
        if (typeof $sortThis !== typeof undefined && $sortThis !== false && $sortThis.length > 0) {
            var orderby = $sortThis.attr("data-orderby");
            var orderExpression = $sortThis.attr("data-order-expression");
            datas[orderby] = true;
            datas.sortByDate = orderExpression === "asc" ? false : true;
        }

        return datas;
    },


    getListOrderBy: function () {
        var datas = [];
        this.cache.$tbl.find("thead").find("th[data-orderby]").each(function (index, element) {
            datas.push($(element).attr("data-orderby"));
        });
        return datas;
    },



    btnPaginationHeaderClick: function () {
        var $parentThis = this;
        $($parentThis.cache.prevHeader).click(function () {
            $parentThis.PageNo = $parentThis.PageNo - 2;
            $parentThis.getTableData();
        });

        $($parentThis.cache.nextHeader).click(function () {
            $parentThis.getTableData();
        });

    },

    checkBtnPaginationHeader: function (totalPage) {
        if (totalPage === 0) {
            $(this.cache.prevHeader).prop("disabled", true);
            $(this.cache.nextHeader).prop("disabled", true);
        }
        if (this.PageNo === 1 && totalPage > 1) {
            $(this.cache.prevHeader).prop("disabled", true);
            $(this.cache.nextHeader).prop("disabled", false);
        }

        if (this.PageNo === 1 && totalPage === 1) {
            $(this.cache.prevHeader + "," + this.cache.nextHeader).prop("disabled", true);
        }

        if (this.PageNo === totalPage && totalPage > 1) {
            $(this.cache.prevHeader).prop("disabled", false);
            $(this.cache.nextHeader).prop("disabled", true);
        }

        if (this.PageNo > 1 && this.PageNo < totalPage && totalPage > 1) {
            $(this.cache.prevHeader).prop("disabled", false);
            $(this.cache.nextHeader).prop("disabled", false);
        }
    }




}
var listEmployee = new getListEmployee();