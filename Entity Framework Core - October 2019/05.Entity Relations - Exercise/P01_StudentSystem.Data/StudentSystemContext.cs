using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        //You will need a constructor, accepting DbContextOptions to test your solution in Judge
        //public StudentSystemContext(DbContextOptions options) 
        //    : base(options)
        //{
        //}

        //protected StudentSystemContext()
        //{
        //}

        public DbSet<Course> Courses { get; set; }

        public DbSet<Homework> HomeworkSubmissions { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=StudentSystem;Integrated Security=True");
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(c => c.CourseId);

                entity.HasMany(c => c.StudentsEnrolled).WithOne(se => se.Course);

                entity.HasMany(c => c.Resources).WithOne(r => r.Course);

                entity.HasMany(c => c.HomeworkSubmissions).WithOne(h => h.Course);

                entity.Property(c => c.Name).HasMaxLength(50).IsRequired().IsUnicode();
                entity.Property(c => c.Description).IsRequired(false).IsUnicode();
            });

            modelBuilder.Entity<Homework>(entity =>
            {
                entity.HasKey(h => h.HomeworkId);

                entity.Property(h => h.Content).IsUnicode(false);
            });

            modelBuilder.Entity<Resource>(entity =>
            {
                entity.HasKey(r => r.ResourceId);
                entity.Property(r => r.Name).HasMaxLength(50).IsUnicode().IsRequired();
                entity.Property(r => r.Url).IsUnicode(false);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(s => s.StudentId);
                entity.Property(s => s.Name).HasMaxLength(100).IsRequired().IsUnicode();
                entity.Property(s => s.PhoneNumber).HasColumnType("CHAR(10)").IsRequired(false).IsUnicode(false);

                entity.HasMany(s => s.HomeworkSubmissions).WithOne(h => h.Student);

                entity.HasMany(s => s.CourseEnrollments).WithOne(cr => cr.Student);
            });

            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity.HasKey(sc => new { sc.StudentId, sc.CourseId });

                entity.HasOne(ce => ce.Student).WithMany(s => s.CourseEnrollments).HasForeignKey(ce => ce.StudentId);

                entity.HasOne(sc => sc.Course).WithMany(c => c.StudentsEnrolled).HasForeignKey(sc => sc.CourseId);
            });
        }
    }
}
