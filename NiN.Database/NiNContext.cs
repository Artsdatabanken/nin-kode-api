namespace NiN.Database
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using NiN.Database.Models;
    using NiN.Database.Models.Codes;
    using NiN.Database.Models.Common;

    public class NiNContext : DbContext
    {
        private readonly int _versionMajor = 2;
        private readonly int _versionMinor = 3;
        private readonly string _versionBeta = "";

        private string _versionPrefix;

        public DbSet<NinVersion> NinVersion { get; set; }
        public DbSet<Natursystem> Natursystem { get; set; }
        public DbSet<Hovedtypegruppe> Hovedtypegruppe { get; set; }
        public DbSet<Hovedtype> Hovedtype { get; set; }
        public DbSet<Grunntype> Grunntype { get; set; }
        public DbSet<Kartleggingsenhet> Kartleggingsenhet { get; set; }
        public DbSet<Miljovariabel> Miljovariabel { get; set; }
        public DbSet<Kode> Kode { get; set; }
        public DbSet<LKMKode> LKMKode { get; set; }
        public DbSet<Trinn> Trinn { get; set; }
        public DbSet<Basistrinn> Basistrinn { get; set; }

        public string ConnectionString { get; private set; }
        public string DbName { get; private set; }
        
        public NiNContext()
        {
            DbName = "NiN";
            ConnectionString = $"data source=localhost;initial catalog={DbName};Integrated Security=SSPI;MultipleActiveResultSets=True;App=EntityFramework";
            InitializeVersion();
        }

        public NiNContext(string connectionString)
        {
            ConnectionString = connectionString;
            InitializeVersion();
        }

        private void InitializeVersion()
        {
            _versionPrefix = $"_v{_versionMajor}.{_versionMinor}{_versionBeta}";
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
            modelBuilder.Entity<NinVersion>().ToTable("NinVersion");
            modelBuilder.Entity<Natursystem>().ToTable("Natursystem");
            modelBuilder.Entity<Hovedtypegruppe>().ToTable("Hovedtypegruppe");
            modelBuilder.Entity<Hovedtype>().ToTable("Hovedtype");
            modelBuilder.Entity<Grunntype>().ToTable("Grunntype");

            //modelBuilder.Entity<Miljovariabel>().ToTable("Miljovariabel");
            //modelBuilder.Entity<Trinn>().ToTable("Trinn");
            //modelBuilder.Entity<Basistrinn>().ToTable("Basistrinn");
            //modelBuilder.Entity<Kartleggingsenhet>().ToTable("Kartleggingsenhet");
            //modelBuilder.Entity<Kode>().ToTable("Kode");
            //modelBuilder.Entity<LKMKode>().ToTable("LKMKode");

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

            modelBuilder.Entity<Trinn>()
                .HasMany(x => x.Basistrinn)
                .WithOne(x => x.Trinn)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Trinn>()
                .HasOne(x => x.Kode)
                .WithOne(x => x.Trinn)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Basistrinn>()
                .HasOne(x => x.Kode)
                .WithOne(x => x.Basistrinn)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
