using Domain.DTO;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Data
{
    public partial class AppDbContext : DbContext
    {


        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

       
        //public virtual DbSet<FoodByKeywords> FoodByKeywords { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }


        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Delivery> Deliveries { get; set; }

        public virtual DbSet<DeliveryAgent> DeliveryAgents { get; set; }

        public virtual DbSet<FoodItem> FoodItems { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderItem> OrderItems { get; set; }

        public virtual DbSet<Restaurant> Restaurants { get; set; }

        public virtual DbSet<Review> Reviews { get; set; }

        public virtual DbSet<User> Users { get; set; }

        //Dto DbSet

        public DbSet<FoodByKeywords> FoodByKeywords { get; set; }

        public DbSet<OrderedItemsByUserDto> OrderDetailsByUserId { get; set; }
        //public AppDbContext CreateDbContext(string[] args)
        //{
        //    var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        //    optionsBuilder.UseSqlServer("Data Source=LTIN490889;User=SA;Password=password-1;Initial Catalog=Food;TrustServerCertificate=true");

        //    return new AppDbContext(optionsBuilder.Options);
        //}



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoodByKeywords>().HasNoKey();
            modelBuilder.Entity<OrderedItemsByUserDto>().HasNoKey();

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.AddressId).HasName("PK__address__CAA247C853D7D107");

                entity.ToTable("address");

                entity.Property(e => e.AddressId)
                  
                    .HasColumnName("address_id");
                entity.Property(e => e.Address1)
                    .HasColumnType("text")
                    .HasColumnName("address");
                entity.Property(e => e.CustId).HasColumnName("cust_id");

                entity.HasOne(d => d.Cust).WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.CustId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__address__cust_id__3A81B327");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId).HasName("PK__category__D54EE9B47021D588");

                entity.ToTable("category");

                entity.Property(e => e.CategoryId)
                    
                    .HasColumnName("category_id");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Delivery>(entity =>
            {
                entity.HasKey(e => e.DeliveryId).HasName("PK__delivery__1C5CF4F5BC3FDD18");

                entity.ToTable("delivery");

                entity.Property(e => e.DeliveryId)
                    .HasColumnName("delivery_id");
                entity.Property(e => e.AgentId).HasColumnName("agent_id");
                entity.Property(e => e.OrderId).HasColumnName("order_id");
                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Agent).WithMany(p => p.Deliveries)
                    .HasForeignKey(d => d.AgentId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__delivery__agent___60A75C0F");

                entity.HasOne(d => d.Order).WithMany(p => p.Deliveries)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__delivery__order___5FB337D6");
            });

            modelBuilder.Entity<DeliveryAgent>(entity =>
            {
                entity.HasKey(e => e.AgentId).HasName("PK__delivery__2C05379EF16E6169");

                entity.ToTable("delivery_agent");

                entity.Property(e => e.AgentId)
                    .ValueGeneratedNever()
                    .HasColumnName("agent_id");
                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Agent).WithOne(p => p.DeliveryAgent)
                    .HasForeignKey<DeliveryAgent>(d => d.AgentId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__delivery___agent__5CD6CB2B");
            });

            modelBuilder.Entity<FoodItem>(entity =>
            {
                entity.HasKey(e => e.ItemId).HasName("PK__food_ite__52020FDD44ECF4CD");
                entity.ToTable("food_items");
                entity.Property(e => e.ItemId)
                    .HasColumnName("item_id");
                entity.Property(e => e.CategoryId).HasColumnName("category_id");
                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);
                entity.Property(e => e.ItemName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("item_name");
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("price");
                entity.Property(e => e.Rating)
                    .HasColumnType("decimal(2, 1)")
                    .HasColumnName("rating");
                entity.Property(e => e.RestaurantId).HasColumnName("restaurant_id");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property("Keywords")
                    .HasColumnName("keywords")
                    .HasColumnType("text");
                entity.HasOne(d => d.Category).WithMany(p => p.FoodItems)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__food_item__categ__4316F928");
                entity.HasOne(d => d.Restaurant).WithMany(p => p.FoodItems)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__food_item__resta__4222D4EF");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId).HasName("PK__orders__46596229241C82EF");

                entity.ToTable("orders");

                entity.Property(e => e.OrderId)
                    .HasColumnName("order_id");
                entity.Property(e => e.RestaurantId).HasColumnName("restaurant_id");
                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Restaurant).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__orders__restaura__46E78A0C");

                entity.HasOne(d => d.User).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__orders__user_id__45F365D3");
            });
            modelBuilder.Entity<OrderItem>()
               .HasKey(oi => new { oi.OrderId, oi.ItemId });

            // OrderItem -> FoodItem (Item)
            modelBuilder.Entity<OrderItem>()
          .HasKey(oi => new { oi.OrderId, oi.ItemId }); // Optional composite key

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Item)
                .WithMany(fi => fi.OrderItems)
                .HasForeignKey(oi => oi.ItemId)
                .IsRequired();

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .IsRequired();
            modelBuilder.Entity<OrderItem>()
    .HasOne(oi => oi.Item)
    .WithMany(fi => fi.OrderItems)
    .HasForeignKey(oi => oi.ItemId)
    .OnDelete(DeleteBehavior.Restrict); // Optional: prevent cascade delete

            //modelBuilder.Entity<OrderItem>(entity =>
            //{
            //    entity
            //        .HasNoKey()
            //        .ToTable("order_items");

            //    entity.Property(e => e.ItemId).HasColumnName("item_id");
            //    entity.Property(e => e.OrderId).HasColumnName("order_id");
            //    entity.Property(e => e.Quantity).HasColumnName("quantity");

            //    entity.HasOne(d => d.Item).WithMany()
            //        .HasForeignKey(d => d.ItemId)
            //        .OnDelete(DeleteBehavior.Restrict)
            //        .HasConstraintName("FK__order_ite__item___49C3F6B7");

            //    entity.HasOne(d => d.Order).WithMany()
            //        .HasForeignKey(d => d.OrderId)
            //        .OnDelete(DeleteBehavior.Restrict)
            //        .HasConstraintName("FK__order_ite__order__48CFD27E");
            //});
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ItemId });

                entity.ToTable("order_items");

                entity.Property(e => e.ItemId).HasColumnName("item_id");
                entity.Property(e => e.OrderId).HasColumnName("order_id");
                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(e => e.Item)
                    .WithMany(fi => fi.OrderItems)
                    .HasForeignKey(e => e.ItemId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__order_ite__item___49C3F6B7");

                entity.HasOne(e => e.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__order_ite__order__48CFD27E");
            });



            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.HasKey(e => e.RestaurantId).HasName("PK__restaura__3B0FAA91A6DFDE71");

                entity.ToTable("restaurant");

                entity.Property(e => e.RestaurantId)
                     .ValueGeneratedNever()
                    .HasColumnName("restaurant_id");
                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.User).WithOne(p => p.Restaurant)
                    .HasForeignKey<Restaurant>(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__restauran__resta__3D5E1FD2");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(e => e.ReviewId).HasName("PK__review__60883D90252E84FF");

                entity.ToTable("review");

                entity.Property(e => e.ReviewId)
                    
                    .HasColumnName("review_id");
                entity.Property(e => e.Comment)
                    .HasColumnType("text")
                    .HasColumnName("comment");
                entity.Property(e => e.Rating).HasColumnName("rating");
                entity.Property(e => e.RestaurantId).HasColumnName("restaurant_id");
                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Restaurant).WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__review__restaura__5812160E");

                entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__review__user_id__571DF1D5");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id)
                .HasName("PK__users__3213E83F9CBDC252");

                entity.ToTable("users");

                entity.Property(e => e.Id)
                   
                    .HasColumnName("id");
                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("name");
                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("password");
                entity.Property(e => e.Phoneno)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("phoneno");
                entity.Property(e => e.IsValid)
                    .HasMaxLength(255)                   
                    .HasColumnName("is_valid");
                entity.Property(e => e.Role)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("role");
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
