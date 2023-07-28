﻿// <auto-generated />
using System;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BusinessObject.Migrations
{
    [DbContext(typeof(MyDBContext))]
    [Migration("20230728002307_V3")]
    partial class V3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BusinessObject.Models.Blog", b =>
                {
                    b.Property<int>("BlogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("blog_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BlogId"));

                    b.Property<string>("BlogName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("blog_name");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int")
                        .HasColumnName("category_id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("content");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("created_time");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("image");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("updated_time");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("BlogId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Blog");
                });

            modelBuilder.Entity("BusinessObject.Models.Cart", b =>
                {
                    b.Property<int>("CartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("cart_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CartId"));

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("created_time");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("updated_time");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("CartId");

                    b.HasIndex("UserId");

                    b.ToTable("Cart");
                });

            modelBuilder.Entity("BusinessObject.Models.CartItem", b =>
                {
                    b.Property<int>("CartItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("cart_item_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CartItemId"));

                    b.Property<int>("CartId")
                        .HasColumnType("int")
                        .HasColumnName("cart_id");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("created_time");

                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("product_id");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("quantity");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("status");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("money")
                        .HasColumnName("total_price");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("updated_time");

                    b.HasKey("CartItemId");

                    b.HasIndex("CartId");

                    b.HasIndex("ProductId");

                    b.ToTable("CartItem");
                });

            modelBuilder.Entity("BusinessObject.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("category_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("category_name");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("created_time");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("image");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("updated_time");

                    b.HasKey("CategoryId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("BusinessObject.Models.Color", b =>
                {
                    b.Property<int>("ColorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("color_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ColorId"));

                    b.Property<string>("ColorName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("color_name");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("created_time");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("image");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("updated_time");

                    b.HasKey("ColorId");

                    b.ToTable("Color");
                });

            modelBuilder.Entity("BusinessObject.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("order_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("created_time");

                    b.Property<decimal>("Freight")
                        .HasColumnType("money")
                        .HasColumnName("freight");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime")
                        .HasColumnName("order_date");

                    b.Property<DateTime>("RequiredDate")
                        .HasColumnType("datetime")
                        .HasColumnName("required_date");

                    b.Property<string>("ShipAddress")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("ship_address");

                    b.Property<string>("ShipCity")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("ship_city");

                    b.Property<string>("ShipCountry")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("ship_country");

                    b.Property<string>("ShipName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("ship_name");

                    b.Property<string>("ShipPostalCode")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("ship_postal_code");

                    b.Property<string>("ShipRegion")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("ship_region");

                    b.Property<int>("ShipViaId")
                        .HasColumnType("int")
                        .HasColumnName("ship_via_id");

                    b.Property<DateTime>("ShippedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("shipped_date");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("updated_time");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("OrderId");

                    b.HasIndex("ShipViaId");

                    b.HasIndex("UserId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("BusinessObject.Models.OrderDetail", b =>
                {
                    b.Property<int>("OrderDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("order_detail_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderDetailId"));

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("created_time");

                    b.Property<int>("Discount")
                        .HasColumnType("int")
                        .HasColumnName("discount");

                    b.Property<int>("OrderId")
                        .HasColumnType("int")
                        .HasColumnName("order_id");

                    b.Property<decimal>("Price")
                        .HasColumnType("money")
                        .HasColumnName("price");

                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("product_id");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("quantity");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("updated_time");

                    b.HasKey("OrderDetailId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderDetail");
                });

            modelBuilder.Entity("BusinessObject.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("product_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int")
                        .HasColumnName("category_id");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("created_time");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<int>("Discount")
                        .HasColumnType("int")
                        .HasColumnName("discount");

                    b.Property<string>("ImageBehind")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("image_behind");

                    b.Property<string>("ImageFront")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("image_front");

                    b.Property<string>("ImageLeft")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("image_left");

                    b.Property<string>("ImageRight")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("image_right");

                    b.Property<decimal>("Price")
                        .HasColumnType("money")
                        .HasColumnName("price");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("product_name");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("quantity");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("updated_time");

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("BusinessObject.Models.ProductColor", b =>
                {
                    b.Property<int>("ProductColorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("product_color_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductColorId"));

                    b.Property<int>("ColorId")
                        .HasColumnType("int")
                        .HasColumnName("color_id");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("created_time");

                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("product_id");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("updated_time");

                    b.HasKey("ProductColorId");

                    b.HasIndex("ColorId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductColor");
                });

            modelBuilder.Entity("BusinessObject.Models.ProductSize", b =>
                {
                    b.Property<int>("ProductSizeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("product_size_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductSizeId"));

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("created_time");

                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("product_id");

                    b.Property<int>("SizeId")
                        .HasColumnType("int")
                        .HasColumnName("size_id");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("updated_time");

                    b.HasKey("ProductSizeId");

                    b.HasIndex("ProductId");

                    b.HasIndex("SizeId");

                    b.ToTable("ProductSize");
                });

            modelBuilder.Entity("BusinessObject.Models.Rate", b =>
                {
                    b.Property<int>("RateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("rate_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RateId"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("content");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("created_time");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("image");

                    b.Property<int>("ProductId")
                        .HasColumnType("int")
                        .HasColumnName("product_id");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("updated_time");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("RateId");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("Rate");
                });

            modelBuilder.Entity("BusinessObject.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("role_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("created_time");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("role_name");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("updated_time");

                    b.HasKey("RoleId");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("BusinessObject.Models.ShipVia", b =>
                {
                    b.Property<int>("ShipViaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ship_via_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShipViaId"));

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("created_time");

                    b.Property<string>("ShipViaName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("ship_via_name");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("updated_time");

                    b.HasKey("ShipViaId");

                    b.ToTable("ShipVia");
                });

            modelBuilder.Entity("BusinessObject.Models.Size", b =>
                {
                    b.Property<int>("SizeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("size_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SizeId"));

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("created_time");

                    b.Property<string>("SizeName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("size_name");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("updated_time");

                    b.HasKey("SizeId");

                    b.ToTable("Size");
                });

            modelBuilder.Entity("BusinessObject.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("created_time");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("first_name");

                    b.Property<string>("Gender")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("gender");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("image");

                    b.Property<string>("LastName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("last_name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("password");

                    b.Property<string>("Phone")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("phone");

                    b.Property<int>("RoleId")
                        .HasColumnType("int")
                        .HasColumnName("role_id");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime")
                        .HasColumnName("updated_time");

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("BusinessObject.Models.Blog", b =>
                {
                    b.HasOne("BusinessObject.Models.Category", "Category")
                        .WithMany("Blogs")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessObject.Models.User", "User")
                        .WithMany("Blogs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BusinessObject.Models.Cart", b =>
                {
                    b.HasOne("BusinessObject.Models.User", "User")
                        .WithMany("Carts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BusinessObject.Models.CartItem", b =>
                {
                    b.HasOne("BusinessObject.Models.Cart", "Cart")
                        .WithMany("CartItems")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessObject.Models.Product", "Product")
                        .WithMany("CartItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("BusinessObject.Models.Order", b =>
                {
                    b.HasOne("BusinessObject.Models.ShipVia", "ShipVia")
                        .WithMany("Orders")
                        .HasForeignKey("ShipViaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessObject.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ShipVia");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BusinessObject.Models.OrderDetail", b =>
                {
                    b.HasOne("BusinessObject.Models.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessObject.Models.Product", "Product")
                        .WithMany("OrderDetails")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("BusinessObject.Models.Product", b =>
                {
                    b.HasOne("BusinessObject.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("BusinessObject.Models.ProductColor", b =>
                {
                    b.HasOne("BusinessObject.Models.Color", "Color")
                        .WithMany("ProductColors")
                        .HasForeignKey("ColorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessObject.Models.Product", "Product")
                        .WithMany("ProductColors")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Color");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("BusinessObject.Models.ProductSize", b =>
                {
                    b.HasOne("BusinessObject.Models.Product", "Product")
                        .WithMany("ProductSizes")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessObject.Models.Size", "Size")
                        .WithMany("ProductSizes")
                        .HasForeignKey("SizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Size");
                });

            modelBuilder.Entity("BusinessObject.Models.Rate", b =>
                {
                    b.HasOne("BusinessObject.Models.Product", "Product")
                        .WithMany("Rates")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusinessObject.Models.User", "User")
                        .WithMany("Rates")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BusinessObject.Models.User", b =>
                {
                    b.HasOne("BusinessObject.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("BusinessObject.Models.Cart", b =>
                {
                    b.Navigation("CartItems");
                });

            modelBuilder.Entity("BusinessObject.Models.Category", b =>
                {
                    b.Navigation("Blogs");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("BusinessObject.Models.Color", b =>
                {
                    b.Navigation("ProductColors");
                });

            modelBuilder.Entity("BusinessObject.Models.Order", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("BusinessObject.Models.Product", b =>
                {
                    b.Navigation("CartItems");

                    b.Navigation("OrderDetails");

                    b.Navigation("ProductColors");

                    b.Navigation("ProductSizes");

                    b.Navigation("Rates");
                });

            modelBuilder.Entity("BusinessObject.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("BusinessObject.Models.ShipVia", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("BusinessObject.Models.Size", b =>
                {
                    b.Navigation("ProductSizes");
                });

            modelBuilder.Entity("BusinessObject.Models.User", b =>
                {
                    b.Navigation("Blogs");

                    b.Navigation("Carts");

                    b.Navigation("Rates");
                });
#pragma warning restore 612, 618
        }
    }
}
