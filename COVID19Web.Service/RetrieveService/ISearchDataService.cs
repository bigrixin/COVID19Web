using COVID19Web.Model.ViewModel;
using System.Collections.Generic;

namespace COVID19Web.Service
{
    public interface ISearchDataService
    {
        string CombineConfirmedCaseURL(string postcode);
        string CombineRetrieveSuburbURL(string postcode);
        List<ConfirmedCaseViewModel> GetSearchResultList(string url);
        List<string> GetSuburbByPostcode(string url);
    }
}
