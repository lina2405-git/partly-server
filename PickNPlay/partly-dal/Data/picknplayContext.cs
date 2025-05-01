using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PickNPlay.picknplay_dal.Entities;

namespace PickNPlay.picknplay_dal.Data
{
    public partial class picknplayContext : DbContext
    {
        public picknplayContext()
        {
        }

        public picknplayContext(DbContextOptions<picknplayContext> options)
            : base(options)
        {
        }

        // public virtual DbSet<AccountProvider> AccountProviders { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Favourite> Favourites { get; set; } = null!;
        // public virtual DbSet<Game> Games { get; set; } = null!;
        public virtual DbSet<Listing> Listings { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Review> Reviews { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Deposit> Deposits { get; set; } = null!;

        public virtual DbSet<ListingImage> ListingsImages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=picknplay.db");
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ListingImage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ImageUrl).IsRequired();
                entity.HasOne(e => e.Listing) 
                    .WithMany(l => l.ListingImages) 
                    .HasForeignKey(e => e.ListingId) 
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            modelBuilder.Entity<Deposit>(entity =>
            {
                entity.HasKey(e => e.DepositId)
                    .HasName("PK_Deposits");
                entity.HasOne(u => u.User)
                        .WithMany(e => e.Deposits)
                        .HasForeignKey(e => e.UserId)
                        .HasConstraintName("FK_deposits_users");
            });

            // modelBuilder.Entity<AuthProvider>(entity =>
            // {
            //     entity.HasKey(e => e.ProviderId)
            //         .HasName("PK_Auth_Providers");
            // });

            modelBuilder.Entity<Favourite>(entity =>
            {
                entity.HasOne(d => d.Listing)
                    .WithMany(p => p.Favourites)
                    .HasForeignKey(d => d.ListingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_favourites_listings");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Favourites)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_favourites_users");
            });

            modelBuilder.Entity<Listing>(entity =>
            {
                entity.Property(e => e.Title);
                entity.Property(e => e.Description);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Status).HasDefaultValue("Active");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Listings)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_listings_categories");

                // entity.HasOne(d => d.Game)
                //     .WithMany(p => p.Listings)
                //     .HasForeignKey(d => d.GameId)
                //     .OnDelete(DeleteBehavior.ClientSetNull)
                //     .HasConstraintName("FK_listings_games");
                //
                // entity.HasOne(d => d.Provider)
                //     .WithMany(p => p.Listings)
                //     .HasForeignKey(d => d.ProviderId)
                //     .HasConstraintName("FK_listings_account_providers");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Listings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_listings_users");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.MessageReceivers)
                    .HasForeignKey(d => d.ReceiverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_messages_receivers");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.MessageSenders)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_messages_senders");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.TransactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_reviews_transactions");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_reviews_users");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Status).HasDefaultValue("In Process");

                entity.HasOne(d => d.Buyer)
                    .WithMany(p => p.TransactionBuyers)
                    .HasForeignKey(d => d.BuyerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_transactions_buyers");

                entity.HasOne(d => d.Listing)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.ListingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_transactions_listings");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.TransactionSellers)
                    .HasForeignKey(d => d.SellerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_transactions_sellers");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.UserRole)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserRoleId)
                    .HasConstraintName("FK_users_user_roles");
            });

            // modelBuilder.Entity<UserAuthProvider>(entity =>
            // {
            //     entity.HasOne(d => d.Provider)
            //         .WithMany(p => p.UserAuthProviders)
            //         .HasForeignKey(d => d.ProviderId)
            //         .OnDelete(DeleteBehavior.ClientSetNull)
            //         .HasConstraintName("FK_user_auth_providers_auth_providers");
            //
            //     entity.HasOne(d => d.User)
            //         .WithMany(p => p.UserAuthProviders)
            //         .HasForeignKey(d => d.UserId)
            //         .OnDelete(DeleteBehavior.ClientSetNull)
            //         .HasConstraintName("FK_user_auth_providers_users");
            // });

            modelBuilder.Entity<UserRating>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRatings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_ratings_users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}