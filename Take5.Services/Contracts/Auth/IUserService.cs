using Microsoft.AspNetCore.Identity;
using Take5.Models.Auth;
using Take5.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Take5.Services.Contracts.Auth
{
    public interface IUserService
    {
        Task<List<UserModel>> GetAllUsers();
        Task<UserModel> CreateUser(UserModel model);
        Task<bool> UpdateUser(UserModel model);
        Task<bool> DeleteUser(string id);
        Task<UserModel> GetUser(string id);
        Task<UserModel> GetUserByUserName(string userName);
        public List<string> GetUserRoles(UserModel model);
        Task<List<UserModel>> GetUsersByRole(string roleid);

        Task<UserModel> SearchEmail(string email);
        //Task<AspNetUser> UserLogin(long EmployeeNumber, string Email, bool RememberMe);





    }
}
