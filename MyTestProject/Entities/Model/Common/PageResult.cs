using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model.Common
{
    public class PageResult<T>
    {
        #region Public Properties
        public int Status;

        public string Message;
        /// <summary>
        /// 总数据数
        /// </summary>
        public int TotalCounts;

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages;

        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize;

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex;
        /// <summary>
        /// 当前页数据
        /// </summary>
        public List<T> Data;
        #endregion

        #region Public Fields
        /// <summary>
        /// 获取一个当前类型的空值
        /// </summary>
        public static readonly PageResult<T> Empty = new PageResult<T>(0,null,0, 0, 0, 0, null);
        #endregion

        #region 构造
        public PageResult()
        {
            Data = new List<T>();
        }

        public PageResult(int status, string message, int totalCounts, int totalPages, int pageSize, int pageIndex, List<T> data)
        {
            Status = status;
            Message = message;
            TotalPages = totalPages;
            TotalCounts = totalCounts;
            PageSize = pageSize;
            PageIndex = pageIndex;
            Data = data;
        }
        #endregion

    }
}
