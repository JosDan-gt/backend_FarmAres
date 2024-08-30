using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MyProyect_Granja.Models
{
    public partial class GranjaAres1Context : DbContext
    {
        public GranjaAres1Context()
        {
        }

        public GranjaAres1Context(DbContextOptions<GranjaAres1Context> options)
            : base(options)
        {
        }

        public virtual DbSet<ClasificacionHuevo> ClasificacionHuevos { get; set; } = null!;
        public virtual DbSet<Corral> Corral { get; set; } = null!;
        public virtual DbSet<EstadoLote> EstadoLotes { get; set; } = null!;
        public virtual DbSet<Etapa> Etapas { get; set; } = null!;
        public virtual DbSet<Lote> Lotes { get; set; } = null!;
        public virtual DbSet<ProduccionGallina> ProduccionGallinas { get; set; } = null!;
        public virtual DbSet<RazaGallina> RazaGallinas { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;
        public virtual DbSet<VistaClasificacionHuevo> VistaClasificacionHuevos { get; set; } = null!;
        public virtual DbSet<VistaDashboard> VistaDashboard { get; set; } = null!;
        public virtual DbSet<VistaEstadoLotePorFecha> VistaEstadoLotePorFechas { get; set; } = null!;
        public virtual DbSet<VistaInformacionClasificacionHuevo> VistaInformacionClasificacionHuevos { get; set; } = null!;
        public virtual DbSet<VistaStockRestanteHuevo> VistaStockRestanteHuevos { get; set; } = null!;
        public virtual DbSet<VwEstadoLotePorSemana> VwEstadoLotePorSemanas { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP-ECOQDBI2;Database=GranjaAres1;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClasificacionHuevo>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FechaClaS).HasColumnType("datetime");

                entity.Property(e => e.Tamano)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TotalUnitaria).HasComputedColumnSql("([dbo].[CalcularCantidadTotalUnitaria]([Cajas],[CartonesExtras],[HuevosSueltos]))", false);

                entity.HasOne(d => d.IdProdNavigation)
                    .WithMany(p => p.ClasificacionHuevos)
                    .HasForeignKey(d => d.IdProd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClasificacionHuevos_ProduccionGallinas");
            });

            modelBuilder.Entity<Corral>(entity =>
            {
                entity.HasKey(e => e.IdCorral);

                entity.ToTable("Corral");

                entity.Property(e => e.IdCorral).ValueGeneratedNever();

                entity.Property(e => e.NumCorral).HasMaxLength(50);
            });

            modelBuilder.Entity<EstadoLote>(entity =>
            {
                entity.HasKey(e => e.IdEstado)
                    .HasName("PK_EstadoLote_G");

                entity.ToTable("EstadoLote");

                entity.Property(e => e.IdEstado).HasColumnName("Id_Estado");

                entity.Property(e => e.CantidadG).HasColumnName("Cantidad_G");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Registro");

                entity.Property(e => e.IdEtapa).HasColumnName("Id_Etapa");

                entity.HasOne(d => d.IdEtapaNavigation)
                    .WithMany(p => p.EstadoLotes)
                    .HasForeignKey(d => d.IdEtapa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EstadoLote_Etapas");

                entity.HasOne(d => d.IdLoteNavigation)
                    .WithMany(p => p.EstadoLotes)
                    .HasForeignKey(d => d.IdLote)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EstadoLote_Lote");
            });

            modelBuilder.Entity<Etapa>(entity =>
            {
                entity.HasKey(e => e.IdEtapa);

                entity.Property(e => e.IdEtapa).ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Lote>(entity =>
            {
                entity.HasKey(e => e.IdLote)
                    .HasName("PK_Table1");

                entity.ToTable("Lote");

                entity.Property(e => e.IdLote).ValueGeneratedNever();

                entity.Property(e => e.FechaAdq).HasColumnType("datetime");

                entity.Property(e => e.NumLote)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdCorralNavigation)
                    .WithMany(p => p.Lotes)
                    .HasForeignKey(d => d.IdCorral)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Table1_Corral");

                entity.HasOne(d => d.IdRazaNavigation)
                    .WithMany(p => p.Lotes)
                    .HasForeignKey(d => d.IdRaza)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Table1_RazaGallina");
            });

            modelBuilder.Entity<ProduccionGallina>(entity =>
            {
                entity.HasKey(e => e.IdProd)
                    .HasName("PK__Producci__E40D971DE2CB60D6");

                entity.Property(e => e.CantTotal).HasComputedColumnSql("([dbo].[CalcularCantidadTotalProduccion]([CantCajas],[CantCartones],[CantSueltos]))", false);

                entity.Property(e => e.Defectuosos).HasDefaultValueSql("((0))");

                entity.Property(e => e.FechaRegistroP).HasColumnType("datetime");

                entity.HasOne(d => d.IdLoteNavigation)
                    .WithMany(p => p.ProduccionGallinas)
                    .HasForeignKey(d => d.IdLote)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProduccionGallinas_Lote");
            });

            modelBuilder.Entity<RazaGallina>(entity =>
            {
                entity.HasKey(e => e.IdRaza);

                entity.ToTable("RazaGallina");

                entity.Property(e => e.IdRaza).ValueGeneratedNever();

                entity.Property(e => e.CaractEspec)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.Color)
                    .HasMaxLength(90)
                    .IsUnicode(false);

                entity.Property(e => e.ColorH)
                    .HasMaxLength(90)
                    .IsUnicode(false);

                entity.Property(e => e.Origen)
                    .HasMaxLength(90)
                    .IsUnicode(false);

                entity.Property(e => e.Raza)
                    .HasMaxLength(90)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Nombre).HasMaxLength(50);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(e => e.Contrasena).HasMaxLength(255);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.NombreUser).HasMaxLength(50);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Usuarios__RoleId__55209ACA");
            });

            modelBuilder.Entity<VistaClasificacionHuevo>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VistaClasificacionHuevos");

                entity.Property(e => e.Tamaño)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VistaDashboard>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VistaDashboard");

                entity.Property(e => e.Raza)
                    .HasMaxLength(90)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VistaEstadoLotePorFecha>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VistaEstadoLotePorFecha");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Registro");

                entity.Property(e => e.IdEtapa).HasColumnName("Id_Etapa");
            });

            modelBuilder.Entity<VistaInformacionClasificacionHuevo>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VistaInformacionClasificacionHuevos");

                entity.Property(e => e.Tamaño)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VistaStockRestanteHuevo>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VistaStockRestanteHuevos");

                entity.Property(e => e.FechaProdu).HasColumnType("datetime");

                entity.Property(e => e.IdProduccion).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<VwEstadoLotePorSemana>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_EstadoLotePorSemana");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Registro");

                entity.Property(e => e.IdEtapa).HasColumnName("Id_Etapa");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
