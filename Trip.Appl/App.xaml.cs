using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using Trip.App.Services;
using Trip.App.Stores;
using Trip.App.ViewModels;
using Trip.Database;
using Trip.Database.Repos;
using Trip.Repos;
using Trip.Services;

namespace Trip.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => 
                {
                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("Trip.InMemory.Database");
                    });
                    // Repos
                    services.AddScoped<INotificationRepo, NotificationRepo>();
                    services.AddScoped<IAccountRepo, AccountRepo>();
                    services.AddScoped<ITripRepo, TripRepo>();
                    services.AddScoped<IBookingRepo, BookingRepo>();
                    services.AddScoped<IReviewRepo, ReviewRepo>();
                    // Services
                    services.AddScoped<INotificationService, NotificationService>();
                    services.AddScoped<IAccountService, AccountService>();
                    services.AddScoped<ITripService, TripService>();
                    services.AddScoped<IBookingService, BookingService>();
                    services.AddScoped<IReviewService, ReviewService>();
                    // Stores
                    services.AddSingleton<AccountStore>();
                    services.AddSingleton<NavigationStore>();
                    // View models
                    services.AddTransient<HistoryViewModel>();
                    services.AddTransient<ProfileViewModel>();
                    services.AddTransient<MyTripViewModel>();
                    services.AddTransient<MyTripsViewModel>();
                    services.AddTransient<DetailsMyTripViewModel>();
                    services.AddTransient<CreateTripViewModel>();
                    services.AddTransient<SearchTripViewModel>();
                    services.AddTransient<HomeViewModel>();
                    services.AddTransient<LoginViewModel>();
                    services.AddTransient<RegisterViewModel>();
                    services.AddSingleton<MainViewModel>();
                    // Navigation services
                    services.AddSingleton<NavigationService<HistoryViewModel>>((services) =>
                    {
                        return new NavigationService<HistoryViewModel>(
                            services.GetRequiredService<NavigationStore>(),
                            () => services.GetRequiredService<HistoryViewModel>()
                        );
                    });
                    services.AddSingleton<NavigationService<ProfileViewModel>>((services) =>
                    {
                        return new NavigationService<ProfileViewModel>(
                            services.GetRequiredService<NavigationStore>(),
                            () => services.GetRequiredService<ProfileViewModel>()
                        );
                    });
                    services.AddSingleton<NavigationService<MyTripViewModel>>((services) =>
                    {
                        return new NavigationService<MyTripViewModel>(
                            services.GetRequiredService<NavigationStore>(),
                            () => services.GetRequiredService<MyTripViewModel>()
                        );
                    });
                    services.AddSingleton<NavigationService<MyTripsViewModel>>((services) =>
                    {
                        return new NavigationService<MyTripsViewModel>(
                            services.GetRequiredService<NavigationStore>(),
                            () => services.GetRequiredService<MyTripsViewModel>()
                        );
                    });
                    services.AddSingleton<NavigationService<SearchTripViewModel>>((services) =>
                    {
                        return new NavigationService<SearchTripViewModel>(
                            services.GetRequiredService<NavigationStore>(),
                            () => services.GetRequiredService<SearchTripViewModel>()
                        );
                    });
                    services.AddSingleton<NavigationService<CreateTripViewModel>>((services) =>
                    {
                        return new NavigationService<CreateTripViewModel>(
                            services.GetRequiredService<NavigationStore>(),
                            () => services.GetRequiredService<CreateTripViewModel>()
                        );
                    });
                    services.AddSingleton<NavigationService<HomeViewModel>>((services) =>
                    {
                        return new NavigationService<HomeViewModel>(
                            services.GetRequiredService<NavigationStore>(),
                            () => services.GetRequiredService<HomeViewModel>()
                        );
                    });
                    services.AddSingleton<NavigationService<LoginViewModel>>((services) =>
                    {
                        return new NavigationService<LoginViewModel>(
                            services.GetRequiredService<NavigationStore>(),
                            () => services.GetRequiredService<LoginViewModel>()
                        );
                    });
                    services.AddSingleton<NavigationService<RegisterViewModel>>((services) => 
                    {
                        return new NavigationService<RegisterViewModel>(
                            services.GetRequiredService<NavigationStore>(),
                            () => services.GetRequiredService<RegisterViewModel>()
                        );
                    });
                    //
                    services.AddSingleton<MainWindow>((services) => 
                    {
                        return new MainWindow
                        {
                            DataContext = services.GetRequiredService<MainViewModel>(),
                        };
                    });
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
