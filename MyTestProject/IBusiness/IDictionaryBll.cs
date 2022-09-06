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
    public interface IDictionaryBll : IAutofac
    {
        Task<B_DictionaryType> GetDictionaryTypeById(Guid dictionaryTypeId);

        Task<B_Dictionary> GetDictionaryById(Guid dictionaryId);

        Task<IQueryable<B_Dictionary>> GetDictionaryListAll();

        Task<PageResult<B_Dictionary>> GetDictionaryList(PageSearchModel searchModel, DictionarySearch search);

        Task<IQueryable<B_Dictionary>> GetDictionaryListByDictionaryTypeCode(string dictionaryTypeCode);

        Task<PageResult<B_DictionaryType>> GetDictionaryTypeList(PageSearchModel searchModel, DictionaryTypeSearch search);

        Task<List<B_DictionaryType>> GetDictionaryTypeAll();

        Task<bool> SaveDictionary(B_Dictionary dictionary);

        Task<bool> SaveDictionaryType(B_DictionaryType dictionaryType);

        Task<bool> DeleteDictionary(Guid dictionaryId);

        Task<bool> DeleteDictionaryType(Guid dictionaryTypeId);
    }
}
