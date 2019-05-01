namespace VTIntranet.intranetdb
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class intranetContext : DbContext
    {
        public intranetContext()
            : base("name=intranetContext")
        {
        }

        public virtual DbSet<tblattachments> tblattachments { get; set; }
        public virtual DbSet<tblattachmentstags> tblattachmentstags { get; set; }
        public virtual DbSet<tblpermissions> tblpermissions { get; set; }
        public virtual DbSet<tblprofiles> tblprofiles { get; set; }
        public virtual DbSet<tblprofilestags> tblprofilestags { get; set; }
        public virtual DbSet<tbltags> tbltags { get; set; }
        public virtual DbSet<tblusers> tblusers { get; set; }
        public virtual DbSet<tbluserspermissions> tbluserspermissions { get; set; }
        public virtual DbSet<tblusersprofiles> tblusersprofiles { get; set; }
        public virtual DbSet<tbluserstags> tbluserstags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblattachments>()
                .HasMany(e => e.tblattachmentstags)
                .WithRequired(e => e.tblattachments)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblpermissions>()
                .Property(e => e.permissionImageClass)
                .IsFixedLength();

            modelBuilder.Entity<tblpermissions>()
                .HasMany(e => e.tbluserspermissions)
                .WithRequired(e => e.tblpermissions)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblprofiles>()
                .HasMany(e => e.tblprofilestags)
                .WithRequired(e => e.tblprofiles)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblprofiles>()
                .HasMany(e => e.tblusersprofiles)
                .WithRequired(e => e.tblprofiles)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbltags>()
                .HasMany(e => e.tblattachmentstags)
                .WithRequired(e => e.tbltags)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tbltags>()
                .HasMany(e => e.tblprofilestags)
                .WithRequired(e => e.tbltags)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblusers>()
                .HasMany(e => e.tbluserspermissions)
                .WithRequired(e => e.tblusers)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblusers>()
                .HasMany(e => e.tblusersprofiles)
                .WithRequired(e => e.tblusers)
                .WillCascadeOnDelete(false);
        }
    }
}
