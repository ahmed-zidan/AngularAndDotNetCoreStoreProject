using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Dtos
{
    public class UserLoginResDto
    {
        public string Name { get; set; }
        public DateTime expired { get; set; }

        public string Token { get; set; }
    }
}
