using Trip.Service;
using Trip.Services.Dtos.Accounts;

namespace Trip.Services
{
    public interface IAccountService
    {
        Result CreateAppUser(CreateAppUserDto registerAppUserDto);
        Result<AppUserDto> LoginAppUser(LoginAppUserDto loginAppUserDto);
        Result<AppUserDto> GetAppUserById(int appUserId);
        Result EditAppUser(EditAppUserDto editAppUserDto);
    }
}
