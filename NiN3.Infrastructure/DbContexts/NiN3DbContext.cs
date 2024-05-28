using Microsoft.EntityFrameworkCore;
using NiN3.Core.Models;
using System.Reflection.Metadata;

namespace NiN3.Infrastructure.DbContexts
{
    public class NiN3DbContext : DbContext
    {
        public NiN3DbContext(DbContextOptions<NiN3DbContext> options)
    : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /********* SEEDING moved to loader service***********/
            modelBuilder.Entity<SearchResult>().HasNoKey().HasIndex(sr => sr.Navn);
            base.OnModelCreating(modelBuilder);
        }

        // TYPER
        public DbSet<Versjon> Versjon { get; set; }
        public DbSet<NiN3.Core.Models.Type> Type { get; set; }
        public DbSet<Hovedtypegruppe> Hovedtypegruppe { get; set; }
        public DbSet<Hovedtype> Hovedtype { get; set; }
        public DbSet<Grunntype> Grunntype { get; set; }
        public DbSet<Kartleggingsenhet> Kartleggingsenhet { get; set; }
        public DbSet<Hovedtype_Kartleggingsenhet> Hovedtype_Kartleggingsenhet { get; set; }
        public DbSet<Kartleggingsenhet_Grunntype> Kartleggingsenhet_Grunntype { get; set; }
     
        //VARIABLER
        public DbSet<Variabel> Variabel { get; set; }
        public DbSet<Variabelnavn> Variabelnavn { get; set; }
        public DbSet<Hovedtypegruppe_Hovedoekosystem> Hovedtypegruppe_Hovedoekosystem { get; set; }

        public DbSet<VariabelnavnMaaleskala> VariabelnavnMaaleskala { get; set; }

        public DbSet<Maaleskala> Maaleskala { get; set; }
        public DbSet<Trinn> Trinn { get; set; }

        public DbSet<Konvertering> Konvertering { get; set; }

        //Type classes and Variabeltrinn
        public DbSet<GrunntypeVariabeltrinn> GrunntypeVariabeltrinn { get; set; }

        public DbSet<HovedtypeVariabeltrinn> HovedtypeVariabeltrinn { get; set; }

        /* For rapportservice and convenience */
        public DbSet<Endringslogg> Endringslogg { get; set; }
        public DbSet<AlleKortkoder> AlleKortkoder { get; set; }
        public DbSet<Enumoppslag> Enumoppslag { get; set; }
        public DbSet<SearchResult> AlleLangkoderView { get; set; }
        //ANDRE
        // ...

    }
}
