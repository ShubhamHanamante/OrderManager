using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OrderManager.API.Data.Entities;

namespace OrderManager.API.Data;

public partial class OrderManagerDbContext : DbContext
{
    public OrderManagerDbContext(DbContextOptions<OrderManagerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Shipment> Shipments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasIndex(e => e.CustomerCode, "IX_Customers_CustomerCode").IsUnique();

            entity.HasIndex(e => e.Email, "IX_Customers_Email").IsUnique();

            entity.Property(e => e.CreatedAtUtc).HasDefaultValueSql("strftime('%Y-%m-%dT%H:%M:%fZ', 'now')");
            entity.Property(e => e.IsActive).HasDefaultValue(1);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasIndex(e => e.OrderNumber, "IX_Orders_OrderNumber").IsUnique();

            entity.HasIndex(e => e.CustomerId, "IX_Orders_CustomerId");

            entity.HasIndex(e => e.OrderDateUtc, "IX_Orders_OrderDateUtc");

            entity.HasIndex(e => e.OrderStatus, "IX_Orders_OrderStatus");

            entity.Property(e => e.CreatedAtUtc).HasDefaultValueSql("strftime('%Y-%m-%dT%H:%M:%fZ', 'now')");
            entity.Property(e => e.Currency).HasDefaultValue("INR");
            entity.Property(e => e.OrderStatus).HasDefaultValue("Pending");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasIndex(e => e.OrderId, "IX_OrderItems_OrderId");

            entity.Property(e => e.Sku).HasColumnName("SKU");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<OrderStatusHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId);

            entity.ToTable("OrderStatusHistory");

            entity.HasIndex(e => e.OrderId, "IX_OrderStatusHistory_OrderId");

            entity.Property(e => e.ChangedAtUtc).HasDefaultValueSql("strftime('%Y-%m-%dT%H:%M:%fZ', 'now')");
            entity.Property(e => e.ChangedBy).HasDefaultValue("System");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderStatusHistories)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasIndex(e => e.PaymentReference, "IX_Payments_PaymentReference").IsUnique();

            entity.HasIndex(e => e.OrderId, "IX_Payments_OrderId");

            entity.Property(e => e.CreatedAtUtc).HasDefaultValueSql("strftime('%Y-%m-%dT%H:%M:%fZ', 'now')");
            entity.Property(e => e.Currency).HasDefaultValue("INR");
            entity.Property(e => e.PaymentStatus).HasDefaultValue("Pending");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasIndex(e => e.Sku, "IX_Products_SKU").IsUnique();

            entity.Property(e => e.CreatedAtUtc).HasDefaultValueSql("strftime('%Y-%m-%dT%H:%M:%fZ', 'now')");
            entity.Property(e => e.IsActive).HasDefaultValue(1);
            entity.Property(e => e.Sku).HasColumnName("SKU");
        });

        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.HasIndex(e => e.TrackingNumber, "IX_Shipments_TrackingNumber").IsUnique();

            entity.HasIndex(e => e.OrderId, "IX_Shipments_OrderId");

            entity.Property(e => e.CreatedAtUtc).HasDefaultValueSql("strftime('%Y-%m-%dT%H:%M:%fZ', 'now')");
            entity.Property(e => e.ShipmentStatus).HasDefaultValue("Pending");

            entity.HasOne(d => d.Order).WithMany(p => p.Shipments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
