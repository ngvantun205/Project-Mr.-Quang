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
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizResult> QuizResults { get; set; }
        public DbSet<Vocabulary> Vocabularies { get; set; }
        public DbSet<ReadingLesson> ReadingLessons { get; set; }
        public DbSet<ReadingQuestion> ReadingQuestions { get; set; }
        public DbSet<UserReadingResult> UserReadingResults { get; set; }
        public DbSet<ListeningLesson> ListeningLessons { get; set; }
        public DbSet<ListeningQuestion> ListeningQuestions { get; set; }
        public DbSet<UserListeningResult> UserListeningResults { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite("Data Source=TDEduData.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReadingLesson>()
                .HasMany(r => r.Questions)
                .WithOne() 
                .HasForeignKey("ReadingLessonId") 
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReadingQuestion>();
            modelBuilder.Entity<UserReadingResult>()
                .HasOne<User>() 
                .WithMany()
                .HasForeignKey(urr => urr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserReadingResult>()
                .HasOne<ReadingLesson>() 
                .WithMany()
                .HasForeignKey(urr => urr.ReadingLessonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuizResult>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(qr => qr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuizResult>()
                .HasOne<Quiz>()
                .WithMany()
                .HasForeignKey(qr => qr.QuizId)
                .OnDelete(DeleteBehavior.Cascade);            

            modelBuilder.Entity<Vocabulary>();
        }


    }
}
