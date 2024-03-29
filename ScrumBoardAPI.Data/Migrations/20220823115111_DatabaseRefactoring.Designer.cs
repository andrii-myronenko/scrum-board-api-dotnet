// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ScrumBoardAPI.Data;

#nullable disable

namespace ScrumBoardAPI.Migrations
{
    [DbContext(typeof(ScrumBoardDbContext))]
    [Migration("20220823115111_DatabaseRefactoring")]
    partial class DatabaseRefactoring
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AUserWorkspace", b =>
                {
                    b.Property<string>("UsersId")
                        .HasColumnType("text");

                    b.Property<int>("WorkspacesId")
                        .HasColumnType("integer");

                    b.HasKey("UsersId", "WorkspacesId");

                    b.HasIndex("WorkspacesId");

                    b.ToTable("WorkspaceUser", (string)null);

                    b.HasData(
                        new
                        {
                            UsersId = "1",
                            WorkspacesId = 1
                        });
                });

            modelBuilder.Entity("ScrumBoardAPI.Data.AUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AUser");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Email = "example@example.com",
                            Name = "John Doe",
                            PasswordHash = "pass"
                        },
                        new
                        {
                            Id = "2",
                            Email = "example2@example.com",
                            Name = "Jane Doe",
                            PasswordHash = "pass2"
                        });
                });

            modelBuilder.Entity("ScrumBoardAPI.Data.Task", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AssigneeId")
                        .HasColumnType("text");

                    b.Property<string>("CreatorId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Priority")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("WorkspaceId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AssigneeId");

                    b.HasIndex("CreatorId");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("Task");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AssigneeId = "1",
                            CreatorId = "1",
                            Description = "Task 1 description",
                            Name = "Task 1",
                            Priority = "High",
                            WorkspaceId = 1
                        },
                        new
                        {
                            Id = 2,
                            AssigneeId = "1",
                            CreatorId = "2",
                            Description = "Task 2 description",
                            Name = "Task 2",
                            Priority = "Medium",
                            WorkspaceId = 1
                        });
                });

            modelBuilder.Entity("ScrumBoardAPI.Data.Workspace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Workspace");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Workspace 1"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Workspace 2"
                        });
                });

            modelBuilder.Entity("AUserWorkspace", b =>
                {
                    b.HasOne("ScrumBoardAPI.Data.AUser", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ScrumBoardAPI.Data.Workspace", null)
                        .WithMany()
                        .HasForeignKey("WorkspacesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ScrumBoardAPI.Data.Task", b =>
                {
                    b.HasOne("ScrumBoardAPI.Data.AUser", "Assignee")
                        .WithMany("AssignedTasks")
                        .HasForeignKey("AssigneeId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("ScrumBoardAPI.Data.AUser", "Creator")
                        .WithMany("CreatedTasks")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ScrumBoardAPI.Data.Workspace", "Workspace")
                        .WithMany()
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Assignee");

                    b.Navigation("Creator");

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("ScrumBoardAPI.Data.AUser", b =>
                {
                    b.Navigation("AssignedTasks");

                    b.Navigation("CreatedTasks");
                });
#pragma warning restore 612, 618
        }
    }
}
