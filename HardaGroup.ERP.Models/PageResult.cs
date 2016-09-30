using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.Models
{
    public class PageResult<T>
    {
        #region BootStrap Table
        public int total { get; set; }

        public List<T> rows { get; set; }

        public PageResult(int totalCount, List<T> pageRows)
        {
            total = totalCount;
            rows = pageRows;
        }
        #endregion

        #region BootStrap Paginator
        public Int32 totalPages { get; set; }

        public List<T> pageDatas { get; set; }

        public PageResult(Int32 totalCount, Int32 totalpages, List<T> pageResult)
        {
            total = totalCount;
            totalPages = totalpages;
            pageDatas = pageResult;
        }
        #endregion
    }
}
