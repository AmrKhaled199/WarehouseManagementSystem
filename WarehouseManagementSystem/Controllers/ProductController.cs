using Microsoft.AspNetCore.Mvc;
using WarehouseManagementSystem.Filters;
using WarehouseManagementSystem.ViewModels;
using WMS.BLL.Interfaces;
using WMS.Models;

namespace WarehouseManagementSystem.Controllers
{
    [AuthFilter]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly INotificationService _notificationService;
        private readonly IStorageFeeService _storageFeeService;

        public ProductController(
            IProductService productService,
            INotificationService notificationService,
            IStorageFeeService storageFeeService)
        {
            _productService = productService;
            _notificationService = notificationService;
            _storageFeeService = storageFeeService;
        }

        public IActionResult Index(string? search)
        {
            var products = string.IsNullOrWhiteSpace(search)
                ? _productService.GetAll()
                : _productService.Search(search);

            ViewBag.TotalCount = _productService.GetTotalCount();
            ViewBag.TotalWeight = _productService.GetTotalWeight();
            ViewBag.Search = search ?? "";
            return View(products);
        }

        public IActionResult Add() => View();

        [HttpPost]
        public async Task<IActionResult> Add(ProductViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var product = new Product
            {
                Name = model.Name,
                Category = model.Category,
                WeightKg = model.WeightKg,
                ExpiryDate = model.ExpiryDate,
                SenderName = model.SenderName,
                SenderEmail = model.SenderEmail,
                ReceiverName = model.ReceiverName,
                ReceiverEmail = model.ReceiverEmail
            };

            bool result = _productService.Add(product);

            if (!result)
            {
                ModelState.AddModelError("", "The warehouse has reached its maximum weight or number limit!");
                return View(model);
            }

            // ✅ بعد الإضافة: ابعت إيميل دخول واحسب رسوم
            // Email في try-catch — لو فشل الـ SMTP مش هيكسر الـ flow
            try { await _notificationService.SendEntryEmail(product.Id); }
            catch { /* SMTP failure is non-critical — product was saved successfully */ }

            _storageFeeService.Calculate(product.Id);

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var product = _productService.GetById(id);
            if (product == null) return NotFound();

            // ✅ عرض رسوم التخزين في صفحة التفاصيل
            ViewBag.StorageFee = _storageFeeService.Calculate(id);

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Deliver(int id)
        {
            _productService.UpdateStatus(id, "Delivered");

            // ✅ بعد التسليم: ابعت إيميل خروج وشحن واحسب الرسوم النهائية
            try
            {
                await _notificationService.SendExitEmail(id);
                await _notificationService.SendShippingEmail(id);
            }
            catch { /* SMTP failure is non-critical — status was updated successfully */ }

            _storageFeeService.Calculate(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [AdminOnlyFilter]
        public IActionResult Delete(int id)
        {
            _productService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}