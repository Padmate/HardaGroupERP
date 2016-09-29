using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.Utility
{
    public class Common
    {
        /// <summary>
        /// 用于存放用户信息的SessionID
        /// </summary>
        public const string SessionLoginUser = "hardaerploginsession";

        /// <summary>
        /// 用于密码在客户端加密的的Salt
        /// </summary>
        public const string Password_Client_Salt = "lajsldkfjlaksdflkhalksdf";

        /// <summary>
        /// 系统默认登录用户
        /// 默认密码为:123456
        /// </summary>
        public const string DefaultLoginUser = "SysAdmin";
    }
}
