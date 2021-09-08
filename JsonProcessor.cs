using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using log4net;

namespace BitventureAssessment
{
    class JsonProcessor: DataProcessor
    {
        // logging to the file
        static ILog s_log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public JsonData jsonData { get; set; }

        //
        // Summary:
        //     Load data from file and populates the and deserilize the objects
        //
        // Parameters:
        //   filepath:
        //     filepath of the data file.
        //
        // Returns:
        //     void.
        //
        public override void LoadData(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                using (StreamReader file = new StreamReader(filePath))
                {
                    try
                    {
                        this.jsonData = JsonConvert.DeserializeObject<JsonData>(file.ReadToEnd());
                    }
                    catch (Exception ex)
                    {
                        s_log.Error(ex.Message);

                        this.jsonData = null;
                    }
                }
            }
            else
            {
                Console.WriteLine("The specified file path is Empty {0}",filePath);
            }
                
        }

        //
        // Summary:
        //     Removes the unwanted character from the begining and the end of BaseUrl and Resource.
        //
        // Parameters:
        //   no input parameters. 
        //
        // Returns:
        //     void.
        //
        public override void SanitizeData()
        {
            try
            {
                if (this.jsonData != null)
                {
                    foreach (Service service in jsonData.services)
                    {
                        service.BaseUrl = service.SanitiseBaseUrl();

                        foreach (Endpoint endpoint in service.Endpoints)
                        {
                            endpoint.Resource = endpoint.SanitiseResource();
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Json Data was not loaded from file");
                }
            }
            catch (Exception ex)
            {
                s_log.Error(ex.Message);
            }
        }

        //
        // Summary:
        //     Sends the http queries to the endpoints and processes the response.
        //
        // Parameters:
        //      Processes data after the SanitizeData function
        // Returns:
        //      void, everything is printed on the console.
        //
        public override async Task ProcessData()
        {
            foreach (Service service in jsonData.services)
            {
                if (service.Enabled)
                {
                    using (var client = new HttpClient())
                    {
                        // Set the client Base address.
                        client.BaseAddress = new Uri(service.BaseUrl.ToString());

                        // Initialization of Response Message.  
                        HttpResponseMessage httpResponse = new HttpResponseMessage();

                        foreach (Endpoint endpoint in service.Endpoints)
                        {
                            if (endpoint.Enabled)
                            {
                                // HTTP GET  
                                httpResponse = await client.GetAsync(endpoint.Resource).ConfigureAwait(false);


                                if (httpResponse.IsSuccessStatusCode)
                                {
                                    //Compare Results
                                    Response.ComapareResults(endpoint,httpResponse);
                                }
                                else
                                {
                                    Console.WriteLine("{0} : Request Returned {1}", service.BaseUrl+endpoint.Resource,httpResponse.StatusCode.ToString());
                                }

                            }
                            else
                            {
                                Console.WriteLine("{0} : Endpoint unabled", endpoint.Resource);
                            }

                        }
                    }
                }
                else
                {
                    Console.WriteLine("{0} : Service unabled",service.BaseUrl);
                }

            }   
        }

    }
}
