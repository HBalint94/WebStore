using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class AccountService : IAccountService
    {
        private readonly WebStoreContext context;
        private readonly HttpContext httpContext;
        private readonly ApplicationState applicationState;

        public AccountService(WebStoreContext context, IHttpContextAccessor httpContextAccessor, ApplicationState applicationState)
        {
            this.context = context;
            this.httpContext = httpContextAccessor.HttpContext;
            this.applicationState = applicationState;
        }

        public int UserCount
        {
            get => (int)applicationState.UserCount;
            set => applicationState.UserCount = value;
        }


        public string CurrentUserName => httpContext.Session.GetString("user");

        public ShoppingCart CurrentShoppingCart
        {
            get =>SessionExtensions.Get<ShoppingCart>(httpContext.Session,"shoppingCart");
        }
        public bool Create(CustomerViewModel customer, out string userName)
        {
            userName = "user" + Guid.NewGuid(); // a felhasználónevet generáljuk

            if (customer == null)
                return false;

            // ellenőrizzük az annotációkat
            if (!Validator.TryValidateObject(customer, new ValidationContext(customer, null, null), null))
                return false;

            // elmentjük a felhasználó adatait
            context.Customers.Add(new Customer
            {
                Name = customer.CustomerName,
                Address = customer.CustomerAddress,
                Email = customer.CustomerEmail,
                PhoneNumber = customer.CustomerPhoneNumber,
                UserName = userName
            });

            try
            {
                context.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public Customer GetCustomer(string userName)
        {
            if(userName == null)
            {
                return null;
            }
            return context.Customers.FirstOrDefault(c => c.UserName == userName); // megkeressük a vásárlót
        }

        public bool Login(LoginViewModel user)
        {
            if (user == null)
                return false;

            // ellenőrizzük az annotációkat
            if (!Validator.TryValidateObject(user, new ValidationContext(user, null, null), null))
                return false;

            Customer customer = context.Customers.FirstOrDefault(c => c.UserName == user.UserName); // megkeressük a felhasználót

            if (customer == null)
                return false;

            // ellenőrizzük a jelszót (ehhez a kapott jelszót hash-eljük)
            Byte[] passwordBytes = null;
            using (SHA512CryptoServiceProvider provider = new SHA512CryptoServiceProvider())
            {
                passwordBytes = provider.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
            }

            if (!passwordBytes.SequenceEqual(customer.Password))
                return false;

            // ha sikeres volt az ellenőrzés
            httpContext.Session.SetString("user", user.UserName); // felvesszük a felhasználó nevét a munkamenetbe
            ISession session = httpContext.Session;
            SessionExtensions.Set<ShoppingCart>(session,"shoppingCart", customer.ShoppingCart);// felvesszük a felhasználó kosarát a munkamenetbe

            UserCount++;
            return true;
        }

        public bool Logout()
        {
            if (!httpContext.Session.Keys.Contains("user"))
                return false;

            // töröljük a munkafolyamatból
            httpContext.Session.Remove("user");

            // módosítjuk a felhasználók számát
            UserCount--;

            return true;
        }

        public bool Register(RegistrationViewModel customer)
        {
            if (customer == null)
                return false;

            // ellenőrizzük az annotációkat
            if (!Validator.TryValidateObject(customer, new ValidationContext(customer, null, null), null))
                return false;

            if (context.Customers.Count(c => c.UserName == customer.UserName) != 0)
                return false;

            // kódoljuk a jelszót
            Byte[] passwordBytes = null;
            using (SHA512CryptoServiceProvider provider = new SHA512CryptoServiceProvider())
            {
                passwordBytes = provider.ComputeHash(Encoding.UTF8.GetBytes(customer.Password));
            }

            // elmentjük a felhasználó adatait
            context.Customers.Add(new Customer
            {
                Name = customer.CustomerName,
                Address = customer.CustomerAddress,
                Email = customer.CustomerEmail,
                PhoneNumber = customer.CustomerPhoneNumber,
                UserName = customer.UserName,
                Password = passwordBytes,
            });

            try
            {
                context.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
