<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AsposeWordImport.ascx.cs" Inherits="Aspose.SiteFinity.WordImport.AsposeWordImport" %>


<div id="div_main" style="margin: 0 auto; width: 800px;">
    <br />
    <asp:Literal ID="OutputLiteral" runat="server"></asp:Literal>
    <br />
    <b>Please select a Word Processing file and then click on Import from Word button below ...</b>
    <br />
    <br />
    <asp:FileUpload ID="ImportFileUpload" runat="server" />
    <br />
    <br />
    <asp:Button ID="ImportButton" runat="server" Text="Import from Word" OnClick="ImportButton_Click" />
    <br />
</div>
