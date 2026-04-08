using Microsoft.EntityFrameworkCore;
using Sales.Domain.Entities;
using Sales.Domain.Enums;
using Sales.Application.Abstractions.Persistence;

namespace Sales.Infrastructure.Persistence;

public sealed class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Company> Companies => Set<Company>();

    public DbSet<UserCompany> UserCompanies => Set<UserCompany>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Service> Services => Set<Service>();

    public DbSet<Sale> Sales => Set<Sale>();

    public DbSet<SaleItem> SaleItems => Set<SaleItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedNever();
            entity.Property(x => x.Name).IsRequired().HasMaxLength(150);
            entity.Property(x => x.Email).IsRequired().HasMaxLength(200);
            entity.Property(x => x.GoogleSubject).IsRequired().HasMaxLength(200);
            entity.Property(x => x.AuthProvider)
                .HasConversion<int>()
                .IsRequired();
            entity.Property(x => x.IsActive).IsRequired();
            entity.Property(x => x.CreatedAt).IsRequired();
            entity.HasIndex(x => x.Email).IsUnique();
            entity.HasIndex(x => x.GoogleSubject).IsUnique();
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Companies");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedNever();
            entity.Property(x => x.Name).IsRequired().HasMaxLength(150);
            entity.Property(x => x.Cnpj).IsRequired().HasMaxLength(18);
            entity.Property(x => x.IsActive).IsRequired();
            entity.Property(x => x.CreatedAt).IsRequired();
            entity.HasIndex(x => x.Cnpj).IsUnique();
        });

        modelBuilder.Entity<UserCompany>(entity =>
        {
            entity.ToTable("UserCompanies");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedNever();
            entity.Property(x => x.Role)
                .HasConversion<int>()
                .IsRequired();
            entity.Property(x => x.CreatedAt).IsRequired();
            entity.HasIndex(x => new { x.UserId, x.CompanyId }).IsUnique();
            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne<Company>()
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedNever();
            entity.Property(x => x.CompanyId).IsRequired();
            entity.Property(x => x.Name).IsRequired().HasMaxLength(150);
            entity.Property(x => x.Category).IsRequired().HasMaxLength(100);
            entity.Property(x => x.CostPrice).HasPrecision(18, 2);
            entity.Property(x => x.SalePrice).HasPrecision(18, 2);
            entity.Property(x => x.StockQuantity).HasPrecision(18, 2);
            entity.Property(x => x.IsActive).IsRequired();
            entity.Property(x => x.CreatedAt).IsRequired();
            entity.Property(x => x.UpdatedAt).IsRequired();
            entity.HasIndex(x => x.CompanyId);
            entity.HasOne<Company>()
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.ToTable("Services");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedNever();
            entity.Property(x => x.CompanyId).IsRequired();
            entity.Property(x => x.Name).IsRequired().HasMaxLength(150);
            entity.Property(x => x.Description).HasMaxLength(500);
            entity.Property(x => x.CostPrice).HasPrecision(18, 2);
            entity.Property(x => x.SalePrice).HasPrecision(18, 2);
            entity.Property(x => x.IsActive).IsRequired();
            entity.Property(x => x.CreatedAt).IsRequired();
            entity.Property(x => x.UpdatedAt).IsRequired();
            entity.HasIndex(x => x.CompanyId);
            entity.HasOne<Company>()
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.ToTable("Sales");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedNever();
            entity.Property(x => x.CompanyId).IsRequired();
            entity.Property(x => x.PaymentMethod)
                .HasConversion<int>()
                .IsRequired();
            entity.Property(x => x.Discount).HasPrecision(18, 2);
            entity.Property(x => x.Subtotal).HasPrecision(18, 2);
            entity.Property(x => x.Total).HasPrecision(18, 2);
            entity.Property(x => x.Profit).HasPrecision(18, 2);
            entity.Property(x => x.CreatedAt).IsRequired();
            entity.HasIndex(x => x.CompanyId);
            entity.HasOne<Company>()
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(x => x.Items)
                .WithOne()
                .HasForeignKey(x => x.SaleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SaleItem>(entity =>
        {
            entity.ToTable("SaleItems");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedNever();
            entity.Property(x => x.ItemType)
                .HasConversion<int>()
                .IsRequired();
            entity.Property(x => x.Name).IsRequired().HasMaxLength(150);
            entity.Property(x => x.Quantity).HasPrecision(18, 2);
            entity.Property(x => x.UnitCostPrice).HasPrecision(18, 2);
            entity.Property(x => x.UnitSalePrice).HasPrecision(18, 2);
            entity.Property(x => x.Subtotal).HasPrecision(18, 2);
            entity.Property(x => x.Profit).HasPrecision(18, 2);
        });
    }
}
