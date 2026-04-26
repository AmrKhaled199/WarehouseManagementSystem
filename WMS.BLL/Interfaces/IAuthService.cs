using System;
using System.Collections.Generic;
using System.Text;
using WMS.Models;

namespace WMS.BLL.Interfaces
{
    public interface IAuthService
    {
        User? Login(string email, string password);
        bool Register(User user, string password);
        bool IsEmailExists(string email);
    }
}
