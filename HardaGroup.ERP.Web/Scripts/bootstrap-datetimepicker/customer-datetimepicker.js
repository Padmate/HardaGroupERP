/*****************************
使用示例

<div class="input-group date form_datetime" data-date=""
data-date-format="yyyy-mm-dd hh:ii:ss" data-link-field="tbPubTime">
<input class="form-control" size="16" type="text" value="@DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")" readonly>
<span class="input-group-addon"><span class="glyphicon glyphicon-th"></span></span>
</div>
<input type="hidden" id="tbPubTime" name="PubTime" value="@DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")" /><br />

*******************/

///参数datetimepicker 可以是id或者class
function InitDatetimepicker(datetimepicker)
{

    //时间控件
    $(datetimepicker).datetimepicker({
        weekStart: 1,
        todayBtn: 1,
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        forceParse: 0,
        showMeridian: 1,
        language: 'zh-CN'
    });

    //datetimepicker 关闭事件执行时会导致modal的关闭事件触发，添加一下代码防止触发bootstrap modal关闭事件
    $(datetimepicker).datetimepicker().on('changeDate', function (ev) {
        //
    }).on('hide', function (event) {
        event.preventDefault();
        event.stopPropagation();
    });
}

