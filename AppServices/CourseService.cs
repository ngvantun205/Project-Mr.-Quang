using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDEduEnglish.Repository;

namespace TDEduEnglish.AppServices {
    internal class CourseService : ICourseService {
        private readonly IRepository<Course> _courseRepository;
        public CourseService(IRepository<Course> courseRepository) {
            _courseRepository = courseRepository;
        }
        public async Task<IEnumerable<Course>> GetAll() => await _courseRepository.GetAll();
        public async Task<Course?> GetById(int id) => await _courseRepository.GetById(id);
        public async Task Add(Course course) => await _courseRepository.Add(course);
        public async Task Update(Course course) => await _courseRepository.Update(course);
        public async Task Delete(int id) => await _courseRepository.Delete(id);
    }
}
