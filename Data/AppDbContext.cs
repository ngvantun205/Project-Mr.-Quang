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
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizResult> QuizResults { get; set; }
        public DbSet<WritingSubmission> WritingSubmissions { get; set; }
        public DbSet<Progress> Progresses { get; set; }
        public DbSet<Vocabulary> Vocabularies { get; set; }
        public DbSet<ReadingLesson> ReadingLessons { get; set; }
        public DbSet<ReadingQuestion> ReadingQuestions { get; set; }
        public DbSet<AnswerOption> AnswerOptions { get; set; }
        public DbSet<UserReadingResult> UserReadingResults { get; set; }
        public DbSet<ListeningLesson> ListeningLessons { get; set; }
        public DbSet<ListeningQuestion> ListeningQuestions { get; set; }
        public DbSet<UserListeningResult> UserListeningResults { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite("Data Source=TDEduData.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            // ReadingLesson -> ReadingQuestion
            modelBuilder.Entity<ReadingLesson>()
                .HasMany(r => r.Questions)
                .WithOne() // chưa có property navigation ngược
                .HasForeignKey("ReadingLessonId") // EF sẽ tạo cột FK
                .OnDelete(DeleteBehavior.Cascade);

            // ReadingQuestion -> AnswerOption
            modelBuilder.Entity<ReadingQuestion>()
                .HasMany(q => q.Options)
                .WithOne()
                .HasForeignKey("ReadingQuestionId")
                .OnDelete(DeleteBehavior.Cascade);

            // User -> UserReadingResult
            modelBuilder.Entity<UserReadingResult>()
                .HasOne<User>() // không có navigation property User trong model
                .WithMany()
                .HasForeignKey(urr => urr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserReadingResult>()
                .HasOne<ReadingLesson>() // không có navigation property ReadingLesson trong model
                .WithMany()
                .HasForeignKey(urr => urr.ReadingLessonId)
                .OnDelete(DeleteBehavior.Cascade);

            // QuizResult -> User
            modelBuilder.Entity<QuizResult>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(qr => qr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // QuizResult -> Quiz
            modelBuilder.Entity<QuizResult>()
                .HasOne<Quiz>()
                .WithMany()
                .HasForeignKey(qr => qr.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            // Leaderboard -> User
            modelBuilder.Entity<Leaderboard>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(lb => lb.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Progress -> User
            modelBuilder.Entity<Progress>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // WritingSubmission -> User
            modelBuilder.Entity<WritingSubmission>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(ws => ws.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // WritingSubmission -> Lesson (nếu có bảng Lesson thì mapping thêm, còn hiện tại giữ nguyên int LessonId)
            // Community -> User (UserId)
            modelBuilder.Entity<Community>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Khai báo các bảng khác như Vocabulary không có quan hệ
            modelBuilder.Entity<Vocabulary>();
        }


    }
}
