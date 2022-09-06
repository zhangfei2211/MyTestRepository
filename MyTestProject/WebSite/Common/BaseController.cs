using IBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utlis;

namespace WebSite.Common
{
    public class BaseController : Controller
    {
        protected IUserBll userBll;

        protected IMenuBll menuBll;

        protected IRoleBll roleBll;

        protected ICustomerBll customerBll;

        protected IDictionaryBll dictionaryBll;

        protected IClothYardBll clothYardBll;

        protected IMeterSampleBll meterSampleBll;

        public static IEnumerable<SelectListItem> GetSelectList<T>(List<T> list, string textName, string valueName, string orderName, string order = "asc")
        {
            List<T> newList = (List<T>)list;

            var types = typeof(T).GetProperties();
            var isExitsOrderName = types.Where(k => k.Name == orderName);//是否存在属性
            if (isExitsOrderName != null || isExitsOrderName.Count() > 0)
            {
                var listType = isExitsOrderName.FirstOrDefault().PropertyType.Name;
                newList.Sort(delegate (T a, T b)
                {
                    var value1 = a.GetType().GetProperty(orderName).GetValue(a, null);
                    var value2 = b.GetType().GetProperty(orderName).GetValue(b, null);

                    IComparable comparableObj = value1 as IComparable;
                    comparableObj = comparableObj ?? value2 as IComparable;

                    return comparableObj != null ? comparableObj.CompareTo(value2) : 0;
                });
                if (order != "asc")
                {
                    newList.Reverse();
                }
            }

            //var isExitsText = types.Where(k => k.Name == textName);//是否存在属性
            //var isExitsValueName = types.Where(k => k.Name == valueName);//是否存在属性

            var slist = newList.Select(d => new SelectListItem
            {
                Text = d.GetType().GetProperty(textName).GetValue(d, null) != null ? d.GetType().GetProperty(textName).GetValue(d, null).ToString() : null,
                Value = d.GetType().GetProperty(valueName).GetValue(d, null) != null ? d.GetType().GetProperty(valueName).GetValue(d, null).ToString() : null,
            }) .ToList();

            slist.Insert(0, new SelectListItem
            {
                Text = "请选择",
                Value = ""
            });

            return slist;
        }
    }
}