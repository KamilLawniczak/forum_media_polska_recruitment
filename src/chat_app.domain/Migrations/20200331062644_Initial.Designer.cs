﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using chat_app.domain.Data;

namespace chat_app.domain.Migrations
{
    [DbContext(typeof(ChatContext))]
    [Migration("20200331062644_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("chat_app.domain.ChatUser", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(64)")
                        .HasMaxLength(64);

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.ToTable("ChatUsers");
                });
#pragma warning restore 612, 618
        }
    }
}