using Entities;
using Entities.Model.Common;
using IDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class BaseBll
    {
        protected IBaseRepository<B_User> userDal;

        protected IBaseRepository<B_UserToken> userTokenDal;

        protected IBaseRepository<B_Menu> menuDal;

        protected IBaseRepository<B_Role> roleDal;

        protected IBaseRepository<B_RoleType> roleTypeDal;

        protected IBaseRepository<B_RoleMenu> roleMenuDal;

        protected IBaseRepository<B_UserRole> userRoleDal;

        protected IBaseRepository<D_Factory> factoryDal;

        protected IBaseRepository<B_Customer> customerDal;

        protected IBaseRepository<B_Dictionary> dictionaryDal;

        protected IBaseRepository<B_DictionaryType> dictionaryTypeDal;

        protected IBaseRepository<B_ClothYard> clothYardDal;

        protected IBaseRepository<B_ClothYardWeightList> clothYardWeightListDal;

        protected IBaseRepository<B_ClothYardPaymentRecord> clothYardPaymentRecordDal;

        protected IBaseRepository<B_MeterSampleBill> meterSampleDal;

        protected IBaseRepository<B_MeterSampleList> meterSampleListDal;

        protected IBaseRepository<View_MeterSampleList> v_meterSampleListDal;

        protected IBaseRepository<B_SN> snDal;

        /// <summary>
        /// 专用于执行sql
        /// </summary>
        protected IBaseRepositoryForSql sqlDal;

        /// <summary>
        /// 获取空lamda
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected Expression<Func<T, bool>> GetExpression<T>()
        {
            Expression<Func<T, bool>> exp = d => 1 == 1;
            return exp;
        }
    }
}
