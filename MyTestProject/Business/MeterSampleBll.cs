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
        public MeterSampleBll(IBaseRepository<B_MeterSampleBill> _meterSampleDal,
            IBaseRepository<B_MeterSampleList> _meterSampleListDal,
            IBaseRepository<View_MeterSampleList> _v_meterSampleListDal,
            IBaseRepository<B_SN> _snDal)
        {
            meterSampleDal = _meterSampleDal;
            meterSampleListDal = _meterSampleListDal;
            v_meterSampleListDal = _v_meterSampleListDal;
            snDal = _snDal;
        }

        public async Task<B_MeterSampleBill> GetMeterSampleById(Guid meterSampleId) 
        {
            return await meterSampleDal.FindAsync(d => d.Id == meterSampleId);
        }

        public async Task<IQueryable<B_MeterSampleList>> GetMeterSampleChildListById(Guid meterSampleId)
        {
            return await meterSampleListDal.FindListAsync(d => d.MeterSampleBillId == meterSampleId && !d.IsDelete);
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

            if (search.IsPayment.IsNotNull())
            {
                whereLambda = whereLambda.And(d => d.IsPayment == search.IsPayment.Value);
            }

            if (search.StartDeliveryTime.IsNotNull())
            {
                var date = search.StartDeliveryTime.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                var startdate = Convert.ToDateTime(date);
                whereLambda = whereLambda.And(d => d.DeliveryTime >= startdate);
            }

            if (search.EndDeliveryTime.IsNotNull())
            {
                var date = search.EndDeliveryTime.Value.ToString("yyyy-MM-dd") + " 23:59:59";
                var enddate = Convert.ToDateTime(date);
                whereLambda = whereLambda.And(d => d.DeliveryTime <= enddate);
            }

            return await meterSampleDal.FindPageListAsync(searchModel, whereLambda);
        }

        public async Task<PageResult<View_MeterSampleList>> GetMeterSampleStatementList(PageSearchModel searchModel, MeterSampleSearch search)
        {
            if (search == null)
            {
                search = new MeterSampleSearch();//如果search为空，则new一个，避免写判断
            }

            var whereLambda = GetExpression<View_MeterSampleList>();

            whereLambda = whereLambda.And(d => !d.IsDelete);

            if (search.CustomerId.IsNotNull())
            {
                whereLambda = whereLambda.And(d => d.CustomerId == search.CustomerId);
            }

            if (search.StartDeliveryTime.IsNotNull())
            {
                var date = search.StartDeliveryTime.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                var startdate = Convert.ToDateTime(date);
                whereLambda = whereLambda.And(d => d.DeliveryTime >= startdate);
            }

            if (search.EndDeliveryTime.IsNotNull())
            {
                var date = search.EndDeliveryTime.Value.ToString("yyyy-MM-dd") + " 23:59:59";
                var enddate = Convert.ToDateTime(date);
                whereLambda = whereLambda.And(d => d.DeliveryTime <= enddate);
            }

            return await v_meterSampleListDal.FindPageListAsync(searchModel, whereLambda);
        }

        public async Task<bool> SaveMeterSample(B_MeterSampleBill meterSample, List<B_MeterSampleList> list)
        {
            try
            {
                decimal totalPrice = 0;
                if (meterSample.Id.IsNull())
                {
                    meterSample.Id = Guid.NewGuid();
                    meterSample.SN = await GetSN();
                    foreach (var l in list)
                    {
                        l.MeterSampleBillId = meterSample.Id;
                        totalPrice += l.UnitPrice * l.Length;
                        await meterSampleListDal.AddAsync(l, false);
                    }
                    meterSample.TotalPrice = totalPrice;
                    await meterSampleDal.AddAsync(meterSample, false);
                }
                else
                {
                    //先删除原米样明细
                    var meterSampleListOld = await meterSampleListDal.FindListAsync(d => d.MeterSampleBillId == meterSample.Id);
                    foreach (var old in meterSampleListOld)
                    {
                        await meterSampleListDal.DeletePhysicalDataAsync(old, false);
                    }

                    foreach (var l in list)
                    {
                        l.MeterSampleBillId = meterSample.Id;
                        totalPrice += l.UnitPrice * l.Length;

                        await meterSampleListDal.AddAsync(l, false);
                    }

                    meterSample.TotalPrice = totalPrice;

                    await meterSampleDal.UpdateAsync(meterSample, false);
                }

                return await meterSampleDal.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> PaymentMeterSample(B_MeterSampleBill meterSample)
        {
            return await meterSampleDal.UpdateAsync(meterSample);
        }

        public async Task<bool> DeleteMeterSample(Guid meterSampleId)
        {
            var meterSample = await meterSampleDal.FindAsync(d => d.Id == meterSampleId);
            return await meterSampleDal.DeleteAsync(meterSample);
        }

        private async Task<string> GetSN()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            var sn = await snDal.FindAsync(d => d.Year == year && d.Month == month && d.Type == "MeterSample");
            if (sn.IsNull())
            {
                sn = new B_SN
                {
                    Id = Guid.NewGuid(),
                    Year = year,
                    Month = month,
                    Number = 1,
                    Type = "MeterSample",
                    CreateDate = DateTime.Now
                };

                await snDal.AddAsync(sn, false);
            }
            else
            {
                sn.Number++;

                await snDal.UpdateAsync(sn, false);
            }

            return sn.Year.ToString() + sn.Month.ToString().PadLeft(2, '0') + sn.Number.ToString().PadLeft(4, '0');
        }
    }
}
