﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public interface IStoreService
    {
        IEnumerable<Category> Categories { get; }

        IEnumerable<Product> Products { get; }

        IEnumerable<Rent> Rents { get; }

        IEnumerable<RentProductConnection> RentProductConnection { get; }

        IEnumerable<ShoppingCartItem> ShoppingCartItems { get; }
        // megadja, hogy egy adott rendeléshez egy adott modellből mennyi tartozik
        int GetRentedProductCountInARent(int prodModellNumber, int rentId);

        //kilistázza, hogy egy adott rendeléshez mely productok tartoznak és azokból mennyi ( rendelés azonosító alapján)
        IEnumerable<RentProductConnection> GetRentProductionConnectionWithRentId(int rentId);

        IEnumerable<RentProductConnection> GetRentProductionConnectionWithProdModellNumber(int prodModellNumber);

        Product GetProduct(int prodModellNumber);

        Category GetCategory(int categoryId);

        Rent GetRent(int rentId);

        int GetPriceOfRent(int rentId);

        IEnumerable<Product> GetProductsBasedOnCategoryId(int categoryId);

        void AddShoppingCartItem(ShoppingCartItem shoppingCartItem);

        void RemoveShoppingCartItem(ShoppingCartItem shoppingCartItem);

        ShoppingCartItem GetShoppingCartItem(Product product, string shoppingCartId);

        List<ShoppingCartItem> GetShoppingCartItems(string shoppingCartId);

        void ClearCart(string shoppingCartId);

        int GetShoppingCartTotal(string shoppingCartId);

        void SaveChanges();
       
    }
}