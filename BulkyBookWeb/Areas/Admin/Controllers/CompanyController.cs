using BulkyBook.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BulkyBookWeb.Controllers;
[Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
    
        public CompanyController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
            
        }
        public IActionResult Index()
        {
            return View();
        }

        //Get
        public IActionResult Upsert(int? id)
        {
        Company company = new();
        
            if(id == null || id == 0)
            {
                //create product
                return View(company);
            }
            else
            {
            company = _UnitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
                //update product
            }
           return View(company);
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj, IFormFile? file)
        {
            
            if (ModelState.IsValid)
            {
           
            if (obj.Id == 0)
            { 
                _UnitOfWork.Company.Add(obj);
                TempData["success"] = "Company Created Successfully";
            }
            else
            {
                _UnitOfWork.Company.Update(obj);
                TempData["success"] = "Company Updated Successfully";
            }
            _UnitOfWork.Save();
            return RedirectToAction("Index");
            }

        return View(obj);
        }
          
    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var companyList = _UnitOfWork.Company.GetAll();
        return Json(new { data = companyList});
    }
    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var obj = _UnitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
        if (obj == null)
        {
            return Json(new { success = false, message = "Error while Deleting"});
        }
       

        _UnitOfWork.Company.Remove(obj);
        _UnitOfWork.Save();
        return Json(new { success = true, message = "Delete Successful" });


    }


    #endregion
}

