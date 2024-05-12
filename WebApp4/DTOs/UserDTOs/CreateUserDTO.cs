namespace WebApp4.DTOs.UserDTOs
{
    public class CreateUserDTO
    {
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }
}
