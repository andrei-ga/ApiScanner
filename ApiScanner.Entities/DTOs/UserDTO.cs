using System;

namespace ApiScanner.Entities.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordRepeat { get; set; }
        public bool? RememberLogin { get; set; }
        public bool? Subscribe { get; set; }
        public string ResetToken { get; set; }
    }
}
