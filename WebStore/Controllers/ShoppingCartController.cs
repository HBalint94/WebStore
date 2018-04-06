using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ShoppingCart shoppingCart;
        private readonly IStoreService storeService;
        

        public ShoppingCartController(ShoppingCart shoppingCart, IStoreService storeService)          
        {
            this.storeService = storeService;
            this.shoppingCart = shoppingCart;
            
        }

        public ViewResult Index()
        {
            var items = shoppingCart.GetShoppingCartItems();
            shoppingCart.ShoppingCartItems = items;

            var shoppingViewModel = new ShoppingCartViewModel
            {
                ShoppingCart = shoppingCart,
                ShoppingCartTotal = shoppingCart.GetShoppingCarTotal()
            };

            return View(shoppingViewModel);
        }

        public RedirectToActionResult AddToShoppingCart(int ModellNumber)
        {
            var selectedProduct = storeService.GetProduct(ModellNumber);
            if(selectedProduct != null)
            {
                shoppingCart.AddToCart(selectedProduct, 1);
            }
            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveFromShoppingCart(int productModellNumber)
        {
            var selectedProduct = storeService.GetProduct(productModellNumber);
            if (selectedProduct != null)
            {
                shoppingCart.RemoveFromCart(selectedProduct);
            }

            return RedirectToAction("Index");
        }
    }
}
