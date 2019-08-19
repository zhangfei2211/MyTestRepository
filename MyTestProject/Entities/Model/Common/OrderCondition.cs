using Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model.Common
{
    public class OrderCondition
    {
        public string OrderbyField { get; set; }

        public bool IsAsc { get; set; }
    }
}
