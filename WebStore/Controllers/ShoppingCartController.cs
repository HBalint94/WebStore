using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class ShoppingCartController : BaseController
    {
        private readonly ShoppingCartService shoppingCartService;

        public ShoppingCartController(IAccountService accountService,IStoreService storeService,ShoppingCartService service)
            :base(accountService,storeService)
        {
            this.shoppingCartService = service;
        }

        public ViewResult Index()
        {
 
            var shoppingViewModel = new ShoppingCartViewModel
            {
                ShoppingCartItems = shoppingCartService.GetShoppingCartItems(),
                ShoppingCartTotal = shoppingCartService.GetShoppingCartTotal()
            };

            return View(shoppingViewModel);
        }

        public RedirectToActionResult AddToShoppingCart(int productModellNumber)
        {
            var selectedProduct = storeService.GetProduct(productModellNumber);
            if(selectedProduct != null)
            {
                shoppingCartService.AddShoppingCartItem(productModellNumber);
            }
            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveFromShoppingCart(int productModellNumber)
        {
            var selectedProduct = storeService.GetProduct(productModellNumber);
            if (selectedProduct != null)
            {
                shoppingCartService.RemoveShoppingCartItem(productModellNumber);
            }
            return RedirectToAction("Index");
        }
    }
}
