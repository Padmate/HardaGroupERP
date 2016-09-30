//为避免冲突，将我们的方法用一个匿名方法包裹起来
(function ($) {

    //扩展这个方法到jquery
    $.fn.extend({

        //插件名字
        loadtree: function (options) {

            //设置默认值并用逗号隔开
            var defaults = {  
                url: ''
             }  
                  
            var options = $.extend(defaults, options);

            //遍历匹配元素的集合
            return this.each(function () {

                var o = options;
                //将元素集合赋给变量
                var obj = $(this);
                
                var dynamicHtml = "";
                jQuery.ajax({
                    type: "post",//使用get方法访问后台
                    dataType: "json",//返回json格式的数据
                    url: o.url,
                    async: false,
                    success: function (data) {
                       
                        if(data !=null && data.length >0)
                        {
                            for (var i = 0; i < data.length;i++)
                            {
                                dynamicHtml += generatelist(data[i]);

                            }
                        }

                    }
                });

                obj.html(dynamicHtml);

            });
        }
    });

    //传递jQuery到方法中，这样我们可以使用任何javascript中的变量来代替"$"      
})(jQuery);


function generatelist(data)
{
    var dynamicHtml ="";
    if (data != null)
    {

        //是否最后一个节点
        if(data.leaf)
        {
            dynamicHtml += '<li>';
            dynamicHtml += '<a class="J_menuItem" href="' + data.href + '" data-id="'+data.id+'">';
            //dynamicHtml += '<i class="'+data.iconCls+'"></i>';
            dynamicHtml += '<span class="nav-label">'+data.text+'</span>';
            dynamicHtml += '</a>';
            dynamicHtml += '</li>';

        }else{
            dynamicHtml += '<li>';
            dynamicHtml += '<a href="#">';
            dynamicHtml += '<i class="' + data.iconCls + '"></i>';
            dynamicHtml += '<span class="nav-label">' + data.text + '</span>';
            dynamicHtml += '<span class="fa arrow"></span>';
            dynamicHtml += '</a>';
            dynamicHtml += '<ul class="nav nav-second-level">';

            if(data.children != null && data.children.length >0)
            {
                for (var i = 0; i < data.children.length; i++) {
                    dynamicHtml += generatelist(data.children[i]);
                }
            }
            

            dynamicHtml += '</ul>';
            dynamicHtml += '</li>';
        }

        return dynamicHtml;
    }
}