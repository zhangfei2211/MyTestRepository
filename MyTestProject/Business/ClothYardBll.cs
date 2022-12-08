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
using Entities.Model.Business;

namespace Business
{
    internal class ClothYardBll : BaseBll, IClothYardBll
    {
        public ClothYardBll(IBaseRepository<B_ClothYard> _clothYardDal,
            IBaseRepository<B_ClothYardWeightList> _clothYardWeightListDal,
            IBaseRepository<B_ClothYardPaymentRecord> _clothYardPaymentRecordDal,
            IBaseRepository<B_SN> _snDal,
            IBaseRepositoryForSql _sqlDal)
        {
            clothYardDal = _clothYardDal;
            clothYardWeightListDal = _clothYardWeightListDal;
            clothYardPaymentRecordDal = _clothYardPaymentRecordDal;
            snDal = _snDal;
            sqlDal = _sqlDal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clothYardId"></param>
        /// <returns></returns>
        public async Task<B_ClothYard> GetClothYardById(Guid clothYardId)
        {
            return await clothYardDal.FindAsync(d => d.Id == clothYardId);
        }

        public async Task<PageResult<B_ClothYard>> GetClothYardList(PageSearchModel searchModel, ClothYardSearch search)
        {
            if (search == null)
            {
                search = new ClothYardSearch();//如果search为空，则new一个，避免写判断
            }

            var whereLambda = GetExpression<B_ClothYard>();

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
                whereLambda = whereLambda.And(d => d.Colour.Contains(search.Colour));
            }

            if (search.StartReportTime.IsNotNull())
            {
                var date = search.StartReportTime.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                var startdate = Convert.ToDateTime(date);
                whereLambda = whereLambda.And(d => d.ReportTime >= startdate);
            }

            if (search.EndReportTime.IsNotNull())
            {
                var date = search.EndReportTime.Value.ToString("yyyy-MM-dd") + " 23:59:59";
                var enddate = Convert.ToDateTime(date);
                whereLambda = whereLambda.And(d => d.ReportTime <= enddate);
            }

            if (search.IsDelivery.IsNotNull())
            {
                whereLambda = whereLambda.And(d => d.IsDelivery == search.IsDelivery.Value);
            }

            if (search.IsPaymentAll.IsNotNull())
            {
                whereLambda = whereLambda.And(d => d.IsPaymentAll == search.IsPaymentAll.Value);
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

            return await clothYardDal.FindPageListAsync(searchModel, whereLambda);
        }

        public async Task<IQueryable<ClothYardUnitPrice>> GetClothYardUnitPriceByCustomerId(string customerId)
        {
            var sql = @"select distinct top(20) b.DictionaryName as ClothTypeName, a.ReportTime, a.Colour,a.UnitPrice from B_ClothYard a
                        left join B_Dictionary b on a.ClothType=b.Id
                      where CustomerId = '" + customerId + @"'
                      order by ReportTime desc,ClothTypeName,Colour,UnitPrice";
            return await sqlDal.FindListBySQLAsync<ClothYardUnitPrice>(sql);
        }

        public async Task<PageResult<ClothYardMainReport>> GetClothYardMainReport(PageSearchModel searchModel, ClothYardMainReportSearch search)
        {
            string whereString = " where a.IsDelete='false' and b.IsDelete='false' ";

            if (search.CustomerId.IsNotNull())
            {
                whereString += " and a.CustomerId='" + search.CustomerId.ToString() + "' ";
            }

            if (search.StartReportTime.IsNotNull())
            {
                var date = search.StartReportTime.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                whereString += " and a.ReportTime >='" + date + "' ";
            }

            if (search.EndReportTime.IsNotNull())
            {
                var date = search.EndReportTime.Value.ToString("yyyy-MM-dd") + " 23:59:59";
                whereString += " and a.ReportTime <='" + date + "' ";
            }

            var sql = @"select  b.Id,b.CustomerName,Sum(a.[Count]) as TotalCount,SUM(a.TotalPrice) as TotalPrice from 
                                B_ClothYard a
                                left join B_Customer b on a.CustomerId=b.Id" + whereString +
                                @"group by b.Id,b.CustomerName";

            //用于分页的排序字段
            searchModel.OrderBy= " TotalCount desc,Id";

            return await sqlDal.FindPageListBySQLAsync<ClothYardMainReport>(sql,searchModel);
        }

        public async Task<IQueryable<B_ClothYardWeightList>> GetClothYardWeightListByClothYardId(Guid clothYardId)
        {
            return await clothYardWeightListDal.FindListAsync(d => d.ClothYardId == clothYardId && !d.IsDelete);
        }

        public async Task<IQueryable<B_ClothYardPaymentRecord>> GetB_ClothYardPaymentRecordListByClothYardId(Guid clothYardId)
        {
            return await clothYardPaymentRecordDal.FindListAsync(d => !d.IsDelete);
        }

        public async Task<bool> SaveClothYardWeightList(Guid clothYardId, List<float> weightList)
        {
            return true;
        }

        public async Task<bool> SaveClothYardPaymentList(Guid clothYardId, List<B_ClothYardPaymentRecord> paymentList)
        {
            return true;
        }

        public async Task<bool> SaveClothYard(List<B_ClothYard> clothYardList)
        {
            foreach (var clothYard in clothYardList)
            {
                
                if (clothYardDal.Find(d => d.Id == clothYard.Id) == null)
                {
                    clothYard.Id = Guid.NewGuid();

                    clothYard.SN = await GetSN();
                    //用lothYard列存储，删除行存储
                    //foreach (var c in clothYardWeightList)
                    //{
                    //    c.Id = Guid.NewGuid();
                    //    c.ClothYardId = clothYard.Id;
                    //    await clothYardWeightListDal.AddAsync(c, false);
                    //}
                    await clothYardDal.AddAsync(clothYard, false);
                }
                else
                {
                    await clothYardDal.UpdateAsync(clothYard, false);

                    ////先删除原码单明细
                    //var clothYardWeightListOld = await clothYardWeightListDal.FindListAsync(d => d.ClothYardId == clothYard.Id);
                    //foreach (var old in clothYardWeightListOld)
                    //{
                    //    await clothYardWeightListDal.DeletePhysicalDataAsync(old, false);
                    //}

                    ////添加新码单明细
                    //foreach (var c in clothYardWeightList)
                    //{
                    //    c.Id = Guid.NewGuid();
                    //    c.ClothYardId = clothYard.Id;
                    //    await clothYardWeightListDal.AddAsync(c, false);
                    //}
                }
            }
            return await clothYardDal.SaveChangesAsync();
        }

        public async Task<bool> DeleteClothYard(Guid clothYardId)
        {
            var clothYard = await clothYardDal.FindAsync(d => d.Id == clothYardId);
            return await clothYardDal.DeleteAsync(clothYard);
        }

        public async Task<bool> DeleteClothYardWeightList(Guid clothYardWeightId)
        {
            return await clothYardWeightListDal.DeleteByIdAsync(clothYardWeightId);
        }

        public async Task<bool> DeleteClothYardPaymentRecord(Guid roleTypeId)
        {
            return await roleTypeDal.DeleteByIdAsync(roleTypeId);
        }

        private async Task<string> GetSN()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            var sn = await snDal.FindAsync(d => d.Year == year && d.Month == month && d.Type == "ClothYard");
            if (sn.IsNull())
            {
                sn = new B_SN
                {
                    Id = Guid.NewGuid(),
                    Year = year,
                    Month = month,
                    Number = 1,
                    Type = "ClothYard",
                    CreateDate= DateTime.Now
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
