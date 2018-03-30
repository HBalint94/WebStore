﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class StoreService : IStoreService
    {
        private readonly WebStoreContext context;
        

        public StoreService(WebStoreContext context)
        {
            this.context = context;
        }


        public IEnumerable<Category> Categories => context.Categories;

        public IEnumerable<Product> Products => context.Products;

        public IEnumerable<Rent> Rents => context.Rents;

        public IEnumerable<RentProductConnection> RentProductConnection => context.RentProductConnections;

        public IEnumerable<ShoppingCartItem> ShoppingCartItems => context.ShoppingCartItems;

        public int GetRentedProductCountInARent(int prodModellNumber, int rentId)
        {
            if (prodModellNumber == 0 || rentId == 0) return 0;

            RentProductConnection rent= context.RentProductConnections.FirstOrDefault(rentProdConnection => rentProdConnection.ProductModellNumber == prodModellNumber && rentProdConnection.RentId == rentId);

            return rent.CountProduct;
        }

        public IEnumerable<RentProductConnection> GetRentProductionConnectionWithRentId(int rentId)
        {
            if (rentId == 0 || !context.RentProductConnections.Any(prodRentConn => prodRentConn.RentId == rentId)) return null;

            return context.RentProductConnections.Where(prodRentConn => prodRentConn.RentId == rentId);
        }

        public IEnumerable<RentProductConnection> GetRentProductionConnectionWithProdModellNumber(int prodModellNumber)
        {
            if (prodModellNumber == 0|| !context.RentProductConnections.Any(prodRentConn => prodRentConn.ProductModellNumber == prodModellNumber)) return null;

            return context.RentProductConnections.Where(prodRentConn => prodRentConn.ProductModellNumber == prodModellNumber);
        }

        public Product GetProduct(int prodModellNumber)
        {
            if (prodModellNumber == 0|| !context.Products.Any(prod => prod.ModellNumber == prodModellNumber)) return null;

            return context.Products.FirstOrDefault(prod => prod.ModellNumber == prodModellNumber);
        }

        public Category GetCategory(int categoryId)
        {
            if (categoryId == 0 || !context.Categories.Any(category => category.Id == categoryId)) return null;

            return context.Categories.FirstOrDefault(category => category.Id == categoryId);
        }

        public Rent GetRent(int rentId)
        {
            if (rentId ==  0 || !context.Rents.Any(rent => rent.Id == rentId)) return null;

            return context.Rents.FirstOrDefault(rent => rent.Id == rentId);
        }

        public int GetPriceOfRent(int rentId)
        {
            // ha nem is létezik ezzel az id val rendelés akkor 0 t adunk vissza.
            if (rentId == 0 || !context.Rents.Any(rent => rent.Id == rentId)) return 0;
            int price = 0;
            IEnumerable<RentProductConnection> connections = context.RentProductConnections.Where(rent => rent.RentId == rentId);
            foreach(RentProductConnection rent in connections)
            {
                price = GetProduct(rent.ProductModellNumber).Price * rent.CountProduct;
            }
            return price;
        }

        public IEnumerable<Product> GetProductsBasedOnCategoryId(int categoryId)
        {
            if (categoryId == 0) return null;

            return context.Products.Where(product => product.CategoryId == categoryId);
        }

        public void AddShoppingCartItem(ShoppingCartItem shoppingCartItem)
        {
            context.ShoppingCartItems.Add(shoppingCartItem);
        }

        public ShoppingCartItem GetShoppingCartITem(Product product, string shoppingCartId)
        {
           return context.ShoppingCartItems.SingleOrDefault(
                s => s.Product.ModellNumber == product.ModellNumber && s.ShoppingCartId == shoppingCartId);
        }

        void IStoreService.SaveChanges()
        {
            context.SaveChanges();
        }

        public void RemoveShoppingCartItem(ShoppingCartItem shoppingCartItem)
        {
            context.ShoppingCartItems.Remove(shoppingCartItem);
        }

        public ShoppingCartItem GetShoppingCartItem(Product product, string shoppingCartId)
        {
            throw new NotImplementedException();
        }

        public List<ShoppingCartItem> GetShoppingCartItems(string shoppingCartId)
        {
            return context.ShoppingCartItems.Where(s => s.ShoppingCartId == shoppingCartId)
                .Include(s => s.Product)
                .ToList();
        }

        public void ClearCart(string shoppingCartId)
        {
            var cartItems = context.ShoppingCartItems.Where(cart => cart.ShoppingCartId == shoppingCartId);
            context.ShoppingCartItems.RemoveRange(cartItems);

        }

        public int GetShoppingCartTotal(string shoppingCartId)
        {
            int total = context.ShoppingCartItems.Where(c => c.ShoppingCartId == shoppingCartId)
                .Select(c => c.Product.Price * c.Quantity).Sum();

            return total;
        }
    }
}
