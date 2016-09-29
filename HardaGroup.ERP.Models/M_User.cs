using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.Models
{
    public class M_User
    {

    }

    [Serializable]
    public class LoginUser
    {
        public string UserName { get; set; }
        public string CompanyCode { get; set; }
        public string UserType { get; set; }
    }
}
