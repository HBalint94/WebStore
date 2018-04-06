using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class BaseController : Controller
    {
        // a logikát modell osztályok mögé rejtjük
        protected readonly IAccountService accountService;
        protected readonly IStoreService storeService;

        public BaseController(IAccountService accountService, IStoreService storeService)
        {
            this.accountService = accountService;
            this.storeService = storeService;
        }

        /// <summary>
        /// Egy akció meghívása után végrehajtandó metódus.
        /// </summary>
        /// <param name="context">Az akció kontextus argumentuma.</param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            // a minden oldalról elérhető információkat össze gyűjtjük
            ViewBag.Categories = storeService.Categories.ToArray();
            ViewBag.UserCount = accountService.UserCount;

            if (accountService.CurrentUserName != null)
                ViewBag.CurrentCustomerName = accountService.GetCustomer(accountService.CurrentUserName).Name;
        }
    }
}
