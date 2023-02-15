using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Take5.Models.Auth;
using Take5.Models.Models;
using Take5.Services.Contracts.Auth;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Take5.Services.Contracts;
using Take5.Services.Models.MasterModels;

namespace Take5.Service.Implementation.Auth
{
   public class UserService : IUserService
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public UserService(UserManager<AspNetUser> userManager,
        ILogger<UserService> logger, 
        IMapper mapper)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }
        async Task<List<UserModel>> IUserService.GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            var models = new List<UserModel>();
             models = _mapper.Map<List<UserModel>>(users);
            return models;
        }

     public async Task<UserModel> CreateUser(UserModel model)
        {
            AspNetUser user = new AspNetUser();
            user.UserName = model.Email;
            user.Email = model.Email;
        
            try
            {
                var result= await  _userManager.CreateAsync(user, model.Password);
                if(result.Succeeded)
                {
                    result = await _userManager.AddToRolesAsync(user, model.AsignedRolesNames);
                }
                if(result.Succeeded)
                {
                     AspNetUser addedUser = _userManager.FindByEmailAsync(model.Email).Result;
                    UserModel userModel = _mapper.Map<UserModel>(addedUser);

                    return userModel;
                }
                else
                {
                    return null;
                }

            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());

            }
            return (null);

        }

        async Task<bool> IUserService.DeleteUser(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);               
                List<string> userRoles =_userManager.GetRolesAsync(user).Result.ToList();
                var result =_userManager.RemoveFromRolesAsync(user, userRoles);
                if (result.IsCompletedSuccessfully)
                {
                    var response = _userManager.DeleteAsync(user);
                    if(response.IsCompletedSuccessfully)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                
            }
            catch(Exception e)
            {
                return false;
            }
    
        }

        async Task<UserModel> IUserService.GetUser(string id)
        {
            AspNetUser user = await _userManager.FindByIdAsync(id);
            var model = _mapper.Map<UserModel>(user);
            return model;
        }

        async Task <UserModel> IUserService.GetUserByUserName(string userName)
        {
            try
            {
                AspNetUser user = await _userManager.FindByNameAsync(userName);
                var model = _mapper.Map<UserModel>(user);
                return model;
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return null;
        }

        async Task<bool> IUserService.UpdateUser(UserModel model)
        {
            try
            {
                AspNetUser user = await _userManager.FindByIdAsync(model.id);
                user.Email = model.Email;
                var response = await _userManager.UpdateAsync(user);
                response = await _userManager.AddToRolesAsync(user, model.AsignedRolesNames);
                return true;
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return false;
        }

       public   List<string> GetUserRoles(UserModel model)
        {
            AspNetUser user = new AspNetUser();
            user = _mapper.Map<AspNetUser>(model);
            List<string> rolesName =  (List<string>)_userManager.GetRolesAsync(user).Result;
            return rolesName;
        }

        public Task<List<UserModel>> GetUsersByRole(RoleModel role)
        {
            try
            {
                
            }
            catch
            {

            }
            return null;
        }

         Task<List<UserModel>> IUserService.GetUsersByRole(string roleid)
        {
            return null;
        }

        public async Task<UserModel> SearchEmail(string email)
        {
            try
            {
               AspNetUser aspNetUser = _userManager.FindByEmailAsync(email).Result;
                UserModel userModel = _mapper.Map<UserModel>(aspNetUser);

                return userModel;
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return null;
            }
        }

        //public async Task<AspNetUser> UserLogin(long EmployeeNumber, string Password, bool RememberMe)
        //{
        //    try
        //    {
        //       EmployeeModel employee =  _employeeService.GetEmployee(EmployeeNumber);
        //        if(employee != null)
        //        {
        //           AspNetUser user = _userManager.FindByIdAsync(employee.UserId).Result;
        //           //bool result = _userManager.CheckPasswordAsync(user, Password).Result;
        //            if(user != null)
        //            {
        //                return user;
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //    }
        //    catch(Exception e)
        //    {
        //    }
        //    return null;

        //}
    }
}
