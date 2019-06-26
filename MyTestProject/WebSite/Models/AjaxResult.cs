using Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public class AjaxResult<T> where T:class
    {
        public AjaxStatus Status;

        public string Message;

        public T Data;
    }
}