using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Data;

namespace TDEduEnglish.Repository {
    internal class CourseRepository : IRepository<Course> {
        private readonly AppDbContext _context;
        public CourseRepository(AppDbContext context) {
            _context = context;
        }
        public async Task<IEnumerable<Course>> GetAll() {
            return await _context.Courses.ToListAsync();
        }
        public async Task<Course?> GetById(int id) {
            return await _context.Courses.FindAsync(id);
        }
        public async Task Add(Course entity) {
            await _context.Courses.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task Update(Course entity) {
            var course = await GetById(entity.CourseId);
            if (course != null) {
                course.Title = entity.Title;
                course.Description = entity.Description;
                course.Level = entity.Level;
                course.CreateDate = entity.CreateDate;
                course.Category = entity.Category;
                course.CreateBy = entity.CreateBy;
                await _context.SaveChangesAsync();  
            }
        }
        public async Task Delete(int id) {
            var course = await GetById(id);
            if (course != null) {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
        }
    }
}
