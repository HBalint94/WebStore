using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class HomeController : BaseController
    {
       //private readonly IStoreService storeService;

        public HomeController(IAccountService accountService, IStoreService storeService)
            :base(accountService,storeService)
        { }
        
        

        // kezdő oldal
        public IActionResult Index()
        {
            return View("Index",storeService.Categories.ToList());
        }

        public IActionResult Products(int categoryId)
        {
            // termékek listája
            IList<Product> prods = storeService.GetProductsBasedOnCategoryId(categoryId).ToList();

            return View("Products", prods);
        }
    }
}
