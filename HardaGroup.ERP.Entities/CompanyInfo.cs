using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.Entities
{
    public class CompanyInfo
    {
        public int Id { get; set; }
        
        /// <summary>
        /// 公司代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }

        public CompanyInfo()
        {
            Users = new List<User>();
        }
    }
}
