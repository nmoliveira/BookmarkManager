using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using BookmarkManager.Model;

namespace BookmarkManager.Repository
{
    public class RepositoryBase<C> : IDisposable
        where C : DbContext, IDisposedTracker, new()
    {
        private C _DataContext;

        public virtual C DataContext
        {
            get
            {
                if (_DataContext == null || _DataContext.IsDisposed)
                {
                    _DataContext = new C();
                    AllowSerialization = true;
                    //Disable ProxyCreationDisabled to prevent the "In order to serialize the parameter, add the type to the known types collection for the operation using ServiceKnownTypeAttribute" error
                }
                return _DataContext;
            }
        }

        public virtual bool AllowSerialization
        {
            get
            {
                //return ((IObjectContextAdapter) _DataContext)
                //.ObjectContext.ContextOptions.ProxyCreationEnabled = false;
                return _DataContext.Configuration.ProxyCreationEnabled;
            }
            set
            {
                _DataContext.Configuration.ProxyCreationEnabled = !value;
            }
        }

        public virtual T Get<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            if (predicate != null)
            {
                using (DataContext)
                {
                    return DataContext.Set<T>().Where(predicate).SingleOrDefault();
                }
            }
            else
            {
                throw new ApplicationException("Predicate value must be passed to Get<T>.");
            }
        }

        public virtual IQueryable<T> GetList<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            try
            {
                return DataContext.Set<T>().Where(predicate);
            }
            catch (Exception ex)
            {
                //Log error
            }
            return null;
        }

        public virtual IQueryable<T> GetList<T, TKey>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, TKey>> orderBy) where T : class
        {
            try
            {
                return GetList(predicate).OrderBy(orderBy);
            }
            catch (Exception ex)
            {
                //Log error
            }
            return null;
        }

        public virtual IQueryable<T> GetList<T, TKey>(Expression<Func<T, TKey>> orderBy) where T : class
        {
            try
            {
                return GetList<T>().OrderBy(orderBy);
            }
            catch (Exception ex)
            {
                //Log error
            }
            return null;
        }

        public virtual IQueryable<T> GetList<T>() where T : class
        {
            try
            {
                return DataContext.Set<T>();
            }
            catch (Exception ex)
            {
                //Log error
            }
            return null;
        }

        public virtual OperationStatus Save<T>(T entity) where T : class
        {
            OperationStatus opStatus = new OperationStatus { Status = true };

            try
            {
                opStatus.Status = DataContext.SaveChanges() > 0;
            }
            catch (Exception exp)
            {
                opStatus = OperationStatus.CreateFromException("Error saving " + typeof(T) + ".", exp);
            }

            return opStatus;
        }

        public virtual OperationStatus Update<T>(T entity, params string[] propsToUpdate) where T : class
        {
            OperationStatus opStatus = new OperationStatus { Status = true };

            try
            {
                DataContext.Set<T>().Attach(entity);
                DataContext.Entry(entity).State = System.Data.EntityState.Modified;
                DataContext.ChangeTracker.DetectChanges();
                opStatus.Status = DataContext.SaveChanges() > 0;
            }
            catch (Exception exp)
            {
                opStatus = OperationStatus.CreateFromException("Error updating " + typeof(T) + ".", exp);
            }

            return opStatus;
        }

        public OperationStatus ExecuteStoreCommand(string cmdText, params object[] parameters)
        {
            var opStatus = new OperationStatus { Status = true };

            try
            {
                //opStatus.RecordsAffected = DataContext.ExecuteStoreCommand(cmdText, parameters);
                //TODO: [Papa] = Have not tested this yet.
                opStatus.RecordsAffected = DataContext.Database.ExecuteSqlCommand(cmdText, parameters);
            }
            catch (Exception exp)
            {
                OperationStatus.CreateFromException("Error executing store command: ", exp);
            }
            return opStatus;
        }

        //public virtual OperationStatus Delete<T>(T entity) where T : class
        //{
        //    OperationStatus opStatus = new OperationStatus { Status = true };

        //    try
        //    {
        //        ObjectSet<T> objectSet = DataContext.CreateObjectSet<T>();
        //        objectSet.Attach(entity);
        //        objectSet.DeleteObject(entity);
        //        opStatus.Status = DataContext.SaveChanges() > 0;
        //    }
        //    catch (Exception exp)
        //    {
        //        return OperationStatus.CreateFromException("Error deleting " + typeof(T), exp);
        //    }

        //    return opStatus;
        //}

        public void Dispose()
        {
            if (DataContext != null) DataContext.Dispose();
        }
    }
}
