using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Aspose.Pdf;

namespace Aspose.SiteFinity.PDFImport
{
    public partial class AsposePDFImport : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }

        protected void ImportButton_Click(object sender, EventArgs e)
        {
            if (ImportFileUpload.HasFile)
            {
                // Check for license and apply if exists
                string licenseFile = Server.MapPath("~/App_Data/Aspose.Total.lic");
                if (File.Exists(licenseFile))
                {
                    License license = new License();
                    license.SetLicense(licenseFile);
                }

                // Initialize the stream to read the uploaded file.
                Stream myStream = ImportFileUpload.FileContent;
                //open document
                Document pdfDocument = new Document(myStream);
                
                // Instantiate HTML Save options object
                HtmlSaveOptions newOptions = new HtmlSaveOptions();

                // Enable option to embed all resources inside the HTML
                newOptions.PartsEmbeddingMode = HtmlSaveOptions.PartsEmbeddingModes.EmbedAllIntoHtml;

                // This is just optimization for IE and can be omitted 
                newOptions.LettersPositioningMethod = HtmlSaveOptions.LettersPositioningMethods.UseEmUnitsAndCompensationOfRoundingErrorsInCss;
                newOptions.RasterImagesSavingMode = HtmlSaveOptions.RasterImagesSavingModes.AsEmbeddedPartsOfPngPageBackground;
                newOptions.FontSavingMode = HtmlSaveOptions.FontSavingModes.SaveInAllFormats;

                // Output file path 
                string filePath = Server.MapPath("~/temp/");
                if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

                filePath += "\\" + System.Guid.NewGuid().ToString();
                string path = filePath + "-" + ImportFileUpload.FileName.Replace(".pdf", ".html");
                pdfDocument.Save(path, newOptions);
                string extractedText = File.ReadAllText(path);
                OutputLiteral.Text = extractedText;
            }
            else
            {
                OutputLiteral.Text = "Please Upload File";
            }
        }
    }
}