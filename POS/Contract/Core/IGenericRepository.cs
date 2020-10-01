using POS.Models;
using POS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace POS.Contract.Core
{
    public interface IGenericRepository<TD>
        where TD : ModelBase
    {
        TD Add(TD entity);
        IList<TD> AddList(IList<TD> entity);
        void Edit(TD entity, bool hasMap = false);
        void EditList(IList<TD> entityList);
        void Delete(Expression<Func<TD, bool>> predicate, int deletedBy);
        void DeleteList(Expression<Func<TD, bool>> predicate, int deletedBy);
        void Save();
        int GetTotalNoOfRecords(Expression<Func<TD, bool>> predicate, SearchViewModel searchViewModel);
        List<TD> FindBy(Expression<Func<TD, bool>> predicate, SearchViewModel searchViewModel);
        TD SingleOrDefault(Expression<Func<TD, bool>> predicate, SearchViewModel search = null);
    }
}
