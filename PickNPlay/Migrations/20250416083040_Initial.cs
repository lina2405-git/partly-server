using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickNPlay.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    category_name = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    user_role_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_role_name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_roles", x => x.user_role_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    username = table.Column<string>(type: "TEXT", unicode: false, maxLength: 50, nullable: false),
                    user_image = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    email = table.Column<string>(type: "TEXT", unicode: false, maxLength: 100, nullable: true),
                    password_hash = table.Column<string>(type: "TEXT", unicode: false, maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    email_verified_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    number_verified_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    email_verification_token = table.Column<string>(type: "TEXT", nullable: true),
                    number_verification_token = table.Column<string>(type: "TEXT", nullable: true),
                    password_reset_token = table.Column<string>(type: "TEXT", nullable: true),
                    reset_token_expires = table.Column<DateTime>(type: "TEXT", nullable: true),
                    phone_number = table.Column<string>(type: "TEXT", maxLength: 13, nullable: true),
                    phone_number_approved = table.Column<bool>(type: "INTEGER", nullable: true),
                    email_approved = table.Column<bool>(type: "INTEGER", nullable: true),
                    user_role_id = table.Column<int>(type: "INTEGER", nullable: true),
                    balance = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    profile_description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_users_user_roles",
                        column: x => x.user_role_id,
                        principalTable: "user_roles",
                        principalColumn: "user_role_id");
                });

            migrationBuilder.CreateTable(
                name: "deposits",
                columns: table => new
                {
                    deposit_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    deposit_amount = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    session_id = table.Column<string>(type: "TEXT", nullable: false),
                    deposit_status = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deposits", x => x.deposit_id);
                    table.ForeignKey(
                        name: "FK_deposits_users",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "listings",
                columns: table => new
                {
                    listing_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    category_id = table.Column<int>(type: "INTEGER", nullable: false),
                    title = table.Column<string>(type: "TEXT", unicode: false, maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    seller_price = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    final_price = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    status = table.Column<string>(type: "TEXT", unicode: false, maxLength: 20, nullable: true, defaultValue: "Active"),
                    amount = table.Column<int>(type: "INTEGER", nullable: false),
                    likes_amount = table.Column<int>(type: "INTEGER", nullable: false),
                    views_amount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_listings", x => x.listing_id);
                    table.ForeignKey(
                        name: "FK_listings_categories",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "category_id");
                    table.ForeignKey(
                        name: "FK_listings_users",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "user_rating",
                columns: table => new
                {
                    rating_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    rating = table.Column<int>(type: "INTEGER", nullable: false),
                    number_of_reviews = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_rating", x => x.rating_id);
                    table.ForeignKey(
                        name: "FK_user_ratings_users",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "favourites",
                columns: table => new
                {
                    favourite_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    listing_id = table.Column<int>(type: "INTEGER", nullable: false),
                    user_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_favourites", x => x.favourite_id);
                    table.ForeignKey(
                        name: "FK_favourites_listings",
                        column: x => x.listing_id,
                        principalTable: "listings",
                        principalColumn: "listing_id");
                    table.ForeignKey(
                        name: "FK_favourites_users",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    message_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    sender_id = table.Column<int>(type: "INTEGER", nullable: false),
                    listing_id = table.Column<int>(type: "INTEGER", nullable: true),
                    receiver_id = table.Column<int>(type: "INTEGER", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    isRead = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.message_id);
                    table.ForeignKey(
                        name: "FK_messages_listings_listing_id",
                        column: x => x.listing_id,
                        principalTable: "listings",
                        principalColumn: "listing_id");
                    table.ForeignKey(
                        name: "FK_messages_receivers",
                        column: x => x.receiver_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_messages_senders",
                        column: x => x.sender_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    transaction_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    listing_id = table.Column<int>(type: "INTEGER", nullable: false),
                    amount = table.Column<int>(type: "INTEGER", nullable: false),
                    buyer_id = table.Column<int>(type: "INTEGER", nullable: false),
                    seller_id = table.Column<int>(type: "INTEGER", nullable: false),
                    buyer_paid = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    seller_get = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    status = table.Column<string>(type: "TEXT", unicode: false, maxLength: 20, nullable: true, defaultValue: "In Process"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    session_id = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.transaction_id);
                    table.ForeignKey(
                        name: "FK_transactions_buyers",
                        column: x => x.buyer_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_transactions_listings",
                        column: x => x.listing_id,
                        principalTable: "listings",
                        principalColumn: "listing_id");
                    table.ForeignKey(
                        name: "FK_transactions_sellers",
                        column: x => x.seller_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    review_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    transaction_id = table.Column<int>(type: "INTEGER", nullable: false),
                    user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    rating = table.Column<int>(type: "INTEGER", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reviews", x => x.review_id);
                    table.ForeignKey(
                        name: "FK_reviews_transactions",
                        column: x => x.transaction_id,
                        principalTable: "transactions",
                        principalColumn: "transaction_id");
                    table.ForeignKey(
                        name: "FK_reviews_users",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_deposits_user_id",
                table: "deposits",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_favourites_listing_id",
                table: "favourites",
                column: "listing_id");

            migrationBuilder.CreateIndex(
                name: "IX_favourites_user_id",
                table: "favourites",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_listings_category_id",
                table: "listings",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_listings_user_id",
                table: "listings",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_messages_listing_id",
                table: "messages",
                column: "listing_id");

            migrationBuilder.CreateIndex(
                name: "IX_messages_receiver_id",
                table: "messages",
                column: "receiver_id");

            migrationBuilder.CreateIndex(
                name: "IX_messages_sender_id",
                table: "messages",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_transaction_id",
                table: "reviews",
                column: "transaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_user_id",
                table: "reviews",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_buyer_id",
                table: "transactions",
                column: "buyer_id");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_listing_id",
                table: "transactions",
                column: "listing_id");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_seller_id",
                table: "transactions",
                column: "seller_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_rating_user_id",
                table: "user_rating",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_user_role_id",
                table: "users",
                column: "user_role_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "deposits");

            migrationBuilder.DropTable(
                name: "favourites");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "user_rating");

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "listings");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "user_roles");
        }
    }
}
