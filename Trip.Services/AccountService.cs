using System.Text.RegularExpressions;
using Trip.Domain;
using Trip.Repos;
using Trip.Service;
using Trip.Services.Dtos.Accounts;
using Trip.Services.Helpers;

namespace Trip.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepo _accountRepo;
        private readonly INotificationService _notificationService;

        public AccountService(IAccountRepo accountRepo, INotificationService notificationService)
        {
            _accountRepo = accountRepo;
            _notificationService = notificationService;
        }

        public Result CreateAppUser(CreateAppUserDto registerAppUserDto)
        {
            if (!Regex.IsMatch(registerAppUserDto.UserName, @"^[a-zA-Z0-9]+$"))
            {
                return Result.Fail("Invalid username");
            }
            if (!Regex.IsMatch(registerAppUserDto.Email, @"^[a-zA-Z0-9]+@gmail.com+$"))
            {
                return Result.Fail("Invalid email");
            }
            if (_accountRepo.GetAppUserByEmail(registerAppUserDto.Email) != null)
            {
                return Result.Fail("This email address is already registered");
            }
            if ((registerAppUserDto.Password == null || registerAppUserDto.PasswordConfirm == null) || 
                (!Regex.IsMatch(registerAppUserDto.Password, @"^[a-zA-Z0-9]+$") &&
                !string.Equals(registerAppUserDto.Password, registerAppUserDto.PasswordConfirm)))
            {
                return Result.Fail("Passwords do not match or invalid password");
            }
            // TODO: [idea] Add password hashing
            var appUser = new AppUser
            {
                UserName = registerAppUserDto.UserName,
                Email = registerAppUserDto.Email,
                Password = registerAppUserDto.Password,
            };
            _accountRepo.CreatAppUser(appUser);
            _accountRepo.SaveChanges();
            _notificationService.SendNotification(NotificationHelper.WelcomeNotification(appUser.AppUserId));
            return Result.Success();
        }

        public Result EditAppUser(EditAppUserDto editAppUserDto)
        {
            var appUser = _accountRepo.GetAppUserById(editAppUserDto.AppUserId);
            if (appUser == null)
            {
                return Result.Fail("The user id is not found");
            }
            appUser.UserName = editAppUserDto.UserName;
            appUser.Email = editAppUserDto.Email;
            appUser.Password = editAppUserDto.Password;
            _accountRepo.UpdateAppUser(appUser);
            _accountRepo.SaveChanges();
            return Result.Success();
        }

        public Result<AppUserDto> GetAppUserById(int appUserId)
        {
            var appUser = _accountRepo.GetAppUserById(appUserId);
            if (appUser == null)
            {
                return Result<AppUserDto>.Fail("The user id is not found");
            }
            return Result<AppUserDto>.Success(new AppUserDto 
            {
                AppUserId = appUser.AppUserId,
                UserName = appUser.UserName,
                Email = appUser.Email,
                Password = appUser.Password,
            });
        }

        public Result<AppUserDto> LoginAppUser(LoginAppUserDto loginAppUserDto)
        {
            var appUser = _accountRepo.GetAppUserByEmail(loginAppUserDto.Email);
            if (appUser == null)
            {
                return Result<AppUserDto>.Fail("Incorrect password");
            }
            if (!string.Equals(appUser.Password, loginAppUserDto.Password))
            {
                return Result<AppUserDto>.Fail("User is not registered");
            }
            // TODO: Use automapper
            var appUserDto = new AppUserDto
            {
                AppUserId = appUser.AppUserId,
                UserName = appUser.UserName,
                Email = appUser.Email,
                Password = appUser.Password,
            };
            return Result<AppUserDto>.Success(appUserDto);
        }
    }
}
