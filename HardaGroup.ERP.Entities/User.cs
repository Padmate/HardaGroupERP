using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public int WebERPUserTypeId { get; set; }

        public virtual UserType UserType { get; set; }

        public int WebERPCompanyInfoId { get; set; }
        public virtual CompanyInfo CompanyInfo { get; set; }


    }
}
