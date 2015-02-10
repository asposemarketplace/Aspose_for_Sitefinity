<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AsposePDFImport.ascx.cs" Inherits="Aspose.SiteFinity.PDFImport.AsposePDFImport" %>
<div id="div_main" style="margin: 0 auto; width: 800px;">
    <br />
    <asp:Literal ID="OutputLiteral" runat="server"></asp:Literal>
    <br />
    <b>Please select a PDF file and then click on Import from PDF button below ...</b>
    <br />
    <br />
    <asp:FileUpload ID="ImportFileUpload" runat="server" />
    <br />
    <br />
    <asp:Button ID="ImportButton" runat="server" Text="Import from PDF" OnClick="ImportButton_Click" />
    <br />
</div>
