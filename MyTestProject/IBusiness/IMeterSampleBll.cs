using Entities;
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
    public interface IMeterSampleBll : IAutofac
    {
        Task<B_MeterSampleBill> GetMeterSampleById(Guid meterSampleId);

        Task<IQueryable<B_MeterSampleList>> GetMeterSampleChildListById(Guid meterSampleId);

        Task<PageResult<B_MeterSampleBill>> GetMeterSampleList(PageSearchModel searchModel, MeterSampleSearch search);

        Task<bool> SaveMeterSample(B_MeterSampleBill meterSample, List<B_MeterSampleList> list);

        Task<bool> PaymentMeterSample(B_MeterSampleBill meterSample);

        Task<bool> DeleteMeterSample(Guid meterSampleId);
    }
}
