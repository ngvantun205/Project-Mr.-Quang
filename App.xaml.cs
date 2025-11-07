using GenerativeAI;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Runtime.InteropServices.JavaScript;
using System.Windows;
using TDEduEnglish.AppServices;
using TDEduEnglish.Data;
using TDEduEnglish.Repository;
using TDEduEnglish.Services;
using TDEduEnglish.ViewModels;
using TDEduEnglish.ViewModels.CoursePageViewModel;
using TDEduEnglish.ViewModels.SuperAdminViewModel;
using TDEduEnglish.ViewModels.WindowViewModel;
using TDEduEnglish.Views.CoursesPageView;
using TDEduEnglish.Views.Pages;
using TDEduEnglish.Views.SuperAdminWindow;
using TDEduEnglish.Views.Windows;
using CourseVocabularyViewModel = TDEduEnglish.ViewModels.CoursePageViewModel.CourseVocabularyViewModel;

namespace TDEduEnglish {

    public partial class App : Application {
        private static IServiceProvider? _serviceProvider;
        public static IServiceProvider? Provider { get => _serviceProvider ??= ConfigureServices(); }

        public App() { }

        private static IServiceProvider ConfigureServices() {
            return new ServiceCollection()
            .AddDbContext<Data.AppDbContext>()

            .AddScoped<IRepository<User>, UserRepository>()
            .AddScoped<IRepository<Vocabulary>, VocabularyRepository>()
            .AddScoped<IRepository<ReadingQuestion>, ReadingQuestionRepository>()
            .AddScoped<IVocabularyRepository, VocabularyRepository>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IReadingRepository, ReadingRepository>()
            .AddScoped<IListeningRepository, ListeningRepository>()
            .AddScoped<IReadingQuestionRepository, ReadingQuestionRepository>()
            .AddScoped<IUserReadingResultRepository, UserReadingResultRepository>()
            .AddScoped<IListeningQuestionRepository, ListeningQuestionRepository>()
            .AddScoped<IUserListeningResultRepository, UserListeningResultRepository>()
            .AddScoped<IWritingRepository, WritingRepository>()
            .AddScoped<IUserVocabularyRepository, UserVocabularyRepository>()
            .AddScoped<IQuizRepository, QuizRepository>()
            .AddScoped<IQuizQuestionRepository, QuizQuestionRepository>()
            .AddScoped<IUserScoreRepository, UserScoreRepository>()
            .AddScoped<IUserAttemptRepository, UserAttemptRepository>()
            .AddScoped<IUserSpeakingRecordRepository, UserSpeakingRecordRepository>()
            .AddScoped<ITopicRepository, TopicRepository>()
            .AddScoped<ISpeakingSentenceRepository, SpeakingSentenceRepository>()

            .AddScoped<UserReadingResult>()
            .AddScoped<UserListeningResult>()

            .AddScoped<IUserService, UserService>()
            .AddScoped<IVocabularyService, VocabularyService>()
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<ISessonService, SessonService>()
            .AddScoped<IReadingService, ReadingService>()
            .AddScoped<IListeningService, ListeningService>()
            .AddScoped<IReadingQuestionService, ReadingQuestionService>()
            .AddScoped<IUserReadingResultService, UserReadingResultService>()
            .AddScoped<IListeningQuestionService, ListeningQuestionService>()
            .AddScoped<IUserListeningResultService, UserListeningResultService>()
            .AddScoped<IWritingService, WritingService>()
            .AddScoped<IUserVocabularyService, UserVocabularyService>()
            .AddScoped<IQuizService, QuizService>()
            .AddScoped<IQuizQuestionService, QuizQuestionService>()
            .AddScoped<ILeaderBoardService, LeaderBoardService>()
            .AddScoped<IUserScoreService, UserScoreService>()
            .AddScoped<ISpeechService, SpeechService>()
            .AddScoped<ITopicService, TopicService>()
            .AddScoped<ISpeakingSentenceService, SpeakingSentenceService>()

            .AddScoped<AppNavigationService>(sp => new AppNavigationService(null))

            .AddTransient<CoursesViewModel>()
            .AddSingleton<HomeViewModel>()
            .AddTransient<LeaderboardViewModel>()
            .AddTransient<UserProfileViewModel>()
            .AddTransient<QuizzesViewModel>()
            .AddTransient<LogViewModel>()
            .AddTransient<SuperAdminViewModel>()
            .AddTransient<CourseReadingListViewModel>()
            .AddTransient<ReadingViewModel>()
            .AddTransient<UserReadingResultViewModel>()
            .AddTransient<ListeningViewModel>()
            .AddTransient<QuizViewModel>()
            .AddTransient<LeaderboardViewModel>()
            

            .AddTransient<ManageListeningViewModel>()
            .AddTransient<ManageUserViewModel>()
            .AddTransient<ManageVocabularyViewModel>()
            .AddTransient<ManageReadingViewModel>()
            .AddTransient<ManageQuizViewModel>()
            .AddTransient<ManageSpeakingViewModel>()


            .AddTransient<CourseListeningListViewModel>()
            .AddTransient<CourseVocabularyViewModel>()
            .AddTransient<CourseGrammarViewModel>()
            .AddTransient<CoursesVocabularyListViewModel>()
            .AddTransient<CourseWritingViewModel>()
            .AddTransient<CourseMyVocabularyViewModel>()
            .AddTransient<CourseSpeakingViewModel>()
            .AddTransient<CourseTopicSpeakingViewModel>()


            .AddSingleton<MainWindow>()
            .AddTransient<LogWindow>()
            .AddSingleton<SuperAdminWindow>()
            .AddTransient<ReadingWindow>()
            .AddTransient<ManageReadingWindow>()
            .AddTransient<UserReadingResultWindow>()
            .AddTransient<ListeningWindow>()
            .AddTransient<ManageListeningWindow>()
            .AddTransient<ManageUserWindow>()
            .AddTransient<ManageVocabularyWindow>()
            .AddTransient<QuizWindow>()
            .AddTransient<ManageQuizWindow>()
            .AddTransient<ManageSpeakingWindow>()

            .AddSingleton<HomePage>()
            .AddTransient<CoursesPage>()
            .AddTransient<LeaderboardPage>()
            .AddTransient<UserProfilePage>()
            .AddTransient<QuizzesPage>()
            .AddTransient<CourseVocabularyPage>()
            .AddTransient<CourseGrammarPage>()
            .AddTransient<CoursesVocabularyListPage>()
            .AddTransient<CourseReadingListPage>()
            .AddTransient<CourseListeningListPage>()
            .AddTransient<CourseWritingPage>()
            .AddTransient<CourseMyVocabularyPage>()
            .AddTransient<LeaderboardPage>()
            .AddTransient<CourseSpeakingPage>()
            .AddTransient<CourseTopicSpeakingPage>()


            .AddScoped<GenerativeModel>()

            .BuildServiceProvider();


        }

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
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
