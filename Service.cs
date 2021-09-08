using log4net;
using Newtonsoft.Json;
using System;




namespace BitventureAssessment
{
    public partial class Service
    {
        // logging to the file
        static ILog s_log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [JsonProperty("baseURL")]
        public Uri BaseUrl { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("endpoints")]
        public Endpoint[] Endpoints { get; set; }

        
        //
        // Summary:
        //     Returns BaseUrl that ends with a '/' for the http client 
        //
        // Parameters:
        //   parameter:
        //     The BaseUrl from the current object is used.
        //
        // Returns:
        //     The sanitised Uri for this object.

        public Uri SanitiseBaseUrl()
        {
            Uri response;

            try
            {
                if (!BaseUrl.ToString().EndsWith("/"))
                {
                    response = new Uri(BaseUrl.ToString() + "/");
                }
                else
                {
                    response = BaseUrl; 
                }
            }
            catch (Exception ex)
            {
                // Logs the data
                s_log.Error(ex.Message);

                response = null; 
            }

            return response;
        }

    }
}
