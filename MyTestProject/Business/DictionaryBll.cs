using System;
using IBusiness;
using IDal;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Utlis;
using Utlis.Extension;
using Entities.Model.Common;
using Entities.Model.Search;
using System.Linq.Expressions;

namespace Business
{
    public class DictionaryBll : BaseBll, IDictionaryBll
    {
        public DictionaryBll(IBaseRepository<B_Dictionary> _dictionaryDal,
                IBaseRepository<B_DictionaryType> _dictionaryTypeDal)
        {
            dictionaryDal = _dictionaryDal;
            dictionaryTypeDal = _dictionaryTypeDal;
        }

        public async Task<B_DictionaryType> GetDictionaryTypeById(Guid dictionaryTypeId)
        {
            return await dictionaryTypeDal.FindAsync(d => d.Id == dictionaryTypeId);
        }

        public async Task<B_Dictionary> GetDictionaryById(Guid dictionaryId)
        {
            return await dictionaryDal.FindAsync(d => d.Id == dictionaryId);
        }

        public async Task<IQueryable<B_Dictionary>> GetDictionaryListAll()
        {
            return await dictionaryDal.FindListAsync(d => !d.IsDelete);
        }

        public async Task<PageResult<B_Dictionary>> GetDictionaryList(PageSearchModel searchModel, DictionarySearch search)
        {
            if (search == null)
            {
                search = new DictionarySearch();//如果search为空，则new一个，避免写判断
            }

            var whereLambda = GetExpression<B_Dictionary>();

            if (search.DictionaryName.IsNotEmpty())
            {
                whereLambda = whereLambda.And(d => d.DictionaryName.Contains(search.DictionaryName));
            }

            if (search.DictionaryTypeCode.IsNotNull())
            {
                whereLambda = whereLambda.And(d => d.DictionaryTypeCode == search.DictionaryTypeCode);
            }

            return await dictionaryDal.FindPageListAsync(searchModel, whereLambda);
        }

        public async Task<IQueryable<B_Dictionary>> GetDictionaryListByDictionaryTypeCode(string dictionaryTypeCode)
        {
            return await dictionaryDal.FindListAsync(d => d.DictionaryTypeCode == dictionaryTypeCode);
        }

        public async Task<PageResult<B_DictionaryType>> GetDictionaryTypeList(PageSearchModel searchModel, DictionaryTypeSearch search)
        {
            if (search == null)
            {
                search = new DictionaryTypeSearch();//如果search为空，则new一个，避免写判断
            }

            var whereLambda = GetExpression<B_DictionaryType>();

            if (search.DictionaryTypeName.IsNotEmpty())
            {
                whereLambda = whereLambda.And(d => d.DictionaryTypeName.Contains(search.DictionaryTypeName));
            }

            return await dictionaryTypeDal.FindPageListAsync(searchModel, whereLambda);
        }

        public async Task<List<B_DictionaryType>> GetDictionaryTypeAll()
        {
            return (await dictionaryTypeDal.FindListAsync(d => d.IsDelete == false)).ToList();
        }

        public async Task<bool> SaveDictionary(B_Dictionary dictionary)
        {
            if (dictionary.Id.IsNull())
            {
                dictionary.Id = Guid.NewGuid();
                return await dictionaryDal.AddAsync(dictionary);
            }
            else
            {
                return await dictionaryDal.UpdateAsync(dictionary);
            }
        }

        public async Task<bool> SaveDictionaryType(B_DictionaryType dictionaryType)
        {
            if (dictionaryType.Id.IsNull())
            {
                dictionaryType.Id = Guid.NewGuid();
                return await dictionaryTypeDal.AddAsync(dictionaryType);
            }
            else
            {
                return await dictionaryTypeDal.UpdateAsync(dictionaryType);
            }
        }

        public async Task<bool> DeleteDictionary(Guid dictionaryId)
        {
            return await dictionaryDal.DeleteByIdAsync(dictionaryId);
        }

        public async Task<bool> DeleteDictionaryType(Guid dictionaryTypeId)
        {
            return await dictionaryTypeDal.DeleteByIdAsync(dictionaryTypeId);
        }
    }
}
