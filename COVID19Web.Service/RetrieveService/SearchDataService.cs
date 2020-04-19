using COVID19Web.Model.ViewModel;
using COVID19Web.Model.ViewModels;
using HtmlAgilityPack;
using mshtml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml;

namespace COVID19Web.Service
{
    public class SearchDataService : ISearchDataService
    {
        public string CombineConfirmedCasesDailyCountURL(string postcode)
        {
            string part1 = ConfigurationManager.AppSettings["ConfirmedCasesByPostcode1"];
            string part2 = ConfigurationManager.AppSettings["ConfirmedCasesByPostcode2"];
            return part1 + " '" + postcode + "' " + part2;
        }

        public string CombineConfirmedCasesDetailsURL(string postcode)
        {
            string url = ConfigurationManager.AppSettings["ConfirmedCaseAPIEndpoint"] + "&q=";
            return url + postcode;
        }

        public string CombineRetrieveSuburbURL(string postcode)
        {
            return ConfigurationManager.AppSettings["PostcodeAPIEndpoint"] + postcode + "/api.xml";// ".json";
        }

        public List<NSWCaseStatisticsViewModel> GetNSWCaseStatistics()
        {
            string url = ConfigurationManager.AppSettings["NSWCaseStatistics"];
            string htmlString = WebRequestGetHtmlString(url);
            string caseTitleClassName = "moh-rteTableEvenCol-6";
            string caseCountClassName = "moh-rteTableOddCol-6";
            List<NSWCaseStatisticsViewModel> listVM = new List<NSWCaseStatisticsViewModel>();


            HtmlDocument resultat = new HtmlDocument();
            resultat.LoadHtml(htmlString);

            List<HtmlNode> caseTitleList = resultat.DocumentNode.Descendants().Where
(x => (x.Name == "td" && x.Attributes["class"] != null &&
x.Attributes["class"].Value.Contains(caseTitleClassName))).ToList();

            List<HtmlNode> caseCountList = resultat.DocumentNode.Descendants().Where
(x => (x.Name == "td" && x.Attributes["class"] != null &&
   x.Attributes["class"].Value.Contains(caseCountClassName))).ToList();

            for (int i = 0; i < caseTitleList.Count - 1; i++)
            {
                listVM.Add(new NSWCaseStatisticsViewModel { Title = caseTitleList[i].InnerText, Count = caseCountList[i].InnerText });
            }
            return listVM;
        }

        public List<ConfirmedCasesDailyCountViewModel> GetCasesDailyCountList(string url)
        {
            List<ConfirmedCasesDailyCountViewModel> listVM = new List<ConfirmedCasesDailyCountViewModel>();

            string htmlString = WebRequestGetHtmlString(url);

            JObject jObject = JObject.Parse(htmlString);
            var records = jObject["result"]["records"];

            foreach (var item in records)
            {
                ConfirmedCasesDailyCountViewModel cVM = new ConfirmedCasesDailyCountViewModel();
                cVM = JsonConvert.DeserializeObject<ConfirmedCasesDailyCountViewModel>(item.ToString());
                listVM.Add(cVM);
            }
            return listVM;
        }

        public List<ConfirmedCasesDetailsViewModel> GetCasesDetialsList(string url)
        {
            List<ConfirmedCasesDetailsViewModel> listVM = new List<ConfirmedCasesDetailsViewModel>();
            string htmlString = WebRequestGetHtmlString(url);

            JObject jObject = JObject.Parse(htmlString);
            var records = jObject["result"]["records"];

            foreach (var item in records)
            {
                ConfirmedCasesDetailsViewModel cVM = new ConfirmedCasesDetailsViewModel();
                cVM = JsonConvert.DeserializeObject<ConfirmedCasesDetailsViewModel>(item.ToString());
                listVM.Add(cVM);
            }
            return listVM;
        }

        public List<string> GetSuburbByPostcode(string url)
        {
            List<string> data = WebRequestGetXMLString(url);

            return data;
        }

        public string GetNSWConfirmedCasesCount()
        {
            string url = ConfigurationManager.AppSettings["NSWConfirmedCasesCount"];

            NSWConfirmedCasesViewModel nswVM = new NSWConfirmedCasesViewModel();
            string htmlString = WebRequestGetHtmlString(url);

            JObject jObject = JObject.Parse(htmlString);
            var records = jObject["result"]["records"];
            foreach (var item in records)
            {
                nswVM = JsonConvert.DeserializeObject<NSWConfirmedCasesViewModel>(item.ToString());
            }
            return nswVM.Count;
        }

        #region help

        // get a XML page content by url
        private List<string> WebRequestGetXMLString(string url)
        {
            List<string> Suburbs = new List<string>();
            string xmlNodeName = "place";

            XmlTextReader reader = new XmlTextReader(url);
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    while (reader.MoveToNextAttribute())   // Read the attributes.
                    {
                        if (reader.Name == xmlNodeName)
                            Suburbs.Add(reader.Value);
                    }
                }

            }
            return Suburbs;

        }

        // get a webpage content by url
        private string WebRequestGetHtmlString(string url)
        {
            // Creates an HttpWebRequest with the specified URL. 
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            // Sends the HttpWebRequest and waits for the response.	
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string htmlString = "";

            if (response.StatusCode == HttpStatusCode.OK)
            {
                // Gets the stream associated with the response.
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                htmlString = readStream.ReadToEnd();
                // Debug.WriteLine(data);

                response.Close();
                readStream.Close();
                // SaveSearchPageToFile(webpageString);
            }
            return htmlString;
        }



        // save search webpage to a file
        private void SaveSearchPageToFile(string data)
        {
            DateTime dt = DateTime.Now;
            // Date fomat as file name  
            string fileName = string.Format("GoogleSearch-{0:yyyyMMddhhmmss}.html", DateTime.Now);
            // Set a variable to the Documents path.
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Write the string to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, fileName)))
            {
                outputFile.WriteLine(data);
            }
        }


        #endregion
    }
}