using System;
using System.Collections.Generic;
using Labb2_DbFirst_Template.Models;
using Microsoft.EntityFrameworkCore;

namespace Labb2_DbFirst_Template.Data;

public partial class BookstoreDbErDiagramContext : DbContext
{
    public static BookstoreDbErDiagramContext _bookstoreDbContext;

    public BookstoreDbErDiagramContext()
    {
    }
    public static BookstoreDbErDiagramContext GetInstance()
    {
        if (_bookstoreDbContext is null)
        {
            _bookstoreDbContext = new BookstoreDbErDiagramContext();
        }

        return _bookstoreDbContext;
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Series> Series { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<VPreferencesPerCustomer> VPreferencesPerCustomers { get; set; }

    public virtual DbSet<VTitlesPerAuthor> VTitlesPerAuthors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DX13_IR\\SQL_DEV;Initial Catalog=BookstoreDB_ER_diagram;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__authors__3214EC07F3924E08");

            entity.ToTable("authors");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasMany(d => d.Isbn13s).WithMany(p => p.Authors)
                .UsingEntity<Dictionary<string, object>>(
                    "BookAuthor",
                    r => r.HasOne<Book>().WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__book_auth__ISBN1__60A75C0F"),
                    l => l.HasOne<Author>().WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__book_auth__Autho__5FB337D6"),
                    j =>
                    {
                        j.HasKey("AuthorId", "Id").HasName("PK__book_aut__536585D46B85E284");
                        j.ToTable("book_authors");
                        j.IndexerProperty<string>("Id")
                            .HasMaxLength(13)
                            .IsUnicode(false)
                            .HasColumnName("ISBN13");
                    });
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__books__3BF79E0389BFDB20");

            entity.ToTable("books");

            entity.Property(e => e.Id)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("ISBN13");
            entity.Property(e => e.Language)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Publisher)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.Translator).HasMaxLength(50);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__customer__3214EC07ABD162B8");

            entity.ToTable("customers");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Adress).HasMaxLength(150);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.County).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("Phone_number");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__orders__3214EC07E5B835C4");

            entity.ToTable("orders");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.CollectionShop).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CollectionShopId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__orders__Collecti__4D94879B");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__orders__Customer__4CA06362");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.Isbn13, e.OrderId }).HasName("PK__order_de__D7CE9BBF75D41735");

            entity.ToTable("order_details");

            entity.Property(e => e.Isbn13)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("ISBN13");

            entity.HasOne(d => d.Isbn13Navigation).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.Isbn13)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__order_det__ISBN1__5629CD9C");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__order_det__Order__571DF1D5");
        });

        modelBuilder.Entity<Series>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__series__3214EC072A534ED8");

            entity.ToTable("series");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.SeriesName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasMany(d => d.Isbn13s).WithMany(p => p.Series)
                .UsingEntity<Dictionary<string, object>>(
                    "SeriesBook",
                    r => r.HasOne<Book>().WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__series_bo__ISBN1__6477ECF3"),
                    l => l.HasOne<Series>().WithMany()
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__series_bo__Serie__6383C8BA"),
                    j =>
                    {
                        j.HasKey("SeriesId", "Id").HasName("PK__series_b__D01EB8813A869E3F");
                        j.ToTable("series_books");
                        j.IndexerProperty<string>("Id")
                            .HasMaxLength(13)
                            .IsUnicode(false)
                            .HasColumnName("ISBN13");
                    });
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__shops__3214EC07BFBE1875");

            entity.ToTable("shops");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ShopAdress).HasMaxLength(100);
            entity.Property(e => e.ShopName).HasMaxLength(50);
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => new { e.ShopId, e.Isbn13 }).HasName("PK__stock__447A2E29F9E3D43D");

            entity.ToTable("stock");

            entity.Property(e => e.Isbn13)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("ISBN13");

            entity.HasOne(d => d.Book).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.Isbn13)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__stock__ISBN13__412EB0B6");

            entity.HasOne(d => d.Shop).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.ShopId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__stock__ShopId__403A8C7D");
        });

        modelBuilder.Entity<VPreferencesPerCustomer>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vPreferencesPerCustomer");

            entity.Property(e => e.FullName)
                .HasMaxLength(201)
                .IsUnicode(false);
            entity.Property(e => e.MostBoughtSerie)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(38, 2)");
        });

        modelBuilder.Entity<VTitlesPerAuthor>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vTitlesPerAuthor");

            entity.Property(e => e.Name)
                .HasMaxLength(101)
                .IsUnicode(false);
            entity.Property(e => e.TolatPriceForAllbooksInStock)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("TolatPriceForALLBooksInStock");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
