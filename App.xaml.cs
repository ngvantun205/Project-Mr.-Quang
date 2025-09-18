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
using TDEduEnglish.ViewModels.WindowViewModel;
using TDEduEnglish.Views.CoursesPageView;
using TDEduEnglish.Views.Pages;
using TDEduEnglish.Views.Windows;
using CourseVocabularyViewModel = TDEduEnglish.ViewModels.CoursePageViewModel.CourseVocabularyViewModel;

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
            .AddScoped<IVocabularyRepository, VocabularyRepository>()

            .AddScoped<IUserService, UserService>()
            .AddScoped<ICourseService, CourseService>()
            .AddScoped<IVocabularyService, VocabularyService>()
            .AddScoped<ISessonService, SessonService>()
            .AddScoped<IAuthService, AuthService>()


            .AddSingleton<AppNavigationService>(sp => new AppNavigationService(null))

            .AddTransient<CoursesViewModel>()
            .AddSingleton<HomeViewModel>()
            .AddTransient<LeaderboardViewModel>()
            .AddTransient<UserProfileViewModel>()
            .AddTransient<QuizzesViewModel>()
            .AddTransient<CommunityViewModel>()
            .AddTransient<LogViewModel>()
            .AddTransient<SuperAdminViewModel>()

            .AddTransient<CourseVocabularyViewModel>()
            .AddTransient<CourseGrammarViewModel>()
            .AddTransient<CourseListViewModel>()
            .AddTransient<CoursesVocabularyListViewModel>()

            .AddSingleton<MainWindow>()
            .AddTransient<LogWindow>()
            .AddSingleton<SuperAdminWindow>()

            .AddSingleton<HomePage>()
            .AddTransient<CoursesPage>()
            .AddTransient<LeaderboardPage>()
            .AddTransient<UserProfilePage>()
            .AddTransient<QuizzesPage>()
            .AddTransient<CommunityPage>()
            .AddTransient<CourseVocabularyPage>()
            .AddTransient<CourseGrammarPage>()
            .AddTransient<CourseListPage>()
            .AddTransient<CoursesVocabularyListPage>()

            .BuildServiceProvider();


        }

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            //Env.Load();
            using (var scope = Provider?.CreateScope()) {
                var context = scope?.ServiceProvider.GetRequiredService<AppDbContext>();
                //context?.Database.EnsureDeleted();
                context?.Database.EnsureCreated();
                //context?.SeedData();

            }
            var mainWindow = Provider?.GetRequiredService<LogWindow>();
            mainWindow?.Show();
        }
    }


}
