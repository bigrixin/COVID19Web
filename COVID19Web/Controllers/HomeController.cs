﻿using COVID19Web.Model.ViewModel;
using COVID19Web.Service;
using System.Collections.Generic;
using System.Web.Mvc;

namespace COVID19Web.Controllers
{
    public class HomeController : Controller 
    {
        #region Fields

        private readonly ISearchDataService _searchDataService;

        #endregion

        public HomeController(ISearchDataService iSearchDataService)
        {
            // Injected from Autofac
            _searchDataService = iSearchDataService;
        }

        public HomeController()
        {

        }

        public ActionResult Index()
        {
            return View(new RetrieveDataViewModel());
        }

        [HttpPost]
        public ActionResult Index(RetrieveDataViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string webAddress = _searchDataService.CombineConfirmedCaseURL(model.Postcode);
            model.Result = _searchDataService.GetSearchResultList(webAddress);

            webAddress = _searchDataService.CombineRetrieveSuburbURL(model.Postcode);
            List<string> suburbs = _searchDataService.GetSuburbByPostcode(webAddress);
            model.Suburbs = suburbs;
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Confirmed COVID-19 cases in NSW";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact me.";

            return View();
        }
    }
}