using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.DomainModels;
using TDEduEnglish.Views.Pages;

namespace TDEduEnglish.Data {
    internal class AppDbContext : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<Leaderboard> Leaderboards { get; set; }
        public DbSet<Course> MyProperty { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizResult> QuizResults { get; set; }
        public DbSet<WritingSubmission> WritingSubmissions { get; set; }
        public DbSet<Progress> Progresses { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite("Data Source=TDEduData.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // USER
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // COURSE - USER (CreateBy)
            modelBuilder.Entity<Course>()
                .HasOne<User>()                // 1 User tạo nhiều Course
                .WithMany()
                .HasForeignKey(c => c.CreateBy)
                .OnDelete(DeleteBehavior.Restrict);

            // LESSON - COURSE
            modelBuilder.Entity<Lesson>()
                .HasOne<Course>()
                .WithMany()
                .HasForeignKey(l => l.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // QUIZ - LESSON
            modelBuilder.Entity<Quiz>()
                .HasOne<Lesson>()
                .WithMany()
                .HasForeignKey(q => q.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            // QUIZRESULT - QUIZ
            modelBuilder.Entity<QuizResult>()
                .HasOne<Quiz>()
                .WithMany()
                .HasForeignKey(qr => qr.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            // QUIZRESULT - USER
            modelBuilder.Entity<QuizResult>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(qr => qr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // PROGRESS - USER
            modelBuilder.Entity<Progress>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // PROGRESS - COURSE
            modelBuilder.Entity<Progress>()
                .HasOne<Course>()
                .WithMany()
                .HasForeignKey(p => p.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // PROGRESS - LESSON
            modelBuilder.Entity<Progress>()
                .HasOne<Lesson>()
                .WithMany()
                .HasForeignKey(p => p.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            // WRITINGSUBMISSION - USER
            modelBuilder.Entity<WritingSubmission>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(ws => ws.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // WRITINGSUBMISSION - LESSON
            modelBuilder.Entity<WritingSubmission>()
                .HasOne<Lesson>()
                .WithMany()
                .HasForeignKey(ws => ws.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            // LEADERBOARD - USER
            modelBuilder.Entity<Leaderboard>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // COMMUNITY - USER
            modelBuilder.Entity<Community>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // COMMUNITY COMMENT (self reference)
            modelBuilder.Entity<Community>()
                .HasOne<Community>()
                .WithMany()
                .HasForeignKey(c => c.CommentId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
