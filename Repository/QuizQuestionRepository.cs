using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Data;

namespace TDEduEnglish.Repository {
    public class QuizQuestionRepository : IQuizQuestionRepository { 
        private readonly AppDbContext _context;
        public QuizQuestionRepository(AppDbContext context) {
            _context = context;
        }
        public async Task<IEnumerable<QuizQuestion>> GetAll() => await _context.QuizQuestions.ToListAsync();
        public async Task<QuizQuestion?> GetById(int id) => await _context.QuizQuestions.FindAsync(id);
        public async Task Add(QuizQuestion entity) {
            await _context.QuizQuestions.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id) {
            var q = await GetById(id);
            if (q != null) {
                _context.QuizQuestions.Remove(q);
                await _context.SaveChangesAsync();
            }
        }
        public async Task Update(QuizQuestion entity) {
            var q = await GetById(entity.QuizId);
            if (q != null) {
                q.QuestionText = entity.QuestionText;
                q.CorrectAnswer = entity.CorrectAnswer;
                q.Explaination = entity.Explaination;
                q.AnswerTime = entity.AnswerTime;
                q.Option1 = entity.Option1;
                q.Option2 = entity.Option2;
                q.Option3 = entity.Option3;
                q.Option4 = entity.Option4;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<QuizQuestion>> GetByQuizId(int quizid) => await _context.QuizQuestions.Where(x => x.QuizId == quizid).ToListAsync();
        public async Task AddListAsync(IEnumerable<QuizQuestion> questions) {
            await _context.QuizQuestions.AddRangeAsync(questions);
            await _context.SaveChangesAsync();
        }
    }
}
