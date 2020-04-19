using COVID19Web.Model.ViewModel;
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
            RetrieveDataViewModel dataVM= new RetrieveDataViewModel();
            dataVM.NSWCaseStatisticsVM=_searchDataService.GetNSWCaseStatistics();
            return View(dataVM);
        }

        [HttpPost]
        public ActionResult Index(RetrieveDataViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string webAddress = _searchDataService.CombineConfirmedCasesDetailsURL(model.Postcode);
            model.DetailsResult = _searchDataService.GetCasesDetialsList(webAddress);

            webAddress = _searchDataService.CombineRetrieveSuburbURL(model.Postcode);
            List<string> suburbs = _searchDataService.GetSuburbByPostcode(webAddress);
            model.Suburbs = suburbs;
            model.NSWCount = _searchDataService.GetNSWConfirmedCasesCount();

            webAddress = _searchDataService.CombineConfirmedCasesDailyCountURL(model.Postcode);
            model.DailyCountResult = _searchDataService.GetCasesDailyCountList(webAddress);

            return View(model);
        }
 
        public ActionResult Details(string postcode)
        {
            RetrieveDataViewModel model = new RetrieveDataViewModel();
            model.Postcode = postcode;
            string webAddress = _searchDataService.CombineConfirmedCasesDetailsURL(postcode);
            model.DetailsResult = _searchDataService.GetCasesDetialsList(webAddress);

            webAddress = _searchDataService.CombineRetrieveSuburbURL(postcode);
            List<string> suburbs = _searchDataService.GetSuburbByPostcode(webAddress);
            model.Suburbs = suburbs;
            model.NSWCount = _searchDataService.GetNSWConfirmedCasesCount();
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