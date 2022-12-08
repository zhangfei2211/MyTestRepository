using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model.Common
{
    public class PageSearchModel
    {
        public int PageSize;

        public int PageIndex;

        public List<OrderCondition> OrderConditions;

        //排序字段，用于sql分页查询
        public string OrderBy;
    }
}
