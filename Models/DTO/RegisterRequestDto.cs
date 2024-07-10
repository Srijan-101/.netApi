using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NZWalksAPI.Models.DTO
{
    public class RegisterRequestDto
    {   
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }

        public string[] Roles {get;set;}
    }
}