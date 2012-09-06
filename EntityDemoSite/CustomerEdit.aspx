<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerEdit.aspx.cs" Inherits="EntityDemoSite.CustomerEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <legend>Customer</legend>
        <div class="editor-label">
            First Name
        </div>
        <div class="editor-field">
            <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox> 
        </div>

</fieldset>
          <%--  <asp:BoundField DataField="FirstName" HeaderText="First Name" ItemStyle-VerticalAlign="Top" /> 
            <asp:BoundField DataField="LastName" HeaderText="Las tName" ItemStyle-VerticalAlign="Top" /> 
            <asp:BoundField DataField="Address" HeaderText="Address" ItemStyle-VerticalAlign="Top" /> 
            <asp:BoundField DataField="City" HeaderText="City" ItemStyle-VerticalAlign="Top" /> 
            <asp:BoundField DataField="State" HeaderText="State" ItemStyle-VerticalAlign="Top" /> 
            <asp:BoundField DataField="Zip" HeaderText="Zip" ItemStyle-VerticalAlign="Top" /> --%>

</asp:Content>
