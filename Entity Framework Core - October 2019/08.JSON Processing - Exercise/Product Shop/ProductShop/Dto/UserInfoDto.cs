using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.Dto
{
    // for problem 8
    public class UserInfoDto
    {
        public int UsersCount { get; set; }

        public UserDetailsDto[] Users { get; set; }
    }
}
