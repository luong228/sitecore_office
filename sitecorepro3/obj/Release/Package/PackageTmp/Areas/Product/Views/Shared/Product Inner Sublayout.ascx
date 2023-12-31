﻿<%@ Control Language="c#" AutoEventWireup="true" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<div id="InnerCenter">
	<div id="Header">
	    <img src="-/media/Default Website/sc_logo.ashx" alt="Sitecore" id="scLogo" />
	</div>
	<div id="Content">
		<div id="LeftContent">
			<sc:placeholder runat="server" key="content" />
			<sc:placeholder runat="server" key="search" />
			<sc:placeholder runat="server" key="product" />
			<sc:placeholder runat="server" key="listproduct" />
		</div>				
	</div>
	<div id="Footer"><hr class="divider"/>&#169; <%= Sitecore.DateUtil.ToServerTime(DateTime.UtcNow).Year.ToString()%> Sitecore</div>
</div>
