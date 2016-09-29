using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.Models
{
    public class M_Tree
    {
        public string id { get; set; }

        /// <summary>
        /// 本节点标签上的文本。
        /// </summary>
        public string text { get; set; }

        /// <summary>
        ///iframe src 
        /// </summary>
        public string href { get; set; }

        /// <summary>
        /// 设置为true表明本节点没有子节点
        /// </summary>
        public bool leaf { get; set; }

        /// <summary>
        /// True如果节点是展开的。
        /// </summary>
        public bool expanded { get; set; }

        /// <summary>
        /// 应用于本节点的图标的URL
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 应用于本节点的图标的CSS类
        /// </summary>
        public string iconCls { get; set; }

        public List<M_Tree> children { get; set; }
    }
}
