using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProductInventorySystem.Models;

public partial class ProductInventoryDbContext : DbContext
{
    public ProductInventoryDbContext()
    {
    }

    public ProductInventoryDbContext(DbContextOptions<ProductInventoryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductVarient> ProductVarients { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VariantOption> VariantOptions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source =.; Initial Catalog = ProductInventoryDB; Integrated Security = True; Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC074308E8DA");

            entity.HasIndex(e => e.ProductCode, "UQ__Products__2F4E024F86185451").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.Hsncode)
                .HasMaxLength(100)
                .HasColumnName("HSNCode");
            entity.Property(e => e.ProductCode).HasMaxLength(50);
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.TotalStock).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedDate).HasDefaultValueSql("(sysdatetimeoffset())");

            entity.HasOne(d => d.CreatedUser).WithMany(p => p.Products)
                .HasForeignKey(d => d.CreatedUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_Products_Users");
        });

        modelBuilder.Entity<ProductVarient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductV__3214EC07FAC0A43E");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.VarientName).HasMaxLength(1);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductVarients)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_product_Varients");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0782DB901C");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(16);
            entity.Property(e => e.UserName).HasMaxLength(20);
        });

        modelBuilder.Entity<VariantOption>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VariantO__3214EC072A975C4A");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.OptionValue).HasMaxLength(50);
            entity.Property(e => e.Stock).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Varient).WithMany(p => p.VariantOptions)
                .HasForeignKey(d => d.VarientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_varients_options");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
