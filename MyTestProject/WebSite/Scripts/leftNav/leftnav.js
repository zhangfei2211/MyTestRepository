var menuMethod = {
    init: function (ele, data) {
        var $ele = $(ele);
        $ele.html("");
        $(data).each(function (index, item) {
            if (item.ParentId == null) {
                var div = '<li id="li_' + item.Id + '">'
                    + '<div class="link"><i class="fa fa-leaf"></i>' + item.Name + '<i class="fa fa-chevron-down"></i></div>'
                    + '<ul class="submenu"></ul>'
                    + '</li>';
                $ele.append(div);
            }

            if (item.ParentId != null) {
                var href = item.Href == null ? "javascript:;" : (item.Href == "" ? "javascript:;" : item.Href);
                var div = '<li id="li_' + item.Id + '"><a link="' + href + '">' + item.Name + '</a></li>';
                $("#li_" + item.ParentId +" .submenu").append(div);
            }
        });
    },
};

$.fn.menu = function (menuJson,contentDivId) {
    menuMethod.init($(this), menuJson);

    var Accordion = function (el, multiple) {
        this.el = el || {};
        this.multiple = multiple || false;

        // Variables privadas
        var links = this.el.find('.link');
        // Evento
        links.on('click', { el: this.el, multiple: this.multiple }, this.dropdown)
    };

    Accordion.prototype.dropdown = function (e) {
        var $el = e.data.el;
        $this = $(this);
        $next = $this.next();

        $next.slideToggle();
        $this.parent().toggleClass('open');

        if (!e.data.multiple) {
            $el.find('.submenu').not($next).slideUp().parent().removeClass('open');
        }
    };

    var accordion = new Accordion($(this), false);

    $('.submenu li').click(function () {
        $(".submenu li.current").removeClass('current');
        $(this).addClass('current');
        $.get($(this).find('a').attr("link"),
            function (html) {
                $("#" + contentDivId).html(html);
            });
    });

    //SelectCurrentMenu($(this));
}

function SelectCurrentMenu(nav) {
    var locHref = location.href.toLocaleLowerCase();

    var aList = $(nav).find('a');

    $(aList).each(function (index, item) {
        var ahref = $(item).attr("href").replace("../", "").toLocaleLowerCase();
        if (locHref.indexOf(ahref) >= 0) {
            var li = $(item).parent();
            li.parent().parent().addClass("open");
            li.parent().show();
            li.addClass('current');
            return;
        }
    });
}