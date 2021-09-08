using log4net;
using Newtonsoft.Json;
using System;




namespace BitventureAssessment
{
    public partial class Endpoint
    {
        // logging to the file
        static ILog s_log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("response")]
        public Response[] Response { get; set; }


        //
        // Summary:
        //     Returns Resource that does not starts with a '/' for the http client 
        //
        // Parameters:
        //   parameter:
        //     The resource from the current object is used.
        //
        // Returns:
        //     The string to bind this object.

        public string SanitiseResource()
        {
            string response = string.Empty;

            try
            {
                if (Resource.StartsWith("/"))
                {
                    response = Resource.Remove(0,1);
                }
                else
                {
                    response = Resource;
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
