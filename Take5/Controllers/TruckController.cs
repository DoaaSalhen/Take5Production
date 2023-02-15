using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Take5.Services.Contracts;
using Take5.Services.Models.MasterModels;

namespace Take5.Controllers
{
    [Authorize(Roles = "Supervisor, Admin")]
    public class TruckController : BaseController
    {
        private readonly ITruckService _truckService;

        public TruckController(ITruckService truckService)
        {
            _truckService = truckService;
        }
        // GET: TruckController
        public ActionResult Index()
        {
            try
            {
                List<TruckModel> truckModels = _truckService.GetAllTrucks().ToList();
                return View(truckModels);
            }
            catch(Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        // GET: TruckController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TruckController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TruckController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TruckModel model)
        {
            try
            {

                bool result = _truckService.CreateTruck(model).Result;
                if (result == true)
                {
                    TempData["Message"] = "Truck Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Error"] = "Truck Created Successfully";
                    return View(model);
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        // GET: TruckController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TruckController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }

        // GET: TruckController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TruckController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return RedirectToAction("ERROR404");
            }
        }
    }
}
