using GemCare.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemCare.Data.Interfaces
{
    public interface IUserRepository
    {
        (int status, string message, UserProfileDTO profile) GetUserPRofile(int id);
        (int status, string message) UpdateUserPRofile(UserProfileDTO model);

        (int status, string message) UpdateProfileImage(string ImageUrl, int Userid);
    }
}
