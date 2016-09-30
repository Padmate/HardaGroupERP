using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.Models
{
    public class BaseModel
    {
        /// <summary>
        /// 用于bootstrap table field 占位
        /// </summary>
        public string ButtonColumn { get; set; }
        #region EXT
        //public int page { get; set; }
        //public int limit { get; set; }
        #endregion
        #region BootStrap Tables
        public int limit { get; set; }
        //偏移量
        public int offset { get; set; }
        #endregion

        #region BootStrap Paginator

        /// <summary>
        /// 当前页码
        /// </summary>
        public int page { get; set; }
        #endregion

        #region 实体验证方法
        public Message validate()
        {
            Message message = new Message();
            message = validate(this);
            return message;
        }

        /// <summary>
        /// 验证视图数据属性，可递归调用
        /// </summary>
        /// <param name="obj">视图数据对象</param>
        /// <param name="retMsg">返回消息</param>
        /// <returns></returns>
        public Message validate(BaseModel obj)
        {
            Message message = new Message();
            message.Success = true;

            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance); ;
            if (properties != null)
            {
                foreach (var property in properties)
                {
                    ///拥有的验证特性
                    object[] objs = property.GetCustomAttributes(typeof(ValidationAttribute), true);
                    foreach (var attrib in objs)
                    {
                        ValidationAttribute vAttrib = attrib as ValidationAttribute;
                        if (vAttrib != null)
                        {
                            ///验证
                            if (!vAttrib.IsValid(property.GetValue(obj, null)))
                            {
                                message.Content = vAttrib.ErrorMessage + "<br/><br/>";
                                message.Success = false;
                                return message;
                            }
                        }
                        //也属于需要验证的对象
                        if (property.GetType() == typeof(BaseModel))
                        {
                            //递归验证
                            message = validate(property.GetValue(obj, null) as BaseModel);
                            if (!message.Success)
                                return message;
                        }
                    }
                }
            }

            return message;
        }

        #endregion

    }
}
