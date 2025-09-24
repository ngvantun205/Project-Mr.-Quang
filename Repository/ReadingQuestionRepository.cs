using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TDEduEnglish.Data;

namespace TDEduEnglish.Repository {
    internal class ReadingQuestionRepository : IReadingQuestionRepository {
        private readonly AppDbContext _context;
        public ReadingQuestionRepository(AppDbContext context) {
                       _context = context;
        }
        public async Task<IEnumerable<ReadingQuestion>> GetAll() {
            return await _context.ReadingQuestions.ToListAsync();
        }
        public async Task<ReadingQuestion?> GetById(int id) {
            return await _context.ReadingQuestions.FindAsync(id);
        }
        public async Task Add(ReadingQuestion entity) {
            await _context.ReadingQuestions.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task Update(ReadingQuestion entity) {
            var question = await GetById(entity.ReadingQuestionId);
            if (question != null) {
                question.QuestionText = entity.QuestionText;
                question.Option1 = entity.Option1;
                question.Option2 = entity.Option2;
                question.Option3 = entity.Option3;
                question.Option4 = entity.Option4;
                question.CorrectAnswer = entity.CorrectAnswer;
                question.QuestionNumber = entity.QuestionNumber;
                question.Explanation = entity.Explanation;
                question.ReadingQuestionId = entity.ReadingQuestionId;
                await _context.SaveChangesAsync();
            }
        }
        public async Task Delete(int id) {
            var question = await GetById(id);
            if (question != null) {
                _context.ReadingQuestions.Remove(question);
                await _context.SaveChangesAsync();
            }
        }
        public async Task AddListAsync(IEnumerable<ReadingQuestion> questions) {
            await _context.ReadingQuestions.AddRangeAsync(questions);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<ReadingQuestion>> GetByLessonId(int lessonId) {
            return await _context.ReadingQuestions
                .Where(q => q.ReadingLessonId == lessonId).OrderBy(q => q.QuestionNumber)
                .ToListAsync();
        }
    }
}
