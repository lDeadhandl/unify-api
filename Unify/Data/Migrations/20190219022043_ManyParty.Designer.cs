﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Unify.Data;

namespace Unify.Data.Migrations
{
    [DbContext(typeof(UnifyContext))]
    [Migration("20190219022043_ManyParty")]
    partial class ManyParty
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Unify.Data.Guests", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PartyId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("PartyId");

                    b.HasIndex("UserId");

                    b.ToTable("Guests");
                });

            modelBuilder.Entity("Unify.Data.Party", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Party");
                });

            modelBuilder.Entity("Unify.Data.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayName");

                    b.Property<string>("Email");

                    b.Property<string>("Product");

                    b.Property<string>("SpotifyAccessToken");

                    b.Property<string>("SpotifyRefreshToken");

                    b.Property<string>("Uri");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Unify.Data.Guests", b =>
                {
                    b.HasOne("Unify.Data.Party", "Party")
                        .WithMany("PartyUsers")
                        .HasForeignKey("PartyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Unify.Data.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Unify.Data.Party", b =>
                {
                    b.HasOne("Unify.Data.User", "User")
                        .WithMany("Parties")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
