using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using ProyectoCalidadSoftware.Models;

namespace ProyectoCalidadSoftware.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Criterio> Criterio { get; set; }
        public DbSet<Pregunta> Pregunta { get; set; }
        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Software> Software { get; set; }
        public DbSet<Prueba> Prueba { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de la entidad Usuario
            modelBuilder.Entity<Usuario>(tb =>
            {
                tb.HasKey(col => col.Id);
                tb.Property(col => col.Id)
                    .UseIdentityColumn()
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Usuario>().ToTable("Usuario");

            // Configuración de la entidad Criterio
            modelBuilder.Entity<Criterio>(tb =>
            {
                tb.HasKey(col => col.Id);

                // Relación uno-a-muchos con Pregunta
                tb.HasMany(col => col.Preguntas)
                    .WithOne(p => p.Criterio)
                    .HasForeignKey(p => p.CriterioId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Criterio>().ToTable("Criterio");

            // Configuración de la entidad Pregunta
            modelBuilder.Entity<Pregunta>(tb =>
            {
                tb.HasKey(col => col.Id);
            });

            modelBuilder.Entity<Pregunta>().ToTable("Pregunta");

            modelBuilder.Entity<Empresa>(tb =>
            {
                tb.HasKey(col => col.Id);

                tb.HasMany(e => e.Softwares)
                .WithOne(s=>s.Empresa)
                .HasForeignKey(s=>s.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Software>(tb =>
            {
                tb.HasMany(s => s.Pruebas)
                .WithOne(e => e.Software)
                .HasForeignKey(e => e.SoftwareId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Prueba>(tb =>
            {
                // Relación con Criterio
                tb.HasOne(e => e.Criterio)
                  .WithMany()
                  .HasForeignKey(e => e.CriterioId)
                  .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
