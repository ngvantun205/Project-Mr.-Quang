using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.DomainModels;
using TDEduEnglish.Views.Pages;

namespace TDEduEnglish.Data {
    public class AppDbContext : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<QuizResult> QuizResults { get; set; }
        public DbSet<Vocabulary> Vocabularies { get; set; }
        public DbSet<ReadingLesson> ReadingLessons { get; set; }
        public DbSet<ReadingQuestion> ReadingQuestions { get; set; }
        public DbSet<UserReadingResult> UserReadingResults { get; set; }
        public DbSet<ListeningLesson> ListeningLessons { get; set; }
        public DbSet<ListeningQuestion> ListeningQuestions { get; set; }
        public DbSet<UserListeningResult> UserListeningResults { get; set; }
        public DbSet<Writing> Writings { get; set; }
        public DbSet<UserVocabulary> UserVocabularies { get; set; }
        public DbSet<UserScore> UserScores { get; set; }
        public DbSet<UserAttempt> UserAttempts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite("Data Source=TDEduData.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // ListeningLesson – ListeningQuestion
            modelBuilder.Entity<ListeningQuestion>()
                .HasOne(q => q.ListeningLesson)
                .WithMany(l => l.Questions)
                .HasForeignKey(q => q.ListeningLessonId)
                .OnDelete(DeleteBehavior.Cascade);

            // ReadingLesson – ReadingQuestion
            modelBuilder.Entity<ReadingQuestion>()
                .HasOne(q => q.ReadingLesson)
                .WithMany(l => l.Questions)
                .HasForeignKey(q => q.ReadingLessonId)
                .OnDelete(DeleteBehavior.Cascade);

            // User – UserListeningResult
            modelBuilder.Entity<UserListeningResult>()
                .HasOne(r => r.User)
                .WithMany(u => u.ListeningResults)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<UserListeningResult>()
                .HasOne(r => r.ListeningLesson)
                .WithMany(l => l.UserListeningResults)
                .HasForeignKey(r => r.ListeningLessonId);

            // User – UserReadingResult
            modelBuilder.Entity<UserReadingResult>()
                .HasOne(r => r.User)
                .WithMany(u => u.ReadingResults)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<UserReadingResult>()
                .HasOne(r => r.ReadingLesson)
                .WithMany(l => l.UserReadingResults)
                .HasForeignKey(r => r.ReadingLessonId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserScore)
                .WithOne(us => us.User)
                .HasForeignKey<UserScore>(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
