using ConsistencyAnalysis.Service.AnalysisContrastId;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ConsistencyAnalysis.Web.UI_AnalysisContrastId
{
    public partial class AnalysisContrastId : WebStyleBaseForEnergy.webStyleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.InitComponts();
            if (!IsPostBack)
            {
#if DEBUG
                ////////////////////调试用,自定义的数据授权
                List<string> m_DataValidIdItems = new List<string>() { "zc_nxjc_byc_byf", "zc_nxjc_qtx_tys", "zc_nxjc_klqc_klqf", "zc_nxjc_znc_znf"
                ,"zc_nxjc_ychc_yfcf","zc_nxjc_tsc_tsf","zc_nxjc_qtx_efc","zc_nxjc_ychc_ndf","zc_nxjc_szsc_szsf","zc_nxjc_ychc_lsf"};
                AddDataValidIdGroup("ProductionOrganization", m_DataValidIdItems);
#elif RELEASE
#endif
                this.OrganisationTree_ProductionLine.Organizations = GetDataValidIdGroup("ProductionOrganization");                         //向web用户控件传递数据授权参数
                this.OrganisationTree_ProductionLine.PageName = "AnalysisContrastId.aspx";   //向web用户控件传递当前调用的页面名称
                this.OrganisationTree_ProductionLine.LeveDepth = 5;
                ///以下是接收js脚本中post过来的参数
                string m_FunctionName = Request.Form["myFunctionName"] == null ? "" : Request.Form["myFunctionName"].ToString();             //方法名称,调用后台不同的方法
                string m_Parameter1 = Request.Form["myParameter1"] == null ? "" : Request.Form["myParameter1"].ToString();                   //方法的参数名称1
                string m_Parameter2 = Request.Form["myParameter2"] == null ? "" : Request.Form["myParameter2"].ToString();                   //方法的参数名称2
                if (m_FunctionName == "ExcelStream")
                {
                    //ExportFile("xls", "导出报表1.xls");
                    string m_ExportTable = m_Parameter1.Replace("&lt;", "<");
                    m_ExportTable = m_ExportTable.Replace("&gt;", ">");
                    //m_ExportTable = m_ExportTable.Replace("&nbsp", "  ");
                    AnalysisContrastIdService.ExportExcelFile("xls", m_Parameter2 + "一致性分析报表.xls", m_ExportTable);
                }                
            }
        }
        [WebMethod]
        public static string GetPrcessTypeItem(string myOrganizationId)
        {
            //mUserId
            DataTable table = AnalysisContrastIdService.GetProcessTypeInfo(myOrganizationId);
            string json = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(table);
            return json;
        }
        [WebMethod]
        public static string GetRecordNameItem(string myOrganizationId, string ProductionprocessId)
        {
            DataTable table = AnalysisContrastIdService.GetReportNameInfo(myOrganizationId, ProductionprocessId);
            string json = EasyUIJsonParser.DataGridJsonParser.DataTableToJson(table);
            return json;
        }
        [WebMethod]
        public static string GetContrastId(string mOrganizationID, string beginTime, string endTime, string m_productionprocessId, string m_RecordName)
        {
            DataTable table = AnalysisContrastIdService.GetContrastIdTable(mOrganizationID,beginTime,endTime,m_productionprocessId,m_RecordName);
            string json = EasyUIJsonParser.TreeGridJsonParser.DataTableToJsonByLevelCode(table, "LevelCode");
            return json;
        }
    }     
}