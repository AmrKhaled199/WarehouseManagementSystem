using System;
using System.Collections.Generic;
using System.Text;
using WMS.Models;

namespace WMS.BLL.Interfaces
{
    public interface IUserService
    {
        List<User> GetAll();
        User? GetById(int id);
        bool ChangeRole(int id, string newRole);

    }
}
