using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utlis.Autofac;
using Entities;
using Entities.Model.Common;
using Entities.Model.Search;

namespace IBusiness
{
    public interface ICustomerBll : IAutofac
    {
        /// <summary>
        /// 获取所以客户名单，非删除
        /// </summary>
        /// <returns></returns>
        Task<List<B_Customer>> GetCustomerAll();

        Task<PageResult<B_Customer>> GetCustomerList(PageSearchModel searchModel, CustomerSearch search);

        Task<B_Customer> GetCustomerById(Guid customerId);

        Task<B_Customer> GteCustomerByCustomerName(string customerName);

        Task<bool> SaveCustomer(B_Customer customer);

        Task<bool> DeleteCustomer(Guid customerId);
    }
}
