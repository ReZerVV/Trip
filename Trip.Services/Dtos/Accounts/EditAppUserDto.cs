namespace Trip.Services.Dtos.Accounts
{
    public class EditAppUserDto
    {
        public int AppUserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
