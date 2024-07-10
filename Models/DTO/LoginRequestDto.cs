using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NZWalksAPI.Models.DTO
{
    public class LoginRequestDto
    {     
          [Required]
          [DataType(DataType.EmailAddress)]
          public string Username { get; set; }

          public string Password{ get; set; }
    }
}