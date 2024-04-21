using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DTO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;

#nullable disable

namespace DatabaseModels
{
    public partial class LibraryManagementContext : IdentityDbContext<IdentityUser>
    {
        public IConfiguration Configuration { get; }
        public LibraryManagementContext()
        {
        }

        public LibraryManagementContext(DbContextOptions<LibraryManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Login> Logins { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Role> RolesLocal { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Book> BookLists { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<SpResponse> PlaceAndReturn { get; set; }
        public virtual DbSet<StudentProfile> StudentProfiless { get; set; }
        public virtual DbSet<AdminOrderDetails> AdminOrderDetailSp { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-525574A\\SQLEXPRESS;Database=LibraryManagement;Trusted_Connection=True;");
                
                //optionsBuilder.UseSqlServer(Configuration.GetConnectionString("ConnectionStrings:SqlConnection"));
                //optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(e => e.AuthorName)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.BookTitle)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Login>(entity =>
            {
                entity.ToTable("Login");

                entity.HasIndex(e => e.StudentId, "UQ__Login__32C52B98FCB07206")
                    .IsUnique();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.Student)
                    .WithOne(p => p.Login)
                    .HasForeignKey<Login>(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentIdInRoles");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderDate).HasColumnType("date");

                entity.Property(e => e.ReturnDate).HasColumnType("date");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BookId");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentId");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .IsUnicode(false);
            });


            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasIndex(e => e.Usn, "Uk_usn")
                    .IsUnique();

                entity.Property(e => e.StudentName)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Usn)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("USN");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleId");
            });

            base.OnModelCreating(modelBuilder);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
