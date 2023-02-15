using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Take5.Models.Auth;
using Take5.Models.Models;
using Take5.Services.Contracts.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Take5.Service.Implementation.Auth
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<AspNetUser> _userManager;

        private readonly RoleManager<AspNetRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleService> _logger;

        public RoleService( UserManager<AspNetUser> userManager,
         RoleManager<AspNetRole> roleManager,
         IMapper mapper,
        ILogger<RoleService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _mapper = mapper;
        }       

        async Task<bool> IRoleService.CreateRole(RoleModel model)
        {
            try
            {
                AspNetRole role = new AspNetRole();
                role.Name = model.Name;
                role.IsVisible = true;
                role.IsDelted = false;
                role.CreatedDate = DateTime.Now;
                role.UpdatedDate = DateTime.Now;
                var response = await _roleManager.CreateAsync(role);
                return response.Succeeded;
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return false;
        }

        async Task<bool> IRoleService.DeleteRole(RoleModel model)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(model.Id);
                //var role = _mapper.Map<IdentityRole>(model);
                var response = await _roleManager.DeleteAsync(role);
                return true;
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return false;
        }

        async Task<List<RoleModel>> IRoleService.GetAllRoles()
        {
            List<AspNetRole> roles=_roleManager.Roles.ToList();
            List<RoleModel> models = _mapper.Map<List<RoleModel>>(roles);
            return models;
        }

        async Task<RoleModel> IRoleService.GetRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var model = _mapper.Map<RoleModel>(role);
            return model;
        }

        async Task<RoleModel> IRoleService.GetRoleByName(string name)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync(name);
                var model = _mapper.Map<RoleModel>(role);
                return model;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        async Task<bool> IRoleService.UpdateRole(RoleModel model)
        {
            try
            {
                //model.IsVisible = true;
                AspNetRole role = await _roleManager.FindByIdAsync(model.Id);
                role.Name = model.Name;
                //var role = _mapper.Map<IdentityRole>(model);
                var response =await _roleManager.UpdateAsync(role);
                return true;

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
            return false;
        }
    }
}
