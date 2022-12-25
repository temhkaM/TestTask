using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

public class ApplicationContext : DbContext
{
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<CategoryField> CategoryFields { get; set; } = null!;
    public DbSet<ProductCategoryField> ProductCategoryFields { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        //Database.EnsureDeleted();
        //Database.EnsureCreated();   // создаем базу данных при первом обращении
    }
    public ApplicationContext()
    { 
        
    }

    protected async override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Apple", Price = 100, Description = "123", CategoryId = 11 },
                new Product { Id = 2, Name = "Orange", Price = 120, Description = "1231254127", CategoryId = 11 },
                new Product { Id = 3, Name = "Pants", Price = 1000, Description = "123125", CategoryId = 22 },
                new Product { Id = 4, Name = "Pants", Price = 1000, Description = "123125412", CategoryId = 22 }
        );
        
        modelBuilder.Entity<Category>().HasData(
                new Category { Id = 11, Name = "Fruits", Description = "only for Fruits" },
                new Category { Id = 22, Name = "Clothes", Description = "only for Clothes" }
        );
        
        modelBuilder.Entity<CategoryField>().HasData(
                new CategoryField { Id = 111, FieldName = "Color", CategoryId = 22 },
                new CategoryField { Id = 222, FieldName = "Weight", CategoryId = 11 },
                new CategoryField { Id = 333, FieldName = "Count", CategoryId = 11 }
        );

        modelBuilder.Entity<ProductCategoryField>().HasData(
                new ProductCategoryField { ProductId = 1, CategoryFieldId = 222, FieldValue = "13" },
                new ProductCategoryField { ProductId = 1, CategoryFieldId = 333, FieldValue = "2" },
                new ProductCategoryField { ProductId = 2, CategoryFieldId = 222, FieldValue = "17" },
                new ProductCategoryField { ProductId = 2, CategoryFieldId = 333, FieldValue = "6" },
                new ProductCategoryField { ProductId = 3, CategoryFieldId = 111, FieldValue = "Black" },
                new ProductCategoryField { ProductId = 4, CategoryFieldId = 111, FieldValue = "Brown" }
        );

        modelBuilder.Entity<Product>()
            .HasKey(p => new { p.Id});

        modelBuilder.Entity<Category>()
            .HasKey(c => new { c.Id });

        modelBuilder.Entity<CategoryField>()
            .HasKey(cf => new {cf.Id});

        modelBuilder.Entity<ProductCategoryField>()
            .HasKey(pcf => new { pcf.ProductId, pcf.CategoryFieldId});
    }



}