using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PcStore.Models;

public partial class PcStoreContext : DbContext
{
    public PcStoreContext()
    {
    }

    public PcStoreContext(DbContextOptions<PcStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Producto> Productos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__producto__07F4A132A90FB764");

            entity.ToTable("productos");

            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.Categoria)
                .HasMaxLength(20)
                .HasColumnName("categoria");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(120)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(40)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio).HasColumnName("precio");
            entity.Property(e => e.UrlImagen)
                .HasMaxLength(500)
                .HasColumnName("url_imagen");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
