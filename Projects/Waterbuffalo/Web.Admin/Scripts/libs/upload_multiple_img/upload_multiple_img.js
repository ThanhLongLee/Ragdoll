var pluginUploadImg_count = 0;

var inputFileName = '<input style="display: none" id="filesName" type="text" />';


var inputFileMulti =
    '<label for="inputFileMulti"><span>Tải Hình</span></label>' +
    '<input style= "display: none" type="file" multiple="multiple" id="inputFileMulti" name="inputFileMulti" accept="image/*" />';

var inputFileMultiWrap =
    '<div class= "upload-file-input">' +
    inputFileMulti
    + '</div>';

var htmlToAppend = '<li class= "preview sort-item">'
    + '<div class="preview-inner">'
    + '<div class="backgroundImg">'
    + '</div>'
    + '</div>'
    + '</li>';

var tool_remove = '<div id="removeImg" class="tools tool-remove"></div>';
var tool_rotate = '<div id="rotateImg" class="tools tool-rotate"></div>';


var objImage = {
    Id: "",
    Name: "",
    Path: "",
    Rotate: 0,
}

var UploadMultiImg = function (parents, options, pluginUploadFileCount) {

    var config;

    this.objImages = []

    this.classes = {
        uploaded: 'uploaded'
    }

    this.plugin_count = pluginUploadFileCount;

    // default config
    config = {
        uploadFile: true,
        urlAjaxUpload: "uploadFileImgTemp",
        pramPassAjax: "files", // name pass to server ajax
        pramPassServer: "filesName",
        sortable: true,
        remove: true, // tool remove
        rotate: true,
        maximunSize: 1000, // size file
        minValidCount: 3,
        maxValidCount: 0,
        btnSave: "#btnSave",
        messageMaxValid: "Tối đa ",
        //onRemoveExecuting: function () {return true},
    }

    // js config extends default config
    $.extend(config, options);
    this.options = config;

    // cache for links to all DOM elements
    this.$cache = {
        cont: null,
        parents: $(parents),
        charSlipt: '|',
    }

    // Appends template to a DOM
    this.$cache.cont = this.$cache.parents;
    this.$cache.cont.addClass("js-ulf-slide");
    this.$cache.cont.addClass("js-ulf-slide-" + this.plugin_count);

    if (this.$cache.cont.find("ul.sortable").length === 0) {
        this.$cache.cont.append('<ul class="sortable"></ul>');
    }

    this.$cache.cont.prepend(inputFileName);
    if (this.options.uploadFile) {
        this.$cache.cont.append(inputFileMultiWrap);

    }


    this.$cache.cont.find(".upload-file-input").addClass("ulf-slide-" + this.plugin_count);
    this.$cache.cont.find(".sortable").addClass("ulf-slide-" + this.plugin_count);

    this.$cache.htmlToAppend = $($.parseHTML(htmlToAppend)).addClass("ulf-slide-" + this.plugin_count)[0].outerHTML;

    if (this.options.remove) {
        var html = $.parseHTML(this.$cache.htmlToAppend);
        $(html).find(".preview-inner").append(tool_remove);
        this.$cache.htmlToAppend = html[0].outerHTML;
    }

    if (this.options.rotate) {
        var html = $.parseHTML(this.$cache.htmlToAppend);
        $(html).find(".preview-inner").append(tool_rotate);
        this.$cache.htmlToAppend = html[0].outerHTML;
    }
 

    // Get Element
    this.$cache.inputFile = this.$cache.cont.find('.upload-file-input input[type=file]');

    this.$cache.inputFile.attr("id", "inputFileMulti_ulf-slide-" + this.plugin_count);

    this.$cache.lblInputFile = this.$cache.cont.find('.upload-file-input.ulf-slide-' + this.plugin_count + ' label');
    this.$cache.lblInputFile.attr("for", "inputFileMulti_ulf-slide-" + this.plugin_count);

    this.$cache.inputFilesName = this.$cache.cont.find('input[id="filesName"]');
    this.$cache.inputFilesName.attr("name", this.options.pramPassServer);
    this.$cache.sortable = this.$cache.cont.find('.sortable.ulf-slide-' + this.plugin_count);

    this.init();


}


UploadMultiImg.prototype = {

    init: function () {
        this.bindEvents();
        if (this.options.sortable) {
            this.fncSort();
        }

        // Check have file get file name
        if (this.$cache.sortable.find("li").length > 0) {
            this.getImageName();
        }
        this.checkValid();
        // this.valid();
    },


    bindEvents: function () {
        if (this.options.uploadFile) {
            this.$cache.cont.on('change', ".upload-file-input.ulf-slide-" + this.plugin_count + " input[type=file][id=" + "inputFileMulti_ulf-slide-" + this.plugin_count + "]", this.inputChange.bind(this, "change"));
        }

        if (this.options.uploadFile && this.options.maxValidCount > 0) {
            this.$cache.cont.on('click', ".upload-file-input.ulf-slide-" + this.plugin_count + " input[type=file][id=" + "inputFileMulti_ulf-slide-" + this.plugin_count + "]", this.checkValidMaxCount.bind(this, "click"));
        }

        this.$cache.cont.on('click', ".sortable.ulf-slide-" + this.plugin_count + " #removeImg", this.removeImg.bind(this, "click"));
        this.$cache.cont.on('click', ".sortable.ulf-slide-" + this.plugin_count + " #rotateImg", this.rotateImg.bind(this, "click"));
    },


    // Check valid Max upload file if input[type=file] Click
    checkValidMaxCount: function (target, e) {
        if (this.countFilesUpload() >= this.options.maxValidCount) {
            alert(this.options.messageMaxValid + this.options.maxValidCount + " hình");
            e.preventDefault();
            return false;
        }
    },


    countFilesUpload: function () {
        var arrFile = this.$cache.inputFilesName.val().split('|');
        return arrFile.length;
    },


    inputChange: function (target, e) {

        this.resetValue(e.currentTarget);
        // create New input file
        var lableNew = $($(inputFileMulti)[0]).attr("for", e.currentTarget.id);
        var inputNew = $($(inputFileMulti)[1]).attr("id", e.currentTarget.id);

        this.$cache.cont.addClass("updating");
        var $this = this;
        if (this.isImage()) {

            this.fncAjaxUpload(function (reponseListName, reponsePathFolder) {
                //show  privew image
                $this.privewImgUpload(reponseListName, reponsePathFolder);

                // Remove input file Old, add new input file
                $this.$cache.inputFile.remove();
                $this.$cache.cont.find(".upload-file-input").html("");
                $this.$cache.cont.find(".upload-file-input").append(lableNew);
                $this.$cache.cont.find(".upload-file-input").append(inputNew);
                $this.$cache.cont.removeClass("updating");
            });
        } else {
            alert("Error file type.");
            $this.$cache.cont.removeClass("updating");

        }
    },


    fncAjaxUpload: function (callback) {
        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        var formData = this.getFormData();
        formData.append("__RequestVerificationToken", token);
        this.$cache.parents.addClass("loader-circle");
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
                    callback(data.listImage);
                } else {
                    if (data.status === false && data.message != null) {
                        alert("Error!");
                        console.log(data.message);
                    }
                }
                $this.$cache.parents.removeClass("loader-circle");
            },
            error: function (message) {
                alert(message.statusText);
            }
        });
    },


    getFormData: function () {
        var formData = new FormData();
        var totalFiles = this.$cache.inputFile.files.length;
        for (var i = 0; i < totalFiles; i++) {
            var file = this.$cache.inputFile.files[i];
            formData.append("files", file);
        }

        // check valid Max files upload
        if (this.options.maxValidCount > 0) {
            if (this.countFilesUpload() + totalFiles > this.options.maxValidCount) {
                alert(this.options.messageMaxValid + this.options.maxValidCount + " hình");
                formData = new FormData();
            }
        }
        return formData;
    },


    privewImgUpload: function (listImage) {

        if (listImage.length === 0)
            return false;

        var $this = this;

        $.each(listImage, function (i, obj) {
            var html = $.parseHTML($this.$cache.htmlToAppend);
            var attrId = document.createAttribute("data-id");
            var attrName = document.createAttribute("data-name");
            var attrPath = document.createAttribute("data-path");

            attrId.value = obj.Id;
            attrName.value = obj.Name;
            attrPath.value = obj.Path;

            html[0].setAttributeNode(attrId);
            html[0].setAttributeNode(attrName);
            html[0].setAttributeNode(attrPath);

            $(".backgroundImg", html).css("background-image", 'url(' + obj.Path + "/" + obj.Name + ')');
            $this.$cache.sortable.append(html);
        });

        this.getImageName();
    },


    getImageName: function () {
        var arr = [];
        var $this = this;
        $this.objImages = [];

        this.$cache.sortable.find(".preview").each(function (i) {
            var fileData = $(this).attr("data-id");

            var newObj = Object.assign({}, objImage);
            newObj.Id = $(this).attr("data-id");
            newObj.Name = $(this).attr("data-name");
            newObj.Path = $(this).attr("data-path");

            // if rotate enable
            if ($this.options.rotate) {
                if ($(this).attr("data-rotate") != null && $(this).attr("data-rotate").length > 0) {
                    var name = fileData.split(".")[0];
                    var typeFile = fileData.split(".")[1];
                    var rotate = $(this).attr("data-rotate");
                    if (typeFile != undefined && typeFile !== "") {
                        fileData = name + "." + rotate + "." + typeFile;
                    } else {
                        fileData = name + "." + rotate;
                    }
                    newObj.Rotate = parseInt(rotate);
                }
            }

            $this.objImages.push(newObj);
            arr.push(fileData);
        });

        console.log(this.objImages);

        this.$cache.inputFilesName.val(arr.join(this.$cache.charSlipt)).trigger('change');
        this.checkValid();

    },


    removeImg: function (target, e) {

        this.resetValue(e.currentTarget);
        $(e.currentTarget).parents(".preview").remove();
        this.getImageName();
    },


    rotateImg: function (target, e) {
        this.resetValue(e.currentTarget);
        var $imgWrap = $(e.currentTarget).parents(".preview");
        if ($imgWrap.attr("data-rotate") != null && $imgWrap.attr("data-rotate").length > 0) {
            var oldRotate = $imgWrap.attr("data-rotate");
            if (oldRotate == 270) {
                $imgWrap.removeClass("rotate-" + oldRotate);
                $imgWrap.removeAttr("data-rotate");
            } else {
                var newRotate = parseInt(oldRotate) + 90;
                $imgWrap.attr("data-rotate", newRotate);
                $imgWrap.removeClass("rotate-" + oldRotate);
                $imgWrap.addClass("rotate-" + newRotate);
            }
        } else {
            $imgWrap.attr("data-rotate", 90);
            $imgWrap.addClass("rotate-90");
        }
        this.getImageName();
    },


    fncSort: function () {
        var $this = this;
        $this.$cache.sortable.sortable({
            placeholder: "ui-state-highlight",
        });

        $this.$cache.sortable.sortable({
            update: function (event, ui) {
                $this.resetValue($(ui.item));
                $this.getImageName();

            }
        });
    },


    resetValue: function ($element) {
        this.$cache.cont = $($element).parents(".js-ulf-slide");
        this.$cache.inputFile = this.$cache.cont.find('.upload-file-input input[type=file]').get(0);
        this.$cache.inputFilesName = this.$cache.cont.find("input[id='filesName']");
        this.$cache.lblInputFile = this.$cache.cont.find('.upload-file-input label');
        this.$cache.sortable = this.$cache.cont.find('.sortable');
    },


    isImage: function () {
        var totalFiles = this.$cache.inputFile.files.length;
        for (var i = 0; i < totalFiles; i++) {
            var file = this.$cache.inputFile.files[i];
            var fileType = file["type"];
            var validImageTypes = ["image/gif", "image/jpeg", "image/png", "image/jpg"];
            if ($.inArray(fileType, validImageTypes) < 0) {
                return false;
            }
        }
        return true;
    },


    checkValid: function () {
        var arrFile = this.$cache.inputFilesName.val().split('|');
        if (arrFile[0] === "" || (arrFile.length < this.options.minValidCount)) {
            this.$cache.cont.find(this.options.btnSave).prop("disabled", true);
            return false;
        } else {
            if (this.options.maxValidCount > 0) {
                if (arrFile.length > this.options.maxValidCount) {
                    this.$cache.cont.find(this.options.btnSave).prop("disabled", true);
                    return false;
                } else {
                    this.$cache.cont.find(this.options.btnSave).prop("disabled", false);
                }
            } else {
                this.$cache.cont.find(this.options.btnSave).prop("disabled", false);
            }
        }
        return true;
    },

}

$.fn.uploadMultiImg = function (options) {
    if (!$.data(this, "UploadMultiImg")) {
        var newUploadMultiImg = new UploadMultiImg(this, options, pluginUploadImg_count++)
        $.data(this, "UploadMultiImg", newUploadMultiImg);
        return newUploadMultiImg;
    }
};
