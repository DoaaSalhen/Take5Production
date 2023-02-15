using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Take5.Models.Models;
using Take5.Services.Contracts;
using Take5.Services.Models.MasterModels;

namespace Take5.Controllers
{
    [Authorize(Roles = "Supervisor, Admin")]
    public class DriverController : BaseController
    {
        private readonly IDriverService _driverService;
        private readonly  UserManager<AspNetUser> _userManager;
        private readonly  RoleManager<AspNetRole> _roleManager;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly IExcelService _excelService;
        public DriverController(IDriverService driverService,
            UserManager<AspNetUser> userManager,
            RoleManager<AspNetRole> roleManager,
            IWebHostEnvironment environment,
            IConfiguration configuration,
            IExcelService excelService)
        {
            _driverService = driverService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _environment = environment;
            _excelService = excelService;
        }

        public bool CheckDriverNumberIdentity(long driverId)
        {
            DriverModel driverModel = _driverService.GetDriver(driverId);
            if (driverModel != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        // GET: DriverController
        public ActionResult Index()
        {
            try
            {
                List<DriverModel> driverModels = _driverService.GetAllDrivers();
                if (TempData["Message"] != null)
                {
                    ViewBag.Message = TempData["Message"];
                }
                return View(driverModels);
            }
            catch (Exception e) 
            { 
                return RedirectToAction("ERROR404"); 
            }
        }

        // GET: DriverController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DriverController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DriverController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DriverModel model)
        {
            try
            {
                AspNetUser aspNetUser = new AspNetUser { Email = model.Email, UserName = model.Email, CreatedDate = DateTime.Now, UpdatedDate =DateTime.Now };
                var result = await _userManager.CreateAsync(aspNetUser, model.Password);
                if(result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(aspNetUser, "Driver");
                    model.UserId = aspNetUser.Id;
                    var isDriverCreated = _driverService.CreateDriver(model).Result;
                    if (isDriverCreated == true)
                    {
                        TempData["Message"] = "Driver with number " + model.Id + " are created successfully";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                       var deleteresult = _userManager.DeleteAsync(aspNetUser);
                        if(deleteresult.IsCompletedSuccessfully)
                        {
                            TempData["Error"] = "Failed to add new driver";
                        }
                        else
                        {
                            TempData["Error"] = "there is an fatal error, please contact your technical support";
                        }
                        return View(model);
                    }
                }
                else
                {
                    TempData["Error"] = "Failed to add new user";
                    return View(model);
                }
            }
            catch
            {
                return RedirectToAction("ERROR404");

            }
        }

        // GET: DriverController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                DriverModel driverModel = _driverService.GetDriver(id);
                return View(driverModel);
            }
            catch(Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        // POST: DriverController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DriverModel driverModel)
        {
            try
            {
                DriverModel driverModel1 = _driverService.GetDriver(driverModel.Id);
                driverModel1.FullName = driverModel.FullName;
                driverModel1.Address = driverModel1.Address;
                driverModel1.PhoneNumber = driverModel.PhoneNumber;
                driverModel1.IsVisible = driverModel.IsVisible;
                bool result = _driverService.UpdateDriver(driverModel1).Result;
                if(result == true)
                {
                    TempData["Message"] = "Successful Process";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Failed Process";
                }
                return View(driverModel1);
            }
            catch
            {
                return RedirectToAction("ERROR404");
            }
        }

        // GET: DriverController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DriverController/Delete/5
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
                return RedirectToAction("ERROR404");
            }
        }

        public async Task<IActionResult> AddDriver()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDriver(IFormFile postedFile)
        {
            try
            {
                
                bool fixResult = false;
                string ExcelConnectionString = this._configuration.GetConnectionString("ExcelCon");
                DataTable dt = null;
                string Email = "";
                string password = "";
                if (postedFile != null)
                {
                    //Create a Folder.
                    string path = Path.Combine(this._environment.WebRootPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    //Save the uploaded Excel file.
                    string fileName = Path.GetFileName(postedFile.FileName);
                    string filePath = Path.Combine(path, fileName);
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                    }
                    dt =_excelService.ReadExcelData(filePath, ExcelConnectionString);
                    Dictionary<long, string> DriverData = new Dictionary<long, string>();
                    if (dt != null)
                    {
                        for (int index = 0; index < dt.Columns.Count; index++)
                        {
                            dt.Columns[index].ColumnName = dt.Columns[index].ColumnName.Trim();
                        }
                
                            foreach (DataRow row in dt.Rows)
                            {
                             Email = "Driver" +row["Number"] + "@gmail.com";
                             password = _driverService.CreateRandomPassword(4);
                             
                             AspNetUser aspNetUser = new AspNetUser { Email = Email, UserName = Email, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now };
                             var result = await _userManager.CreateAsync(aspNetUser, password);
                                if (result.Succeeded)
                                {
                                    //row["PhoneNumber"] = "0" + row["PhoneNumber"].ToString();
                                    await _userManager.AddToRoleAsync(aspNetUser, "Driver");
                                    DriverModel driverModel = new DriverModel();
                                    driverModel.Id = Convert.ToInt64(row["Number"]);
                                    driverModel.FullName = row["FullName"].ToString();
                                    driverModel.PhoneNumber = "0" + row["PhoneNumber"].ToString();
                                    driverModel.Address = "Assiut";
                                    driverModel.IsDelted = false;
                                    driverModel.IsVisible = true;
                                    driverModel.UpdatedDate = DateTime.Now;
                                    driverModel.CreatedDate = DateTime.Now;
                                    driverModel.UserId = aspNetUser.Id;
                                   var createResult = await _driverService.CreateDriver(driverModel);
                                   if(createResult == true)
                                    {
                                        DriverData.Add(driverModel.Id, password);
                                    }
                                }
                                else
                                {
                                    await _userManager.DeleteAsync(aspNetUser);
                                }
                            }
                        }
                    if (DriverData.Count > 0)
                    {
                       var memoryStream = _excelService.ExportDriversDataToExcel(DriverData);
                        return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExportDriverData.xlsx");
                    }
                }
                else
                {
                    ViewBag.Error = "File Not Uploaded, Please Select Valid File";
                    return View();
                }
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }
    }
}
