using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Take5.Services.Contracts;

namespace Take5.Controllers
{
    [Authorize(Roles = "Supervisor, Admin")]
    public class MeasureControlController : BaseController
    {
        private readonly IDangerService _dangerService;
        private readonly IDangerCategoryService _dangerCategoryService;
        private readonly IMeasureControlService _measureControlService;


        public MeasureControlController(IDangerService dangerService,
            IDangerCategoryService dangerCategoryService,
            IMeasureControlService measureControlService)
        {
            _dangerService = dangerService;
            _dangerCategoryService = dangerCategoryService;
            _measureControlService = measureControlService;
        }

        // GET: MeasureControlController
        public ActionResult Index()
        {
            return View();
        }

        // GET: MeasureControlController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MeasureControlController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MeasureControlController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: MeasureControlController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MeasureControlController/Edit/5
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
                return View();
            }
        }

        // GET: MeasureControlController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MeasureControlController/Delete/5
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
    }
}
