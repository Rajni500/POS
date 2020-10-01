using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Models
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly",
           Justification = "Inherited from IDbContext interface, which exists to support using.")]
    public partial class POSDBContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoDBContext" /> class.
        /// </summary>
        public POSDBContext(DbContextOptions options)
            : base(options)
        {

        }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<BillItems> BillItems { get; set; }
        public virtual DbSet<Category> Category { get; set; }

        /// <summary>
        /// Set Modified
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="propertyName">propertyName</param>
        public virtual void SetModified(object entity
            , string propertyType)
        {
            Entry(entity).Property(propertyType).IsModified = true;
        }
        public virtual void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        public virtual void SetModified(object entity, int i)
        {
            Entry(entity).State = EntityState.Deleted;
        }

        public DbSet<T> GetEntityContext<T>() where T : class
        {
            return base.Set<T>();
        }

        public virtual void SetModified(object entity, string property1, int property2)
        {
            Entry(entity).Property(property1);
        }

        public virtual void SetModified2(object entity)
        {
            Entry(entity).Reload();
        }

        /// <summary>
        /// This methods execute on model creating.
        /// </summary>
        /// <param name="modelBuilder">The modelBuilder.</param>
        /// <remarks> Typically, this method is called only once when the first instance of a derived context
        ///                 is created.  The model for that context is then cached and is for all further instances of
        ///                 the context in the app domain.  This caching can be disabled by setting the ModelCaching
        ///                 property on the given ModelBuilder, but note that this can seriously degrade performance.
        ///                 More control over caching is provided through use of the DBModelBuilder and DBContextFactory
        ///                 classes directly.</remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
