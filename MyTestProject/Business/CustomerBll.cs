using Entities;
using Entities.Model.Common;
using Entities.Model.Search;
using IBusiness;
using IDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utlis.Extension;

namespace Business
{
    public class CustomerBll: BaseBll,ICustomerBll
    {

        public CustomerBll(IBaseRepository<B_Customer> _customerDal)
        {
            customerDal = _customerDal;
        }
        public async Task<List<B_Customer>> GetCustomerAll()
        {
            return (await customerDal.FindListAsync(d => d.IsDelete == false)).ToList();
        }

        public async Task<PageResult<B_Customer>> GetCustomerList(PageSearchModel searchModel, CustomerSearch search)
        {
            if (search == null)
            {
                search = new CustomerSearch();//如果search为空，则new一个，避免写判断
            }

            var whereLambda = GetExpression<B_Customer>();

            if (search.CustomerName.IsNotEmpty())
            {
                whereLambda = whereLambda.And(d => d.CustomerName.Contains(search.CustomerName));
            }

            return await customerDal.FindPageListAsync(searchModel, whereLambda);
        }

        public async Task<B_Customer> GetCustomerById(Guid customerId)
        {
            return await customerDal.FindAsync(d => d.Id == customerId);
        }

        public async Task<B_Customer> GteCustomerByCustomerName(string customerName) 
        {
            return await customerDal.FindAsync(d => d.CustomerName == customerName);
        }

        public async Task<bool> SaveCustomer(B_Customer customer)
        {
            if (customer.Id.IsNull())
            {
                customer.Id = Guid.NewGuid();
                return await customerDal.AddAsync(customer);
            }
            else
            {
                return await customerDal.UpdateAsync(customer);
            }
        }

        public async Task<bool> DeleteCustomer(Guid customerId)
        {
            return await customerDal.DeleteByIdAsync(customerId);
        }
    }
}
