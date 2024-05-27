using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Dtos
{
    public class RegisterDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]

        public string Password { get; set; }
    }
}
