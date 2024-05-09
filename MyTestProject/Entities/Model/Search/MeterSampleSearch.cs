using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model.Search
{
    public class MeterSampleSearch
    {
        /// <summary>
        /// 客户
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// 是否已付款
        /// </summary>
        public bool? IsPayment { get; set; }

        ///// <summary>
        ///// 布匹种类
        ///// </summary>
        //public Guid ClothType { get; set; }

        ///// <summary>
        ///// 颜色
        ///// </summary>
        //public Guid Colour { get; set; }


        /// <summary>
        /// 起始发货时间
        /// </summary>
        public DateTime? StartDeliveryTime { get; set; }

        /// <summary>
        /// 截止发货时间
        /// </summary>
        public DateTime? EndDeliveryTime { get; set; }
    }
}
