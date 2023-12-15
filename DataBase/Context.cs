using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ProgressControlApp.DataBase
{
    public partial class Context : DbContext
    {
        public Context()
            : base("name=Context")
        {
        }
        public static Context Instance { get; } = new Context();

        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<Mark> Mark { get; set; }
        public virtual DbSet<Professor> Professor { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>()
                .HasMany(e => e.Student)
                .WithRequired(e => e.Group)
                .HasForeignKey(e => e.IDGroup)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.User)
                .WithRequired(e => e.Role)
                .HasForeignKey(e => e.IDRole)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.Mark)
                .WithOptional(e => e.Student)
                .HasForeignKey(e => e.IDStudent);

            modelBuilder.Entity<Subject>()
                .HasMany(e => e.Mark)
                .WithOptional(e => e.Subject)
                .HasForeignKey(e => e.IDSubject);

            modelBuilder.Entity<Subject>()
                .HasMany(e => e.Professor)
                .WithRequired(e => e.Subject)
                .HasForeignKey(e => e.IDSubject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Student)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.IDUser)
                .WillCascadeOnDelete(false);
        }
    }
}
