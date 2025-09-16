using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Runtime.InteropServices.JavaScript;
using System.Windows;
using TDEduEnglish.AppServices;
using TDEduEnglish.Data;
using TDEduEnglish.Repository;
using TDEduEnglish.ViewModels;
using TDEduEnglish.ViewModels.CoursePageViewModel;
using TDEduEnglish.ViewModels.PageViewModel;
using TDEduEnglish.Views.CoursesPageView;
using TDEduEnglish.Views.Pages;

namespace TDEduEnglish {

    public partial class App : Application {
        private static IServiceProvider? _serviceProvider;
        public static IServiceProvider? Provider { get => _serviceProvider ??= ConfigureServices();  }

        public App() {

        }

        private static IServiceProvider ConfigureServices() {
            return new ServiceCollection()
            .AddDbContext<Data.AppDbContext>()

            .AddScoped<IRepository<User>, UserRepository>()
            .AddScoped<IRepository<Course>, CourseRepository>()
            .AddScoped<IRepository<Vocabulary>, VocabularyRepository>()

            .AddScoped<IUserService, UserService>()
            .AddScoped<ICourseService, CourseService>()
            .AddScoped<IVocabularyService, VocabularyService>()
            .AddScoped<ISessonService, SessonService>()
            .AddScoped<IAuthService, AuthService>()


            .AddSingleton<AppNavigationService>(sp => new AppNavigationService(null))

            .AddTransient<CoursesViewModel>()
            .AddTransient<HomeViewModel>()
            .AddTransient<LoginViewModel>()
            .AddTransient<RegisterViewModel>()
            .AddTransient<LeaderboardViewModel>()
            .AddTransient<UserProfileViewModel>()
            .AddTransient<QuizzesViewModel>()
            .AddTransient<CourseVocabularyViewModel>()
            .AddTransient<CommunityViewModel>()

            .AddTransient<CourseGrammarViewModel>()
            .AddTransient<CourseListViewModel>()
            .AddTransient<CourseVocabularyListViewModel>()

            .AddTransient<MainWindow>()
            
            .AddTransient<LoginPage>()
            .AddTransient<RegisterPage>()
            .AddTransient<HomePage>()
            .AddTransient<CoursesPage>()
            .AddTransient<LeaderboardPage>()
            .AddTransient<UserProfilePage>()
            .AddTransient<QuizzesPage>()
            .AddTransient<CommunityPage>()
            .AddTransient<CourseVocabularyPage>()
            .AddTransient<CourseGrammarPage>()
            .AddTransient<CourseListPage>()
            .AddTransient<CoursesVocabularyList>()

            .BuildServiceProvider();


        }

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            //Env.Load();
            using (var scope = Provider?.CreateScope()) {
                var context = scope?.ServiceProvider.GetRequiredService<AppDbContext>();
                //context?.SeedData();

            }
            var mainWindow = Provider?.GetRequiredService<MainWindow>();
            mainWindow?.Show();
        }
    }


}
