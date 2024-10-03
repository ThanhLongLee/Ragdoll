/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */


CKEDITOR.editorConfig = function (config) {

    config.allowedContent = {
        $1: {
            // Use the ability to specify elements as an object.
            elements: CKEDITOR.dtd,
            attributes: true,
            styles: true,
            classes: true
        }
    };


    config.language = 'vi';
    config.disallowedContent = 'script; *[on*]';
    config.filebrowserBrowseUrl = '/Scripts/libs/ckfinder/ckfinder.html';
    config.filebrowserUploadUrl = '/Scripts/libs/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = '/Scripts/libs/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images';
    config.filebrowserWindowWidth = '1000';
    config.filebrowserWindowHeight = '700';
    config.extraPlugins = 'youtube,tableresize,letterspacing';
    config.entities_latin = false;
    //config.autoGrow_minHeight = 250;
    //config.autoGrow_maxHeight = 600;

    config.youtube_width = '854';
    config.youtube_height = '480';

    //config.disallowedContent = 'img{width,height}';


    CKEDITOR.on('instanceReady', function (ev) {
        var editor = ev.editor,
            dataProcessor = editor.dataProcessor,
            htmlFilter = dataProcessor && dataProcessor.htmlFilter;

        // Output properties as attributes, not styles.
        htmlFilter.addRules(
            {
                elements:
                {
                    $: function (element) {
                        // Output dimensions of images as width and height
                        //if (element.name == 'img') {
                        //    var style = element.attributes.style;

                        //    if (style) {
                        //        // Get the width from the style.
                        //        var match = /(?:^|\s)width\s*:\s*(\d+)px/i.exec(style),
                        //            width = match && match[1];

                        //        // Get the height from the style.
                        //        match = /(?:^|\s)height\s*:\s*(\d+)px/i.exec(style);
                        //        var height = match && match[1];

                        //        if (width) {
                        //            element.attributes.style = element.attributes.style.replace(/(?:^|\s)width\s*:\s*(\d+)px;?/i, '');
                        //            element.attributes.width = '100%';
                        //        }

                        //        if (height) {
                        //            element.attributes.style = element.attributes.style.replace(/(?:^|\s)height\s*:\s*(\d+)px;?/i, '');
                        //            element.attributes.height = 'auto';
                        //        }
                        //    }
                        //}

                        if (!element.attributes.style)
                            delete element.attributes.style;

                        return element;
                    }
                }
            });
    });
};
