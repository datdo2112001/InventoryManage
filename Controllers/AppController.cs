using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SaleManagement.Models;
using Microsoft.AspNetCore.Http;

// Bắt các truy vấn từ phía người dùng và trả về view tương ứng.
namespace SaleManagement.Controllers
{
    public class AppController : Controller
    {
        private readonly Service _service;

        public AppController(Service service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        // bắt truy vấn và trả về view mặc định của app.
        public IActionResult InventoryManage(int page = 1, string orderBy = "Price")
        {

            var model = _service.Paging(page, orderBy);
            ViewData["orderBy"] = orderBy;
            ViewData["Pages"] = model.pages;
            ViewData["Page"] = model.page;
            ViewData["Count"] = _service.GetAll().Count<Product>();
            return View(model.products);
        }

        // trả về thông tin ghi chú.
        [HttpGet]
        public JsonResult GetNote(string productCode)
        {
            Product p = _service.Get(productCode);
            return Json(p.Note);
        }

        // trả về giao diện tạo sản phẩm mới.
        public IActionResult Create() => View(_service.Create());

        // cập nhật thông tin sản phẩm vào db và quay về giao diện mặc định.
        [HttpPost]
        public IActionResult Create(Product p, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                _service.Add(p);
                _service.UploadImage(p, file);
                return RedirectToAction("InventoryManage");
            }
            return View();
        }

        // trả về đường dẫn ảnh
        [HttpGet]

        public JsonResult GetImage(string productCode)
        {
            Product p = _service.Get(productCode); 
            return Json(p.ImagePath);
        }

        // trả về giao diện xác nhận xóa sản phẩm
        public IActionResult ConfirmDelete(string productCode)
        {
            Product p = _service.Get(productCode);
            if (p == null)
            {
                return NotFound();
            }
            return View(p);

        }

        // xóa sản phẩm và quay về trang ban đầu
        [HttpPost]
        public IActionResult Delete(string productCode) 
        {
            Product p = _service.Get(productCode);
            _service.Delete(p);
            return RedirectToAction("InventoryManage");
        }

        // trả về giao diện edit sản phẩm.
        public IActionResult Edit(string productCode)
        {
            Product p = _service.Get(productCode);
            if (p == null) return NotFound();
            else return View(p);
        }

        // lưu những thông tin thay đổi và quay về trang mặc định.
        [HttpPost]
        public IActionResult Edit(Product p, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                _service.Update(p);
                _service.UploadImage(p, file);
                return RedirectToAction("InventoryManage");
            }
            return View();
        }

        // trả về giao hiện hiển thị danh sách sản phẩm tìm kiếm được.
        public IActionResult Search(string keyword)
        {
            if (keyword != null)
            {
                return View("InventoryManage", _service.GetSearchResults(keyword));
            }
            else return null;
        }



    }
}
