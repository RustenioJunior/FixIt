﻿// <auto-generated />
using System;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ServiceManagement.Migrations.Machine
{
    [DbContext(typeof(MachineContext))]
    partial class MachineContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Models.Machine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("Company_id")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("Created_date")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<int?>("IsActive")
                        .HasColumnType("integer")
                        .HasColumnName("isActive");

                    b.Property<DateTime?>("Last_maintenance_date")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_maintenance_date");

                    b.Property<int?>("Machine_mod_id")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("Modified_date")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modified_date");

                    b.Property<int?>("Number_hours")
                        .HasColumnType("integer")
                        .HasColumnName("number_hours");

                    b.Property<string>("Serial_number")
                        .HasColumnType("text")
                        .HasColumnName("serial_number");

                    b.HasKey("Id");

                    b.ToTable("machines");
                });
#pragma warning restore 612, 618
        }
    }
}
