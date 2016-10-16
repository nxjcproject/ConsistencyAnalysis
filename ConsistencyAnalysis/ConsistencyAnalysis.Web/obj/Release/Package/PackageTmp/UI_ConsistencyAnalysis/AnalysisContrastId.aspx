<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnalysisContrastId.aspx.cs" Inherits="ConsistencyAnalysis.Web.UI_AnalysisContrastId.AnalysisContrastId" %>

<%@ Register Src="~/UI_WebUserControls/OrganizationSelector/OrganisationTree.ascx" TagPrefix="uc1" TagName="OrganisationTree" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>一致性分析</title>
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/gray/easyui.css"/>
	<link rel="stylesheet" type="text/css" href="/lib/ealib/themes/icon.css"/>
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtIcon.css"/>
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtCss.css"/>

	<script type="text/javascript" src="/lib/ealib/jquery.min.js" charset="utf-8"></script>
	<script type="text/javascript" src="/lib/ealib/jquery.easyui.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/easyui-lang-zh_CN.js" charset="utf-8"></script>

    <script type="text/javascript" src="/lib/ealib/extend/jquery.PrintArea.js" charset="utf-8"></script> 
    <script type="text/javascript" src="/lib/ealib/extend/jquery.jqprint.js" charset="utf-8"></script>
    <!--[if lt IE 8 ]><script type="text/javascript" src="/js/common/json2.min.js"></script><![endif]-->
    <script type="text/javascript" src="/js/common/PrintFile.js" charset="utf-8"></script> 

    <script type="text/javascript" src="js/page/AnalysisContrastId.js" charset="utf-8"></script>
</head>
<body>
    <div id="cc" class="easyui-layout"data-options="fit:true,border:false" >    
         <div data-options="region:'west',split:true" style="width: 230px;">
            <uc1:OrganisationTree ID="OrganisationTree_ProductionLine" runat="server" />
        </div>
          <div id="toorBar" title="" style="height:40px;padding:10px;">
            <div>
                <table>
                    <tr>
                         <td>组织机构:</td>
                        <td >                               
                            <input id="organizationName" class="easyui-textbox" readonly="readonly"style="width:80px" />  
                            <input id="organizationId" readonly="readonly" style="display: none;" />             
                       </td>
                        <td>工序选择</td>
                        <td style="width: 100px;">
                            <input id="comb_ProcessType" class="easyui-combobox" style="width: 130px;"data-options="panelHeight:'auto'" />
                        </td>                           
                        <td>操作记录</td>
                        <td style="width: 100px;">
                            <input id="comb_LineType" class="easyui-combobox" style="width: 100px;"data-options="panelHeight:'auto'" />
                        </td>               
                        <td>开始时间：</td>
                        <td>
                             <input id="startTime" type="text" class="easyui-datetimebox" style="width:150px;" required="required"/>
                        </td>
                           <td>结束时间：</td>
                         <td>
                             <input id="endTime" type="text" class="easyui-datetimebox" style="width:150px;" required="required"/>
                        </td>              
                         <td>
                            <a id="btn" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="Query()">查询</a>
                        </td>
                    </tr>
                </table>         
            </div>
	    </div> 
         <div data-options="region:'center'" style="padding:5px;background:#eee;">
             <table id="grid_Main"class="easyui-datagrid"></table>
         </div>
    </div>
 	  <%--<div id="showMes"></div>--%>
</body>
</html>
