using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model.Search
{
    public class ClothYardMainReportSearch
    {
        /// <summary>
        /// 客户
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// 起始发货时间
        /// </summary>
        public DateTime? StartReportTime { get; set; }

        /// <summary>
        /// 截止发货时间
        /// </summary>
        public DateTime? EndReportTime { get; set; }
    }
}
