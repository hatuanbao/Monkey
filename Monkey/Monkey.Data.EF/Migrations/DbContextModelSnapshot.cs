﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System;

namespace Monkey.Data.EF.Migrations
{
    [DbContext(typeof(DbContext))]
    partial class DbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Monkey.Data.Entities.ProfileEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTimeOffset>("CreatedTime");

                    b.Property<int?>("DeletedBy");

                    b.Property<DateTimeOffset?>("DeletedTime");

                    b.Property<string>("GlobalId");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTimeOffset?>("LastUpdatedTime");

                    b.Property<int?>("UpdatedBy");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("Profile");
                });

            modelBuilder.Entity("Monkey.Data.Entities.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CreatedBy");

                    b.Property<DateTimeOffset>("CreatedTime");

                    b.Property<int?>("DeletedBy");

                    b.Property<DateTimeOffset?>("DeletedTime");

                    b.Property<string>("GlobalId");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTimeOffset?>("LastUpdatedTime");

                    b.Property<int?>("UpdatedBy");

                    b.Property<string>("UserName");

                    b.Property<string>("UserNameNorm");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("User");
                });
        }
    }
}