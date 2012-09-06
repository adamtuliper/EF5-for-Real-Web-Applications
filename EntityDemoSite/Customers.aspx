<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Customers.aspx.cs" Inherits="EntityDemoSite.Customers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <h2>Customers</h2> 
     Enter any part of the name or leave the box blank to see all names: 
    <asp:TextBox ID="SearchTextBox" runat="server" AutoPostBack="true"></asp:TextBox> 
    &nbsp;<asp:Button ID="SearchButton" runat="server" Text="Search" />

    <%-- Note you can use ViewModels here as well, but will need a layer between the repository to map to/from repository entities to ViewModels --%>
    <asp:ObjectDataSource ID="CustomerObjectDataSource" runat="server"  
        TypeName="EntityDemoSite.DataAccess.Repositories.CustomerRepository" DataObjectTypeName="EntityDemoSite.Domain.Entities.Customer"
        SelectMethod="GetCustomersByName" DeleteMethod="Delete" UpdateMethod="Update" 
        OnUpdated="CustomerObjectDataSource_Updated" SortParameterName="sortExpression"
        OnDeleted="CustomerObjectDataSource_Deleted">
        <SelectParameters> 
            <asp:ControlParameter ControlID="SearchTextBox" Name="nameSearchString" PropertyName="Text" 
                Type="String" /> 
        </SelectParameters> 
    </asp:ObjectDataSource>
    <asp:ValidationSummary ID="CustomersValidationSummary" runat="server"  
        ShowSummary="true" DisplayMode="BulletList" style="color: Red; width: 40em;"  />
    <asp:GridView ID="CustomersGridView" runat="server" AutoGenerateColumns="False" 
        DataSourceID="CustomerObjectDataSource"  
        DataKeyNames="CustomerId,Timestamp"  
        OnRowUpdating="CustomersGridView_RowUpdating" 
        OnRowDataBound="CustomersGridView_RowDataBound" 
        AllowSorting="True" AllowPaging="True" PageSize="20" >
        <Columns> 
            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" 
                ItemStyle-VerticalAlign="Top"> 
            </asp:CommandField> 
            <asp:BoundField DataField="FirstName" HeaderText="FirstName" SortExpression="FirstName" ItemStyle-Width="50px" ItemStyle-VerticalAlign="Top" /> 
            <asp:BoundField DataField="LastName" HeaderText="LastName" SortExpression="LastName" ItemStyle-VerticalAlign="Top" /> 
            <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" ItemStyle-VerticalAlign="Top" /> 
            <asp:BoundField DataField="City" HeaderText="City" SortExpression="City" ItemStyle-VerticalAlign="Top" /> 
            <asp:BoundField DataField="State" HeaderText="State" SortExpression="State" ItemStyle-VerticalAlign="Top" /> 
            <asp:BoundField DataField="Zip" HeaderText="Zip" SortExpression="Zip" ItemStyle-VerticalAlign="Top" /> 
        </Columns> 
    </asp:GridView>


</asp:Content>
