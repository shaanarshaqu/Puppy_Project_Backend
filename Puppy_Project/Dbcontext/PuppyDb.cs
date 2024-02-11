using Microsoft.EntityFrameworkCore;
using Puppy_Project.Depandancies;
using Puppy_Project.Models;

namespace Puppy_Project.Dbcontext
{
    public class PuppyDb:DbContext
    {
        private readonly string _connection_string;
        public PuppyDb(IConfiguration configuration)
        {
            _connection_string = configuration["ConnectionStrings:DefaultConnection"];
        }

        public DbSet<User> UsersTb { get; set; }
        public DbSet<Category> CategoryTB { get; set; }
        public DbSet<Product> ProductsTb { get; set; }
        public DbSet<Cart> CartTb { get; set; }
        public DbSet<CartItem> CartItemTb { get; set; }
        public DbSet<Order> OrderTb { get; set; }
        public DbSet<OrderItem> OrderItemTb { get; set; }
        public DbSet<WishList> WishListTb { get; set; }
        public DbSet<WishListItem> WishListItemTb { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u=>u.Role)
                .HasDefaultValue("user");
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.Category_id);
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.userid)
                .WithOne(u => u.cartuser)
                .HasForeignKey<Cart>(c => c.UserId);
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.cart)
                .WithMany(c => c.cartItemDTOs)
                .HasForeignKey(ci => ci.Cart_id);
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.product)
                .WithMany(p => p.cartItems)
                .HasForeignKey(ci => ci.Product_Id);
            modelBuilder.Entity<CartItem>()
                .Property(ci=>ci.Qty)
                .HasDefaultValue(1);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.user)
                .WithOne(u => u.userorder)
                .HasForeignKey<Order>(o => o.User_Id);
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.order)
                .WithMany(o => o.orderItems)
                .HasForeignKey(oi => oi.Order_Id);
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.product)
                .WithMany(p => p.orderItems)
                .HasForeignKey(oi => oi.Product_Id);
            modelBuilder.Entity<WishList>()
                .HasOne(w => w.user)
                .WithOne(u => u.wishList)
                .HasForeignKey<WishList>(w => w.UserId);
            modelBuilder.Entity<WishListItem>()
                .HasOne(wi => wi.wishList)
                .WithMany(w => w.wishListItems)
                .HasForeignKey(wi => wi.WishList_Id);
            modelBuilder.Entity<WishListItem>()
                .HasOne(wi => wi.product)
                .WithMany(p => p.wishListitems)
                .HasForeignKey(wi => wi.Product_Id);

        }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connection_string);
            }
        }

    }
}
