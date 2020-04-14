using COVID19Web.Model.ViewModel;
using COVID19Web.Model.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
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
            return part1 +" '"+ postcode+"' " + part2;
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

        public List<ConfirmedCasesDailyCountViewModel> GetCasesDailyCountList(string url)
        {
            List<ConfirmedCasesDailyCountViewModel> listVM = new List<ConfirmedCasesDailyCountViewModel>();

            string data = WebRequestGetJsonString(url);

            JObject jObject = JObject.Parse(data);
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
            string data = WebRequestGetJsonString(url);

            JObject jObject = JObject.Parse(data);
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
            string data = WebRequestGetJsonString(url);

            JObject jObject = JObject.Parse(data);
            var records = jObject["result"]["records"];
            foreach (var item in records)
            {
                nswVM = JsonConvert.DeserializeObject<NSWConfirmedCasesViewModel>(item.ToString());
            }
            return nswVM.Count;
        }

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


        private string WebRequestGetJsonString(string url)
        {
            // Creates an HttpWebRequest with the specified URL. 
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            // Sends the HttpWebRequest and waits for the response.	
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string jsonString = "";

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

                jsonString = readStream.ReadToEnd();
                // Debug.WriteLine(data);

                response.Close();
                readStream.Close();
                // SaveSearchPageToFile(jsonString);
            }
            return jsonString;
        }


        // Save search webpage to a file
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

    }
}