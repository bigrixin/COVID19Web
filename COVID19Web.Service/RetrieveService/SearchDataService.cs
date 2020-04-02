using COVID19Web.Model.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;

namespace COVID19Web.Service
{
    public class SearchDataService : ISearchDataService
    {

        public string CombineConfirmedCaseURL(string postcode)
        {
            string url = ConfigurationManager.AppSettings["ConfirmedCaseAPIEndpoint"] + "&q=";
            return url + postcode;
        }

        public string CombineRetrieveSuburbURL(string postcode)
        {
            return ConfigurationManager.AppSettings["PostcodeAPIEndpoint"] + postcode + "/api.xml";// ".json";
        }

        public List<ConfirmedCaseViewModel> GetSearchResultList(string url)
        {
            List<ConfirmedCaseViewModel> listVM = new List<ConfirmedCaseViewModel>();
            string data = WebRequestGetJsonString(url);

            JObject jObject = JObject.Parse(data);
            var records = jObject["result"]["records"];

            foreach (var item in records)
            {
                ConfirmedCaseViewModel cVM = new ConfirmedCaseViewModel();
                cVM = JsonConvert.DeserializeObject<ConfirmedCaseViewModel>(item.ToString());
                listVM.Add(cVM);
            }
            return listVM;
        }

        public List<string> GetSuburbByPostcode(string url)
        {
            List<string> data = WebRequestGetXMLString(url);

            return data;
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


                //switch (reader.NodeType)
                //{
                //    case XmlNodeType.Element: // The node is an element.
                //        Debug.Write("<" + reader.Name);

                //        while (reader.MoveToNextAttribute()) // Read the attributes.
                //        {
                //            if (reader.Name == "place")
                //                Suburbs.Add(reader.Value);
                //            Debug.Write(" " + reader.Name + "='" + reader.Value + "'");
                //        }
                //        Debug.Write(">");
                //        Debug.WriteLine(">");
                //        break;
                //    case XmlNodeType.Text: //Display the text in each element.
                //        Debug.WriteLine(reader.Value);
                //        break;
                //    case XmlNodeType.EndElement: //Display the end of the element.
                //        Debug.Write("</" + reader.Name);
                //        Debug.WriteLine(">");
                //        break;
                //}
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