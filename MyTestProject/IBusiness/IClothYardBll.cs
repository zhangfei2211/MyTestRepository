using Entities;
using Entities.Model.Business;
using Entities.Model.Common;
using Entities.Model.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utlis.Autofac;
namespace IBusiness
{
    public interface IClothYardBll : IAutofac
    {
        Task<B_ClothYard> GetClothYardById(Guid clothYardId);

        Task<PageResult<B_ClothYard>> GetClothYardList(PageSearchModel searchModel, ClothYardSearch search);

         Task<IQueryable<ClothYardUnitPrice>> GetClothYardUnitPriceByCustomerId(string customerId);

        Task<PageResult<ClothYardMainReport>> GetClothYardMainReport(PageSearchModel searchModel, ClothYardMainReportSearch search);

        Task<IQueryable<B_ClothYardWeightList>> GetClothYardWeightListByClothYardId(Guid clothYardId);

        Task<IQueryable<B_ClothYardPaymentRecord>> GetB_ClothYardPaymentRecordListByClothYardId(Guid clothYardId);

        Task<bool> SaveClothYardWeightList(Guid clothYardId, List<float> weightList);

        Task<bool> SaveClothYardPaymentList(Guid clothYardId, List<B_ClothYardPaymentRecord> paymentList);

        Task<bool> SaveClothYard(List<B_ClothYard> clothYardList);

        Task<bool> DeleteClothYard(Guid roleId);

        Task<bool> DeleteClothYardWeightList(Guid roleTypeId);

        Task<bool> DeleteClothYardPaymentRecord(Guid roleTypeId);

    }
}
