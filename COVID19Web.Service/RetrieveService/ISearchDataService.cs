using COVID19Web.Model.ViewModel;
using COVID19Web.Model.ViewModels;
using HtmlAgilityPack;
using System.Collections.Generic;

namespace COVID19Web.Service
{
    public interface ISearchDataService
    {
        string CombineConfirmedCasesDailyCountURL(string postcode);
        string CombineConfirmedCasesDetailsURL(string postcode);
        string CombineRetrieveSuburbURL(string postcode);
        string GetNSWConfirmedCasesCount();
       // string GetNSWCaseStatistics();
       //  List<NSWCaseStatisticsViewModel> GetNSWCaseStatistics();
        AustraliaAndWorldCaseStatisticsViewModel GetAuAndWorldCaseStatisticsFromWHO();
        List<ConfirmedCasesDailyCountViewModel> GetCasesDailyCountList(string url);
        List<ConfirmedCasesDetailsViewModel> GetCasesDetialsList(string url);
        List<string> GetSuburbByPostcode(string url);
    }
}
