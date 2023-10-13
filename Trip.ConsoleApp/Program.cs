using Microsoft.EntityFrameworkCore;
using Trip.ConsoleApp;
using Trip.Database;
using Trip.Database.Repos;
using Trip.Services;
using Trip.Services.Dtos.Accounts;
using Trip.Services.Dtos.Bookings;
using Trip.Services.Dtos.Notifications;
using Trip.Services.Dtos.Reviews;
using Trip.Services.Dtos.Trips;

internal sealed class Program
{
    private static bool isOpen;
    private static IAccountService _accountService;
    private static ITripService _tripService;
    private static IBookingService _bookingService;
    private static IReviewService _reviewService;
    private static INotificationService _notificationService;

    public static void Main(string[] argv)
    {
        {
            var appDbContext = new AppDbContext(
                new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("Trip.InMemory.Database")
                .Options
            );
            
            var accountRepo = new AccountRepo(appDbContext);
            var tripRepo = new TripRepo(appDbContext);
            var bookingRepo = new BookingRepo(appDbContext);
            var reviewRepo = new ReviewRepo(appDbContext);
            var notificationRepo = new NotificationRepo(appDbContext);

            _notificationService = new NotificationService(notificationRepo, accountRepo);
            _accountService = new AccountService(accountRepo, _notificationService);
            _tripService = new TripService(tripRepo, bookingRepo, accountRepo);
            _bookingService = new BookingService(bookingRepo, accountRepo, tripRepo, _notificationService);
            _reviewService = new ReviewService(reviewRepo, accountRepo, tripRepo, _notificationService);
        }

        isOpen = true;
        {
            Console.WriteLine("Commands:");
            Console.WriteLine("\tAccounts");
            Console.WriteLine("\tTrips");
            Console.WriteLine("\tBookings");
            Console.WriteLine("\tReviews");
            Console.WriteLine("\tNotifications");
            Console.WriteLine("\tExit");
        }
        while (isOpen)
        {
            switch (Console.ReadLine().ToUpper())
            {
                case "EXIT":
                    isOpen = false;
                    Console.WriteLine("Goodbye!");
                    break;

                case "ACCOUNTS":
                    Console.Clear();
                    AccountsSection();
                    Console.Clear();
                    {
                        Console.WriteLine("Commands:");
                        Console.WriteLine("\tAccounts");
                        Console.WriteLine("\tTrips");
                        Console.WriteLine("\tBookings");
                        Console.WriteLine("\tReviews");
                        Console.WriteLine("\tNotifications");
                        Console.WriteLine("\tExit");
                    }
                    break;

                case "TRIPS":
                    Console.Clear();
                    TripsSection();
                    Console.Clear();
                    {
                        Console.WriteLine("Commands:");
                        Console.WriteLine("\tAccounts");
                        Console.WriteLine("\tTrips");
                        Console.WriteLine("\tBookings");
                        Console.WriteLine("\tReviews");
                        Console.WriteLine("\tNotifications");
                        Console.WriteLine("\tExit");
                    }
                    break;

                case "BOOKINGS":
                    Console.Clear();
                    BookingsSection();
                    Console.Clear();
                    {
                        Console.WriteLine("Commands:");
                        Console.WriteLine("\tAccounts");
                        Console.WriteLine("\tTrips");
                        Console.WriteLine("\tBookings");
                        Console.WriteLine("\tReviews");
                        Console.WriteLine("\tNotifications");
                        Console.WriteLine("\tExit");
                    }
                    break;

                case "REVIEWS":
                    Console.Clear();
                    ReviewsSection();
                    Console.Clear();
                    {
                        Console.WriteLine("Commands:");
                        Console.WriteLine("\tAccounts");
                        Console.WriteLine("\tTrips");
                        Console.WriteLine("\tBookings");
                        Console.WriteLine("\tReviews");
                        Console.WriteLine("\tNotifications");
                        Console.WriteLine("\tExit");
                    }
                    break;

                case "NOTIFICATIONS":
                    Console.Clear();
                    NotificationsSection();
                    Console.Clear();
                    {
                        Console.WriteLine("Commands:");
                        Console.WriteLine("\tAccounts");
                        Console.WriteLine("\tTrips");
                        Console.WriteLine("\tBookings");
                        Console.WriteLine("\tReviews");
                        Console.WriteLine("\tNotifications");
                        Console.WriteLine("\tExit");
                    }
                    break;

                default:
                    ConsoleWriter.Error("Commang not found");
                    break;
            }
        }
    }

    public static void AccountsSection()
    {
        {
            Console.WriteLine("Main -> Accounts[Commands]:");
            Console.WriteLine("\tLogin");
            Console.WriteLine("\tCreate");
            Console.WriteLine("\tGetById");
            Console.WriteLine("\tEdit");
            Console.WriteLine("\tExit");
        }
        while (isOpen)
        {
            switch (Console.ReadLine().ToUpper())
            {
                case "EXIT":
                    isOpen = false;
                    Console.WriteLine("Goodbye!");
                    break;

                case "LOGIN":
                    {
                        var loginAppUserDto = new LoginAppUserDto();
                        {
                            Console.Write("Email: ");
                            loginAppUserDto.Email = Console.ReadLine();
                            Console.Write("Password: ");
                            loginAppUserDto.Password = Console.ReadLine();
                        }
                        var result = _accountService.LoginAppUser(loginAppUserDto);
                        if (!result.Succeed)
                        {
                            ConsoleWriter.Error("Errors:");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine("\t" + error);
                            }
                            break;
                        }
                        ConsoleWriter.Info("Success");
                        ConsoleWriter.Table(
                            new[] { "Id", "UserName", "Email", "Password" },
                            new[,] { 
                                { result.Data.AppUserId.ToString(), result.Data.UserName, result.Data.Email, result.Data.Password } 
                            }
                        );
                    }
                    break;

                case "CREATE":
                    {
                        var createAppUserDto = new CreateAppUserDto();
                        {
                            Console.Write("UserName: ");
                            createAppUserDto.UserName = Console.ReadLine();
                            Console.Write("Email: ");
                            createAppUserDto.Email = Console.ReadLine();
                            Console.Write("Password: ");
                            createAppUserDto.Password = Console.ReadLine();
                            Console.Write("Confirm password: ");
                            createAppUserDto.PasswordConfirm = Console.ReadLine();
                        }
                        var result = _accountService.CreateAppUser(createAppUserDto);
                        if (!result.Succeed)
                        {
                            ConsoleWriter.Error("Errors:");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine("\t" + error);
                            }
                            break;
                        }
                        ConsoleWriter.Info("Success");
                    }
                    break;

                case "GETBYID":
                    {
                        Console.Write("Id: ");
                        if (!int.TryParse(Console.ReadLine(), out var appUserId)) 
                        {
                            ConsoleWriter.Error("Invalid format AppUserId");
                        }
                        var result = _accountService.GetAppUserById(appUserId);
                        if (!result.Succeed)
                        {
                            ConsoleWriter.Error("Errors:");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine("\t" + error);
                            }
                            break;
                        }
                        ConsoleWriter.Info("Success");
                        ConsoleWriter.Table(
                            new[] { "Id", "UserName", "Email", "Password" },
                            new[,] {
                                { result.Data.AppUserId.ToString(), result.Data.UserName, result.Data.Email, result.Data.Password }
                            }
                        );
                    }
                    break;

                case "EDIT":
                    {
                        var editAppUserDto = new EditAppUserDto();
                        {
                            Console.Write("Id: ");
                            if (!int.TryParse(Console.ReadLine(), out var appUserId))
                            {
                                ConsoleWriter.Error("Invalid format AppUserId");
                            }
                            editAppUserDto.AppUserId = appUserId;
                            Console.Write("UserName: ");
                            editAppUserDto.UserName = Console.ReadLine();
                            Console.Write("Email: ");
                            editAppUserDto.Email = Console.ReadLine();
                            Console.Write("Password: ");
                            editAppUserDto.Password = Console.ReadLine();
                        }
                        var result = _accountService.EditAppUser(editAppUserDto);
                        if (!result.Succeed)
                        {
                            ConsoleWriter.Error("Errors:");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine("\t" + error);
                            }
                            break;
                        }
                        ConsoleWriter.Info("Success");
                    }
                    break;

                default:
                    ConsoleWriter.Error("Commang not found");
                    break;
            }
        }
        isOpen = true;
    }

    public static void TripsSection()
    {
        {
            Console.WriteLine("Main -> Trips[Commands]:");
            Console.WriteLine("\tCreate");
            Console.WriteLine("\tGetById");
            Console.WriteLine("\tEdit");
            Console.WriteLine("\tCancel");
            Console.WriteLine("\tComplete");
            Console.WriteLine("\tHistory");
            Console.WriteLine("\tSearch");
            Console.WriteLine("\tExit");
        }
        while (isOpen)
        {
            switch (Console.ReadLine().ToUpper())
            {
                case "EXIT":
                    isOpen = false;
                    Console.WriteLine("Goodbye!");
                    break;

                case "CREATE":
                    {
                        var createTripDto = new CreateTripDto();
                        {
                            Console.Write("DriverId: ");
                            if (!int.TryParse(Console.ReadLine(), out var driverId))
                            {
                                ConsoleWriter.Error("Invalid format DriverId");
                            }
                            createTripDto.DriverId = driverId;
                            createTripDto.Date = DateTime.Now.AddMinutes(31);
                            Console.Write("From: ");
                            createTripDto.From = Console.ReadLine();
                            Console.Write("To: ");
                            createTripDto.To = Console.ReadLine();
                            Console.Write("Number of seats: ");
                            createTripDto.NumberOfSeats = int.Parse(Console.ReadLine());
                            Console.Write("Price: ");
                            createTripDto.Price = decimal.Parse(Console.ReadLine());
                        }
                        var result = _tripService.CreateTrip(createTripDto);
                        if (!result.Succeed)
                        {
                            ConsoleWriter.Error("Errors");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine("\t" + error);
                            }
                            break;
                        }
                        ConsoleWriter.Info("Success");
                    }
                    break;

                case "GETBYID":
                    {
                        Console.Write("Id: ");
                        if (!int.TryParse(Console.ReadLine(), out var tripId))
                        {
                            ConsoleWriter.Error("Invalid format DriverId");
                        }
                        var result = _tripService.GetTirpById(tripId);
                        if (!result.Succeed)
                        {
                            ConsoleWriter.Error("Errors");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine("\t" + error);
                            }
                            break;
                        }
                        ConsoleWriter.Info("Success");
                        ConsoleWriter.Table(
                            new[] { "Id", "Driver", "From", "To", "NoS", "Price" },
                            new[,] {
                                { 
                                    result.Data.TripId.ToString(), 
                                    result.Data.DriverName ?? "null", 
                                    result.Data.From, 
                                    result.Data.To, 
                                    result.Data.NumberOfOccupiedSeats.ToString() + "/" + result.Data.NumberOfSeats.ToString(), 
                                    result.Data.Price.ToString()
                                }
                            }
                        );
                    }
                    break;

                case "EDIT":
                    {
                        var editTripDto = new EditTripDto();
                        {
                            Console.Write("Id: ");
                            if (!int.TryParse(Console.ReadLine(), out var tripId))
                            {
                                ConsoleWriter.Error("Invalid format DriverId");
                            }
                            editTripDto.TripId = tripId;
                            editTripDto.Date = DateTime.Now.AddMinutes(31);
                            Console.Write("From: ");
                            editTripDto.From = Console.ReadLine();
                            Console.Write("To: ");
                            editTripDto.To = Console.ReadLine();
                            Console.Write("Number of seats: ");
                            editTripDto.NumberOfSeats = int.Parse(Console.ReadLine());
                            Console.Write("Price: ");
                            editTripDto.Price = decimal.Parse(Console.ReadLine());
                        }
                        var result = _tripService.EditTrip(editTripDto);
                        if (!result.Succeed)
                        {
                            ConsoleWriter.Error("Errors");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine("\t" + error);
                            }
                            break;
                        }
                        ConsoleWriter.Info("Success");
                    }
                    break;

                case "CANCEL":
                    {
                        Console.Write("Id: ");
                        if (!int.TryParse(Console.ReadLine(), out var tripId))
                        {
                            ConsoleWriter.Error("Invalid format DriverId");
                        }
                        var result = _tripService.CancelTrip(tripId);
                        if (!result.Succeed)
                        {
                            ConsoleWriter.Error("Errors");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine("\t" + error);
                            }
                            break;
                        }
                        ConsoleWriter.Info("Success");
                    }
                    break;

                case "COMPLETE":
                    {
                        Console.Write("Id: ");
                        if (!int.TryParse(Console.ReadLine(), out var tripId))
                        {
                            ConsoleWriter.Error("Invalid format DriverId");
                        }
                        var result = _tripService.CompleteTrip(tripId);
                        if (!result.Succeed)
                        {
                            ConsoleWriter.Error("Errors");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine("\t" + error);
                            }
                            break;
                        }
                        ConsoleWriter.Info("Success");
                    }
                    break;

                case "HISTORY":
                    {
                        Console.Write("AppUserId: ");
                        if (!int.TryParse(Console.ReadLine(), out var appUserId))
                        {
                            ConsoleWriter.Error("Invalid format DriverId");
                        }
                        var result = _tripService.GetHistoryTripByAppUserId(appUserId);
                        if (!result.Succeed)
                        {
                            ConsoleWriter.Error("Errors");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine("\t" + error);
                            }
                            break;
                        }
                        ConsoleWriter.Info("Success");
                        ConsoleWriter.Table(
                            new[] { "Id", "Driver", "From", "To", "NoS", "Price" },
                            result.Data.Select(trip => new[]
                            {
                                trip.TripId.ToString(),
                                trip.DriverName,
                                trip.From,
                                trip.To,
                                trip.NumberOfOccupiedSeats.ToString() + "/" + trip.NumberOfSeats.ToString(),
                                trip.Price.ToString()
                            }).ToArray()
                        );
                    }
                    break;

                case "SEARCH":
                    {
                        var searchQueryTripDto = new SearchQueryTripDto();
                        {
                            Console.Write("From: ");
                            searchQueryTripDto.From = Console.ReadLine();
                            Console.Write("To: ");
                            searchQueryTripDto.To = Console.ReadLine();
                            Console.Write("Date: ");
                            var dateString = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(dateString))
                            {
                                if (!DateTime.TryParse(dateString, out DateTime date))
                                {
                                    break;
                                }
                                searchQueryTripDto.Date = date;
                            }
                            Console.Write("Min & Max Price [ex.10 100]: ");
                            var minMaxPriceString = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(dateString))
                            {
                                var pricesString = minMaxPriceString.Split(" ");
                                if (pricesString.Length != 2)
                                {
                                    Console.WriteLine("[Error] Invalid min or max price value");
                                    break;
                                }
                                if (!decimal.TryParse(pricesString[0], out decimal minPrice))
                                {
                                    Console.WriteLine("[Error] Invalid min or max price value");
                                    break;
                                }
                                if (!decimal.TryParse(pricesString[1], out decimal maxPrice))
                                {
                                    Console.WriteLine("[Error] Invalid min or max price value");
                                    break;
                                }
                                searchQueryTripDto.MinPrice = minPrice;
                                searchQueryTripDto.MaxPrice = maxPrice;
                            }
                        }
                        var result = _tripService.SearchTrip(searchQueryTripDto);
                        if (!result.Succeed)
                        {
                            ConsoleWriter.Error("Errors");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine("\t" + error);
                            }
                            break;
                        }
                        ConsoleWriter.Info("Success");
                        ConsoleWriter.Table(
                            new[] { "Id", "Driver", "From", "To", "NoS", "Price" },
                            result.Data.Select(trip => new[]
                            {
                                trip.TripId.ToString(),
                                trip.DriverName,
                                trip.From,
                                trip.To,
                                trip.NumberOfOccupiedSeats.ToString() + "/" + trip.NumberOfSeats.ToString(),
                                trip.Price.ToString()
                            }).ToArray()
                        );
                    }
                    break;

                default:
                    ConsoleWriter.Error("Commang not found");
                    break;
            }
        }
        isOpen = true;
    }

    public static void BookingsSection()
    {
        {
            Console.WriteLine("Main -> Bookings[Commands]:");
            Console.WriteLine("\tCreate");
            Console.WriteLine("\tGetByTripId");
            Console.WriteLine("\tConfirm");
            Console.WriteLine("\tCancel");
            Console.WriteLine("\tExit");
        }
        while (isOpen)
        {
            switch (Console.ReadLine().ToUpper())
            {
                case "EXIT":
                    isOpen = false;
                    Console.WriteLine("Goodbye!");
                    break;

                case "CREATE":
                    {
                        var createBookingDto = new CreateBookingDto();
                        {
                            Console.Write("TripId: ");
                            if (!int.TryParse(Console.ReadLine(), out int tripId))
                            {
                                break;
                            }
                            createBookingDto.TripId = tripId;
                            Console.Write("PassengerId: ");
                            if (!int.TryParse(Console.ReadLine(), out int passengerId))
                            {
                                break;
                            }
                            createBookingDto.PassengerId = passengerId;
                        }
                        var result = _bookingService.CreateBooking(createBookingDto);
                        if (!result.Succeed)
                        {
                            ConsoleWriter.Error("Errors");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine("\t" + error);
                            }
                            break;
                        }
                        ConsoleWriter.Info("Success");
                    }
                    break;

                case "CONFIRM":
                    {
                        var confirmBookingDto = new ConfirmBookingDto();
                        {
                            Console.Write("AppUserId: ");
                            if (!int.TryParse(Console.ReadLine(), out int appUserId))
                            {
                                break;
                            }
                            confirmBookingDto.AppUserId = appUserId;
                            Console.Write("BookingId: ");
                            if (!int.TryParse(Console.ReadLine(), out int bookingId))
                            {
                                break;
                            }
                            confirmBookingDto.BookingId = bookingId;
                        }
                        var result = _bookingService.ConfirmBooking(confirmBookingDto);
                        if (!result.Succeed)
                        {
                            ConsoleWriter.Error("Errors");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine("\t" + error);
                            }
                            break;
                        }
                        ConsoleWriter.Info("Success");
                    }
                    break;

                case "CANCEL":
                    {
                        var cancelBookingDto = new CancelBookingDto();
                        {
                            Console.Write("AppUserId: ");
                            if (!int.TryParse(Console.ReadLine(), out int appUserId))
                            {
                                break;
                            }
                            cancelBookingDto.AppUserId = appUserId;
                            Console.Write("BookingId: ");
                            if (!int.TryParse(Console.ReadLine(), out int bookingId))
                            {
                                break;
                            }
                            cancelBookingDto.BookingId = bookingId;
                        }
                        var result = _bookingService.CancelBooking(cancelBookingDto);
                        if (!result.Succeed)
                        {
                            ConsoleWriter.Error("Errors");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine("\t" + error);
                            }
                            break;
                        }
                        ConsoleWriter.Info("Success");
                    }
                    break;

                case "GETBYTRIPID":
                    {
                        Console.Write("TripId: ");
                        if (!int.TryParse(Console.ReadLine(), out int tripId))
                        {
                            break;
                        }
                        var result = _bookingService.GetByTripId(tripId);
                        if (!result.Succeed)
                        {
                            ConsoleWriter.Error("Errors");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine("\t" + error);
                            }
                            break;
                        }
                        ConsoleWriter.Info("Success");
                        ConsoleWriter.Table(
                            new[] { "Id", "TripId", "PassengerId" },
                            result.Data
                                .Select(booking => new[] 
                                {
                                    booking.BookingId.ToString(),
                                    booking.TripId.ToString(),
                                    booking.PassengerId.ToString(),
                                })
                                .ToArray()
                        );
                    }
                    break;
            }
        }
        isOpen = true;
    }

    public static void ReviewsSection()
    {
        {
            Console.WriteLine("Main -> Reviews[Commands]:");
            Console.WriteLine("\tCreate");
            Console.WriteLine("\tEdit");
            Console.WriteLine("\tDelete");
            Console.WriteLine("\tGetByTripId");
            Console.WriteLine("\tGetByAppUserId");
            Console.WriteLine("\tGetById");
            Console.WriteLine("\tExit");
        }
        while (isOpen)
        {
            switch (Console.ReadLine().ToUpper())
            {
                case "CREATE":
                    {
                        var createReviewDto = new CreateReviewDto();
                        {
                            Console.Write("TripId: ");
                            if (!int.TryParse(Console.ReadLine(), out int tripId))
                            {
                                break;
                            }
                            createReviewDto.TripId = tripId;
                            Console.Write("ReviewerId: ");
                            if (!int.TryParse(Console.ReadLine(), out int reviewerId))
                            {
                                break;
                            }
                            createReviewDto.ReviewerId = reviewerId;
                            Console.Write("Grade: ");
                            if (!int.TryParse(Console.ReadLine(), out int grade))
                            {
                                break;
                            }
                            createReviewDto.Grade = grade;
                            Console.Write("Description: ");
                            createReviewDto.Description = Console.ReadLine();
                        }
                        var result = _reviewService.CreateReview(createReviewDto);
                        if (!result.Succeed)
                        {
                            ConsoleWriter.Error("Errors");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine("\t" + error);
                            }
                            break;
                        }
                        ConsoleWriter.Info($"Success\n\t{result.Data}");
                    }
                    break;

                case "EDIT":
                    {
                        var editReviewDto = new EditReviewDto();
                        {
                            Console.Write("Id: ");
                            if (!int.TryParse(Console.ReadLine(), out int reviewId))
                            {
                                break;
                            }
                            editReviewDto.ReviewId = reviewId;
                            Console.Write("Grade: ");
                            if (!int.TryParse(Console.ReadLine(), out int grade))
                            {
                                break;
                            }
                            editReviewDto.Grade = grade;
                            Console.Write("Description: ");
                            editReviewDto.Description = Console.ReadLine();
                        }
                        var result = _reviewService.EditReview(editReviewDto);
                        if (!result.Succeed)
                        {
                            ConsoleWriter.Error("Errors");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine("\t" + error);
                            }
                            break;
                        }
                        ConsoleWriter.Info($"Success");
                    }
                    break;

                case "DELETE":
                    {
                        Console.Write("ReviewId: ");
                        if (!int.TryParse(Console.ReadLine(), out int reviewId))
                        {
                            break;
                        }
                        var result = _reviewService.DeleteReview(reviewId);
                        if (!result.Succeed)
                        {
                            ConsoleWriter.Error("Errors");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine("\t" + error);
                            }
                            break;
                        }
                        ConsoleWriter.Info("Success");
                    }
                    break;

                case "GETBYID":
                    {
                        Console.Write("ReviewId: ");
                        if (!int.TryParse(Console.ReadLine(), out int reviewId))
                        {
                            break;
                        }
                        var result = _reviewService.GetReviewById(reviewId);
                        if (!result.Succeed)
                        {
                            ConsoleWriter.Error("Errors");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine("\t" + error);
                            }
                            break;
                        }
                        ConsoleWriter.Info("Success");
                        ConsoleWriter.Table(
                            new[] { "Id", "Author", "Stars", "Description" },
                            new[,] 
                            {
                                {
                                    result.Data.ReviewId.ToString(),
                                    result.Data.ReviewerName ?? "null",
                                    result.Data.Grade.ToString(),
                                    result.Data.Description,
                                }
                            }
                        );
                    }
                    break;

                case "GETBYTRIPID":
                    {
                        Console.Write("TripId: ");
                        if (!int.TryParse(Console.ReadLine(), out int tripId))
                        {
                            break;
                        }
                        var result = _reviewService.GetReviewsByTripId(tripId);
                        if (!result.Succeed)
                        {
                            ConsoleWriter.Error("Errors");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine("\t" + error);
                            }
                            break;
                        }
                        ConsoleWriter.Info("Success");
                        ConsoleWriter.Table(
                            new[] { "Id", "Author", "Stars", "Description" },
                            result.Data
                                .Select(review => new[] 
                                {
                                    review.ReviewId.ToString(),
                                    review.ReviewerName ?? "null",
                                    review.Grade.ToString(),
                                    review.Description,
                                })
                                .ToArray()
                        );
                    }
                    break;

                case "GETBYAPPUSERID":
                    {
                        Console.Write("AppUserId: ");
                        if (!int.TryParse(Console.ReadLine(), out int appUserId))
                        {
                            break;
                        }
                        var result = _reviewService.GetReviewsByAppUserId(appUserId);
                        if (!result.Succeed)
                        {
                            ConsoleWriter.Error("Errors");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine("\t" + error);
                            }
                            break;
                        }
                        ConsoleWriter.Info("Success");
                        ConsoleWriter.Table(
                            new[] { "Id", "Author", "Stars", "Description" },
                            result.Data
                                .Select(review => new[]
                                {
                                    review.ReviewId.ToString(),
                                    review.ReviewerName ?? "null",
                                    review.Grade.ToString(),
                                    review.Description,
                                })
                                .ToArray()
                        );
                    }
                    break;

                case "EXIT":
                    isOpen = false;
                    Console.WriteLine("Goodbye!");
                    break;
            }
        }
        isOpen = true;
    }

    public static void NotificationsSection()
    {
        {
            Console.WriteLine("Main -> Notifications[Commands]:");
            Console.WriteLine("\tSend");
            Console.WriteLine("\tDelete");
            Console.WriteLine("\tGetByAppUserId");
            Console.WriteLine("\tGetById");
            Console.WriteLine("\tExit");
        }
        while (isOpen)
        {
            switch (Console.ReadLine().ToUpper())
            {
                case "SEND":
                    {
                        var sendNotificationDto = new SendNotificationDto();
                        {
                            Console.Write("ReciverId: ");
                            if (!int.TryParse(Console.ReadLine(), out int reciverId))
                            {
                                break;
                            }
                            sendNotificationDto.ReciverId = reciverId;
                            Console.Write("Grade: ");
                            sendNotificationDto.Description = Console.ReadLine();
                            Console.Write("Description: ");
                            sendNotificationDto.Description = Console.ReadLine();
                        }
                        var result = _notificationService.SendNotification(sendNotificationDto);
                        if (!result.Succeed)
                        {
                            ConsoleWriter.Error("Errors");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine("\t" + error);
                            }
                            break;
                        }
                        ConsoleWriter.Info($"Success");
                    }
                    break;

                case "DELETE":
                    {
                        Console.Write("Id: ");
                        if (!int.TryParse(Console.ReadLine(), out int notificationId))
                        {
                            break;
                        }
                        var result = _notificationService.DeleteNotification(notificationId);
                        if (!result.Succeed)
                        {
                            ConsoleWriter.Error("Errors");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine("\t" + error);
                            }
                            break;
                        }
                        ConsoleWriter.Info($"Success");
                    }
                    break;

                case "GETBYID":
                    {
                        Console.Write("Id: ");
                        if (!int.TryParse(Console.ReadLine(), out int notificationId))
                        {
                            break;
                        }
                        var result = _notificationService.GetNotificationById(notificationId);
                        if (!result.Succeed)
                        {
                            ConsoleWriter.Error("Errors");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine("\t" + error);
                            }
                            break;
                        }
                        ConsoleWriter.Info($"Id: {result.Data.NotificationId}");
                        Console.WriteLine($"\tReciver: {result.Data.Reciver.UserName ?? "null"}");
                        Console.WriteLine($"\tTitle: {result.Data.Title}");
                        Console.WriteLine($"\tDescription: {result.Data.Description}");
                    }
                    break;

                case "GETBYAPPUSERID":
                    {
                        Console.Write("AppUserId: ");
                        if (!int.TryParse(Console.ReadLine(), out int appUserId))
                        {
                            break;
                        }
                        var result = _notificationService.GetNotificationsByAppUserId(appUserId);
                        if (!result.Succeed)
                        {
                            ConsoleWriter.Error("Errors");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine("\t" + error);
                            }
                            break;
                        }
                        foreach (var notification in result.Data)
                        {
                            ConsoleWriter.Info($"Id: {notification.NotificationId}");
                            Console.WriteLine($"\tReciver: {notification.Reciver.UserName ?? "null"}");
                            Console.WriteLine($"\tTitle: {notification.Title}");
                            Console.WriteLine($"\tDescription: {notification.Description}");
                        }
                    }
                    break;

                case "EXIT":
                    isOpen = false;
                    Console.WriteLine("Goodbye!");
                    break;
            }
        }
        isOpen = true;
    }
}
