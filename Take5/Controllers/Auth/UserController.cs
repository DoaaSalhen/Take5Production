using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Take5.Services.Contracts.Auth;
using Take5.Models.Auth;

namespace Take5.Controllers.Auth
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _RoleService;

        // GET: ItemController
        public UserController(IUserService userService, IRoleService RoleService)
        {
            _RoleService = RoleService;
            _userService = userService;
        }

        // GET: AdminController
        public ActionResult Index()
        {
            Task<List<UserModel>> models = _userService.GetAllUsers();
            if(models != null)
            {
                foreach(var user in models.Result)
                {
                   var userRoles = _userService.GetUserRoles(user);
                    user.AsignedRolesNames = userRoles.ToArray();
                }
            }
            return View(models.Result);
        }

        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/Create
        public async Task<ActionResult> Create()
        {
            var models = _RoleService.GetAllRoles();

            UserModel model = new UserModel
            {
                roles = models.Result.Where(r => r.Name != "Driver").ToList()
            };
            return View(model);
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response= await _userService.CreateUser(model);
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
            return View();
        }

        // GET: AdminController/Edit/5
        async public Task<ActionResult> Edit(string id)
        {
            var model=await _userService.GetUser(id);
           model.AsignedRolesNames = (string[])_userService.GetUserRoles(model).ToArray();
            var Rolemodels = _RoleService.GetAllRoles();
            model.roles = Rolemodels.Result;
            foreach (string name in model.AsignedRolesNames)
            {
                foreach(RoleModel role in model.roles)
                {
                    if(name ==role.Name)
                    {
                        role.IsChacked = true;
                    }
                }

            }
            return View(model);
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        async public Task<ActionResult> Edit(string id, UserModel model)
        {
            try
            {
                List<string> NewUserRolesNames = new List<string>();

                string[] OldUserRolesNames = (string[])_userService.GetUserRoles(model).ToArray();
                if(OldUserRolesNames.Count()!=0)
                {
                    foreach (string roleName in model.AsignedRolesNames)
                    {
                        foreach (string name in OldUserRolesNames)
                        {
                            if (name != roleName)
                            {
                                NewUserRolesNames.Add(roleName);
                            }
                        }
                    }
                    model.AsignedRolesNames = NewUserRolesNames.ToArray();
                }
               
                await _userService.UpdateUser(model);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, IFormCollection collection)
        {
            try
            {
                await _userService.DeleteUser(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
