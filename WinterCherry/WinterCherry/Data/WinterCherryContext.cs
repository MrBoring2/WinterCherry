using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WinterCherry.Data
{
    public partial class WinterCherryContext : DbContext
    {
        public WinterCherryContext()
            : base("name=WinterCherryContext")
        {
        }

        public virtual DbSet<DeliveryToShop> DeliveryToShop { get; set; }
        public virtual DbSet<DeliveryToStorage> DeliveryToStorage { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<IceCream> IceCream { get; set; }
        public virtual DbSet<IceCreamType> IceCreamType { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<ProductsInDeliveryToShop> ProductsInDeliveryToShop { get; set; }
        public virtual DbSet<ProductsInDeliveryToStorage> ProductsInDeliveryToStorage { get; set; }
        public virtual DbSet<ProductsInOrder> ProductsInOrder { get; set; }
        public virtual DbSet<ProductsInStorage> ProductsInStorage { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Storage> Storage { get; set; }
        public virtual DbSet<Supplier> Supplier { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeliveryToShop>()
                .HasMany(e => e.ProductsInDeliveryToShop)
                .WithRequired(e => e.DeliveryToShop)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.FullName)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Gender)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Login)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Order)
                .WithRequired(e => e.Employee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IceCream>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<IceCream>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<IceCream>()
                .HasMany(e => e.ProductsInOrder)
                .WithRequired(e => e.IceCream)
                .HasForeignKey(e => e.ProductId);

            modelBuilder.Entity<IceCream>()
                .HasMany(e => e.ProductsInDeliveryToStorage)
                .WithRequired(e => e.IceCream)
                .HasForeignKey(e => e.ProductId);

            modelBuilder.Entity<IceCream>()
                .HasMany(e => e.ProductsInDeliveryToShop)
                .WithRequired(e => e.IceCream)
                .HasForeignKey(e => e.ProductId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IceCream>()
                .HasMany(e => e.ProductsInStorage)
                .WithRequired(e => e.IceCream)
                .HasForeignKey(e => e.ProductId);

            modelBuilder.Entity<IceCreamType>()
                .Property(e => e.IceCreamTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<IceCreamType>()
                .HasMany(e => e.IceCream)
                .WithRequired(e => e.IceCreamType)
                .HasForeignKey(e => e.TypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Employee)
                .WithRequired(e => e.Role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Storage>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Name)
                .IsUnicode(false);
        }
    }
}
