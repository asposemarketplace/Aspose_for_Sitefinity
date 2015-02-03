using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Aspose.Words;

namespace Aspose.SiteFinity.WordImport
{
    public partial class AsposeWordImport : System.Web.UI.UserControl
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

                Stream stream = ImportFileUpload.FileContent;
                Document doc = new Document(stream);

                string filePath = Server.MapPath("~/temp/");
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                filePath += "\\" + System.Guid.NewGuid().ToString();

                doc.Save(filePath, SaveFormat.Html);
                string outputText = File.ReadAllText(filePath);

                OutputLiteral.Text = outputText;
            }
        }
    }
}