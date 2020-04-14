using COVID19Web.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace COVID19Web.Test
{
    [TestClass]
    public class UnitTestService
    {
        [TestMethod]
        public void TestCombineConfirmedCaseURL()
        {
            //test ConfigurationManager.AppSettings, need to add below in <appSettings>
            //<add key="ConfirmedCaseAPIEndpoint" value="whatever"/>
            SearchDataService s = new SearchDataService();
            string postcode = "2000";
            string expected = "&q=2000";
            string result = s.CombineConfirmedCasesDetailsURL(postcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestCombineRetrieveSuburbURL()
        {
            SearchDataService s = new SearchDataService();
            string postcode = "2000";
            string expected = "2000/api.xml";
            string result = s.CombineRetrieveSuburbURL(postcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestGetSuburbByPostcode()
        {
            SearchDataService s = new SearchDataService();
            string url = "http://api.epostcodes.com.au/api/codes/2762/api.xml";
            List<string> expected = new List<string> { "Schofields" };
            List<string> result = s.GetSuburbByPostcode(url);

            CollectionAssert.AreEqual(expected, result);  //test array
        }
    }
}
