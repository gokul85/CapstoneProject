﻿namespace AuthService.Models.DTOs
{
    public class ReturnDTO
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public bool ProfileStatus { get; set; }
    }
}
