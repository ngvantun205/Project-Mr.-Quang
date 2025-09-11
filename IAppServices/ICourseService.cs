using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.IAppServices {
    internal interface ICourseService {
        Task<IEnumerable<Course>> GetAll();
        Task<Course> GetById(int id);
        Task Add(Course course);
        Task Update(Course course);
        Task Delete(int id);

    }
}
