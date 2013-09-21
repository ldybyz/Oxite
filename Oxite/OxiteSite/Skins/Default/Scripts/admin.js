/** add/edit post **/
$(document).ready(function() {
    if ($("#post_published").is(":enabled")) {
        $("#post_published").change(function() {
            $("#post_statePublished").attr("checked", "checked");
        });
        $("#post_statePublished").focus(function() {
            $("#post_published").focus();
            if ($.trim($("#post_published").val()) === "") {
                var date = new Date();
                $("#post_published").val(date.toShortString());
            }
            $("#post_published").blur();
        });
        $("#post_published").datepicker({
            duration: "",
            dateFormat: "yy/mm/dd '8:00 AM'",
            showOn: "button",
            buttonImage: window.cssPath + "/images/calendar.png",
            buttonImageOnly: true,
            closeAtTop: false,
            isRTL: true
        });
    };

    $("input[name='postState']").change(function() {
        if ($("#post_statePublished").is(":checked")) {
            $("#post_published").addClass("active");
        } else {
            $("#post_published").removeClass("active");
        }
    });

    $("#post_title").change(function() {
        $("#post_slug").slugify($(this).val());
    });

    $.fn.extend({
        slugify: function(string) {
            if (!this.is(":enabled"))
                return;

            slug = $.trim(string);

            if (slug && slug !== "") {
                var cleanReg = new RegExp("[^A-Za-z0-9-]", "g");
                var spaceReg = new RegExp("\\s+", "g");
                var dashReg = new RegExp("-+", "g");

                slug = slug.replace(spaceReg, '-');
                slug = slug.replace(dashReg, "-");
                slug = slug.replace(cleanReg, "");

                if (slug.length * 2 < string.length) {
                    return "";
                }

                if (slug.Length > 100) {
                    slug = slug.substring(0, 100);
                }
            }

            this.val(slug);
        }
    });
});

Date.prototype.toShortString = function() {
    var y = this.getYear();
    var year = y % 100;
    year += (year < 38) ? 2000 : 1900;
    return (this.getMonth() + 1).toString() + "/" + this.getDate() + "/" + year + " " + this.toLocaleTimeString();
};

/** site settings icon picker **/
$(document).ready(function() {
    $("form#siteSettings span.hint.icons img").each(function() {
        $(this).click(function() {
            $("#favIconUrl").val($(this).attr("title"));
            $(this).siblings(".selected").removeClass("selected");
            $(this).addClass("selected");
        });
        $(this).hoverClassIfy();
    });
});