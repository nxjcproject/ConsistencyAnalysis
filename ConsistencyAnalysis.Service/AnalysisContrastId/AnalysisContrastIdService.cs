using SqlServerDataAdapter;
using ConsistencyAnalysis.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ConsistencyAnalysis.Service.AnalysisContrastId
{
    public class AnalysisContrastIdService
    {
        public static DataTable GetProcessTypeInfo(string organizationId)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
            string mySql = @"select DisplayIndex as id ,ProductionPrcessName as text, ProductionPrcessId as value
                                       from [dbo].[shift_CenterControlRecord] 
                                       where OrganizationID=@organizationId
                                       group by DisplayIndex,ProductionPrcessName,ProductionPrcessId
                                       order by DisplayIndex";
            SqlParameter sqlParameter = new SqlParameter("@organizationId", organizationId);
            DataTable table = dataFactory.Query(mySql, sqlParameter);
            return table;
        }
        public static DataTable GetReportNameInfo(string organizationId, string productionprocessId)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory dataFactory = new SqlServerDataFactory(connectionString);
            string mySql = @"select RecordType as id,RecordName as text,KeyID,DatabaseID 
                                       from [dbo].[shift_CenterControlRecord] 
                                       where ProductionPrcessId=@ProductionPrcessId
                                         and OrganizationId=@organizationId
                                         order by RecordType";
            SqlParameter[] myParameter = { new SqlParameter("@organizationId", organizationId), 
                                                 new SqlParameter("@ProductionPrcessId", productionprocessId) };
            DataTable table = dataFactory.Query(mySql, myParameter);
            return table;
        }
//        public static DataTable GetContrastIdTable(string mOrganizationID, string beginTime, string endTime, string m_productionprocessId, string m_RecordName)
//        {
//            string connectionString = ConnectionStringFactory.NXJCConnectionString;
//            ISqlServerDataFactory factory = new SqlServerDataFactory(connectionString);
//            string mySql = @"SELECT   
//	                       A.[KeyId]
//	                      ,B.[OrganizationID]
//	                      ,B.[DatabaseID]
//	                      ,A.[DCSTableName]
//                          ,A.[ContrastID]     
//                          ,A.[Enabled]
//                          ,C.[VariableDescription]
//                      FROM [NXJC].[dbo].[shift_CenterControlRecordItems] A,[NXJC].[dbo].[shift_CenterControlRecord] B
//                          ,{0}.[dbo].[View_DCSContrast] C
//                      where A.[KeyId]=B.[KeyID]
//                           and C.[VariableName]=A.[ContrastID]
//                           and [Enabled]=1
//	                       and B.[OrganizationID]=@mOrganizationID
//                           and B.[ProductionPrcessId]=@m_productionprocessId
//                           and B.[RecordName]=@m_RecordName
//                      order by [OrganizationID],[DatabaseID],[DCSTableName]";
//            SqlParameter[] para = {
//                                      new SqlParameter("@mOrganizationID", mOrganizationID),
//                                      new SqlParameter("@m_productionprocessId", m_productionprocessId),
//                                      new SqlParameter("@m_RecordName", m_RecordName)
//                                   };
//            DataTable dt = factory.Query(string.Format(mySql, mOrganizationID), para);
//            DataTable result = new DataTable();
//            result.Columns.Add("MaxVaule");
//            result.Columns.Add("MinVaule");
//            result.Columns.Add("AvgVaule");
//            //result.Columns.Add("VarVaule");
//            //result.Columns.Add("StdVaule");
//            result.Columns.Add("ContrastId");
//            result.Columns.Add("VariableDescription");

//            for (int i = 0; i < dt.Rows.Count; i++)
//            {
//                string dataBase = dt.Rows[i]["DataBaseID"].ToString().Trim();
//                string dcsTable = dt.Rows[i]["DcsTableName"].ToString().Trim();
//                string fieldName = dt.Rows[i]["ContrastID"].ToString().Trim();
//                string variableName = dt.Rows[i]["VariableDescription"].ToString().Trim();
//                //                string Sql = @"select max([{2}]) as MaxVaule,min([{2}]) as MinVaule,avg([{2}]) as AvgVaule
//                //                            ,var([{2}])/varp([{2}]) as VarVaule
//                //                            ,stdev([{2}])/stdevp([{2}]) as StdVaule from {0}.[dbo].History_{1}
//                //                               where vDate>@beginTime
//                //                                     and vDate<@endTime
//                //                                     and [{2}]!=0";
//                string Sql = @"select cast (max([{2}]) as decimal(18,2)) as MaxVaule,cast (min([{2}]) as decimal(18,2)) as MinVaule
//                                   ,cast (avg([{2}]) as decimal(18,2)) as AvgVaule
//                                   from {0}.[dbo].History_{1}
//                               where vDate>@beginTime
//                                     and vDate<@endTime";
//                SqlParameter[] paras = {
//                                    new SqlParameter("@beginTime", beginTime),
//                                    new SqlParameter("@endTime", endTime)
//                                         };
//                DataTable table = factory.Query(string.Format(Sql, dataBase, dcsTable, fieldName), paras);
//                table.Columns.Add("ContrastId", typeof(string));
//                table.Rows[0]["ContrastId"] = fieldName;
//                table.Columns.Add("VariableDescription", typeof(string));
//                table.Rows[0]["VariableDescription"] = variableName;
//                //DataTable result = new DataTable();
//                //result.Columns.Add("MaxVaule");
//                //result.Columns.Add("MinVaule");
//                //result.Columns.Add("AvgVaule");
//                //result.Columns.Add("VarVaule");
//                //result.Columns.Add("StdVaule");
//                //result.Columns.Add("ContrastId");
//                DataRow row = table.Rows[0];
//                result.Rows.Add(row.ItemArray);
//            }
//            return result;
//        }

        public static DataTable GetContrastIdTable(string mOrganizationID, string beginTime, string endTime, string m_productionprocessId, string m_RecordName)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory factory = new SqlServerDataFactory(connectionString);
            string mySql = @"SELECT  
                            A.[Name]
                           ,A.[LevelCode] 
	                       ,A.[KeyId]
                           ,B.[OrganizationID]
	                       ,B.[DatabaseID]
                           ,A.[DCSTableName]
                           ,A.[ContrastID]     
                           ,A.[Enabled]
                     FROM [NXJC].[dbo].[shift_CenterControlRecordItems] A,[NXJC].[dbo].[shift_CenterControlRecord] B            
                          where A.[KeyId]=B.[KeyID]
                          and (A.Enabled=1 or A.DataType is NULL)
                          and (A.Name is NOT NULL and Name<> '')
                          and B.[OrganizationID]=@mOrganizationID
                          and B.[ProductionPrcessId]=@m_productionprocessId
                          and B.[RecordName]=@m_RecordName
                      order by A.[DisplayIndex],[OrganizationID],[DatabaseID],[DCSTableName]";
            SqlParameter[] para ={
                                     new SqlParameter("@mOrganizationID", mOrganizationID),
                                     new SqlParameter("@m_productionprocessId", m_productionprocessId),
                                     new SqlParameter("@m_RecordName", m_RecordName)
                                 };
            DataTable dt = factory.Query(mySql, para);
            DataTable result = new DataTable();
            result.Columns.Add("MaxVaule");
            result.Columns.Add("MinVaule");
            result.Columns.Add("AvgVaule");
            result.Columns.Add("StdevpVaule");
            //result.Columns.Add("VarVaule");
            //result.Columns.Add("StdVaule");
            result.Columns.Add("ContrastId");
            result.Columns.Add("Name");
            result.Columns.Add("LevelCode");           
            DataRow[] leafRows = dt.Select("DCSTableName  is  not NULL");
            for (int i = 0; i < leafRows.Length; i++)
            {
                string dataBase = leafRows[i]["DataBaseID"].ToString().Trim();
                string dcsTable = leafRows[i]["DcsTableName"].ToString().Trim();
                string fieldName = leafRows[i]["ContrastID"].ToString().Trim();
                string Name = leafRows[i]["Name"].ToString().Trim();
                string leafCode = leafRows[i]["LevelCode"].ToString().Trim();
                //                string Sql = @"select max([{2}]) as MaxVaule,min([{2}]) as MinVaule,avg([{2}]) as AvgVaule
                //                            ,var([{2}])/varp([{2}]) as VarVaule
                //                            ,stdev([{2}])/stdevp([{2}]) as StdVaule from {0}.[dbo].History_{1}
                //                               where vDate>@beginTime
                //                                     and vDate<@endTime
                //                                     and [{2}]!=0";
                
                string Sql = @"select cast (max([{2}]) as decimal(18,2)) as MaxVaule,cast (min([{2}]) as decimal(18,2)) as MinVaule
                                   ,cast (avg([{2}]) as decimal(18,2)) as AvgVaule
                                   ,cast (stdevp([{2}]) as decimal(18,2)) as StdevpVaule
                                   from {0}.[dbo].History_{1}
                               where vDate>@beginTime
                                     and vDate<@endTime";
                SqlParameter[] paras = {
                                    new SqlParameter("@beginTime", beginTime),
                                    new SqlParameter("@endTime", endTime)
                                         };
                DataTable table = factory.Query(string.Format(Sql, dataBase, dcsTable, fieldName), paras);
                table.Columns.Add("ContrastId", typeof(string));
                table.Rows[0]["ContrastId"] = fieldName;
                table.Columns.Add("Name", typeof(string));
                table.Rows[0]["Name"] = Name;
                table.Columns.Add("LevelCode", typeof(string));
                table.Rows[0]["LevelCode"] = leafCode;
                //DataTable result = new DataTable();
                //result.Columns.Add("MaxVaule");
                //result.Columns.Add("MinVaule");
                //result.Columns.Add("AvgVaule");
                //result.Columns.Add("VarVaule");
                //result.Columns.Add("StdVaule");
                //result.Columns.Add("ContrastId");
                DataRow row = table.Rows[0];
                result.Rows.Add(row.ItemArray);
            }
           
            dt.Columns.Add("MaxVaule");
            dt.Columns.Add("MinVaule");
            dt.Columns.Add("AvgVaule");
            dt.Columns.Add("StdevpVaule");
            dt.Columns.Remove("KeyId");
            dt.Columns.Remove("OrganizationID");
            dt.Columns.Remove("DatabaseID");
            dt.Columns.Remove("DCSTableName");
            dt.Columns.Remove("Enabled");
            dt.Columns["Name"].SetOrdinal(6);
            dt.Columns["LevelCode"].SetOrdinal(6);
            DataRow[] resRows = dt.Select("ContrastId  is NULL");
            for (int j = 0; j < resRows.Length; j++)
            {
                result.Rows.Add(resRows[j].ItemArray);
            }
            result.Columns["LevelCode"].SetOrdinal(0);
             return result;
            
        }
        public static void ExportExcelFile(string myFileType, string myFileName, string myData)
        {
            if (myFileType == "xls")
            {
                UpDownLoadFiles.DownloadFile.ExportExcelFile(myFileName, myData);
            }
        }
    }
}
