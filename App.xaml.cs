using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Runtime.InteropServices.JavaScript;
using System.Windows;
using TDEduEnglish.AppServices;
using TDEduEnglish.Repository;
using TDEduEnglish.ViewModels;

namespace TDEduEnglish {

    public partial class App : Application {
        private IServiceProvider _serviceProvider;

        public App() {
            var services = new ServiceCollection();

            services.AddScoped<IRepository<User>, UserRepository>();
            services.AddScoped<IRepository<Course>, CourseRepository>();
            services.AddScoped<IRepository<Vocabulary>, VocabularyRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IVocabularyService, VocabularyService>();


            services.AddDbContext<Data.AppDbContext>();


            var assembly = typeof(App).Assembly;

            foreach (var type in assembly.GetTypes()
                .Where(t => t.Name.EndsWith("ViewModel"))) {
                services.AddTransient(type);
            }

            foreach (var type in assembly.GetTypes()
                .Where(t => t.Name.EndsWith("Window") || t.Name.EndsWith("Page"))) {
                services.AddTransient(type);
            }

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e) {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }
    }

}
