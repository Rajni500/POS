using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using POS.Contract.Core;
using POS.Models;
using POS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace POS.Repositories.Core
{
    public abstract class GenericRepository<TM> : IGenericRepository<TM>
           where TM : ModelBase
    {
        private POSDBContext entities;
        public IConfiguration Configuration;

        public GenericRepository(POSDBContext context)
        {
            entities = context;
        }

        protected POSDBContext Context { get => entities; set => entities = value; }

        public TM Add(TM entity)
        {
            Context.Set<TM>().Add(entity);
            Save();
            Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public IList<TM> AddList(IList<TM> entity)
        {
            Context.Set<TM>().AddRange(entity);
            Save();
            Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public void Edit(TM entity, bool hasMap = false)
        {
            Context.Set<TM>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;

            Save();

            Context.Entry(entity).State = EntityState.Detached;
        }

        public void EditList(IList<TM> entityList)
        {
            foreach (var item in entityList)
            {
                Edit(item);
            }
        }

        public void Delete(Expression<Func<TM, bool>> predicate, int deletedBy)
        {
            var item = Context.Set<TM>().Where(predicate).FirstOrDefault();

            Context.Entry(item).State = EntityState.Deleted;

            Save();
        }

        public void DeleteList(Expression<Func<TM, bool>> predicate, int deletedBy)
        {
            var item = Context.Set<TM>().Where(predicate).ToList();

            Context.Entry(item).State = EntityState.Deleted;

            Save();
        }

        public virtual void Save()
        {
            Context.SaveChanges();
        }

        public int GetTotalNoOfRecords(Expression<Func<TM, bool>> predicate, SearchViewModel searchViewModel)
        {
            var count = Context.Set<TM>().Where(predicate).Count();

            return count;
        }

        public List<TM> FindBy(Expression<Func<TM, bool>> predicate, SearchViewModel searchViewModel)
        {
            List<TM> query = new List<TM>();

            var list = Context.Set<TM>().Where(predicate).AsQueryable();

            list = IncludeResolvePropertyNames(searchViewModel, list);

            if (searchViewModel.PageSize != 0)
            {
                query = list.Skip(searchViewModel.PageSize * searchViewModel.PageIndex)
                    .Take(searchViewModel.PageSize).ToList();
            }
            else
            {
                query = list.ToList();
            }

            return query.ToList();
        }

        public TM SingleOrDefault(Expression<Func<TM, bool>> predicate, SearchViewModel search = null)
        {
            var itemList = Context.Set<TM>().Where(predicate).AsQueryable();

            itemList = IncludeResolvePropertyNames(search, itemList);

            var item = itemList.FirstOrDefault();

            return item;
        }

        public static IQueryable<TM> IncludeResolvePropertyNames(SearchViewModel searchViewModel, IQueryable<TM> ent)
        {
            if (searchViewModel != null && !string.IsNullOrWhiteSpace(searchViewModel.ResolvePropertyNames))
            {
                foreach (var includeProperty in searchViewModel.ResolvePropertyNames.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    ent = ent.Include(includeProperty);
                }
            }

            return ent;
        }
    }
}
