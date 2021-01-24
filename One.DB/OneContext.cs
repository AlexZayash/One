using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace One.DB
{
    public partial class OneContext : DbContext
    {
        private readonly string _connectionString;

        public OneContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public OneContext(DbContextOptions<OneContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Coord> Coords { get; set; }
        public virtual DbSet<Weather> Weathers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Russian_Russia.1251");

            modelBuilder.Entity<Coord>(entity =>
            {
                entity.ToTable("coord");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Lat).HasColumnName("lat");

                entity.Property(e => e.Lon).HasColumnName("lon");
            });

            modelBuilder.Entity<Weather>(entity =>
            {
                entity.HasKey(e => e.Key)
                    .HasName("weather_pkey");

                entity.ToTable("weather");

                entity.Property(e => e.Key).HasColumnName("key");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.FeelLike).HasColumnName("feel_like");

                entity.Property(e => e.Humidity).HasColumnName("humidity");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasColumnName("id");

                entity.Property(e => e.Pressure).HasColumnName("pressure");

                entity.Property(e => e.Temp).HasColumnName("temp");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
