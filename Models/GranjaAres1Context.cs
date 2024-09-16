using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace MyProyect_Granja.Models
{
    public partial class GranjaAres1Context : DbContext
    {
        private readonly IConfiguration _configuration;  // Agrega este campo para almacenar la configuración

        public GranjaAres1Context(IConfiguration configuration)
        {
            _configuration = configuration;  // Guarda la configuración inyectada
        }

        public GranjaAres1Context(DbContextOptions<GranjaAres1Context> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;  // Inyecta y guarda la configuración
        }


        public virtual DbSet<ClasificacionHuevo> ClasificacionHuevos { get; set; } = null!;
        public virtual DbSet<Cliente> Clientes { get; set; } = null!;
        public virtual DbSet<Corral> Corrals { get; set; } = null!;
        public virtual DbSet<DetallesVentum> DetallesVenta { get; set; } = null!;
        public virtual DbSet<EstadoLote> EstadoLotes { get; set; } = null!;
        public virtual DbSet<Etapa> Etapas { get; set; } = null!;
        public virtual DbSet<Lote> Lotes { get; set; } = null!;
        public virtual DbSet<ProduccionGallina> ProduccionGallinas { get; set; } = null!;
        public virtual DbSet<Producto> Productos { get; set; } = null!;
        public virtual DbSet<RazaGallina> RazaGallinas { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<StockHuevo> StockHuevos { get; set; } = null!;
        public virtual DbSet<TriggerDebugLog> TriggerDebugLogs { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;
        public virtual DbSet<Venta> Ventas { get; set; } = null!;
        public virtual DbSet<VistaClasificacionHuevo> VistaClasificacionHuevos { get; set; } = null!;
        public virtual DbSet<VistaDashboard> VistaDashboards { get; set; } = null!;
        public virtual DbSet<VistaEstadoLotePorFecha> VistaEstadoLotePorFechas { get; set; } = null!;
        public virtual DbSet<VistaInformacionClasificacionHuevo> VistaInformacionClasificacionHuevos { get; set; } = null!;
        public virtual DbSet<VistaStockRestanteHuevo> VistaStockRestanteHuevos { get; set; } = null!;
        public virtual DbSet<VwEstadoLotePorSemana> VwEstadoLotePorSemanas { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("GranjaAres1Database");
                optionsBuilder.UseSqlServer(connectionString);  // Usa la cadena de conexión obtenida
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

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.Property(e => e.ClienteId).HasColumnName("ClienteID");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Estado).HasDefaultValueSql("((1))");

                entity.Property(e => e.NombreCliente)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Corral>(entity =>
            {
                entity.HasKey(e => e.IdCorral);

                entity.ToTable("Corral");

                entity.Property(e => e.IdCorral).ValueGeneratedNever();

                entity.Property(e => e.NumCorral).HasMaxLength(50);
            });

            modelBuilder.Entity<DetallesVentum>(entity =>
            {
                entity.HasKey(e => e.DetalleId)
                    .HasName("PK__Detalles__6E19D6FA7BCF7B7D");

                entity.Property(e => e.DetalleId).HasColumnName("DetalleID");

                entity.Property(e => e.CantidadVendida).HasDefaultValueSql("((0))");

                entity.Property(e => e.Estado).HasDefaultValueSql("((1))");

                entity.Property(e => e.PrecioUnitario)
                    .HasColumnType("decimal(10, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductoId).HasColumnName("ProductoID");

                entity.Property(e => e.TamanoHuevo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoEmpaque)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Total)
                    .HasColumnType("decimal(21, 2)")
                    .HasComputedColumnSql("([CantidadVendida]*[PrecioUnitario])", true);

                entity.Property(e => e.TotalHuevos).HasComputedColumnSql("(case when [TipoEmpaque]='Caja' then [CantidadVendida]*(360) when [TipoEmpaque]='Cartón' then [CantidadVendida]*(30) else [CantidadVendida] end)", true);

                entity.Property(e => e.VentaId).HasColumnName("VentaID");

                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.DetallesVenta)
                    .HasForeignKey(d => d.ProductoId)
                    .HasConstraintName("FK__DetallesV__Produ__1411F17C");

                entity.HasOne(d => d.Venta)
                    .WithMany(p => p.DetallesVenta)
                    .HasForeignKey(d => d.VentaId)
                    .HasConstraintName("FK__DetallesV__Venta__131DCD43");
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

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.Property(e => e.ProductoId).HasColumnName("ProductoID");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Estado).HasDefaultValueSql("((1))");

                entity.Property(e => e.NombreProducto)
                    .HasMaxLength(50)
                    .IsUnicode(false);
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

            modelBuilder.Entity<StockHuevo>(entity =>
            {
                entity.HasKey(e => e.Tamano)
                    .HasName("PK__StockHue__799ADF874A37494A");

                entity.Property(e => e.Tamano).HasMaxLength(50);
            });

            modelBuilder.Entity<TriggerDebugLog>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TriggerDebugLog");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.OperationType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(e => e.Contrasena).HasMaxLength(255);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.FechaDeRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NombreUser).HasMaxLength(50);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Usuarios__RoleId__55209ACA");
            });

            modelBuilder.Entity<Venta>(entity =>
            {
                entity.Property(e => e.VentaId).HasColumnName("VentaID");

                entity.Property(e => e.ClienteId).HasColumnName("ClienteID");

                entity.Property(e => e.Estado).HasDefaultValueSql("((1))");

                entity.Property(e => e.FechaVenta).HasColumnType("date");

                entity.Property(e => e.TotalVenta).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.Venta)
                    .HasForeignKey(d => d.ClienteId)
                    .HasConstraintName("FK__Ventas__ClienteI__7A521F79");
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
