using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.Entities
{
    /// <summary>
    /// 用户类别
    /// </summary>
    public class UserType
    {
        public int Id { get; set; }
        
        /// <summary>
        /// 类别代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }

        public UserType()
        {
            Users = new List<User>();
        }
    }
}
