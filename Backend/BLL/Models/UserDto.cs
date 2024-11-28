using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }

    }
    public class UserDetailsDto : UserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class MemberDetailsDto : UserDetailsDto
    {
        public decimal Balance { get; set; }
    }
    public class EmployeeDto : UserDetailsDto
    {
        public decimal Salary { get; set; }
    }
    public class RegisterDto : LoginDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
    }

    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
