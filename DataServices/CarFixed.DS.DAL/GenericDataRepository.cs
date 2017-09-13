using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity.Validation;

using CarFixed.DS.DM;


namespace CarFixed.DS.DAL
{
    public class GenericDataRepository<T> : IGenericDataRepository<T> where T : class
    {
        internal virtual DbContext CreateContext()
        {
            return null;
        }

        public virtual IList<T> GetAllStrParams(params string[] navigationProperties)
        {
            List<T> list;
            using (var context = this.CreateContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (string str in navigationProperties)
                    dbQuery = dbQuery.Include(str);

                list = dbQuery
                    .AsNoTracking()
                    .ToList<T>();
            }
            return list;
        }

        public virtual IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list;
            using (var context = this.CreateContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                list = dbQuery
                   .AsNoTracking()
                   .ToList<T>();
            }
            return list;
        }

        public virtual IList<T> GetListStrParams(Expression<Func<T, bool>> where,
             params string[] navigationProperties)
        {
            List<T> list;
            using (var context = this.CreateContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (string str in navigationProperties)
                    dbQuery = dbQuery.Include(str);

                list = dbQuery
                    .AsNoTracking()
                    .Where(where)
                    .ToList<T>();
            }
            return list;
        }

        public virtual IList<T> GetList(Expression<Func<T, bool>> where,
             params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list;
            using (var context = this.CreateContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .Where(where)
                    .ToList<T>();
            }
            return list;
        }

        public virtual T GetSingleStrParams(Expression<Func<T, bool>> where,
            params string[] navigationProperties)
        {
            T item = null;
            using (var context = this.CreateContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (string str in navigationProperties)
                    dbQuery = dbQuery.Include(str);

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(where); //Apply where clause
            }
            return item;
        }

        public virtual T GetSingle(Expression<Func<T, bool>> where,
            params Expression<Func<T, object>>[] navigationProperties)
        {
            T item = null;
            using (var context = this.CreateContext())
            {

                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);



                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(where); //Apply where clause
            }
            return item;
        }

        public virtual void Add(params T[] items)
        {
            Update(items);
        }

        public virtual void Update(params T[] items)
        {
            using (var context = this.CreateContext())
            {
                DbSet<T> dbSet = context.Set<T>();
                foreach (T item in items)
                {
                    dbSet.Add(item);
                    foreach (DbEntityEntry<IEntity> entry in context.ChangeTracker.Entries<IEntity>())
                    {

                        IEntity entity = entry.Entity;
                        entry.State = GetEntityState(entity.EntityState);
                    }
                }

                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public virtual void Remove(params T[] items)
        {
            Update(items);
        }

        //public virtual void Add(params T[] items)
        //{
        //    using (var context = this.CreateContext())
        //    {
        //        foreach (T item in items)
        //        {
        //            context.Entry(item).State = System.Data.Entity.EntityState.Added;
        //        }
        //        context.SaveChanges();
        //    }
        //}

        //public virtual void Update(params T[] items)
        //{
        //    using (var context = this.CreateContext())
        //    {
        //        foreach (T item in items)
        //        {
        //            context.Entry(item).State = System.Data.Entity.EntityState.Modified;
        //        }
        //        context.SaveChanges();
        //    }
        //}

        //public virtual void Remove(params T[] items)
        //{
        //    using (var context = this.CreateContext())
        //    {
        //        foreach (T item in items)
        //        {
        //            context.Entry(item).State = System.Data.Entity.EntityState.Deleted;
        //        }
        //        context.SaveChanges();
        //    }
        //}

        protected static System.Data.Entity.EntityState GetEntityState(CarFixed.DS.DM.EntityState entityState)
        {
            switch (entityState)
            {
                case CarFixed.DS.DM.EntityState.Unchanged:
                    return System.Data.Entity.EntityState.Unchanged;
                case CarFixed.DS.DM.EntityState.Added:
                    return System.Data.Entity.EntityState.Added;
                case CarFixed.DS.DM.EntityState.Modified:
                    return System.Data.Entity.EntityState.Modified;
                case CarFixed.DS.DM.EntityState.Deleted:
                    return System.Data.Entity.EntityState.Deleted;
                default:
                    return System.Data.Entity.EntityState.Detached;
            }
        }
    }
}
