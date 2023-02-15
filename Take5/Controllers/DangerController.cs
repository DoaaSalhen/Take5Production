using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Take5.Services.Contracts;
using Take5.Services.Models;
using Take5.Services.Models.MasterModels;

namespace Take5.Controllers
{
    [Authorize(Roles = "Supervisor, Admin")]
    public class DangerController : BaseController
    {
        private readonly IDangerService _dangerService;
        private readonly IDangerCategoryService _dangerCategoryService;
        private readonly IMeasureControlService _measureControlService;


        public DangerController(IDangerService dangerService,
            IDangerCategoryService dangerCategoryService,
            IMeasureControlService measureControlService)
        {
            _dangerService = dangerService;
            _dangerCategoryService = dangerCategoryService;
            _measureControlService = measureControlService;
        }

        public JsonResult GetMeasureControlsByDanger(long dangerId)
        {
           List<MeasureControlModel> measureControlModels = _measureControlService.GetMeasureControlsByDangerId(dangerId);
            
            return Json(new SelectList(measureControlModels, "Id", "Name"));
        }


        // GET: DangerController
        public ActionResult Index()
        {
            try
            {
                if (TempData["error"] != null)
                {
                    ViewBag.Error = TempData["error"];
                }
                List<DangerModel> dangerModels = _dangerService.GetAllDangers().ToList();
                return View(dangerModels);
            }
            catch(Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        // GET: DangerController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                DangerModel dangerModel = _dangerService.GetDanger(id);
                return View(dangerModel);
            }
            catch(Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        // GET: DangerController/Create
        public ActionResult CreateGet(DangerModel model = null)
        {
            try
            {
                model = model ?? new DangerModel();
                List<DangerCategoryModel> dangerCategoryModels = _dangerCategoryService.GetAllDangerCategories().ToList();
                dangerCategoryModels.Insert(0, new DangerCategoryModel { Id = 0, Name = "Select Category" });
                model.DangerCategoryModels = dangerCategoryModels;
                return View("Create", model);
            }
            catch(Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        // POST: DangerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DangerModel model)
        {
            try
            {
                string iconName = await _dangerService.UploadedImageAsync(model.Iconfile, CommanData.DangerIconFolder);
                if (iconName != null)
                {
                    model.Icon = iconName;
                    bool result = _dangerService.CreateDanger(model).Result;
                    if (result == true)
                    {
                        TempData["message"] = "Sucess Process, new Danger created successfully";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["error"] = "Failed Process, Can not upload icon";
                        return RedirectToAction("CreateGet", model);
                    }
                }
                else
                {
                    TempData["error"] = "Failed Process, Can not upload icon";
                    return RedirectToAction("CreateGet", model);
                }
            }
            catch(Exception e)
            {
                TempData["error"] = "Failed Process, Can not upload icon";
                return RedirectToAction("CreateGet", model);
            }
        }

        // GET: DangerController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                DangerModel dangerModel = _dangerService.GetDanger(id);
                if (dangerModel != null)
                {
                    dangerModel.DangerCategoryModels = _dangerCategoryService.GetAllDangerCategories();
                    return View(dangerModel);
                }
                else
                {
                    TempData["error"] = "Failed Process, Danger is not found";
                    return RedirectToAction("Index");
                }
            }
            catch(Exception e)
            {
                return RedirectToAction("ERROR404");
            }

        }

        // POST: DangerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DangerModel model)
        {
            try
            {
                DangerModel danger = _dangerService.GetDanger(model.Id);
                if (model.Iconfile != null)
                {
                    string imageName = _dangerService.UploadedImageAsync(model.Iconfile, CommanData.DangerIconFolder).Result;
                    if(imageName != null)
                    {
                        danger.Icon = imageName;
                    }
                    else
                    {
                        ViewBag.error = "failed Process, can not update danger image";
                        danger.DangerCategoryModels = _dangerCategoryService.GetAllDangerCategories();
                        return View(danger);
                    }
                }
                danger.DangerCategoryId = model.DangerCategoryId;
                danger.IsVisible = model.IsVisible;
                danger.Name = model.Name;
                danger.UpdatedDate = DateTime.Now;
                bool result = _dangerService.UpdateDanger(danger).Result;
                if(result == true)
                {
                    TempData["Message"] = "Sucessful Process, danger is updated";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Message"] = "failed Process, danger not updated";
                    danger.DangerCategoryModels = _dangerCategoryService.GetAllDangerCategories();
                    return View(danger);
                }
            }
            catch(Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        // GET: DangerController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: DangerController/Delete/5
        public bool Delete(int id)
        {
            try
            {
                DangerModel danger = _dangerService.GetDanger(id);
                danger.IsVisible = false;
                danger.IsDelted = true;
                danger.UpdatedDate = DateTime.Now;
                bool result = _dangerService.UpdateDanger(danger).Result;
                if(result == true)
                {
                    result = _measureControlService.DeleteMeasureControlFordanger(id);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false ;
            }
        }
    }
}
