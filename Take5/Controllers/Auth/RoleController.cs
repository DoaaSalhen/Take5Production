using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Take5.Models.Auth;
using Take5.Services.Contracts.Auth;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Take5.Models.Models;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Take5.Services.Models.MasterModels;

namespace MoreForYou.Controllers.Auth
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IMapper _mapper;
        public RoleController(IRoleService roleService,
            UserManager<AspNetUser> userManager,
            IMapper mapper)
        {
            _roleService = roleService;
            _userManager = userManager;
            _mapper = mapper;
        }
        // GET: RoleController
        async public Task<ActionResult> Index()
        {
            var models=await _roleService.GetAllRoles();
            return View(models);
        }

        // GET: RoleController/Details/5
        public ActionResult Details(string id)
        {
            RoleModel role= _roleService.GetRole(id).Result;
            var aspNetUsers = _userManager.GetUsersInRoleAsync(role.Name).Result;
            if(aspNetUsers != null)
            {
                List<User> users = _mapper.Map<List<User>>(aspNetUsers);
                role.users = users;
            }
            return View(role);
        }

        // GET: RoleController/Create
        public  ActionResult Create()
        {
            return View();
        }

        // POST: RoleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleModel model)
        {
            try
            {
                var result=await _roleService.CreateRole(model);
                if(result == true)
                {
                    TempData["Message"] = model.Name + " Role are created successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Error = "Role can not be created";
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: RoleController/Edit/5
        public async Task<ActionResult> Edit(string Name)
        {
            var model = await _roleService.GetRoleByName(Name);
            return View(model);
        }

        // POST: RoleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, RoleModel model)
        {
            try
            {
                var response=await _roleService.UpdateRole(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RoleController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RoleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, RoleModel model)
        {
            try
            {
                var result = await _roleService.DeleteRole(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
