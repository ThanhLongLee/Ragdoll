var pluginUploadImgSingle_count = 0;

var appendHtml = {

    base_html: ' <div class="upload-img-wrap">'
        + '<div class="upload-img-preview"></div>'
        + '<div class="upload-img-input"></div>'
        + '</div>',

    preview: '<div class="preview">'
        + '<span class="mask">'
        + '< i class= "far fa-hand-point-up d-block" ></i>'
        + '<br>Nhấp vào để tải ảnh'
        + '</span>'
        + '</div>',

    inputFile: '<label for="input-single"><i class="icon"></i></label>'
        + '<input id="input-single" type="file" accept="image/*" />',

    tool_remove: '<div id="removeImg" class="tools ic-remove"></div>',
    tool_rotate: '<div id="rotateImg" class="tools ic-rotate"></div>'

}

var UploadImgSingle = function (parent, options, pluginCount) {
    this.parent = parent;
    var config;
    this.classes =
        {
            uploaded: 'uploaded',
            loading: 'uploading'
        }
    this.plugin_count = pluginCount;
    // default config
    config = {
        urlAjaxUpload: "uploadFileImgTemp",
        remove: true, // tool remove
        rotate: true,
        fileType: 'image', // image, all
        maximunSize: 100, // size file
        inputTarget: 'inputThumbnail', // name pass server
        required: false,// is true only single upload

    }

    // js config extends default config
    $.extend(config, options);
    this.options = config;

    // cache for links to all DOM elements
    this.$cache = {
        cont: null,
        parent: $(parent),
    }

    // Appends template to a DOM
    this.$cache.cont = this.$cache.parent;
    this.$cache.cont.addClass("js-ulf-single");
    this.$cache.cont.html(appendHtml.base_html);
    this.$cache.cont.find(".upload-img-wrap").addClass("ulf-single-" + this.plugin_count);
    this.$cache.cont.find(".upload-img-input").addClass("ulf-single-" + this.plugin_count);

    this.$cache.cont.find('.upload-img-input.ulf-single-' + this.plugin_count).html(appendHtml.inputFile);
    if (this.options.required) {
        this.$cache.cont.find('.upload-img-input.ulf-single-' + this.plugin_count + ' input[type=file]').prop('required', true);
    }

    this.$cache.inputFile = this.$cache.cont.find('.upload-img-input.ulf-single-' + this.plugin_count + ' input[type=file]');
    this.$cache.inputFile.attr("id", "input_ulf-single-" + this.plugin_count);
    this.$cache.lblFile = this.$cache.cont.find('.upload-img-input.ulf-single-' + this.plugin_count + ' label');
    this.$cache.lblFile.attr("for", "input_ulf-single-" + this.plugin_count);

    this.$cache.cont.find('.upload-img-wrap.ulf-single-' + this.plugin_count).append(appendHtml.tool_remove);


    if (this.options.rotate) {
        this.$cache.cont.find('.upload-img-wrap.ulf-single-' + this.plugin_count).append(appendHtml.tool_rotate);
    }

    this.init();
}

UploadImgSingle.prototype = {
    init: function () {
        this.parent.html(this.$cache.cont.html());
        this.bindEvents();
        if (this.$cache.cont.attr("data-id").length) {
            this.hadImage();
        }
    },

    bindEvents: function () {
        this.$cache.cont.on('change', ".upload-img-input.ulf-single-" + this.plugin_count + " input[type=file][id=" + "input_ulf-single-" + this.plugin_count + "]", this.inputChange.bind(this, "change"));
        this.$cache.cont.on('click', '.upload-img-wrap.ulf-single-' + this.plugin_count + " #removeImg", this.removeClick.bind(this, "click"));
        this.$cache.cont.on('click', '.upload-img-wrap.ulf-single-' + this.plugin_count + " #rotateImg", this.rotateClick.bind(this, "click"));
    },

    inputChange: function (target, e) {
        this.$cache.cont = $(e.currentTarget).parents(".js-ulf-single");
        this.$cache.inputFile = this.$cache.cont.find('.upload-img-input input[type=file]');

        this.$cache.cont.find('.upload-img-wrap').addClass(this.classes.loading);

        var $this = this;

        if (this.isImage()) {


            this.fncAjaxUpload(function (reponseListName, reponsePathFolder) {
                //show  privew image
                $this.privewImg(reponseListName, reponsePathFolder);

                // remove input file old and new input file
                $this.$cache.cont.find(".upload-img-input").html("");
                $this.$cache.cont.find(".upload-img-input").append(appendHtml.inputFile);
                $this.$cache.inputFile = $this.$cache.cont.find('.upload-img-input.ulf-single-' + $this.plugin_count + ' input[type=file]');
                $this.$cache.inputFile.attr("id", "input_ulf-single-" + $this.plugin_count);
                $this.$cache.lblFile = $this.$cache.cont.find('.upload-img-input.ulf-single-' + $this.plugin_count + ' label');
                $this.$cache.lblFile.attr("for", "input_ulf-single-" + $this.plugin_count);

                // add class
                $this.$cache.cont.find(".upload-img-wrap").removeClass($this.classes.loading);
                $this.$cache.cont.find(".upload-img-wrap").addClass($this.classes.uploaded);

            });
        } else {
            alert("Error file type.");
            this.$cache.cont.removeClass(this.classes);
        }
    },

    fncAjaxUpload: function (callback) {
        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        var formData = this.getFormData();
        formData.append("__RequestVerificationToken", token);
        var $this = this;
        $.ajax({
            type: "POST",
            url: location.origin + "/" + this.options.urlAjaxUpload,
            data: formData,
            contentType: false,
            processData: false,
            cache: false,
            success: function (data) {
                if (data.status === true || data.notFound != null) {
                    callback(data.listName, data.pathFolder);
                } else {
                    if (data.status === false && data.message != null) {
                        alert("Error!");
                        console.log(data.message);
                    }
                }
            },
            error: function (message) {
                alert(message.statusText);
            }
        });
    },

    getFormData: function () {
        var formData = new FormData();
        var file = this.$cache.inputFile[0].files[0];
        if (file.size > this.options.maximunSize) {
            formData.append("files", file);
        }
        return formData;
    },

    privewImg: function (listName, pathFolder) {
        var arrName = listName !== "" ? listName.split(this.$cache.charSlipt) : [];
        this.$cache.cont.find(".upload-img-preview").css("background-image", 'url(' + pathFolder + arrName[0] + "?r=" + Math.floor((1 + Math.random()) * 0x10000) + ')');

        this.$cache.cont.attr("data-id", arrName[0]);
        $(document).find(this.options.inputTarget).val(this.$cache.cont.attr("data-id")).trigger('change');
    },

    hadImage: function () {
        var imageName = this.$cache.cont.attr("data-id");
        var path = this.$cache.cont.attr("data-path");

        //show  privew image
        this.privewImg(imageName, path);
        // add class
        this.$cache.cont.find(".upload-img-wrap").removeClass(this.classes.loading);
        this.$cache.cont.find(".upload-img-wrap").addClass(this.classes.uploaded);
    },

    removeClick: function (target, e) {
        var $remove = $(e.currentTarget);
        var $uploadWrap = $(e.currentTarget).parents(".upload-img-wrap");

        this.$cache.cont = $(e.currentTarget).parents(".js-ulf-single");
        this.$cache.cont.find('.upload-img-wrap').find(".upload-img-preview").css("background-image", "");
        this.$cache.cont.find('.upload-img-wrap').removeClass(this.classes.uploaded);

        this.$cache.cont.removeAttr("data-id data-rotate");
        $uploadWrap.removeClass("rotate-90 rotate-180 rotate-270");

        $(document).find(this.options.inputTarget).val("");
    },

    rotateClick: function (target, e) {
        var $imgWrap = $(e.currentTarget).parents(".upload-img-wrap");
        var newFileName = this.$cache.cont.attr("data-id");

        if (this.$cache.cont.attr("data-rotate") != null && this.$cache.cont.attr("data-rotate").length > 0) {
            var oldRotate = this.$cache.cont.attr("data-rotate");
            if (oldRotate == 270) {
                $imgWrap.removeClass("rotate-" + oldRotate);
                this.$cache.cont.removeAttr("data-rotate");
            } else {
                var newRotate = parseInt(oldRotate) + 90;
                this.$cache.cont.attr("data-rotate", newRotate);
                $imgWrap.removeClass("rotate-" + oldRotate);
                $imgWrap.addClass("rotate-" + newRotate);
            }
        } else {
            this.$cache.cont.attr("data-rotate", 90);
            $imgWrap.addClass("rotate-90");
        }

        // if rotate enable
        if (this.options.rotate) {
            if (this.$cache.cont.attr("data-rotate") != null && this.$cache.cont.attr("data-rotate").length > 0) {
                var name = this.$cache.cont.attr("data-id").split(".")[0];
                var typeFile = this.$cache.cont.attr("data-id").split(".")[1];
                var rotate = this.$cache.cont.attr("data-rotate");
                if (typeFile != undefined && typeFile !== "") {
                    newFileName = name + "." + rotate + "." + typeFile;
                } else {
                    newFileName = name + "." + rotate;
                }
            }
        }

        $(document).find(this.options.inputTarget).val(newFileName).trigger('change');
    },


    isImage: function () {

        var file = this.$cache.inputFile[0].files[0];
        var fileType = file["type"];
        var validImageTypes = ["image/gif", "image/jpeg", "image/png", "image/jpg"];
        if ($.inArray(fileType, validImageTypes) < 0) {
            return false;
        }

        return true;
    },
}

$.fn.uploadImgSingle = function (options) {
    if (!$.data(this, "UploadImgSingle")) {
        $.data(this, "UploadImgSingle", new UploadImgSingle(this, options, pluginUploadImgSingle_count++));
    }
};