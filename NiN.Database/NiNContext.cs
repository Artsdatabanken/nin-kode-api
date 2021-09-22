namespace NiN.Database
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using NiN.Database.Models;
    using NiN.Database.Models.Codes;

    public class NiNContext : DbContext
    {
        public DbSet<Natursystem> Natursystem { get; set; }
        public DbSet<Hovedtypegruppe> Hovedtypegruppe { get; set; }
        public DbSet<Hovedtype> Hovedtype { get; set; }
        public DbSet<Grunntype> Grunntype { get; set; }
        public DbSet<Kartleggingsenhet> Kartleggingsenhet { get; set; }
        public DbSet<Kode> Kode { get; set; }

        public string ConnectionString { get; private set; }
        public string DbName { get; private set; }
        
        public NiNContext()
        {
            DbName = "NiN_v23_test";
            ConnectionString = $"data source=localhost;initial catalog={DbName};Integrated Security=SSPI;MultipleActiveResultSets=True;App=EntityFramework";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;

            optionsBuilder
                .UseSqlServer(ConnectionString)
                //.UseLazyLoadingProxies()
                //.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
                .EnableDetailedErrors();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Natursystem>().ToTable("Natursystem");
            modelBuilder.Entity<Hovedtypegruppe>().ToTable("Hovedtypegruppe");
            modelBuilder.Entity<Hovedtype>().ToTable("Hovedtype");
            modelBuilder.Entity<Grunntype>().ToTable("Grunntype");

            modelBuilder.Entity<Natursystem>()
                .HasMany(x => x.UnderordnetKoder)
                .WithOne(x => x.Natursystem)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Hovedtypegruppe>()
                .HasMany(x => x.UnderordnetKoder)
                .WithOne(x => x.Hovedtypegruppe)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Hovedtype>()
                .HasMany(x => x.UnderordnetKoder)
                .WithOne(x => x.Hovedtype)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Hovedtype>()
                .HasMany(x => x.Kartleggingsenheter)
                .WithOne(x => x.Hovedtype)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Hovedtype>()
                .HasMany(x => x.Miljovariabler)
                .WithOne(x => x.Hovedtype)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Miljovariabel>()
                .HasMany(x => x.Trinn)
                .WithOne(x => x.Miljovariabel)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
