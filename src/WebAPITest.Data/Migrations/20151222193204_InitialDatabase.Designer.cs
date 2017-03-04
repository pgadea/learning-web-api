using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using WebAPITest.Data.Models;

namespace WebAPITest.Data.Migrations
{
    [DbContext(typeof(BookStoreContext))]
    [Migration("20151222193204_InitialDatabase")]
    partial class InitialDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity(" WebAPITest.Data.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("MiddleName");

                    b.Property<string>("NickName");

                    b.HasKey("Id");
                });

            modelBuilder.Entity(" WebAPITest.Data.Models.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AuthorId");

                    b.Property<string>("Genre");

                    b.Property<string>("ISBN");

                    b.Property<int>("NoofPages");

                    b.Property<DateTime>("PublishedOn");

                    b.Property<string>("Title");

                    b.HasKey("Id");
                });

            modelBuilder.Entity(" WebAPITest.Data.Models.Book", b =>
                {
                    b.HasOne(" WebAPITest.Data.Models.Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");
                });
        }
    }
}
