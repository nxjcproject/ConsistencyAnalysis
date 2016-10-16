$(function () {
    InitialDate();
    LoadDataGrid("first");
});
function InitialDate() {
    var nowDate = new Date();
    var beforeDate = new Date();
    beforeDate.setDate(nowDate.getDate());
    var nowString = nowDate.getFullYear() + '-' + (nowDate.getMonth() + 1) + '-' + nowDate.getDate() + " " + nowDate.getHours() + ":" + nowDate.getMinutes() + ":" + nowDate.getSeconds();
    var beforeString = beforeDate.getFullYear() + '-' + (beforeDate.getMonth() + 1) + '-' + beforeDate.getDate() + " 00:00:00";
    $('#startTime').datetimebox('setValue', beforeString);
    $('#endTime').datetimebox('setValue', nowString);
}
function onOrganisationTreeClick(node) {
    $('#organizationName').textbox('setText', node.text);
    $('#organizationId').val(node.OrganizationId);
    mOrganizationId = node.OrganizationId;
    //LoadStaffInfo(mOrganizationId);
    PrcessTypeItem(mOrganizationId);
}
var m_productionprocessId = '';
function PrcessTypeItem(mOrganizationId) {
    $.ajax({
        type: "POST",
        url: "AnalysisContrastId.aspx/GetPrcessTypeItem",
        data: "{myOrganizationId:'" + mOrganizationId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_MsgData = jQuery.parseJSON(msg.d);
            if (m_MsgData.total == 0) {
                $.messager.alert('提示', '未查询到工序', 'info');
            }
            //InitializeEnergyConsumptionGrid(m_GridCommonName, m_MsgData);
            $('#comb_ProcessType').combobox({
                data: m_MsgData.rows,
                valueField: 'id',
                textField: 'text',
                onSelect: function (param) {
                    m_productionprocessId = param.value;
                    RecordNameItem(mOrganizationId, m_productionprocessId);
                }
            });
        },
        error: function () {
            $.messager.alert('提示', '工序加载失败！');
        }
    });
}
function RecordNameItem(OrganizationId, ProductionprocessId) {
    var mOrganizationId = OrganizationId;
    $.ajax({
        type: "POST",
        url: "AnalysisContrastId.aspx/GetRecordNameItem",
        data: "{myOrganizationId:'" + OrganizationId + "',ProductionprocessId:'" + ProductionprocessId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_MsgData = jQuery.parseJSON(msg.d);
            if (m_MsgData.total == 0) {
                $.messager.alert('提示', '未查询到该类别下的产线！')
            }
            $('#comb_LineType').combobox({
                data: m_MsgData.rows,
                valueField: 'id',
                textField: 'text',
                onSelect: function (param) {
                    m_KeyID = param.KeyID;
                    m_DatabaseID = param.DatabaseID;
                    m_RecordName = param.text;
                    QueryTableCount(m_KeyID);
                    LoadHtml(m_KeyID);

                }
            });
        },
        error: function () {
            $.messager.alert('提示', '操作记录类型加载失败！');
        }
    });
}
function LoadDataGrid(type, myData) {
    if (type == "first") {
        $('#grid_Main').datagrid({
            columns: [[
                    { field: 'VariableDescription', title: '中控记录', width: 200 },
                    //{ field: 'ContrastId', title: '标签名', width: 120 },                  
                    { field: 'MaxVaule', title: '最大值', width: 80, align: "left" },
                    { field: 'MinVaule', title: '最小值', width: 80, align: "left" },
                    { field: 'AvgVaule', title: '均值', width: 80, align: "left" }
                    //{ field: 'VarVaule', title: '方差', width: 80, align: "right" },
                    //{ field: 'StdVaule', title: '标准差', width: 60, align: "right" }
            ]],
            fit: true,
            toolbar: "#toorBar",
            rownumbers: true,
            singleSelect: true,
            striped: true,
            data: []
        });
    }
    else {
        $('#grid_Main').datagrid('loadData', myData);
    }
}
function Query() {
    var mOrganizationID = $('#organizationId').val();
    var beginTime = $('#startTime').datebox('getValue');
    var endTime = $('#endTime').datebox('getValue');
    if (mOrganizationID == "") {
        $.messager.alert('警告', '请选择组织机构');
        return;
    }
        $.ajax({
            type: "POST",
            url: "AnalysisContrastId.aspx/GetContrastId",
            data: '{mOrganizationID: "' + mOrganizationID + '",beginTime:"' + beginTime + '",endTime:"' + endTime + '",m_productionprocessId:"' + m_productionprocessId + '",m_RecordName:"' + m_RecordName + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            //beforeSend:function(){
            //    $.messager.alert('提示','数据加载中...');
            //},
            success: function (msg) {
                m_MsgData = jQuery.parseJSON(msg.d);
                if (m_MsgData.total == 0) {
                    $('#grid_Main').datagrid('loadData', []);
                    $.messager.alert('提示', '没有相关数据！');
                }
                else {
                    LoadDataGrid("last", m_MsgData);
                }
            },
            error: function handleError() {
                $('#grid_Main').datagrid('loadData', []);
                $.messager.alert('失败', '获取数据失败');
            }
        });
}