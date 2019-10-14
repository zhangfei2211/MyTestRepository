$(function () {
    
});

///使用ajax.beginform提交后，调用此函数，sucfun为ajax成功时执行的函数，failedfun为ajax失败时实行的函数
function AjaxBeginformComplete(response, sucfun, failedfun) {
    if (response.status = 200) {
        var result = response.responseJSON
        if (result.Status = 1) {
            if (sucfun) {
                layer.alert(result.Message, { icon: 1 },
                    function (index) {
                        sucfun()
                        layer.close(index);
                    });
            }
            else {
                layer.alert(result.Message, { icon: 1 });
            }
        }
        else {
            if (failedfun) {
                layer.alert(result.Message, { icon: 2 },
                    function (index) {
                        failedfun();
                        layer.close(index);
                    });
            }
            else {
                layer.alert(result.Message, { icon: 2 });
            }
        }
    }
    else {
        layer.alert('提交失败', { icon: 2 });
    }
}

function AjaxComplete(data, sucfun, failedfun) {
    if (data.Status = 1) {
        if (sucfun) {
            layer.alert(data.Message, { icon: 1 },
                function (index) {
                    sucfun()
                    layer.close(index);
                });
        }
        else {
            layer.alert(data.Message, { icon: 1 });
        }
    }
    else {
        if (failedfun) {
            layer.alert(data.Message, { icon: 2 },
                function (index) {
                    failedfun();
                    layer.close(index);
                });
        }
        else {
            layer.alert(data.Message, { icon: 2 });
        }
    }
}

function LayuiTableRender(options) {
    var defaultoptions = {
        method: "post",
        contentType: "application/json",
        page: true,
        request: {
            pageName: 'pageIndex', //页码的参数名称，默认：page
            limitName: 'pageSize' //每页数据量的参数名，默认：limit
        },
        response: {
            statusName: 'Status',
            msgName: 'Message',
            countName: 'TotalCounts', //规定数据总数的字段名称，默认：count
            dataName: 'Data', //规定数据列表的字段名称，默认：data
        }
    };
    
    if (options.searchform) {
        var whereJson = GetFormJson("userSearchForm");
        options.where = whereJson;
    }

    $.extend(defaultoptions, options); //合并对象，修改第一个对象

    var table = layui.table.render(defaultoptions);

    return table;
}

function GetFormJson(formId) {
    var o = {};
    var a = $("#" + formId).serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

(function (window, $) {
    $.fn.serializeJson = function () {
        var serializeObj = {};
        var array = this.serializeArray();
        var str = this.serialize();
        $(array).each(
            function () {
                if (serializeObj[this.name]) {
                    if ($.isArray(serializeObj[this.name])) {
                        serializeObj[this.name].push(this.value);
                    } else {
                        serializeObj[this.name] = [
                            serializeObj[this.name], this.value];
                    }
                } else {
                    serializeObj[this.name] = this.value;
                }
            });

        //下面代码处理checkboxfor会出来一个同name的hidden元素的问题
        for (var key in serializeObj) {
            var ele = serializeObj[key];
            if (ele.length == 2 && ele[0] == "true" && ele[1] == "false") {
                if ($("#" + key) && $("#" + key).attr("type") == "checkbox")
                    serializeObj[key] = "true";
            }
        }
        return serializeObj;
    };
})(window, jQuery);

function CreateTips() {
    var titles = $("[tiptitle]");
    titles.each(function (index, item) {
        var tips;
        $(item).on("mouseover", function () {
            var title = $(item).attr("tiptitle");
            tips = layer.tips(title, $(item), {
                tips: [1, '#3595CC'],
                time: 0,
                tipsMore: true
            });
        });

        $(item).on("mouseout", function () {
            layer.close(tips);
        });
    });
}