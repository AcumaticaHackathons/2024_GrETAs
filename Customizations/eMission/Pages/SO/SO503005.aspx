<%@ Page Language="C#" MasterPageFile="~/MasterPages/ListView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="SO503005.aspx.cs" Inherits="Page_SO503005" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/ListView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="eMission.Graph.GRTConsolidateShipment"
        PrimaryView="OpenShipments"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phL" runat="Server">
	<px:PXGrid AutoAdjustColumns="" ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="200" SkinID="Primary" AllowAutoHide="false">
		<Levels>
			<px:PXGridLevel DataMember="OpenShipments">
			    <Columns>
				<px:PXGridColumn TextAlign="Center" AllowCheckAll="True" DataField="Selected" Width="60" Type="CheckBox" ></px:PXGridColumn>
				<px:PXGridColumn DataField="ShipmentType" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn LinkCommand="FakeCommand" DataField="ShipmentNbr" Width="140" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Status" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="ShipDate" Width="90" ></px:PXGridColumn>
				<px:PXGridColumn DataField="SiteID" Width="140" ></px:PXGridColumn>
				<px:PXGridColumn DataField="CustomerID" Width="140" ></px:PXGridColumn>
				<px:PXGridColumn DataField="CustomerLocationID" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="ShipmentQty" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="ShipmentWeight" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="UsrClimateIqLandResult" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="UsrClimateIqAirResult" Width="100" ></px:PXGridColumn></Columns>
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="" MinHeight="150" ></AutoSize>
		<ActionBar >
		</ActionBar>
	</px:PXGrid>
	<px:PXGrid Width="100%" AutoAdjustColumns="True" Caption="Consolidated Shipments" CaptionVisible="True" runat="server" ID="CstPXGrid1" DataSourceID="ds" Height="200" SkinID="Primary" AllowAutoHide="false">
		<Levels>
		<px:PXGridLevel DataMember="ConsolidatedShipments">
            <Columns>
            <px:PXGridColumn TextAlign="Center" AllowCheckAll="True" DataField="Selected" Width="60" Type="CheckBox" ></px:PXGridColumn>
            <px:PXGridColumn DataField="ShipmentType" Width="70" ></px:PXGridColumn>
            <px:PXGridColumn LinkCommand="FakeCommand" DataField="ShipmentNbr" Width="140" ></px:PXGridColumn>
            <px:PXGridColumn DataField="Status" Width="70" ></px:PXGridColumn>
            <px:PXGridColumn DataField="ShipDate" Width="90" ></px:PXGridColumn>
            <px:PXGridColumn DisplayMode="Text" DataField="SiteID" Width="140" ></px:PXGridColumn>
            <px:PXGridColumn DisplayMode="Text" DataField="CustomerID" Width="140" ></px:PXGridColumn>
            <px:PXGridColumn DataField="CustomerLocationID" Width="70" ></px:PXGridColumn>
            <px:PXGridColumn DataField="ShipmentQty" Width="100" ></px:PXGridColumn>
            <px:PXGridColumn DataField="ShipmentWeight" Width="100" ></px:PXGridColumn>
	<px:PXGridColumn DataField="UsrClimateIqLandResultBeforeConsolidation" Width="100" />
            <px:PXGridColumn DisplayMode="Text" DataField="UsrClimateIqLandResult" Width="100" ></px:PXGridColumn>
	<px:PXGridColumn DataField="UsrClimateIqAirResultBeforeConsolidation" Width="100" />
            <px:PXGridColumn DataField="UsrClimateIqAirResult" Width="100" ></px:PXGridColumn></Columns>
        </px:PXGridLevel></Levels>
		<AutoSize Container="Window" Enabled="" MinHeight="150" ></AutoSize>
		</px:PXGrid></asp:Content>
