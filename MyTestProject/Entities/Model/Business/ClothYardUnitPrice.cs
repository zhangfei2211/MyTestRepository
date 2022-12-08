using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model.Business
{
    public class ClothYardUnitPrice
    {
        /// <summary>
        /// 布匹类型
        /// </summary>
        public string ClothTypeName { get; set; }

        /// <summary>
        /// 报单时间
        /// </summary>
        public DateTime ReportTime { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public string Colour { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public Decimal UnitPrice { get; set; }
    }
}
