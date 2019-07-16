/* 
*page plugin 1.0   2016-09-29 by cary
*/
(function ($) {
    //默认参数
    var defaults = {
        totalPages: 9,//总页数
        liNums: 10,//分页的数字按钮数(建议取奇数)
        activeClass: 'active',//active类
        firstPage: '&laquo;',//首页按钮名称
        lastPage: '&raquo;',//末页按钮名称
        prv: '&lsaquo;',//前一页按钮名称
        next: '&rsaquo;',//后一页按钮名称
        hasFirstPage: true,//是否有首页按钮
        hasLastPage: true,//是否有末页按钮
        hasPrv: true,//是否有前一页按钮
        hasNext: true,//是否有后一页按钮
        hasPageInput: true,//是否有页面输入框
        showTotalPages: true,
        callBack: function (page) {
            //回掉，page选中页数
        }
    };

    //插件名称
    $.fn.Page = function (options) {
        //覆盖默认参数
        var opts = $.extend(defaults, options);
        //主函数
        return this.each(function () {
            var obj = $(this);
            var l = opts.totalPages;
            var n = opts.liNums;
            var active = opts.activeClass;

            var dataHtml = '';
            var str = ""
            if (l > 1 && l < n + 1) {
                str = BuildUlHtml(1, l);
            }
            else {
                str = BuildUlHtml(1, n);
            }

            dataHtml = '<ul class="pagination">' + str + '</ul>' + dataHtml;
            obj.html(dataHtml).off("click");//防止插件重复调用时，重复绑定事件

            var indexOffset = 0;//被选中项的偏移
            if (opts.hasFirstPage) {
                indexOffset += 1;
            }

            if (opts.hasPrv) {
                indexOffset += 1;
            }

            obj.find('ul li').eq(indexOffset).addClass(active);//初始化选中项

            obj.on('click', '.next', function () {
                var pageshow = parseInt($('.' + active).find("a").html());
                var nums, flag;
                var a = n % 2;
                if (a == 0) {
                    //偶数
                    nums = n;
                    flag = true;
                } else if (a == 1) {
                    //奇数
                    nums = (n + 1);
                    flag = false;
                }
                if (pageshow >= l) {
                    return;
                } else if (pageshow > 0 && pageshow <= nums / 2) {
                    //最前几项
                    obj.find('.' + active).removeClass(active).next().addClass(active);
                } else if ((pageshow > l - nums / 2 && pageshow < l && flag == false) || (pageshow > l - nums / 2 - 1 && pageshow < l && flag == true)) {
                    //最后几项
                    obj.find('.' + active).removeClass(active).next().addClass(active);
                } else {
                    obj.find('.' + active).removeClass(active).next().addClass(active);
                    fpageShow(pageshow + 1);
                }
                opts.callBack(pageshow + 1);
            });
            obj.on('click', '.prv', function () {
                var pageshow = parseInt($('.' + active).find("a").html());
                var nums = odevity(n);
                if (pageshow <= 1) {
                    return;
                } else if ((pageshow > 1 && pageshow <= nums / 2) || (pageshow > l - nums / 2 && pageshow < l + 1)) {
                    //最前几项或最后几项
                    obj.find('.' + active).removeClass(active).prev().addClass(active);
                } else {
                    obj.find('.' + active).removeClass(active).prev().addClass(active);
                    fpageShow(pageshow - 1);
                }
                opts.callBack(pageshow - 1);
            });

            obj.on('click', '.first', function () {
                var activepage = parseInt($('.' + active).html());
                if (activepage <= 1) {
                    return
                }//当前第一页
                opts.callBack(1);
                fpagePrv(0);
            });
            obj.on('click', '.last', function () {
                var activepage = parseInt($('.' + active).html());
                if (activepage >= l) {
                    return;
                }//当前最后一页
                opts.callBack(l);
                if (l > n) {
                    fpageNext(n - 1);
                } else {
                    fpageNext(l - 1);
                }
            });

            obj.on('click', 'input[type=button]', function () {
                var $input = $(this).parent().prev().find('input');
                var pageshow = $input.val();
                var nums = odevity(n);
                if (pageshow > l || pageshow <= 0) {
                    return;
                }
                opts.callBack(pageshow);
                if (l > n) {
                    if (pageshow > l - nums / 2 && pageshow < l + 1) {
                        //最后几项
                        fpageNext((n - 1) - (l - pageshow));
                    } else if (pageshow > 0 && pageshow < nums / 2) {
                        //最前几项
                        fpagePrv(pageshow - 1);
                    } else {
                        fpageShow(pageshow);
                    }
                } else {
                    obj.find('.' + active).removeClass(active);
                    obj.find('ul li').eq(pageshow + indexOffset - 1).addClass(active);
                }
            });

            obj.on('click', '.page', function () {
                var $this = $(this);
                var pageshow = parseInt($this.find('a').html());
                var nums = odevity(n);
                opts.callBack(pageshow);
                if (l > n) {
                    if (pageshow > l - nums / 2 && pageshow < l + 1) {
                        //最后几项
                        fpageNext((n - 1) - (l - pageshow));
                    } else if (pageshow > 0 && pageshow < nums / 2) {
                        //最前几项
                        fpagePrv(pageshow - 1);
                    } else {
                        fpageShow(pageshow);
                    }
                } else {
                    obj.find('.' + active).removeClass(active);
                    $this.addClass(active);
                }
            });

            //重新渲染结构
            /*activePage 当前项*/
            function fpageShow(activePage) {
                var nums = odevity(n);
                var pageStart = activePage - (nums / 2 - 1);
                var str1 = BuildUlHtml(pageStart, pageStart + n - 1);

                obj.find('ul').html(str1);
                obj.find('ul li').eq(nums / 2 - 1 + indexOffset).addClass(active);
            }
            /*index 选中项索引*/
            function fpagePrv(index) {
                var str1 = '';
                if (l > n - 1) {
                    str1 = BuildUlHtml(1, n);
                }
                else {
                    str1 = BuildUlHtml(1, l);
                }
                obj.find('ul').html(str1);
                obj.find('ul li').eq(index + indexOffset).addClass(active);
            }
            /*index 选中项索引*/
            function fpageNext(index) {
                var str1 = '';
                if (l > n - 1) {
                    str1 = BuildUlHtml(l - (n - 1), l);
                }
                else {
                    str1 = BuildUlHtml(1, l);
                }

                obj.find('ul').html(str1);
                obj.find('ul li').eq(index + indexOffset).addClass(active);
            }
            //判断liNums的奇偶性
            function odevity(n) {
                var a = n % 2;
                if (a == 0) {
                    //偶数
                    return n;
                } else if (a == 1) {
                    //奇数
                    return (n + 1);
                }
            }

            function BuildUlHtml(first, last) {
                var str = '';

                if (opts.hasFirstPage) {
                    str += '<li class="first"><a href="javascript:">' + opts.firstPage + '</a></li>';
                }

                if (opts.hasPrv) {
                    str += '<li class="prv"><a href="javascript:">' + opts.prv + '</a></li>';
                }

                for (i = first; i <= last; i++) {
                    str += '<li class="page"><a href="javascript:">' + i + '</a></li>';
                }

                if (opts.hasNext) {
                    str += '<li class="next"><a href="javascript:">' + opts.next + '</a></li>';
                }

                if (opts.hasLastPage) {
                    str += '<li class="last"><a href="javascript:">' + opts.lastPage + '</a></li>';
                }

                if (opts.hasPageInput) {
                    str += '<li><input type="text" class="w-25 ml-5 mr-5 mt-5 h-25"/></li>';
                    str += '<li><input type="button" value="go" class="w-25 mr-5 h-25 btn btn-primary p-0" style="margin-bottom:3px"/></li>';
                }

                if (opts.showTotalPages) {
                    str += '<li style="display:inline-block">共' + opts.totalPages + '页</li>';
                }

                return str;
            }
        });
    }
})(jQuery);