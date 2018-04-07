﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace WebStore.Models
{
    public class ShoppingCart
    {
        private readonly IStoreService service;

        public string ShoppingCartId { get; set; }

        public string ShoppingCartUserName { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public ShoppingCart(IStoreService service, string userName)
        {
            this.ShoppingCartUserName = userName;
            this.service = service;
            ShoppingCartItems = new List<ShoppingCartItem>();
        }
        
        public void AddToCart(Product product,int quantity)
        {
            var shoppingCartItem = service.GetShoppingCartItem(product, ShoppingCartId);

            if(shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Product = product,
                    Quantity = 1
                };

                service.AddShoppingCartItem(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Quantity++;
            }
            service.SaveChanges();
        }

        public int RemoveFromCart(Product product)
        {
            var shoppingCartItem = service.GetShoppingCartItem(product, ShoppingCartId);

            var localQuantity = 0;

            if(shoppingCartItem != null)
            {
                if(shoppingCartItem.Quantity > 1)
                {
                    shoppingCartItem.Quantity--;
                    localQuantity = shoppingCartItem.Quantity;
                }
                else
                {
                    service.RemoveShoppingCartItem(shoppingCartItem);
                }
            }
            service.SaveChanges();

            return localQuantity;
        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ??
                (ShoppingCartItems = service.GetShoppingCartItems(ShoppingCartId));
        }

        public void ClearCart()
        {
            service.ClearCart(ShoppingCartId);
            service.SaveChanges();
        }

        public int GetShoppingCarTotal()
        {
            return service.GetShoppingCartTotal(ShoppingCartId);
        }

    }
}
