<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerAdd.aspx.cs" Inherits="EntityDemoSite.CustomerAdd" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <h2>
        Add Customer</h2>
            <asp:ValidationSummary ID="CustomersValidationSummary" runat="server"  
        ShowSummary="true" DisplayMode="BulletList" style="color: Red; width: 40em;"  />

        <asp:ObjectDataSource ID="CustomerObjectDataSource" runat="server"  
        TypeName="EntityDemoSite.DataAccess.Repositories.CustomerRepository" DataObjectTypeName="EntityDemoSite.Domain.Entities.Customer"
        SelectMethod="GetCustomersByName"  OnInserted="CustomerObjectDataSource_Inserted" DeleteMethod="Delete" UpdateMethod="Update" InsertMethod="Create"> 
    </asp:ObjectDataSource>
<%--    <asp:EntityDataSource ID="CustomersEntityDataSource" runat="server" ContextTypeName="EntityDemoSite.DataAccess.EntityContext"
        EnableFlattening="False" EntitySetName="Customers" EnableInsert="True" EnableDelete="True">
    </asp:EntityDataSource>
--%>    <asp:DetailsView ID="CustomersDetailsView" runat="server" AutoGenerateRows="False"
        DataSourceID="CustomerObjectDataSource"  DataKeyNames="CustomerId" DefaultMode="Insert">
        <Fields>
            <asp:BoundField DataField="FirstName" HeaderText="First Name" ItemStyle-VerticalAlign="Top" /> 
            <asp:BoundField DataField="LastName" HeaderText="Last Name" ItemStyle-VerticalAlign="Top" /> 
            <asp:BoundField DataField="Address" HeaderText="Address" ItemStyle-VerticalAlign="Top" /> 
            <asp:BoundField DataField="City" HeaderText="City" ItemStyle-VerticalAlign="Top" /> 
            <asp:BoundField DataField="State" HeaderText="State" ItemStyle-VerticalAlign="Top" /> 
            <asp:BoundField DataField="Zip" HeaderText="Zip" ItemStyle-VerticalAlign="Top" /> 
            <asp:CommandField ShowInsertButton="True" />
        </Fields>
    </asp:DetailsView>
</asp:Content>
