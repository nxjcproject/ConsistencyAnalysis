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