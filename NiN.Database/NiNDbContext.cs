namespace NiN.Database
{
    using Microsoft.EntityFrameworkCore;
    using NiN.Database.Models.Code;
    using NiN.Database.Models.Code.Codes;
    using NiN.Database.Models.Common;
    using NiN.Database.Models.Variety;
    using NiN.Database.Models.Variety.Codes;

    public class NiNDbContext : DbContext
    {
        // Common
        public DbSet<NinVersion> NinVersion { get; set; }

        // Codes
        public DbSet<Natursystem> Natursystem { get; set; }
        public DbSet<Hovedtypegruppe> Hovedtypegruppe { get; set; }
        public DbSet<Hovedtype> Hovedtype { get; set; }
        public DbSet<Grunntype> Grunntype { get; set; }
        public DbSet<Kartleggingsenhet> Kartleggingsenhet { get; set; }
        public DbSet<Miljovariabel> Miljovariabel { get; set; }
        public DbSet<NinKode> Kode { get; set; }
        public DbSet<LKMKode> LKMKode { get; set; }
        public DbSet<Trinn> Trinn { get; set; }
        public DbSet<Basistrinn> Basistrinn { get; set; }

        // Variety
        public DbSet<VariasjonKode> VariasjonKode { get; set; }
        public DbSet<VarietyLevel0> VarietyLevel0s { get; set; }
        public DbSet<VarietyLevel1> VarietyLevel1s { get; set; }
        public DbSet<VarietyLevel2> VarietyLevel2s { get; set; }
        public DbSet<VarietyLevel3> VarietyLevel3s { get; set; }
        public DbSet<VarietyLevel4> VarietyLevel4s { get; set; }
        public DbSet<VarietyLevel5> VarietyLevel5s { get; set; }

        public NiNDbContext(DbContextOptions<NiNDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NinVersion>().ToTable("NinVersion");

            CreateCodes(modelBuilder);
            CreateVariety(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
        
        private static void CreateCodes(ModelBuilder modelBuilder)
        {
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
        }

        private static void CreateVariety(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VarietyLevel0>().ToTable("VarietyLevel0");
            modelBuilder.Entity<VarietyLevel1>().ToTable("VarietyLevel1");
            modelBuilder.Entity<VarietyLevel2>().ToTable("VarietyLevel2");
            modelBuilder.Entity<VarietyLevel3>().ToTable("VarietyLevel3");
            modelBuilder.Entity<VarietyLevel4>().ToTable("VarietyLevel4");
            modelBuilder.Entity<VarietyLevel5>().ToTable("VarietyLevel5");

            modelBuilder.Entity<VarietyLevel0>()
                .HasMany(x => x.UnderordnetKoder)
                .WithOne(x => x.OverordnetKode)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<VarietyLevel1>()
                .HasMany(x => x.UnderordnetKoder)
                .WithOne(x => x.OverordnetKode)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<VarietyLevel2>()
                .HasMany(x => x.UnderordnetKoder)
                .WithOne(x => x.OverordnetKode)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<VarietyLevel3>()
                .HasMany(x => x.UnderordnetKoder)
                .WithOne(x => x.OverordnetKode)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<VarietyLevel4>()
                .HasMany(x => x.UnderordnetKoder)
                .WithOne(x => x.OverordnetKode)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
