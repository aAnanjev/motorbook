using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Data.Entity;

namespace CarFixed.DS.DAL
{
    public interface IGenericDataRepository<T>
        where T : class
    //where E : BaseEntities, new()
    {
        IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties);
        IList<T> GetAllStrParams(params string[] navigationProperties);
        IList<T> GetList(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties);
        IList<T> GetListStrParams(Expression<Func<T, bool>> where, params string[] navigationProperties);
        T GetSingle(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties);
        T GetSingleStrParams(Expression<Func<T, bool>> where, params string[] navigationProperties);
        void Add(params T[] items);
        void Update(params T[] items);
        void Remove(params T[] items);
    }
}
