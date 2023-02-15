using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Take5.Models.Auth;
using Take5.Models.Models;
using Take5.Services.Contracts;
using Take5.Services.Contracts.Auth;
using Take5.Services.Models.MasterModels;

namespace Take5.Controllers
{
    [Authorize(Roles = "Supervisor, Admin")]
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService _employeeService;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;


        public EmployeeController(IEmployeeService employeeService,
            UserManager<AspNetUser> userManager,
            IRoleService roleService,
            IUserService userService)
        {
            _employeeService = employeeService;
            _userManager = userManager;
            _roleService = roleService;
            _userService = userService;
        }
        // GET: EmployeeController
        public ActionResult Index()
        {
            try
            {
                List<EmployeeModel> employeeModels = _employeeService.GetAllEmployees();
                employeeModels.ForEach(e => e.Email = _userManager.FindByIdAsync(e.UserId).Result.Email);
                return View(employeeModels);
            }
            catch(Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        // GET: EmployeeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            var models = _roleService.GetAllRoles();

            EmployeeModel model = new EmployeeModel
            {
                roles = models.Result.Where(r => r.Name != "Driver").ToList()
            };
            return View(model);
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EmployeeModel model)
        {
            try
            {
                string[] assignedRoles = { model.roleName };
                AspNetUser user = new AspNetUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    result = await _userManager.AddToRolesAsync(user, assignedRoles);
                    if (result.Succeeded)
                    {

                        model.UserId = user.Id;
                        var employee = _employeeService.CreateEmployee(model);
                        if (employee.Result != null)
                        {
                            TempData["Message"] = "Successful Process, new user are added";
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            var deleteResult = _userManager.RemoveFromRolesAsync(user, assignedRoles);
                            var deleteResult2 = _userService.DeleteUser(user.Id);
                            TempData["Error"] = "Failed Process, Can not add new Employee";
                        }
                    }
                    else
                    {
                        var deleteResult = _userService.DeleteUser(user.Id);
                        TempData["Error"] = "Failed Process, Can not add user to role";
                    }
                }
                else
                {
                    ViewBag.Error = "Failed Process, Can not add new user";
                }
                var models = _roleService.GetAllRoles();
                var roles = models.Result.Where(r => r.Name != "Driver").ToList();
                model.roles = roles;
                return View(model);
            }
            catch(Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("ERROR404");
            }
        }

        // GET: EmployeeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        public ActionResult ShowProfile()
        {
            try
            {

            }
            catch(Exception e)
            {

            }

            return null;
        }
    }
}
