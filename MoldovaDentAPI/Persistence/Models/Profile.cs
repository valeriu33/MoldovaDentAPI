﻿using System;
using System.ComponentModel.DataAnnotations;

namespace MoldovaDentAPI.Persistence.Models
{
    public class Profile
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
    }
}
