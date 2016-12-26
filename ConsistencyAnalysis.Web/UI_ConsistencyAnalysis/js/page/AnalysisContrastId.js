$(function () {
    InitialDate();
    LoadTreeGrid("first");
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
var m_RecordName = "";
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
                    //QueryTableCount(m_KeyID);
                    //LoadHtml(m_KeyID);

                }
            });
        },
        error: function () {
            $.messager.alert('提示', '操作记录类型加载失败！');
        }
    });
}
function LoadTreeGrid(type, myData) {
    if (type == "first") {
        $('#grid_Main').treegrid({
            frozenColumns: [[
                { field: 'Name', title: '中控记录', width: 160 }
            ]],
            columns: [[
                    
                    //{ field: 'ContrastId', title: '标签名', width: 120 },                  
                    { field: 'MaxVaule', title: '最大值', width: 65, align: "left" },
                    { field: 'MinVaule', title: '最小值', width: 65, align: "left" },
                    { field: 'AvgVaule', title: '均值', width: 65, align: "left" },
                    { field: 'StdevpVaule', title: '标准差', width: 65, align: "left" }
                    //{ field: 'VarVaule', title: '方差', width: 80, align: "left" }
                    //{ field: 'VarVaule', title: '方差', width: 80, align: "right" },
                    //{ field: 'StdVaule', title: '标准差', width: 60, align: "right" }
            ]],
            fit: true,
            toolbar: "#toorBar",
            rownumbers: true,
            singleSelect: true,
            striped: true,
            idField: "id",
            treeField: "Name",
            initialState: "collapsed",
            data: []
        });
    }
    else {
        $('#grid_Main').treegrid('loadData', myData);
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
    if (m_productionprocessId == "") {
        $.messager.alert('警告', '请选择工序');
        return;
    }
    if (m_RecordName == "") {
        $.messager.alert('警告', '请选择操作记录');
        return;
    }
    var win = $.messager.progress({
        title: '请稍后',
        msg: '数据载入中...'
    });
        $.ajax({
            type: "POST",
            url: "AnalysisContrastId.aspx/GetContrastId",
            data: '{mOrganizationID: "' + mOrganizationID + '",beginTime:"' + beginTime + '",endTime:"' + endTime + '",m_productionprocessId:"' + m_productionprocessId + '",m_RecordName:"' + m_RecordName + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                $.messager.progress('close');
                m_MsgData = jQuery.parseJSON(msg.d);
                if (m_MsgData.total == 0) {
                    $('#grid_Main').treegrid('loadData', []);
                    $.messager.alert('提示', '没有相关数据！');
                }
                else {
                    LoadTreeGrid("last", m_MsgData);
                }
            },
            beforeSend: function (XMLHttpRequest) {
                //alert('远程调用开始...');
                win;
            },
            error: function handleError() {
                $.messager.progress('close');
                $('#grid_Main').treegrid('loadData', []);
                $.messager.alert('失败', '获取数据失败');
            }
        });
}
function ExportFileFun() {
    var SelectOrganizationName = $('#comb_ProcessType').combobox('getText');
    var SelectDatetime = $('#startTime').datebox('getValue');
    var m_FunctionName = "ExcelStream";
    var m_Parameter1 = GetTreeTableHtml("grid_Main", "一致性分析报表", "Name", SelectOrganizationName, SelectDatetime);
    var m_Parameter2 = SelectOrganizationName;

    var m_ReplaceAlllt = new RegExp("<", "g");
    var m_ReplaceAllgt = new RegExp(">", "g");
    m_Parameter1 = m_Parameter1.replace(m_ReplaceAlllt, "&lt;");
    m_Parameter1 = m_Parameter1.replace(m_ReplaceAllgt, "&gt;");

    var form = $("<form id = 'ExportFile'>");   //定义一个form表单
    form.attr('style', 'display:none');   //在form表单中添加查询参数
    form.attr('target', '');
    form.attr('method', 'post');
    form.attr('action', "AnalysisContrastId.aspx");

    var input_Method = $('<input>');
    input_Method.attr('type', 'hidden');
    input_Method.attr('name', 'myFunctionName');
    input_Method.attr('value', m_FunctionName);
    var input_Data1 = $('<input>');
    input_Data1.attr('type', 'hidden');
    input_Data1.attr('name', 'myParameter1');
    input_Data1.attr('value', m_Parameter1);
    var input_Data2 = $('<input>');
    input_Data2.attr('type', 'hidden');
    input_Data2.attr('name', 'myParameter2');
    input_Data2.attr('value', m_Parameter2);

    $('body').append(form);  //将表单放置在web中 
    form.append(input_Method);   //将查询参数控件提交到表单上
    form.append(input_Data1);   //将查询参数控件提交到表单上
    form.append(input_Data2);   //将查询参数控件提交到表单上
    form.submit();
    //释放生成的资源
    form.remove();
}
function PrintFileFun() {
    var m_ReportTableHtml = GetTreeTableHtml("grid_Main", "能耗日报", "Name", SelectOrganizationName, SelectDatetime);
    PrintHtml(m_ReportTableHtml);
}

