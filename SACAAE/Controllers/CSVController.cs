using CsvHelper;
using CsvHelper.Configuration;
using SACAAE.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SACAAE.Controllers
{
    public class CSVController : Controller
    {
        // GET: CSV
        public ActionResult CargarCSV()
        {
            return View();
        }

        public FileResult DownloadCSVTemplate()
        {
            String templateFileName = "plantillaAsignacion.csv";
            byte[] fileSize;

            using (Stream memoryStream = new MemoryStream())
            {
                StreamWriter streamWriter = new StreamWriter (memoryStream,Encoding.UTF8);

                var writer = new CsvWriter(streamWriter);

                //DataCSV exampledata1 = new DataCSV("2", "Productividad Empresarial", "Alan Henderson García", "Lunes", "7:30", "11:20", "Cartago", "B1-02", "231.ADMINISTRACION DE EMPRESAS 1998", "Diurna");
                //DataCSV exampledata2 = new DataCSV("", "", "", "Martes", "7:30", "11:20", "Cartago", "B1-02", "", "");

                writer.WriteHeader(typeof(DataCSV));

                //writer.WriteRecord(exampledata1);
                //writer.WriteRecord(exampledata2);

                writer.NextRecord();

                /*
                foreach (DataRecord record in records)
                {
                    //Write entire current record
                    writer.WriteRecord(record);

                    //write record field by field
                    writer.WriteField(record.CommonName);
                    writer.WriteField(record.FormalName);
                    writer.WriteField(record.TelephoneCode);
                    writer.WriteField(record.CountryCode);
                    //ensure you write end of record when you are using WriteField method
                    writer.NextRecord();
                }
                 * */

                streamWriter.Flush();
                memoryStream.Flush();
                memoryStream.Position = 0;
                fileSize = new byte[memoryStream.Length];
                memoryStream.Read(fileSize, 0, fileSize.Length);
                streamWriter.Close();
                streamWriter.Dispose();
            }

            return File(fileSize, System.Net.Mime.MediaTypeNames.Application.Octet, templateFileName);
        }
    }
}