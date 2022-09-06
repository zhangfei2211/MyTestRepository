using Entities;
using IBusiness;
using IDal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Model;
using Entities.Enum;
using Utlis.Extension;
using Entities.Model.Common;
using Utlis;
using Entities.Model.Search;


namespace Business
{
    internal class MeterSampleBll : BaseBll, IMeterSampleBll
    {
        public MeterSampleBll(IBaseRepository<B_MeterSampleBill> _meterSampleDal)
        {
            meterSampleDal = _meterSampleDal;
        }

        public async Task<B_MeterSampleBill> GetMeterSampleById(Guid meterSampleId) 
        {
            return await meterSampleDal.FindAsync(d => d.Id == meterSampleId);
        }

        public async Task<PageResult<B_MeterSampleBill>> GetMeterSampleList(PageSearchModel searchModel, MeterSampleSearch search)
        {
            if (search == null)
            {
                search = new MeterSampleSearch();//如果search为空，则new一个，避免写判断
            }

            var whereLambda = GetExpression<B_MeterSampleBill>();

            whereLambda = whereLambda.And(d => !d.IsDelete);

            if (search.CustomerId.IsNotNull())
            {
                whereLambda = whereLambda.And(d => d.CustomerId == search.CustomerId);
            }

            if (search.ClothType.IsNotNull())
            {
                whereLambda = whereLambda.And(d => d.ClothType == search.ClothType);
            }

            if (search.Colour.IsNotNull())
            {
                whereLambda = whereLambda.And(d => d.ColourId == search.Colour);
            }

            if (search.StartDeliveryTimeTime.IsNotNull())
            {
                var date = search.StartDeliveryTimeTime.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                var startdate = Convert.ToDateTime(date);
                whereLambda = whereLambda.And(d => d.DeliveryTime >= startdate);
            }

            if (search.EndDeliveryTimeTime.IsNotNull())
            {
                var date = search.EndDeliveryTimeTime.Value.ToString("yyyy-MM-dd") + " 23:59:59";
                var enddate = Convert.ToDateTime(date);
                whereLambda = whereLambda.And(d => d.DeliveryTime <= enddate);
            }

            return await meterSampleDal.FindPageListAsync(searchModel, whereLambda);
        }

        public async Task<bool> SaveMeterSample(B_MeterSampleBill meterSample)
        {
            if (meterSample.Id.IsNull())
            {
                meterSample.Id = Guid.NewGuid();
                return await meterSampleDal.AddAsync(meterSample);
            }
            else
            {
                return await meterSampleDal.UpdateAsync(meterSample);
            }
        }

        public async Task<bool> DeleteMeterSample(Guid meterSampleId)
        {
            var meterSample = await meterSampleDal.FindAsync(d => d.Id == meterSampleId);
            return await meterSampleDal.DeleteAsync(meterSample);
        }
    }
}
