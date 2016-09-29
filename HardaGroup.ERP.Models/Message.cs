using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.Models
{
    public class Message
    {
        public bool Success { get; set; }

        public string Content { get; set; }

        public int? ReturnId { get; set; }

        public string ReturnStrId { get; set; }
    }
}
