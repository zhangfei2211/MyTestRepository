using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model
{
    public class PagedResult<T>
    {
        #region Public Properties
        /// <summary>
        /// 总数据数
        /// </summary>
        public int TotalCounts { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 当前页数据
        /// </summary>
        public List<T> Data { get; set; }
        #endregion

        #region Public Fields
        /// <summary>
        /// 获取一个当前类型的空值
        /// </summary>
        public static readonly PagedResult<T> Empty = new PagedResult<T>(0, 0, 0, 0, null);
        #endregion

        #region 构造
        public PagedResult()
        {
            Data = new List<T>();
        }

        public PagedResult(int totalCounts, int totalPages, int pageSize, int pageIndex, List<T> data)
        {
            this.TotalPages = totalPages;
            this.TotalCounts = totalCounts;
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.Data = data;
        }
        #endregion

    }
}
