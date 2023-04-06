using Microsoft.EntityFrameworkCore;

namespace SimpleClientsCountsapp.Model.Data
{
    public partial class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Count> Counts { get; set; }
        public DbSet<History> History { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Etb_test;Username=postgres;Password=test2023");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("clients");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Count>(entity =>
            {
                entity.ToTable("counts");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Clientsid).HasColumnName("clientsid");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Counts)
                    .HasForeignKey(d => d.Clientsid)
                    .HasConstraintName("fk_client");
            });

            modelBuilder.Entity<History>(entity =>
            {
                entity.HasKey(e => new { e.Time, e.Countid })
                    .HasName("pk_history");

                entity.Property(e => e.Time).HasColumnName("time");

                entity.Property(e => e.Countid).HasColumnName("countid");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Diff).HasColumnName("diff");

                entity.HasOne(d => d.Count)
                    .WithMany(p => p.History)
                    .HasForeignKey(d => d.Countid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_countsid");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
