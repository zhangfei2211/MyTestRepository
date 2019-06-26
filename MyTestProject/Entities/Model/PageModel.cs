using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model
{
    public class PageModel
    {
        public int PageSize;

        public int PageIndex;

        public OrderField[] OrderFileds;
    }
}
