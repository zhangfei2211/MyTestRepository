using Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model
{
    public class OrderCondition
    {
        public string OrderbyField { get; set; }

        public SortStatus SortStatus { get; set; }
    }
}
