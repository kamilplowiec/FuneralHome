//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FuneralHome
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class FuneralHomeDBContainer : DbContext
    {
        public FuneralHomeDBContainer()
            : base("name=FuneralHomeDBContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Activity> Activity { get; set; }
        public virtual DbSet<Car> Car { get; set; }
        public virtual DbSet<Dictionary> Dictionary { get; set; }
        public virtual DbSet<Dictionary_Type> Dictionary_Type { get; set; }
        public virtual DbSet<Cemetery> Cemetery { get; set; }
        public virtual DbSet<Funeral> Funeral { get; set; }
    }
}
