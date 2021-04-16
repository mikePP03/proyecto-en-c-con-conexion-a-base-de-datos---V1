using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Udi.Comercio.Data.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Clase base para los repositorios.
    /// </summary>
    /// <typeparam name="TEntity">Tipo de entidad para nuestro repositorio</typeparam>
    public class EFRepositorio<TEntity> :IDisposable where TEntity : class, new()
    {
        #region Members
        DbContext _dbContext;
        DbSet<TEntity> dbSet;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor sin parámetros
        /// </summary>

        /// <summary>
        /// Crea una nueva instancia de Repositorio
        /// </summary>
        /// <param name="context">ObjectContext que implementa la interfaz IAuditableObjectContext</param>
        public EFRepositorio()
        {
            //Comprueba precondiciones

            //Establece los valores internos
            DbContext context = new DbContext("ComercioBDEntities");
            _dbContext = context;
            _dbContext.Configuration.ProxyCreationEnabled = false;
            dbSet = _dbContext.Set<TEntity>();
        }


        #endregion

        #region Properties

        /// <summary>
        /// Devuelve el AuditableObjectContext 
        /// </summary>


        #endregion

        #region IRepository<TEntity> Members

        /// <summary>
        ///  Guarda los cambios en el contexto
        /// </summary>
        /// <returns></returns>
        public virtual int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// Añade un elemento dentro del repositorio
        /// </summary>
        /// <param name="item">Elemento a añadir al repositorio</param>
        public virtual void Add(TEntity item)
        {
            //check item
            if (item == null)
                throw new ArgumentNullException("item", " Resources.MessagesImpl.EntityNull");
            dbSet.Add(item);
        }

        /// <summary>
        /// Get by Id
        /// </summary>
        /// <param name="id">clave primaria</param>
        /// <returns></returns>
        public virtual TEntity Get(object id)
        {
            return dbSet.Find(id);
        }

        /// <summary>
        /// Get entity
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "exists")]
        public virtual TEntity Get(TEntity element)
        {
            ObjectContext context = null;
            Object foundEntity = null;
            context = ((IObjectContextAdapter)this._dbContext).ObjectContext;
            //var context = this.Context;
            var objSet = context.CreateObjectSet<TEntity>();
            var entityKey = context.CreateEntityKey(objSet.EntitySet.Name, element);
            var exists = context.TryGetObjectByKey(entityKey, out foundEntity);
            return (TEntity)foundEntity;
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public virtual TEntity Update(TEntity element)
        {
            _dbContext.Entry(element).State = EntityState.Modified;
            return element;
        }


        /// <summary>
        /// Marca el elemento para su borrado en el repositorio
        /// </summary>
        /// <param name="item">Elemento a borrar</param>
        public virtual void Remove(TEntity item)
        {
            //check item
            if (item == (TEntity)null)
                throw new ArgumentNullException("item", "Resources.MessagesImpl.EntityNull");
            if (_dbContext.Entry(item).State == System.Data.Entity.EntityState.Detached)
            {
                dbSet.Attach(item);
            }
            dbSet.Remove(item);
        }

        /// <summary>
        /// Obtiene todos los elementos del tipo {T} en repositorio
        /// </summary>
        /// <returns>Lista de elementos seleccionados</returns>
        public virtual List<TEntity> GetAll()
        {
            //Create IObjectSet and perform query 
            return this.BuildQuery().ToList<TEntity>();
        }

        /// <summary>
        /// Obtiene todos los elementos del tipo {TEntity} en repositorio
        /// </summary>
        /// <param name="specifications">Primero se debe obtener de la propiedad this.BuildQuery() y añadir los filtros requeridos</param> 
        /// <returns>Lista de elementos seleccionados</returns>
        public virtual List<TEntity> GetBySpecifications(IQueryable<TEntity> specifications)
        {
            //Create IObjectSet and perform query 
            if (specifications == (IQueryable<TEntity>)null)
                throw new ArgumentNullException("specifications", "esources.MessagesImpl.SpecificationsNull");
            return specifications.ToList<TEntity>();
        }

        /// <summary>
        /// Obtiene todos los elementos del tipo {TEntity} en el repositorio
        /// </summary>
        /// <param name="startRowIndex">Número de índice donde empieza la página. Por ejemplo si queremos la página número 3 entonces startIndex = 3 *maxRows </param>
        /// <param name="maximunRows">´Número de elementos por página</param>
        /// <param name="orderByExpression">Orden por expression para la query. Es obligatorio</param>
        /// <param name="ascending">Especifica si el orden es ascendente</param>
        /// <param name="count">Devuelve el número de elementos encontrados</param> 
        /// <returns>Lista de elementos seleccionados</returns>
        public virtual List<TEntity> GetPagedElements<S>(int startRowIndex, int maximunRows, System.Linq.Expressions.Expression<Func<TEntity, S>> orderByExpression, bool ascending, out int count)
        {
            return this.GetPagedElements<S>(startRowIndex, maximunRows, orderByExpression, ascending, out  count, null);
        }

        /// <summary>
        /// Obtiene todos los elementos del tipo {TEntity} en el repositorio
        /// </summary>
        /// <param name="startRowIndex">Número de índice donde empieza la página. Por ejemplo si queremos la página número 3 entonces startIndex = 3 *maxRows </param>
        /// <param name="maximunRows">´Número de elementos por página</param>
        /// <param name="orderByExpression">Orden por expression para la query. Es obligatorio</param>
        /// <param name="ascending">Especifica si el orden es ascendente</param>
        /// <param name="count">Devuelve el número de elementos encontrados</param> 
        /// <param name="specifications">Primero se debe obtener de la propiedad this.BuildQuery() y añadir los filtros requeridos</param> 
        /// <returns>Lista de elementos seleccionados</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public virtual List<TEntity> GetPagedElements<S>(int startRowIndex, int maximunRows, System.Linq.Expressions.Expression<Func<TEntity, S>> orderByExpression, bool ascending, out int count, IQueryable<TEntity> specifications)
        {
            //checking arguments for this query 
            if (startRowIndex < 0)
                throw new ArgumentException("Resources.MessagesImpl.InvalidPageIndex", "startIndex");

            if (maximunRows <= 0)
                throw new ArgumentException("Resources.MessagesImpl.InvalidPageCount", "maxRows");

            if (orderByExpression == (Expression<Func<TEntity, S>>)null)
                throw new ArgumentNullException("orderByExpression", "Resources.MessagesImpl.OrderByExpressionCannotBeNull");

            //Create associated IObjectSet and perform query
            IQueryable<TEntity> initialQuery = null;
            if (specifications == null)
                initialQuery = this.BuildQuery();
            else
                initialQuery = specifications;

            // return objectSet.Paginate<TEntity, S>(orderByExpression, pageIndex, pageCount, ascending);
            IQueryable<TEntity> pagedQuery;
            if (ascending)
            {
                pagedQuery = initialQuery.OrderBy(orderByExpression).Skip(startRowIndex).Take(maximunRows);
            }
            else
            {
                pagedQuery = initialQuery.OrderByDescending(orderByExpression).Skip(startRowIndex).Take(maximunRows);
            }
            var query = from ent in pagedQuery
                        select new
                        {
                            entity = ent,
                            count = initialQuery.Count()
                        };
            var result = query.ToList();
            count = (result.Count().Equals(0)) ? 0 : result[0].count;
            return result.Select(z => z.entity).ToList();
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Devuelve el IQueryable de la entidad
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TEntity> BuildQuery()
        {

            return _dbContext.Set<TEntity>().AsQueryable<TEntity>();
        }

        #endregion

        #region Protected Methods

        private bool disposed = false;
        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (_dbContext != null)
                        _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
