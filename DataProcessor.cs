using System.Threading.Tasks;





namespace BitventureAssessment
{
    abstract class DataProcessor
    {
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
        public abstract void LoadData(string filePath);

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
        public abstract void SanitizeData();

        //
        // Summary:
        //     Sends the http queries to the endpoints and processes the response.
        //
        // Parameters:
        //      Processes data after the SanitizeData function
        // Returns:
        //      void, everything is printed on the console.
        //
        public abstract Task ProcessData();

        //
        // Summary:
        //     This is a Template Method that calls the LoadData, SanitizeData and ProcessData functions.
        //     This functions is the only function the user calls.
        //
        // Parameters:
        //   filepath:
        //     filepath of the data file.
        //   
        //
        // Returns:
        //      void, everything is printed on the console.
        //
        public void Process(string filePath)
        {
            LoadData(filePath);
            SanitizeData();
            ProcessData().Wait();
        }
    }
}
