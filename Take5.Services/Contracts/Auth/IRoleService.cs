﻿using Take5.Models.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Take5.Services.Contracts.Auth
{
   public interface IRoleService
    {
        Task<List<RoleModel>> GetAllRoles();
        Task<bool> CreateRole(RoleModel model);
        Task<bool> UpdateRole(RoleModel model);
        Task<bool> DeleteRole(RoleModel model);
        Task<RoleModel> GetRole(string id);
        Task<RoleModel> GetRoleByName(string name);
    }
}
