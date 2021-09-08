using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

namespace BitventureAssessment
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string _filePath = string.Empty;
            JsonProcessor jsonProcessor = new JsonProcessor();


            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "JSON files (*.json)|*.json";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    jsonProcessor.Process(openFileDialog.FileName);
                }
            }

            Console.ReadLine();
        }
    }
}
