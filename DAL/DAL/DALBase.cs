using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore3Sample
{

    public partial class DALBase : DbContext
    {
        public DALBase()
        {
        }

        public DALBase(DbContextOptions<DALBase> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseOracle(@"Data Source=192.168.66.168:1521/orcl;PASSWORD=balm123;USER ID=balm4;");
                //optionsBuilder.UseSqlServer(@"Data Source=DEVDB\SQLSERVER2012;Initial Catalog=BALM4_DEV_V2;User ID=balm;Password=zlbxaM1yYRkFC7V;");
                optionsBuilder.UseNpgsql(@"Server=192.168.66.76;Port=5432; Database=EF6SampleDB;Userid=postgres; Password=surya@123;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppConfig>()
                       .HasKey(c => new { c.version, c.ConfigKey });
        }
    }





}
