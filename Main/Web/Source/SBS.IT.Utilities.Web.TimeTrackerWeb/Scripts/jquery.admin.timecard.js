var tempuserId, tempisSuperAdmin, tempisAdmin, teamCode = '', teamWorkType = '', ALMprojectType = 'other', productionProjectType = 'production support';
//var htmlClone = $(".projectRow").clone();
var applicationArray = [];
var workTypeArray = [];
var projectHoursArry = [];
var employeeArray = [];
//var selectedProjectId = "";
var selectedApplicationId = "";
var selectedProjectTypeId = "";
var selectedWorkTypeId = "";
var projectArray = [];
var selectedHours = "";
var selectedControl;
var productRowCount = 0;
var projectItemPTOval = 0, workTypePTOval = 0;

var sundayDate;
var mondayDate;
var tuesdayDate;
var wednesdayDate;
var thursdayDate;
var fridayDate;
var saturdayDate;
var isTabKey;
var submitted = false;
var currentRowClass = '';

function initializevalues(userId, isSuperAdmin, isAdmin, tempteamCode, tempTeamWorkType) {
    tempuserId = userId;
    tempisSuperAdmin = isSuperAdmin;
    tempisAdmin = isAdmin;
    teamCode = tempteamCode;
    teamWorkType = tempTeamWorkType;
}


function bindProjectType(selectedApplication, ctrl) {
    $(ctrl).find(".clsSelectProjectType").empty();
    var projectType = GetProjectType(selectedApplication);
    if (projectType != null && projectType != undefined && projectType.length > 0) {

        var projectTypeList = "<option value=''>Select Project Item</option>";
        for (i = 0; i < projectType.length; i++) {

            projectTypeList += "<option value='" + projectType[i].ProjectItemId + "'>" + projectType[i].ProjectItemNameDescription + "</option>";

        };
        $(ctrl).parent().parent().find(".clsSelectProjectType").empty();
        $(ctrl).parent().parent().find(".clsSelectProjectType").append(projectTypeList);
        $(ctrl).parent().parent().find(".clsSelectProjectType").attr("disabled", false);

    }
    else {
        $(ctrl).find(".clsSelectProjectType").empty();
        $(ctrl).find(".clsSelectProjectType").attr("disabled", true);
    }
}

function getTimesheetData(startDate, rowCount) {
    var employeeid = $('#selectEmployee').val();
    if (startDate != null && startDate != '') {
        $.ajax({
            type: "Get",
            url: "getExistingTimeSheet?startDate=" + startDate + "&employeeId=" + employeeid,
            dataType: "json",
            cache: false,
            async: false,
            contentType: "application/json",
            success: function (successData) {
                for (var r = 0; r <= rowCount; r++) {
                    //debugger;
                    if (successData != null && successData != '' && successData.length > 0) {
                        for (var i = 0; i < successData.length; i++) {
                            //debugger;
                            var applicationid = 0, projectItemId = 0, projectId = 0, workTypeId = 0;
                            var workItem;
                            $('.projectRow_' + r).find('.ddValue').each(function (index, item) {
                                //debugger;
                                if ($(item).hasClass("ApplicationId"))
                                    applicationid = $(item).val();
                                if ($(item).hasClass("ProjectItemId"))
                                    projectItemId = $(item).val();
                                if ($(item).hasClass("ProjectId"))
                                    projectId = $(item).val();
                                if ($(item).hasClass("WorkTypeId"))
                                    workTypeId = $(item).val();
                                if ($(item).hasClass("workitem"))
                                    workItem = $(item).val();
                            });

                            var tempApplicationid = 0, TempProjectItemId = 0, TempProjectId = 0, TempWorkTypeId = 0;
                            var tempworkItem;

                            $('.projectRow_' + r).find('.readValue').each(function (index, item) {
                                if (successData[i] != null && successData[i] != '' && $(item) != null && $(item) != undefined) {

                                    tempApplicationid = successData[i].ApplicationId;
                                    TempProjectItemId = successData[i].ProjectItemId;
                                    TempProjectId = successData[i].ProjectId;
                                    TempWorkTypeId = successData[i].WorkTypeId;
                                    tempworkItem = (successData[i] != null && successData[i].WorkItem != null) ? successData[i].WorkItem : '';
                                    if ($(item).hasClass("workitem") && TempProjectId == projectId && TempProjectItemId == projectItemId && TempWorkTypeId == workTypeId && tempworkItem == workItem) {
                                        $(item).val(successData[i].WorkItem);
                                    }

                                    //if (teamCode != undefined && teamCode == 'DEV') {
                                    if ($(item).hasClass("Sunday") && successData[i].WeekDays == 'Sunday' && TempProjectId == projectId && TempProjectItemId == projectItemId && TempWorkTypeId == workTypeId && tempworkItem == workItem) {
                                        $(item).val(successData[i].WorkHour);
                                        item.alt = (successData[i] != null && successData[i].Comments != "null") ? successData[i].Comments : '';
                                        $(item).attr('timeentryid', successData[i].TimeEntryId);
                                        $(item).attr('almId', successData[i].WorkItem);
                                        if (projectItemPTOval > 0 && workTypePTOval > 0 && projectItemPTOval == TempProjectItemId && workTypePTOval == TempWorkTypeId)
                                            $(item).attr("disabled", true);
                                        else
                                            $(item).attr("disabled", false);
                                    }
                                    //}
                                    //else {
                                    //    if ($(item).hasClass("Sunday") && successData[i].WeekDays == 'Sunday' && TempProjectId == projectId && TempWorkTypeId == workTypeId) {
                                    //        $(item).val(successData[i].WorkHour);
                                    //        item.alt = (successData[i] != null && successData[i].Comments != "null") ? successData[i].Comments : '';
                                    //        $(item).attr('timeentryid', successData[i].TimeEntryId);
                                    //        if (projectItemPTOval > 0 && workTypePTOval > 0 && projectItemPTOval == TempProjectItemId && workTypePTOval == TempWorkTypeId)
                                    //            $(item).attr("disabled", true);
                                    //        else
                                    //            $(item).attr("disabled", false);
                                    //    }
                                    //}

                                    //if (teamCode != undefined && teamCode == 'DEV') {
                                    if ($(item).hasClass("Monday") && successData[i].WeekDays == 'Monday' && TempProjectId == projectId && TempProjectItemId == projectItemId && TempWorkTypeId == workTypeId && tempworkItem == workItem) {
                                        $(item).val(successData[i].WorkHour);
                                        item.alt = (successData[i] != null && successData[i].Comments != "null") ? successData[i].Comments : '';
                                        $(item).attr('timeentryid', successData[i].TimeEntryId);
                                        $(item).attr('almId', successData[i].WorkItem);
                                    }
                                    //}
                                    //else {
                                    //    if ($(item).hasClass("Monday") && successData[i].WeekDays == 'Monday' && TempProjectId == projectId && TempWorkTypeId == workTypeId) {
                                    //        $(item).val(successData[i].WorkHour);
                                    //        item.alt = (successData[i] != null && successData[i].Comments != "null") ? successData[i].Comments : '';
                                    //        $(item).attr('timeentryid', successData[i].TimeEntryId);
                                    //    }

                                    //}

                                    //if (teamCode != undefined && teamCode == 'DEV') {
                                    if ($(item).hasClass("Tuesday") && successData[i].WeekDays == 'Tuesday' && TempProjectId == projectId && TempProjectItemId == projectItemId && TempWorkTypeId == workTypeId && tempworkItem == workItem) {
                                        $(item).val(successData[i].WorkHour);
                                        item.alt = (successData[i] != null && successData[i].Comments != "null") ? successData[i].Comments : '';
                                        $(item).attr('timeentryid', successData[i].TimeEntryId);
                                        $(item).attr('almId', successData[i].WorkItem);
                                    }
                                    //}
                                    //else {
                                    //    if ($(item).hasClass("Tuesday") && successData[i].WeekDays == 'Tuesday' && TempProjectId == projectId && TempWorkTypeId == workTypeId) {
                                    //        $(item).val(successData[i].WorkHour);
                                    //        item.alt = (successData[i] != null && successData[i].Comments != "null") ? successData[i].Comments : '';
                                    //        $(item).attr('timeentryid', successData[i].TimeEntryId);
                                    //    }
                                    //}

                                    //if (teamCode != undefined && teamCode == 'DEV') {
                                    if ($(item).hasClass("Wednesday") && successData[i].WeekDays == 'Wednesday' && TempProjectId == projectId && TempProjectItemId == projectItemId && TempWorkTypeId == workTypeId && tempworkItem == workItem) {
                                        $(item).val(successData[i].WorkHour);
                                        item.alt = (successData[i] != null && successData[i].Comments != "null") ? successData[i].Comments : '';
                                        $(item).attr('timeentryid', successData[i].TimeEntryId);
                                        $(item).attr('almId', successData[i].WorkItem);
                                    }
                                    //}
                                    //else {
                                    //    if ($(item).hasClass("Wednesday") && successData[i].WeekDays == 'Wednesday' && TempProjectId == projectId && TempWorkTypeId == workTypeId) {
                                    //        $(item).val(successData[i].WorkHour);
                                    //        item.alt = (successData[i] != null && successData[i].Comments != "null") ? successData[i].Comments : '';
                                    //        $(item).attr('timeentryid', successData[i].TimeEntryId);
                                    //    }
                                    //}

                                    //if (teamCode != undefined && teamCode == 'DEV') {
                                    if ($(item).hasClass("Thursday") && successData[i].WeekDays == 'Thursday' && TempProjectId == projectId && TempProjectItemId == projectItemId && TempWorkTypeId == workTypeId && tempworkItem == workItem) {
                                        $(item).val(successData[i].WorkHour);
                                        item.alt = successData[i].Comments;
                                        $(item).attr('timeentryid', successData[i].TimeEntryId);
                                        $(item).attr('almId', successData[i].WorkItem);
                                    }
                                    //}
                                    //else {
                                    //    if ($(item).hasClass("Thursday") && successData[i].WeekDays == 'Thursday' && TempProjectId == projectId && TempWorkTypeId == workTypeId) {
                                    //        $(item).val(successData[i].WorkHour);
                                    //        item.alt = successData[i].Comments;
                                    //        $(item).attr('timeentryid', successData[i].TimeEntryId);
                                    //    }
                                    //}

                                    //if (teamCode != undefined && teamCode == 'DEV') {
                                    if ($(item).hasClass("Friday") && successData[i].WeekDays == 'Friday' && TempProjectId == projectId && TempProjectItemId == projectItemId && TempWorkTypeId == workTypeId && tempworkItem == workItem) {
                                        $(item).val(successData[i].WorkHour);
                                        item.alt = (successData[i] != null && successData[i].Comments != "null") ? successData[i].Comments : '';
                                        $(item).attr('timeentryid', successData[i].TimeEntryId);
                                        $(item).attr('almId', successData[i].WorkItem);
                                    }
                                    //}
                                    //else {
                                    //    if ($(item).hasClass("Friday") && successData[i].WeekDays == 'Friday' && TempProjectId == projectId && TempWorkTypeId == workTypeId) {
                                    //        $(item).val(successData[i].WorkHour);
                                    //        item.alt = (successData[i] != null && successData[i].Comments != "null") ? successData[i].Comments : '';
                                    //        $(item).attr('timeentryid', successData[i].TimeEntryId);
                                    //    }
                                    //}

                                    //if (teamCode != undefined && teamCode == 'DEV') {
                                    if ($(item).hasClass("Saturday") && successData[i].WeekDays == 'Saturday' && TempProjectId == projectId && TempProjectItemId == projectItemId && TempWorkTypeId == workTypeId && tempworkItem == workItem) {
                                        $(item).val(successData[i].WorkHour);
                                        item.alt = (successData[i] != null && successData[i].Comments != "null") ? successData[i].Comments : '';
                                        $(item).attr('timeentryid', successData[i].TimeEntryId);
                                        $(item).attr('almId', successData[i].WorkItem);
                                        if (projectItemPTOval > 0 && workTypePTOval > 0 && projectItemPTOval == TempProjectItemId && workTypePTOval == TempWorkTypeId)
                                            $(item).attr("disabled", true);
                                        else
                                            $(item).attr("disabled", false);
                                    }
                                    //} else {
                                    //    if ($(item).hasClass("Saturday") && successData[i].WeekDays == 'Saturday' && TempProjectId == projectId && TempWorkTypeId == workTypeId) {
                                    //        $(item).val(successData[i].WorkHour);
                                    //        item.alt = (successData[i] != null && successData[i].Comments != "null") ? successData[i].Comments : '';
                                    //        $(item).attr('timeentryid', successData[i].TimeEntryId);
                                    //        if (projectItemPTOval > 0 && workTypePTOval > 0 && projectItemPTOval == TempProjectItemId && workTypePTOval == TempWorkTypeId)
                                    //            $(item).attr("disabled", true);
                                    //        else
                                    //            $(item).attr("disabled", false);
                                    //    }
                                    //}

                                    //if (teamCode != undefined && teamCode == 'DEV') {
                                    if ($(item).hasClass("Total") && TempProjectId == projectId && TempProjectItemId == projectItemId && TempWorkTypeId == workTypeId && tempworkItem == workItem) {

                                        var total = 0;
                                        total = parseFloat($(item).val());
                                        if (total != null && total != 'null' && total != undefined) {
                                            total += parseFloat(successData[i].WorkHour);
                                            $(item).val(total);
                                        }
                                    }
                                    //}
                                    //else {
                                    //    if ($(item).hasClass("Total") && TempProjectId == projectId && TempWorkTypeId == workTypeId) {

                                    //        var total = 0;
                                    //        total = parseInt($(item).val());
                                    //        if (total != null && total != 'null' && total != undefined) {
                                    //            total += parseInt(successData[i].WorkHour);
                                    //            $(item).val(total);
                                    //        }
                                    //    }

                                    //}
                                }
                                $("#_panelmain").show();
                                $("#_panelmainbutton").show();
                            });

                        }
                        BindTotalVertical('');
                    }
                }
                closeModal();
            },
            error: function (error) {

            }
        });
    }
}

/// Method to load project grid
function loadProjects(startDate) {
    var employeeid = $('#selectEmployee').val();
    if (startDate != null && startDate != '') {
        $.ajax({
            type: "Get",
            url: "getExistingTimeSheetProjects?startDate=" + startDate + "&employeeId=" + employeeid,
            dataType: "json",
            cache: false,
            async: false,
            contentType: "application/json",
            success: function (successData) {
                resetGrid();
                $(".parentProject").empty();
                if (successData.length == 0) {
                    CreateTemplate();
                    CalEnd();
                    CrerateNewProjectRow();
                }
                //else if (teamCode != undefined && successData.length == 0) {
                //    CreateBATemplate();
                //    CalEnd();
                //    CrerateBAProjectRow();
                //}
                if (successData != null && successData != '' && successData.length > 0) {
                    $("#_panelmain").show();
                    $("#_panelmainbutton").show();
                    for (k = 0; k < successData.length; k++) {

                        //if (teamCode != null && teamCode != '' && teamCode != undefined && teamCode == 'DEV') {
                        CreateTemplate();
                        CalEnd();
                        CrerateNewProjectRow();
                        //}
                        //else {
                        //    CreateBATemplate();
                        //    CalEnd();
                        //    CrerateBAProjectRow();
                        //}

                    }
                    for (var i = 0; i <= successData.length; i++) {
                        var tempprojId = 0;
                        var selectedProject = '';
                        $('.projectRow_' + i).find('.ddValue').each(function (index, item) {
                            //debugger;
                           
                            if ($(item).hasClass("ApplicationId")) {

                                $(item).val(successData[i].ApplicationId);
                            }
                            //debugger;
                            if ($(item).hasClass("ProjectId")) {
                                tempprojId = successData[i].ProjectId;
                                $(item).val(successData[i].ProjectId);
                                var selectedText = $(item).find("option:selected").text();
                                selectedProject = $(item).find("option:selected").text();
                                if (selectedText.toLowerCase().indexOf('pto') != -1) {
                                    $(item).attr("disabled", true);
                                    //projectItemPTOval = successData[i].ProjectId;
                                }
                                else {
                                    $(item).attr("disabled", false);
                                    //projectItemPTOval = 0;
                                }
                            }
                            if ($(item).hasClass("ProjectItemId")) {
                                bindProjectType(tempprojId, $(item))
                                $(item).val(successData[i].ProjectItemId);
                                var selectedText = $(item).find("option:selected").text();
                                if (selectedText.toLowerCase().indexOf('pto') != -1) {
                                    $(item).attr("disabled", true);
                                    projectItemPTOval = successData[i].ProjectItemId;
                                }
                                else {
                                    $(item).attr("disabled", false);
                                    $(this).parent().parent().find(".workitem").prop("disabled", true);
                                    projectItemPTOval = 0;
                                }

                                if (selectedText.toLowerCase().indexOf('other') != -1 || selectedProject.toLowerCase().indexOf('production') != -1) {
                                    $(this).parent().parent().find(".workitem").prop("disabled", false);
                                }  
                            }
                            if ($(item).hasClass("WorkTypeId")) {
                                $(item).val(successData[i].WorkTypeId);
                                var selectedText = $(item).find("option:selected").text();
                                if (selectedText.toLowerCase().indexOf('pto') != -1) {
                                    $(item).attr("disabled", true);
                                    workTypePTOval = successData[i].WorkTypeId;
                                }
                                else {
                                    if (selectedProject != null && selectedProject != '' && selectedProject.toLowerCase().indexOf("production support") != -1) {
                                        selectedProject = 'Production Support';
                                        $(item).find("option:contains(" + selectedProject + ")").removeAttr('disabled').show();
                                    }
                                    else {
                                        selectedProject = 'Production Support';
                                        $(item).find("option:contains(" + selectedProject + ")").attr('disabled', 'disabled').hide();
                                    }
                                    $(item).attr("disabled", false);
                                    workTypePTOval = 0;
                                }
                            }
                            if ($(item).hasClass("workitem")) {
                                $(item).val(successData[i].WorkItem);
                            }
                        });
                    }
                    getTimesheetData(startDate, successData.length - 1);
                }
                else {
                    CheckIsDateAlreadyUsed(startDate);
                    $(".workitem").prop('disabled', true);
                }
                closeModal();
            },
            error: function (error) {
            }
        });
    }
}

function resetGrid() {
    $('div[class*="divTableRow"]').each(function (index, item) {
        if (index != 0) {
            $(item).remove();
        }
        else {
            $(item).find("select").each(function () {
                this.selectedIndex = 0;
            });

            $(item).find('.hour').val('0');
            productRowCount = 0;
        }
    });
}
$(window).load(function () {
    if (startDate != null && startDate != '') {
        $('#datepicker_start').val(startDate);
        //CalEnd();
        loadProjects(startDate);

    }
    else {
        $("#_panelmain").hide();
        $("#_panelmainbutton").hide();
    }

    $('.parentProject').on('click', '.rowDeleteClick', function () {
        var result = confirm("Are you sure you want to delete this row?");
        if (result) {
            var timeTrackerIds = '';
            var row = $(this).parent().parent().parent();
            row.find('.allowNumericWithDecimal').each(function (index, item) {
                var attr = $(item).attr('timeentryid');
                if (typeof attr !== typeof undefined && attr !== false) {
                    timeTrackerIds += "," + attr;
                }
            });

            if (timeTrackerIds != null && timeTrackerIds != undefined && timeTrackerIds != '') {
                timeTrackerIds = timeTrackerIds.replace(/^,|,$/g, '');
                deleteRow(timeTrackerIds);
            }
            row.remove();
        }
        if ($('.parentProject').children().length == 0) {
            CrerateNewProjectRow();
        }
    });
    $('.parentProject').on('click', '.rowSaveClick', function () {
        var row = $(this).parent().parent().parent();
        row.find('select,input').removeClass('invalid');
        $('#commentSection').hide();
        currentRowClass = $(row).attr("class");
        submitted = true;
        $("#TimeSheetForm").valid();
        if (submitted) {
            var flag = false;
            $('#DayWiseTotal').find('.Total').each(function (index, ele) {
                var total = ele.value;
                if (total >= 24) {
                    var summary = " * <label class='error'>Time entries for a day cannot exceed 24 hours</label></br>";
                    $("#messageBox").empty();
                    $("#messageBox").append(summary);
                    $(ele).addClass('invalid');
                    flag = true;
                }
                else {
                    $(ele).removeClass('invalid');
                }
            });
            if (!flag)
                SaveTimeSheetRecord(currentRowClass);
        }
    });
    BindTotalVertical('');
});

function SaveTimeSheetRecord(classname) {

    var TimeSheetRecordArray = [];

    var employeeId = $("#selectEmployee").val();
    var createUserId = $("#hdnUserId").val();
    $('.divTableRow').each(function (index, item) {
        if (classname == null || classname == '' || classname == undefined || classname == item.className) {
            var row = $(item).children(0)[0];
            if (row == null && row == undefined) {
                swal("oops!", "No record found!", "error");
                return false;
            }
            var tempAppId = 0, tempProjectTypeId = 0, tempWorkTypeId = 0, dailyHr = 0, tempDate = null, dailyComment = '', temptimeEntryId = 0, tempProjectId = 0, workitem = null;
            var cell = row.children;
            for (var i = 0; i < cell.length - 1; i++) {
                var field = $(cell[i])[0].children[0];

                if ($(field).hasClass("ApplicationId"))
                    tempAppId = $(field).val();
                if ($(field).hasClass("ProjectId"))
                    tempProjectId = $(field).val();
                if ($(field).hasClass("ProjectItemId"))
                    tempProjectTypeId = $(field).val();
                if ($(field).hasClass("WorkTypeId"))
                    tempWorkTypeId = $(field).val();
                if ($(field).hasClass("workitem"))
                    workitem = $(field).val();


                if ($(field).hasClass("Sunday")) {
                    var sun = {
                        "EmployeeId": employeeId,
                        "WorkTypeId": tempWorkTypeId,
                        "ApplicationId": tempAppId,
                        "ProjectId": tempProjectId,
                        "ProjectItemId": tempProjectTypeId,
                        "Date": $('#hdtext1').val(),
                        "WorkHour": $(field).val(),
                        "Comments": (field.alt == undefined || field.alt == 'null' || field.alt == null) ? '' : field.alt,
                        "UserId": createUserId,
                        "TimeEntryId": $(field).attr('timeentryid'),
                        "CreateUserId": createUserId,
                        "WorkItem": workitem,
                    };
                    TimeSheetRecordArray.push(sun);
                }
                if ($(field).hasClass("Monday")) {
                    var mon = {
                        "EmployeeId": employeeId,
                        "WorkTypeId": tempWorkTypeId,
                        "ApplicationId": tempAppId,
                        "ProjectId": tempProjectId,
                        "ProjectItemId": tempProjectTypeId,
                        "Date": $('#hdtext2').val(),
                        "WorkHour": $(field).val(),
                        "Comments": (field.alt == undefined || field.alt == 'null' || field.alt == null) ? '' : field.alt,
                        "UserId": createUserId,
                        "TimeEntryId": $(field).attr('timeentryid'),
                        "CreateUserId": createUserId,
                        "WorkItem": workitem,
                    };
                    TimeSheetRecordArray.push(mon);
                }
                if ($(field).hasClass("Tuesday")) {
                    var tue = {
                        "EmployeeId": employeeId,
                        "WorkTypeId": tempWorkTypeId,
                        "ApplicationId": tempAppId,
                        "ProjectId": tempProjectId,
                        "ProjectItemId": tempProjectTypeId,
                        "Date": $('#hdtext3').val(),
                        "WorkHour": $(field).val(),
                        "Comments": (field.alt == undefined || field.alt == 'null' || field.alt == null) ? '' : field.alt,
                        "UserId": createUserId,
                        "TimeEntryId": $(field).attr('timeentryid'),
                        "CreateUserId": createUserId,
                        "WorkItem": workitem,
                    };
                    TimeSheetRecordArray.push(tue);
                }
                if ($(field).hasClass("Wednesday")) {
                    var wed = {
                        "EmployeeId": employeeId,
                        "WorkTypeId": tempWorkTypeId,
                        "ApplicationId": tempAppId,
                        "ProjectId": tempProjectId,
                        "ProjectItemId": tempProjectTypeId,
                        "Date": $('#hdtext4').val(),
                        "WorkHour": $(field).val(),
                        "Comments": (field.alt == undefined || field.alt == 'null' || field.alt == null) ? '' : field.alt,
                        "UserId": createUserId,
                        "TimeEntryId": $(field).attr('timeentryid'),
                        "CreateUserId": createUserId,
                        "WorkItem": workitem,
                    };
                    TimeSheetRecordArray.push(wed);
                }
                if ($(field).hasClass("Thursday")) {
                    var thu = {
                        "EmployeeId": employeeId,
                        "WorkTypeId": tempWorkTypeId,
                        "ApplicationId": tempAppId,
                        "ProjectId": tempProjectId,
                        "ProjectItemId": tempProjectTypeId,
                        "Date": $('#hdtext5').val(),
                        "WorkHour": $(field).val(),
                        "Comments": (field.alt == undefined || field.alt == 'null' || field.alt == null) ? '' : field.alt,
                        "UserId": createUserId,
                        "TimeEntryId": $(field).attr('timeentryid'),
                        "CreateUserId": createUserId,
                        "WorkItem": workitem,
                    };
                    TimeSheetRecordArray.push(thu);
                }
                if ($(field).hasClass("Friday")) {
                    var fri = {
                        "EmployeeId": employeeId,
                        "WorkTypeId": tempWorkTypeId,
                        "ApplicationId": tempAppId,
                        "ProjectId": tempProjectId,
                        "ProjectItemId": tempProjectTypeId,
                        "Date": $('#hdtext6').val(),
                        "WorkHour": $(field).val(),
                        "Comments": (field.alt == undefined || field.alt == 'null' || field.alt == null) ? '' : field.alt,
                        "UserId": createUserId,
                        "TimeEntryId": $(field).attr('timeentryid'),
                        "CreateUserId": createUserId,
                        "WorkItem": workitem,
                    };
                    TimeSheetRecordArray.push(fri);
                }
                if ($(field).hasClass("Saturday")) {
                    var sat = {
                        "EmployeeId": employeeId,
                        "WorkTypeId": tempWorkTypeId,
                        "ApplicationId": tempAppId,
                        "ProjectId": tempProjectId,
                        "ProjectItemId": tempProjectTypeId,
                        "Date": $('#hdtext7').val(),
                        "WorkHour": $(field).val(),
                        "Comments": (field.alt == undefined || field.alt == 'null' || field.alt == null) ? '' : field.alt,
                        "UserId": createUserId,
                        "TimeEntryId": $(field).attr('timeentryid'),
                        "CreateUserId": createUserId,
                        "WorkItem": workitem,
                    };
                    TimeSheetRecordArray.push(sat);
                }
            }

            //var cell1 = cell.find("ApplicationId");
        }

    });

    var FinalRecord = { "UserId": createUserId, "LogonName": "", "TeamCode": teamCode, "TimeEntry": TimeSheetRecordArray };
    SaveTimeSheetData(FinalRecord);
}


function SaveTimeSheetData(data) {



    $.ajax({
        type: "Post",
        //url: "/SBS.IT.Utilities.Web.TimeTrackerWeb/timesheet/SavetimeSheet",
        //url: "/TimeTracker/timesheet/SavetimeSheet",
        url: "SavetimeSheet",
        dataType: "json",
        cache: false,
        async: false,
        contentType: "application/json",
        data: JSON.stringify(data),
        //beforeSend: function () {
        //    openModal();
        //},
        //complete: function () {
        //    closeModal();
        //},
        success: function (successData) {
            $('.modal2').show();
            if (successData && successData.success === false) {
                $('.modal2').hide();
                swal("Error!", successData.error || "An unexpected error occurred. Please try again.", "error");
                return;
            }
            if (successData > 0) {
                $("#messageBox").append(summary);
                swal({
                    title: "Success!",
                    text: "Record saved successfully!",
                    type: "success",
                    icon: "success",
                }).then(function () {
                    // Redirect the user
                    // window.location.href = "WeeklyTimeSheet";

                });


            }

            else {
                $('.modal2').hide();
                var summary = " * <label class='error'>" + successData + "</label></br>";
                $("#messageBox").empty();
                $("#messageBox").append(summary);
            }
            $('.modal2').hide();
        },
        error: function (error) {
            $('.modal2').hide();
            swal("Error!", "Failed to save time entries. Please try again.", "error");
        }
    });
}

$(document).ready(function () {
    $("#btnDeleterow").click(function (e) {
        var result = confirm("Are you sure you want to delete this row?");
        if (result) {
            var timeTrackerIds = '';
            var row = $(this).parent().parent();
            row.find('.allowNumericWithDecimal').each(function (index, item) {
                var attr = $(item).attr('timeentryid');
                if (typeof attr !== typeof undefined && attr !== false) {
                    timeTrackerIds += "," + attr;
                }
            });
        }

        if (timeTrackerIds != null && timeTrackerIds != undefined && timeTrackerIds != '')
            timeTrackerIds = timeTrackerIds.replace(/^,|,$/g, '');
        //debugger;
    });

    $("#datepicker_start").datepicker(
        {
            dateFormat: "yy-mm-dd",
            changeMonth: true,
            changeYear: true,
            constrainInput: false,
            yearRange: new Date().getFullYear()-1 + ':' + new Date().getFullYear(),
            maxDate: "+3w",
            onSelect: function (selectedDate) {
                //debugger;
                if ((new Date(selectedDate).getTime() != new Date(sundayDate).getTime())) {
                    //CalEnd();
                    loadProjects(selectedDate);
                }
            },
            beforeShowDay: function (date) {
                return [date.getDay() === 0, ''];
            }
        });

    $("input:text").focus(function () { $(this).select(); });

    closeModal();

    //GetAllWorkType();
    GetProject();

    BindInitialSelectBox();

    DisableonLoad();

    $("#btnSubmit").click(function () {
        $('.modal2').show();
        $('select,input').removeClass('invalid');
        submitted = true;
        $('#commentSection').hide();
        if ($("#TimeSheetForm").valid()) {
            $('.modal2').show();
            SaveTimeSheetRecord();
        }
    });

    

    $("#datepicker_start").keydown(false);
    $("#datepicker_start").keypress(false);
    $('#datepicker_start').bind("cut copy paste", function (e) {
        e.preventDefault();
    });

    $("#btnAddProject").click(function () {

        //if (teamCode != undefined && teamCode == 'DEV')
        CrerateNewProjectRow();
        //else
        //    CrerateBAProjectRow();
        $('#commentSection').hide();
        $("input:text").focus(function () { $(this).select(); });
        $("#txtAreaSun").val("");
        $("#txtAreaSat").val("");
        $("#txtAreaMon").val("");
        $("#txtAreaTue").val("");
        $("#txtAreaWed").val("");
        $("#txtAreaThu").val("");
        $("#txtAreaFri").val("");

        $("#txtSunALM").val("");
        $("#txtMonALM").val("");
        $("#txtTueALM").val("");
        $("#txtWedALM").val("");
        $("#txtThuALM").val("");
        $("#txtFriALM").val("");
        $("#txtSatALM").val("");
    });

    $("#txtAreaSun").blur(function () {
        var haur = selectedControl.val();
        var timeEntryId = $(selectedControl).attr('timeentryid');
        if (timeEntryId == null && timeEntryId == 'null' && timeEntryId == undefined)
            timeEntryId = 0;
        var sun = { "Hr": haur, "comment": $("#txtAreaSun").val(), "Date": sundayDate, "TimeEntryId": timeEntryId };


        var selectedIndex = GetSelectDayRecordIndex(selectedApplicationId, selectedProjectTypeId, selectedWorkTypeId);
        var selectedRecord = projectHoursArry[selectedIndex];

        if (selectedRecord != undefined) {

            if (selectedRecord.Sun == null) {
                selectedRecord.Sun = sun;
            }
            else {
                selectedControl.children(0).val(selectedRecord.Sun.Hr);
                //  $("#txtAreaMon").val(selectedRecord.Mon.comment);
                projectHoursArry[selectedIndex].Sun.Hr = haur;
                projectHoursArry[selectedIndex].Sun.comment = $("#txtAreaSun").val();
            }



        }

        else {
            if ($("#TimeSheetForm").valid()) {
                var projectRecord = { "applicationId": selectedApplicationId, "ProjectTypeId": selectedProjectTypeId, "WorkTypeId": selectedWorkTypeId, "Sun": sun, "Mon": null, "Tue": null, "Wed": null, "Thu": null, "Fri": null, "Sat": null };
            }
            projectHoursArry.push(projectRecord);


        }


        //$("#txtSunALM").hide();
        //$("#txtAreaSun").hide();
        selectedControl.parent().parent().find(".clsMonday").prop('readonly', false);

        //selectedControl.parent().next().children(0);

    });

    $("#txtAreaMon").blur(function () {
        var haur = selectedControl.val();
        var timeEntryId = $(selectedControl).attr('timeentryid');
        if (timeEntryId == null && timeEntryId == 'null' && timeEntryId == undefined)
            timeEntryId = 0;
        var mon = { "Hr": haur, "comment": $("#txtAreaMon").val(), "Date": mondayDate, "TimeEntryId": timeEntryId };




        var selectedIndex = GetSelectDayRecordIndex(selectedApplicationId, selectedProjectTypeId, selectedWorkTypeId);
        var selectedRecord = projectHoursArry[selectedIndex];

        if (selectedRecord != undefined) {

            if (selectedRecord.Mon == null) {
                selectedRecord.Mon = mon;
            }
            else {
                selectedControl.children(0).val(selectedRecord.Mon.Hr);
                //  $("#txtAreaMon").val(selectedRecord.Mon.comment);
                projectHoursArry[selectedIndex].Mon.Hr = haur;
                projectHoursArry[selectedIndex].Mon.comment = $("#txtAreaMon").val();
            }


        }

        else {
            var projectRecord = { "applicationId": selectedApplicationId, "ProjectTypeId": selectedProjectTypeId, "WorkTypeId": selectedWorkTypeId, "Sun": null, "Mon": mon, "Tue": null, "Wed": null, "Thu": null, "Fri": null, "Sat": null };

            projectHoursArry.push(projectRecord);


        }



        //$("#txtMonALM").hide();
        //$("#txtAreaMon").hide();
        selectedControl.parent().parent().find(".clsTuesday").prop('readonly', false);

        //selectedControl.parent().next().children(0);

    });

    $("#txtAreaTue").blur(function () {
        var haur = selectedControl.val();
        var timeEntryId = $(selectedControl).attr('timeentryid');
        if (timeEntryId == null && timeEntryId == 'null' && timeEntryId == undefined)
            timeEntryId = 0;
        var tue = { "Hr": haur, "comment": $("#txtAreaTue").val(), "Date": tuesdayDate, "TimeEntryId": timeEntryId };
        var selectedIndex = GetSelectDayRecordIndex(selectedApplicationId, selectedProjectTypeId, selectedWorkTypeId);

        var selectedRecord = projectHoursArry[selectedIndex];

        if (selectedRecord != undefined) {

            if (selectedRecord.Tue == null) {
                selectedRecord.Tue = tue;
            }
            else {
                selectedControl.children(0).val(selectedRecord.Tue.Hr);
                // $("#txtAreaTue").val(selectedRecord.Tue.comment);
                projectHoursArry[selectedIndex].Tue.Hr = haur;
                projectHoursArry[selectedIndex].Tue.comment = $("#txtAreaTue").val();
            }
        }

        else {
            var projectRecord = { "applicationId": selectedApplicationId, "ProjectTypeId": selectedProjectTypeId, "WorkTypeId": selectedWorkTypeId, "Sun": null, "Mon": null, "Tue": tue, "Wed": null, "Thu": null, "Fri": null, "Sat": null };

            projectHoursArry.push(projectRecord);
        }

        selectedControl.parent().parent().find(".clsWednesday").prop('readonly', false);

        //selectedControl.parent().next().children(0);

    });

    $("#txtAreaWed").blur(function () {

        var timeEntryId = $(selectedControl).attr('timeentryid');
        if (timeEntryId == null && timeEntryId == 'null' && timeEntryId == undefined)
            timeEntryId = 0;
        var haur = selectedControl.val();
        var wed = { "Hr": haur, "comment": $("#txtAreaWed").val(), "Date": wednesdayDate, "TimeEntryId": timeEntryId };


        var selectedIndex = GetSelectDayRecordIndex(selectedApplicationId, selectedProjectTypeId, selectedWorkTypeId);

        var selectedRecord = projectHoursArry[selectedIndex];

        if (selectedRecord != undefined) {

            if (selectedRecord.Wed == null) {
                selectedRecord.Wed = wed;
            }
            else {
                selectedControl.children(0).val(selectedRecord.Wed.Hr);
                //  $("#txtAreaWed").val(selectedRecord.Wed.comment);
                projectHoursArry[selectedIndex].Wed.Hr = haur;
                projectHoursArry[selectedIndex].Wed.comment = $("#txtAreaWed").val();
            }


        }

        else {
            var projectRecord = { "applicationId": selectedApplicationId, "ProjectTypeId": selectedProjectTypeId, "WorkTypeId": selectedWorkTypeId, "Sun": null, "Mon": null, "Tue": null, "Wed": wed, "Thu": null, "Fri": null, "Sat": null };

            projectHoursArry.push(projectRecord);


        }
        selectedControl.parent().parent().find(".clsThursday").prop('readonly', false);

        //selectedControl.parent().next().children(0);

    });

    $("#txtAreaThu").blur(function () {

        var timeEntryId = $(selectedControl).attr('timeentryid');
        if (timeEntryId == null && timeEntryId == 'null' && timeEntryId == undefined)
            timeEntryId = 0;
        var haur = selectedControl.val();

        var thu = { "Hr": haur, "comment": $("#txtAreaThu").val(), "Date": thursdayDate, "TimeEntryId": timeEntryId };


        var selectedIndex = GetSelectDayRecordIndex(selectedApplicationId, selectedProjectTypeId, selectedWorkTypeId);

        var selectedRecord = projectHoursArry[selectedIndex];

        if (selectedRecord != undefined) {

            if (selectedRecord.Thu == null) {
                selectedRecord.Thu = thu;
            }
            else {
                selectedControl.children(0).val(selectedRecord.Thu.Hr);
                // $("#txtAreaThu").val(selectedRecord.Thu.comment);
                projectHoursArry[selectedIndex].Thu.Hr = haur;
                projectHoursArry[selectedIndex].Thu.comment = $("#txtAreaThu").val();
            }


        }

        else {
            var projectRecord = { "applicationId": selectedApplicationId, "ProjectTypeId": selectedProjectTypeId, "WorkTypeId": selectedWorkTypeId, "Sun": null, "Mon": null, "Tue": null, "Wed": null, "Thu": thu, "Fri": null, "Sat": null };

            projectHoursArry.push(projectRecord);


        }

        selectedControl.parent().parent().find(".clsFriday").prop('readonly', false);

        //selectedControl.parent().next().children(0);

    });

    $("#txtAreaFri").blur(function () {

        var timeEntryId = $(selectedControl).attr('timeentryid');
        if (timeEntryId == null && timeEntryId == 'null' && timeEntryId == undefined)
            timeEntryId = 0;

        var haur = selectedControl.val();
        var fri = { "Hr": haur, "comment": $("#txtAreaFri").val(), "Date": fridayDate, "TimeEntryId": timeEntryId };

        var selectedIndex = GetSelectDayRecordIndex(selectedApplicationId, selectedProjectTypeId, selectedWorkTypeId);


        var selectedRecord = projectHoursArry[selectedIndex];

        if (selectedRecord != undefined) {

            if (selectedRecord.Fri == null) {
                selectedRecord.Fri = fri;
            }
            else {
                selectedControl.children(0).val(selectedRecord.Fri.Hr);
                //   $("#txtAreaFri").val(selectedRecord.Fri.comment);
                projectHoursArry[selectedIndex].Fri.Hr = haur;
                projectHoursArry[selectedIndex].Fri.comment = $("#txtAreaFri").val();
            }


        }

        else {
            var projectRecord = { "applicationId": selectedApplicationId, "ProjectTypeId": selectedProjectTypeId, "WorkTypeId": selectedWorkTypeId, "Sun": null, "Mon": null, "Tue": null, "Wed": null, "Thu": null, "Fri": fri, "Sat": null };

            projectHoursArry.push(projectRecord);


        }

        selectedControl.parent().parent().find(".clsSaturday").prop('readonly', false);

        //selectedControl.parent().next().children(0);

    });


    $("#txtAreaSat").blur(function () {

        var timeEntryId = $(selectedControl).attr('timeentryid');
        if (timeEntryId == null && timeEntryId == 'null' && timeEntryId == undefined)
            timeEntryId = 0;
        var haur = selectedControl.val();

        var sat = { "Hr": haur, "comment": $("#txtAreaSat").val(), "Date": saturdayDate, "TimeEntryId": timeEntryId };

        var selectedIndex = GetSelectDayRecordIndex(selectedApplicationId, selectedProjectTypeId, selectedWorkTypeId);

        var selectedRecord = projectHoursArry[selectedIndex];

        if (selectedRecord != undefined) {

            if (selectedRecord.Sat == null) {
                selectedRecord.Sat = sat;
            }
            else {
                selectedControl.children(0).val(selectedRecord.Sat.Hr);
                // $("#txtAreaSat").val(selectedRecord.Sat.comment);
                projectHoursArry[selectedIndex].Sat.Hr = haur;
                projectHoursArry[selectedIndex].Sat.comment = $("#txtAreaSats").val();
            }


        }

        else {
            var projectRecord = { "applicationId": selectedApplicationId, "ProjectTypeId": selectedProjectTypeId, "WorkTypeId": selectedWorkTypeId, "Sun": null, "Mon": null, "Tue": null, "Wed": null, "Thu": null, "Fri": null, "Sat": sat };

            projectHoursArry.push(projectRecord);


        }
        //selectedControl.parent().find(".clsMonday").prop('readonly', false);

        //selectedControl.parent().next().children(0);

    });

    $(document).on('keypress', '.clsALM', function (e) {
        if (selectedControl != null && selectedControl != undefined) {
            var parentCtrl = selectedControl.parent().find('.hour');
            if (parentCtrl != null && parentCtrl != undefined && parentCtrl.length > 0)
                $(parentCtrl[0]).attr('almId', $(this).val());
        }
        if (e.keyCode === 9) {
            selectedControl.parent().next().children().focus();
        }
    });

    $(document).on('blur', '.clsALM', function (e) {
        if (selectedControl != null && selectedControl != undefined) {
            var parentCtrl = selectedControl.parent().find('.hour');
            if (parentCtrl != null && parentCtrl != undefined && parentCtrl.length > 0)
                $(parentCtrl[0]).attr('almId', $(this).val());
        }
        if (e.keyCode === 9) {
            selectedControl.parent().next().children().focus();
        }
    });

    $(document).on('blur', '.clsComment', function (e) {
        if (selectedControl != null && selectedControl != undefined) {
            var parentCtrl = selectedControl.parent().find('.hour');
            if (parentCtrl != null && parentCtrl != undefined && parentCtrl.length > 0)
                parentCtrl[0].alt = $(this).val();
        }
        if (e.keyCode === 9) {
            selectedControl.parent().next().children().focus();
        }
    });

    $(document).on('keypress', '.clsComment', function (e) {
        if (selectedControl != null && selectedControl != undefined) {
            var parentCtrl = selectedControl.parent().find('.hour');
            if (parentCtrl != null && parentCtrl != undefined && parentCtrl.length > 0)
                parentCtrl[0].alt = $(this).val();
        }
        if (e.keyCode === 9) {
            selectedControl.parent().next().children().focus();
        }
    });

    $(document).on('keydown', '.clsComment', function (e) {
        if (e.keyCode === 9) {
            selectedControl.parent().next().children().focus();
            e.preventDefault();
        }
    });


    $(document).on('keydown', '.hour', function (e) {
        if (e.keyCode === 9) {


            var selectedDay = this.className;

            var selected = selectedDay.split(' ');

            selectedDay = selected[2];


            switch (selectedDay) {
                case "clsSunday":
                    $('#txtAreaSun').show();
                    //$('#txtAreaSun').focus();
                    //e.preventDefault();
                    break;
                case "clsMonday":
                    $("#txtAreaMon").show();
                    //$("#txtAreaMon").focus();
                    //e.preventDefault();
                    break;

                case "clsTuesday":
                    $("#txtAreaTue").show();
                    //$("#txtAreaTue").focus();
                    //e.preventDefault();
                    break;
                case "clsWednesday":
                    $("#txtAreaWed").show();
                    //$("#txtAreaWed").focus();
                    //e.preventDefault();
                    break;
                case "clsThursday":
                    $("#txtAreaThu").show();
                    //$("#txtAreaThu").focus();
                    //e.preventDefault();
                    break;
                case "clsFriday":
                    $("#txtAreaFri").show();
                    //$("#txtAreaFri").focus();
                    //e.preventDefault();
                    break;
                case "clsSaturday":

                    $("#txtAreaSat").show();
                //$("#txtAreaSat").focus();
                //e.preventDefault();
                default:
                    $('#commentSection').hide();

            }
        }
    });


    $(document).on('change', '.clsSelectProject', function () {
        var selectedApplication = this.value;
        selectedApplicationId = selectedApplication;
        if (selectedApplicationId != '' && selectedApplicationId != null) {
            var projectType = GetProjectType(selectedApplication);
            if (projectType != null && projectType != undefined && projectType.length > 0) {

                var projectTypeList = "<option value=''>Select Project Item</option>";
                for (i = 0; i < projectType.length; i++) {

                    projectTypeList += "<option value='" + projectType[i].ProjectItemId + "'>" + projectType[i].ProjectItemNameDescription + "</option>";

                };
                $(this).parent().parent().find(".clsSelectProjectType").empty();
                $(this).parent().parent().find(".clsSelectProjectType").append(projectTypeList);
                $(this).parent().parent().find(".clsSelectProjectType").attr("disabled", false);

            }
            else {
                var projectTypeList = "<option value=''>Select Project Item</option>";
                $(this).parent().parent().find(".clsSelectProjectType").empty();
                $(this).parent().parent().find(".clsSelectProjectType").append(projectTypeList);
                $(this).parent().parent().find(".clsSelectProjectType").attr("disabled", true);
            }
        }
        else {
            var projectTypeList = "<option value=''>Select Project Item</option>";
            $(this).parent().parent().find(".clsSelectProjectType").empty();
            $(this).parent().parent().find(".clsSelectProjectType").append(projectTypeList);
            $(this).parent().parent().find(".clsSelectProjectType").attr("disabled", false);
        }
        $(this).parent().parent().find(".workitem").prop("disabled", true);
        $(this).parent().parent().find(".workitem").val(null);
        var selectedText = $(this).find("option:selected").text();
        if (selectedText != null && selectedText != undefined && selectedText != '' && (selectedText.toLowerCase().indexOf('pto') != -1 || selectedText.toLowerCase().indexOf('holiday') != -1)) {
            selectedText = 'PTO';
            var wtValue = $(this).parent().parent().find(".clsSelectWorkType option:contains(" + selectedText + ")");
            $(this).parent().parent().find(".clsSelectProjectType option:contains(" + selectedText + ")").attr('selected', true);
            $(this).parent().parent().find(".clsSelectWorkType option:contains(" + selectedText + ")").attr('selected', true);
            if (wtValue != null && wtValue.length > 0)
                $(this).parent().parent().find(".clsSelectWorkType").val(wtValue[0].value);
            $(this).parent().parent().find(".clsSelectProjectType").attr("disabled", true);
            $(this).parent().parent().find(".clsSelectWorkType").attr("disabled", true);
            $(this).parent().parent().find(".clsSunday").attr("disabled", true);
            $(this).parent().parent().find(".clsSaturday").attr("disabled", true);
        } else {
            $(this).parent().parent().find(".clsSelectProjectType").attr("disabled", false);
            $(this).parent().parent().find(".clsSelectWorkType").attr("disabled", false);
            $(this).parent().parent().find(".clsSunday").attr("disabled", false);
            //$(this).parent().parent().find(".clsSelectWorkType").val('');
            $(this).parent().parent().find(".clsSaturday").attr("disabled", false);

            // make control readonly for production support
            if (selectedText != null && selectedText != undefined && selectedText != '' && (selectedText.toLowerCase().indexOf('production support') != -1)) {
                selectedText = 'Production Support';
                $(this).parent().parent().find(".clsSelectWorkType option:contains('" + selectedText + "')").removeAttr('disabled').show();
                $(this).parent().parent().find(".clsSelectWorkType option:contains('" + selectedText + "')").attr('selected', 'selected');
                var psValue = $(this).parent().parent().find(".clsSelectWorkType option:contains(" + selectedText + ")");
                if (psValue != null && psValue.length > 0)
                    $(this).parent().parent().find(".clsSelectWorkType").val(psValue[0].value);
                $(this).parent().parent().find(".clsSelectWorkType").attr("disabled", true);
                $(this).parent().parent().find(".workitem").prop("disabled", false);
            } else {
                selectedText = 'Production Support';
                $(this).parent().parent().find(".clsSelectWorkType option:contains('" + selectedText + "')").attr('disabled', 'disabled').hide();
                $(this).parent().parent().find(".clsSelectWorkType option:eq(0)").prop("selected", true);
                $(this).parent().parent().find(".clsSelectWorkType").attr("disabled", false);
            }
        }

      
        
    });

    $(document).on('change', '.clsSelectProjectType', function () {
        var appId = this.parentElement.parentElement.children[0].children[0].value;
        var projecType = this.value;
        selectedProjectTypeId = projecType;
        var worktype = this.parentElement.parentElement.children[2].children[0].value
        var selectedText = $(this).find("option:selected").text();
        var selectedIndex = GetSelectDayRecordIndex(appId, projecType, worktype);

        var workItemctrl = $(this).parent().parent().find(".workitem");
        workItemctrl.val(null);
        if (selectedText.toLowerCase().indexOf('other') != -1 || $(this).parent().parent().find(".clsSelectProject").find("option:selected").text().toLowerCase().indexOf('production') != -1) {
            workItemctrl.prop("disabled", false);

        } else {
            workItemctrl.prop("disabled", true);
        }

        var selectedText = $(this).find("option:selected").text();
        if (selectedText != null && selectedText != undefined && selectedText != '' && (selectedText.toLowerCase().indexOf('pto') != -1 || selectedText.toLowerCase().indexOf('holiday') != -1)) {
            selectedText = 'PTO';
            var wtValue = $(this).parent().parent().find(".clsSelectWorkType option:contains(" + selectedText + ")");
            $(this).parent().parent().find(".clsSelectWorkType option:contains(" + selectedText + ")").attr('selected', true);
            if (wtValue != null && wtValue.length > 0)
                $(this).parent().parent().find(".clsSelectWorkType").val(wtValue[0].value);
            $(this).parent().parent().find(".clsSelectWorkType").attr("disabled", true);
            $(this).parent().parent().find(".clsSunday").attr("disabled", true);
            $(this).parent().parent().find(".clsSaturday").attr("disabled", true);
        } else {
            var selectedText = $(this).parent().parent().find(".clsSelectProject option:selected").text();
            if (selectedText != null && selectedText != undefined && selectedText != '' && (selectedText.toLowerCase().indexOf('production support') != -1)) {

            }
            else {
                $(this).parent().parent().find(".clsSelectWorkType").attr("disabled", false);
                $(this).parent().parent().find(".clsSunday").attr("disabled", false);
                //$(this).parent().parent().find(".clsSelectWorkType").val('');
                $(this).parent().parent().find(".clsSaturday").attr("disabled", false);
            }
        }

        $("#txtSunALM").val("");
        $("#txtMonALM").val("");
        $("#txtTueALM").val("");
        $("#txtWedALM").val("");
        $("#txtThuALM").val("");
        $("#txtFriALM").val("");
        $("#txtSatALM").val("");
        $("#txtWorkItem").val(null);
    });

    $(document).on('change', '.clsSelectEmployee', function () {
        //debugger;
        var empId = $(this).val();
        openModal();
        if (empId != null && empId != undefined) {
            $.ajax({
                type: "Get",
                url: "GetByEmployeeId?employeeId=" + empId,
                dataType: "json",
                cache: false,
                async: false,
                contentType: "application/json",
                success: function (successData) {
                    var empdata = successData;
                    //debugger;
                    //if (empdata != null && empdata != undefined && empdata.TeamCode != '' && empdata.TeamCode == 'DEV') {
                    teamCode = empdata.TeamCode;
                    tempTeamWorkType = empdata.TeamWorkType;
                    $(".parentProject").empty('');
                    //} else {
                    //    teamCode = '';
                    //}
                    var startDate = $('#datepicker_start').val();
                    productRowCount = 0;
                    loadProjects(startDate);
                }
            });
        }
    });

    $(document).on('change', '.clsSelectWorkType', function () {
        var appId = this.parentElement.parentElement.children[0].children[0].value;
        var projecType = this.parentElement.parentElement.children[1].children[0].value;
        var selectVal = this.value;
        selectedWorkTypeId = selectVal;
        var selectedIndex = GetSelectDayRecordIndex(appId, projecType, selectVal);
        var selectedText = $(this).find("option:selected").text();
        var workItemctrl = $(this).parent().parent().find(".workitem");
        workItemctrl.val(null);
        if (selectedText.toLowerCase().indexOf('production') != -1 || $(this).parent().parent().find(".clsSelectProject").find("option:selected").text().toLowerCase().indexOf('production') != -1) {
            workItemctrl.prop("disabled", false);
        } else {
            var ProjectItemText = $(this).parent().parent().find(".clsSelectProjectType").find("option:selected").text();
            if (ProjectItemText.toLowerCase().indexOf('other') <= -1)
                workItemctrl.prop("disabled", true);
        }
    });


    $(document).on('keypress keyup blur', '.hour', function () {

        var applicationText = $(this).parent().parent().children().find(".clsSelectProject option:selected").text();

        if (applicationText != null && applicationText != undefined && applicationText != '' && (applicationText.toLowerCase().indexOf('pto') != -1 || applicationText.toLowerCase().indexOf('holiday') != -1)) {
            var hourField = parseInt($(this).val());
            if (isNaN(hourField) || hourField > 8) {
                $(this).val('0');
                alert('Please enter hour less than or equal 8');
            }
        }

        var totalHours = 0;
        var count = 0;
        selectedControl = $(this);
        //if ($("#TimeSheetForm").valid()) {
        $(this).parent().parent().find(".hour").each(function (index, element) {
            if (index == 7) {
                element.value = totalHours;
            }

            if (element.value != "") {

                var hr = parseFloat(element.value);
                if (isNaN(hr)) {
                    hr = 0;
                }
                totalHours += hr;


            }

        });

        var haur = $(this).val();

        var timeEntryId = $(this).attr('timeentryid');
        if (timeEntryId == null || timeEntryId == 'null' || timeEntryId == undefined)
            timeEntryId = 0;

        var comment = '', commentdate; var currentControlClass = '';
        if ($(this).hasClass("Sunday")) {
            comment = $("#txtAreaSun").val();
            commentdate = sundayDate;
            currentControlClass = 'Sunday';
        }
        if ($(this).hasClass("Monday")) {
            comment = $("#txtAreaMon").val();
            commentdate = mondayDate;
            currentControlClass = 'Monday';
        }
        if ($(this).hasClass("Tuesday")) {
            comment = $("#txtAreaTue").val();
            commentdate = tuesdayDate;
            currentControlClass = 'Tuesday';
        }
        if ($(this).hasClass("Wednesday")) {
            comment = $("#txtAreaWed").val();
            commentdate = wednesdayDate;
            currentControlClass = 'Wednesday';
        }
        if ($(this).hasClass("Thursday")) {
            comment = $("#txtAreaThu").val();
            commentdate = thursdayDate;
            currentControlClass = 'Thursday';
        }
        if ($(this).hasClass("Friday")) {
            comment = $("#txtAreaFri").val();
            commentdate = fridayDate;
            currentControlClass = 'Friday';
        }
        if ($(this).hasClass("Saturday")) {
            comment = $("#txtAreaSat").val();
            commentdate = saturdayDate;
            currentControlClass = 'Saturday';
        }

        var day = { "Hr": haur, "comment": comment, "Date": commentdate, "TimeEntryId": timeEntryId };

        BindTotalVertical(currentControlClass);

        //debugger;
        var selectedIndex = GetSelectDayRecordIndex(selectedApplicationId, selectedProjectTypeId, selectedWorkTypeId);
        var selectedRecord = projectHoursArry[selectedIndex];
        if (selectedRecord == null || selectedRecord == 'null' || selectedRecord == undefined) {
            if ($("#TimeSheetForm").valid()) {
                var projectRecord = { "applicationId": selectedApplicationId, "ProjectTypeId": selectedProjectTypeId, "WorkTypeId": selectedWorkTypeId, "Sun": null, "Mon": null, "Tue": null, "Wed": null, "Thu": null, "Fri": null, "Sat": null };
            }
            projectHoursArry.push(projectRecord);
        }
        selectedRecord = projectHoursArry[selectedIndex];
        if ($(this).hasClass("Sunday")) {
            if (selectedRecord != undefined) {
                if (selectedRecord.Sun == null) {
                    selectedRecord.Sun = day;
                }
                else {
                    $(this).children(0).val(selectedRecord.Sun.Hr);
                    //  $("#txtAreaMon").val(selectedRecord.Mon.comment);
                    projectHoursArry[selectedIndex].Sun.Hr = haur;
                    projectHoursArry[selectedIndex].Sun.comment = $("#txtAreaSun").val();
                }
            }
        }

        if ($(this).hasClass("Monday")) {
            if (selectedRecord != undefined) {
                if (selectedRecord.Mon == null) {
                    selectedRecord.Mon = day;
                }
                else {
                    $(this).children(0).val(selectedRecord.Mon.Hr);
                    //  $("#txtAreaMon").val(selectedRecord.Mon.comment);
                    projectHoursArry[selectedIndex].Mon.Hr = haur;
                    projectHoursArry[selectedIndex].Mon.comment = $("#txtAreaMon").val();
                }
            }
        }

        if ($(this).hasClass("Tuesday")) {
            if (selectedRecord != undefined) {
                if (selectedRecord.Tue == null) {
                    selectedRecord.Tue = day;
                }
                else {
                    $(this).children(0).val(selectedRecord.Tue.Hr);
                    //  $("#txtAreaMon").val(selectedRecord.Mon.comment);
                    projectHoursArry[selectedIndex].Tue.Hr = haur;
                    projectHoursArry[selectedIndex].Tue.comment = $("#txtAreaTue").val();
                }
            }
        }

        if ($(this).hasClass("Wednesday")) {
            if (selectedRecord != undefined) {
                if (selectedRecord.Wed == null) {
                    selectedRecord.Wed = day;
                }
                else {
                    $(this).children(0).val(selectedRecord.Wed.Hr);
                    //  $("#txtAreaMon").val(selectedRecord.Mon.comment);
                    projectHoursArry[selectedIndex].Wed.Hr = haur;
                    projectHoursArry[selectedIndex].Wed.comment = $("#txtAreaWed").val();
                }
            }
        }

        if ($(this).hasClass("Thursday")) {
            if (selectedRecord != undefined) {
                if (selectedRecord.Thu == null) {
                    selectedRecord.Thu = day;
                }
                else {
                    $(this).children(0).val(selectedRecord.Thu.Hr);
                    //  $("#txtAreaMon").val(selectedRecord.Mon.comment);
                    projectHoursArry[selectedIndex].Thu.Hr = haur;
                    projectHoursArry[selectedIndex].Thu.comment = $("#txtAreaThu").val();
                }
            }
        }

        if ($(this).hasClass("Friday")) {
            if (selectedRecord != undefined) {
                if (selectedRecord.Fri == null) {
                    selectedRecord.Fri = day;
                }
                else {
                    $(this).children(0).val(selectedRecord.Fri.Hr);
                    //  $("#txtAreaMon").val(selectedRecord.Mon.comment);
                    projectHoursArry[selectedIndex].Fri.Hr = haur;
                    projectHoursArry[selectedIndex].Fri.comment = $("#txtAreaFri").val();
                }
            }
        }

        if ($(this).hasClass("Saturday")) {
            if (selectedRecord != undefined) {
                if (selectedRecord.Sat == null) {
                    selectedRecord.Sat = day;
                }
                else {
                    $(this).children(0).val(selectedRecord.Sat.Hr);
                    //  $("#txtAreaMon").val(selectedRecord.Mon.comment);
                    projectHoursArry[selectedIndex].Sat.Hr = haur;
                    projectHoursArry[selectedIndex].Sat.comment = $("#txtAreaSat").val();
                }
            }
        }


        //}
    });
    $(document).on('focus', '.hour', function (e) {

        selectedApplicationId = $(this).parent().parent().children().find(".clsSelectProject").val();
        selectedProjectTypeId = $(this).parent().parent().children().find(".clsSelectProjectType").val();
        selectedWorkTypeId = $(this).parent().parent().children().find(".clsSelectWorkType").val();
        $('#commentSection').show();
        var selectedIndex = GetSelectDayRecordIndex(selectedApplicationId, selectedProjectTypeId, selectedWorkTypeId);
        var record = projectHoursArry[selectedIndex];

        var selectedDay = this.className;

        var selected = selectedDay.split(' ');

        selectedDay = selected[2];


        ToggleDayCommentTextArea(selectedDay, record, this);
        var comment = "";


    });

});


function GetSelectDayRecord(projectId) {

    $.each(projectHoursArry, function (i, Project) {

        if (projectId == Project.ProjectId) {
            return Project;
        }
    });

}

function GetSelectDayRecordIndex(applicationId, ProjectTypeId, WorkTypeId) {
    //debugger;
    var index;
    projectHoursArry = projectHoursArry.filter(function (n) { return n != undefined });

    if (projectHoursArry.length > 0) {
        for (i = 0; i < projectHoursArry.length; i++) {

            if (applicationId == projectHoursArry[i].applicationId) {

                if ((ProjectTypeId == projectHoursArry[i].ProjectTypeId) && WorkTypeId == projectHoursArry[i].WorkTypeId) {
                    return i;
                    break;
                }
            }
        }
    }
    else {
        return 0;
    }

}
function CalEnd() {

    var a = $("#datepicker_start").datepicker('getDate').getTimedate1

    //var weekday = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    var weekday = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
    var selectedFrom = $("#datepicker_start").val();


    var FromArray = selectedFrom.split('-');


    var newFromDate = new Date(FromArray[0], FromArray[1] - 2, FromArray[2]);
    var startDate = new Date(FromArray[0], FromArray[1] - 2, FromArray[2]);

    var DisplaystartDate = "";

    if (DisplaystartDate == "") {
        var data = newFromDate;
        //$("#date1").text(data.getDate() + '-' + parseFloat(data.getMonth() + 1));

        $("#date1").text(formatDateToString(data));
        var month = parseFloat(data.getMonth() + 1);
        if (month < 10) { month = '0' + month; }
        sundayDate = data.getFullYear() + '-' + month + '-' + data.getDate();
        var hddate1 = data.getFullYear() + '-' + parseFloat(data.getMonth() + 1) + '-' + data.getDate();

        $("#hdtext1").val(hddate1);
        $("#label1").text('[' + weekday[data.getDay()] + ']');
        $("#DaysofWeek1").val(weekday[data.getDay()]);
    }

    for (var i = 0; i < 7; i++) {
        var tempdate = newFromDate;
        var labelDate = newFromDate.setDate(tempdate.getDate() + 1);

        if (i == 0) {
            $("#label2").text('[' + weekday[data.getDay()] + ']');

            $("#date2").text(formatDateToString(data));

            //$("#date2").text(data.getDate() + '-' + parseFloat(data.getMonth() + 1));
            mondayDate = data.getFullYear() + '-' + parseFloat(data.getMonth() + 1) + '-' + data.getDate();
            hddate2 = data.getFullYear() + '-' + parseFloat(data.getMonth() + 1) + '-' + data.getDate();

            $("#hdtext2").val(hddate2);
            $("#DaysofWeek2").val(weekday[data.getDay()]);
        }
        else if (i == 1) {
            $("#label3").text('[' + weekday[data.getDay()] + ']');

            $("#date3").text(formatDateToString(tempdate));

            //$("#date3").text(tempdate.getDate() + '-' + parseFloat(data.getMonth() + 1));
            tuesdayDate = data.getFullYear() + '-' + parseFloat(data.getMonth() + 1) + '-' + data.getDate();
            hddate3 = data.getFullYear() + '-' + parseFloat(data.getMonth() + 1) + '-' + data.getDate();

            $("#hdtext3").val(hddate3);
            $("#DaysofWeek3").val(weekday[data.getDay()]);
        }
        else if (i == 2) {
            $("#label4").text('[' + weekday[data.getDay()] + ']');
            //$("#date4").text(tempdate.getDate() + '-' + parseFloat(data.getMonth() + 1));
            $("#date4").text(formatDateToString(tempdate));
            wednesdayDate = data.getFullYear() + '-' + parseFloat(data.getMonth() + 1) + '-' + data.getDate();
            hddate4 = data.getFullYear() + '-' + parseFloat(data.getMonth() + 1) + '-' + data.getDate();

            $("#hdtext4").val(hddate4);
            $("#DaysofWeek4").val(weekday[data.getDay()]);
        }
        else if (i == 3) {
            $("#label5").text('[' + weekday[data.getDay()] + ']');
            //$("#date5").text(tempdate.getDate() + '-' + parseFloat(data.getMonth() + 1));

            $("#date5").text(formatDateToString(tempdate));
            thursdayDate = data.getFullYear() + '-' + parseFloat(data.getMonth() + 1) + '-' + data.getDate();
            hddate5 = data.getFullYear() + '-' + parseFloat(data.getMonth() + 1) + '-' + data.getDate();

            $("#hdtext5").val(hddate5);
            $("#DaysofWeek5").val(weekday[data.getDay()]);
        }
        else if (i == 4) {
            $("#label6").text('[' + weekday[data.getDay()] + ']');
            //$("#date6").text(tempdate.getDate() + '-' + parseFloat(data.getMonth() + 1));

            $("#date6").text(formatDateToString(tempdate));

            fridayDate = data.getFullYear() + '-' + parseFloat(data.getMonth() + 1) + '-' + data.getDate();
            hddate6 = data.getFullYear() + '-' + parseFloat(data.getMonth() + 1) + '-' + data.getDate();
            $("#hdtext6").val(hddate6);
            $("#DaysofWeek6").val(weekday[data.getDay()]);
        }
        else if (i == 5) {
            $("#label7").text('[' + weekday[data.getDay()] + ']');
            //$("#date7").text(tempdate.getDate() + '-' + parseFloat(data.getMonth() + 1));

            $("#date7").text(formatDateToString(tempdate));

            saturdayDate = data.getFullYear() + '-' + parseFloat(data.getMonth() + 1) + '-' + data.getDate();
            hddate7 = data.getFullYear() + '-' + parseFloat(data.getMonth() + 1) + '-' + data.getDate();
            $("#hdtext7").val(hddate7);
            $("#DaysofWeek7").val(weekday[data.getDay()]);
        }
    }
}


function CheckIsDateAlreadyUsed(selectedDate) {

    var date2 = $('#datepicker_start').datepicker('getDate', '+1');
    CalEnd();
    $("#_panelmain").show();
    $("#_panelmainbutton").show();

}




function DisableonLoad() {

    $("#text1_p1").prop('readonly', true);
    $("#text2_p1").prop('readonly', true);
    $("#text3_p1").prop('readonly', true);
    $("#text4_p1").prop('readonly', true);
    $("#text5_p1").prop('readonly', true);
    $("#text6_p1").prop('readonly', true);
    $("#text7_p1").prop('readonly', true);





}



function openModal() {
    //document.getElementById('modal').style.display = 'block';
    //document.getElementById('fade').style.display = 'block';
    $('.modal2').show();
}

function closeModal() {
    //document.getElementById('modal').style.display = 'none';
    //document.getElementById('fade').style.display = 'none';
    $('.modal2').hide();
}





function formatDateToString(date) {
    // 01, 02, 03, ... 29, 30, 31
    var dd = (date.getDate() < 10 ? '0' : '') + date.getDate();
    // 01, 02, 03, ... 10, 11, 12
    var MM = ((date.getMonth() + 1) < 10 ? '0' : '') + (date.getMonth() + 1);
    // 1970, 1971, ... 2015, 2016, ...
    var yyyy = date.getFullYear();

    // create the format you want
    // return (dd + "-" + MM + "-" + yyyy);

    //return (dd + "-" + MM);
    return (MM + "-" + dd);
}

$(".allowNumericWithDecimal").on("keypress keyup blur", function (event) {
    //this.value = this.value.replace(/[^0-9\.]/g,'');
    $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
    if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
        event.preventDefault();
    }
});

$(".allowNumericWithComma").on("keypress keyup blur", function (event) {
    var theEvent = event || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    var regex = /[^0-9]+$/;
    if (regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
});

function CrerateNewProjectRow() {



    var projecrList = "";
    for (i = 0; i < projectArray.length; i++) {

        projecrList += "<option value='" + projectArray[i].ProjectId + "'>" + projectArray[i].ProjectName + "</option>";

    };

    var strWorkType = "";
    for (i = 0; i < workTypeArray.length; i++) {

        strWorkType += "<option value='" + workTypeArray[i].WorkTypeId + "'>" + workTypeArray[i].WorkTypeName + "</option>";

    };

    var html = "<div class='projectRow_" + productRowCount + " divTableRow'>" +
        "<div class='row'>" +
        "<div class='col-md-2 divTableCell'>" +
        "<select class='form-control clsSelectProject ddValue ProjectId' name='selectProject_" + productRowCount + "'><option value=''>Select Project</option>" +
        projecrList +
        "</select > " +
        "</div>" +
        "<div class='col-md-2 divTableCell'>" +
        "<select class='form-control clsSelectProjectType ddValue ProjectItemId' name='selectProjectType_" + productRowCount + "'><option value=''>Select Project Item</option></select>" +
        "</div>" +
        "<div class='col-md-2 divTableCell'>" +
        "<select class='form-control clsSelectWorkType ddValue WorkTypeId' name='selectWorkType_" + productRowCount + "'>" +
        "<option value=''>Select Work Type</option>" +
        strWorkType +
        "</select > " +
        "</div>" +
        "<div class='col-md-1 divTableCell'><input class='form-control workitem ddValue readValue' name='WorkItem_" + productRowCount + "' type='text' disabled='disabled' placeholder='ALM&#92;INC here' /></div>" +
        "<div class='col-md-1 divTableCell'>" +
        "<input class='form-control hour clsSunday readValue Sunday allowNumericWithDecimal' maxlength='5' value='0' name='txtSunday_" + productRowCount + "'  type='text'>" +
        "</div>" +
        "<div class='col-md-1 divTableCell'>" +
        "<input class='form-control hour clsMonday readValue Monday allowNumericWithDecimal' maxlength='5' value='0' name='txtMonday_" + productRowCount + "' type='text' >" +
        "</div>" +
        "<div class='col-md-1 divTableCell'>" +
        "<input class='form-control hour clsTuesday readValue Tuesday allowNumericWithDecimal' maxlength='5' value='0' name='txtTuesday_" + productRowCount + "' type='text'>" +
        "</div>" +
        "<div class='col-md-1 divTableCell'>" +
        "<input class='form-control hour clsWednesday readValue Wednesday allowNumericWithDecimal' maxlength='5' value='0' name='txtWednesday_" + productRowCount + "'  type='text'>" +
        "</div>" +
        "<div class='col-md-1 divTableCell'>" +
        "<input class='form-control hour clsThursday readValue Thursday allowNumericWithDecimal' maxlength='5' value='0' name='txtThursday_" + productRowCount + "'  type='text'>" +
        "</div>" +
        " <div class='col-md-1 divTableCell'>" +
        "<input class='form-control hour clsFriday readValue Friday allowNumericWithDecimal' maxlength='5' value='0' name='txtFriday_" + productRowCount + "''  type='text'>" +
        "</div>" +
        "<div class='col-md-1 divTableCell'>" +
        "<input class='form-control hour clsSaturday readValue Saturday allowNumericWithDecimal'  maxlength='5' value='0' name='txtSaturday_" + productRowCount + "' type='text'>" +
        "</div>" +
        "<div class='col-md-1 divTableCell'>" +
        "<input class='form-control hour Total readValue' data-val='true' data-val-number='The field texttotal_p1 must be a number.' data-val-regex='Enter Only Numbers' data-val-regex-pattern='^\d+$' id='texttotal_p1' maxlength='5' name='texttotal_p1' readonly='readonly' type='text' value='0'>" +
        "</div>" +


        "<div class='col-md-1' style='padding-left: 5px; height: 30px; padding-top: 5px'>" +
        "<button type='button' class='btn btn-default btn-sm rowSaveClick' id='btnSaverow'><span class='glyphicon glyphicon-floppy-save'></span></button> " +
        "<button type = 'button' class='btn btn-default btn-sm rowDeleteClick' id = 'btnDeleterow' > <span class='glyphicon glyphicon-remove'></span></button>" +
        "</div>" +


        "</div>" +
        "<div style='margin:10px'></div>" +
        "</div>";


    $(".parentProject").append(html);
    $("input:text").focus(function () { $(this).select(); });

    var workTypectrl = 'selectWorkType_' + productRowCount;
    if (tempTeamWorkType != undefined && tempTeamWorkType != '')
        $('select[name=' + workTypectrl + ']').val(tempTeamWorkType);

    if (teamCode != null && teamCode == 'PS') {
        var ctrl = 'selectProject_' + productRowCount;
        $('[name=' + ctrl + '] option').filter(function () {
            return ($(this).text() == 'Production Support'); //To select Blue
        }).prop('selected', true);

        $('[name=' + ctrl + ']').trigger("change");

        //var workItemctrl = 'WorkItem_' + productRowCount;
        //alert($('[name=' + workItemctrl + ']'));
        //if (workItemctrl != null)
        //    $('[name=' + workItemctrl + ']').prop("disabled", false);
        //else
        //    $('[name=' + workItemctrl + ']').prop("disabled", true);
    }

    $(".allowNumericWithDecimal").on("keypress keyup blur", function (event) {
        //this.value = this.value.replace(/[^0-9\.]/g,'');
        $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
        if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });


    $(".allowNumericWithComma").on("keypress keyup blur", function (event) {
        var theEvent = event || window.event;
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
        var regex = /[^0-9]+$/;
        if (regex.test(key)) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault) theEvent.preventDefault();
        }
    });

    $("select").click(function () {
        $('#commentSection').hide();
    });

    $(".clsSunday").each(function () {
        $(this).rules('add', {
            //required: true,
            max: 23.59,
            number: true,
            maxlength: 5

        });
    });

    $(".clsMonday").each(function () {
        $(this).rules('add', {
            //required: true,
            maxlength: 5,
            max: 23.59,
            number: true
        });
    });
    $(".clsTuesday").each(function () {
        $(this).rules('add', {
            //required: true,
            maxlength: 5,
            max: 23.59,
            number: true
        });
    });
    $(".clsWednesday").each(function () {
        $(this).rules('add', {
            //required: true,
            maxlength: 5,
            max: 23.59,
            number: true
        });
    });
    $(".clsThursday").each(function () {
        $(this).rules('add', {
            //required: true,
            maxlength: 5,
            max: 23.59,
            number: true
        });
    });
    $(".clsFriday").each(function () {
        $(this).rules('add', {
            //required: true,
            maxlength: 5,
            max: 23.59,
            number: true
        });
    });
    $(".clsSaturday").each(function () {
        $(this).rules('add', {
            //required: true,
            maxlength: 5,
            max: 23.59,
            number: true

        });
    });


    $(".clsSelectProject").each(function () {
        var ctrl = 'selectProject_' + productRowCount;
        $(this).rules('add', {
            required: true,
            messages: {
                ctrl: "Please fix the error(s) highlighted in red"
            }
        });
    });

    $(".clsSelectWorkType").each(function () {
        var ctrl = 'selectWorkType_' + productRowCount;
        $(this).rules('add', {
            required: true,
            messages: {
                ctrl: "Please fix the error(s) highlighted in red"
            }
        });
    });

    $(".clsSelectProjectType").each(function () {
        var ctrl = 'selectProjectType_' + productRowCount;
        $(this).rules('add', {
            required: true,
            messages: {
                ctrl: "Please fix the error(s) highlighted in red"
            }
        });
    });

    $("select").click(function () {
        $('#commentSection').hide();
    });

    productRowCount++;

    BindTotalVertical('');
}

function CreateBATemplate() {
    $('.divTableBody').empty();
    var html = "<div class='col-md-2' style='display: none' style='text-align: center; padding-top: 25px;'><label for='Application'>Application</label></div><div class='col-md-2' style='text-align: center; padding-top: 25px;'><label for='Project'>Project</label></div><div class='col-md-2 ' style='text-align: center; padding-top: 25px;'><label for='Work_Type'>Work Type</label></div><div class='col-md-1 divTableCell'><label id='label1'></label><input type='hidden' id='hdtext1'><br><label id='date1'></label></div><div class='col-md-1 divTableCell'><label id='label2'></label><input type='hidden' id='hdtext2'><br><label id='date2'></label></div><div class='col-md-1 divTableCell'><label id='label3'></label><input type='hidden' id='hdtext3'><br><label id='date3'></label></div><div class='col-md-1 divTableCell'><label id='label4'></label><input type='hidden' id='hdtext4'><br><label id='date4'></label></div><div class='col-md-1 divTableCell'><label id='label5'></label><input type='hidden' id='hdtext5'><br><label id='date5'></label></div><div class='col-md-1 divTableCell'><label id='label6'></label><input type='hidden' id='hdtext6'><br><label id='date6'></label></div><div class='col-md-1 divTableCell'><label id='label7'></label><input type='hidden' id='hdtext7'><br><label id='date7'></label></div><div class='col-md-1 divTableCell'><label for='Total'>Total</label><br><label for='Hours'>Hours</label></div><div class='col-md - 2'></div >";
    $('.divTableBody').html(html);
}

function CreateTemplate() {
    $('.divTableBody').empty();
    var html = "<div class='col-md-2' style='text-align: center; padding-top: 25px;'><label for='Project'>Project</label></div><div class='col-md-2' style='text-align: center; padding-top: 25px;'><label for='Project_Item'>Project Item</label></div><div class='col-md-2 ' style='text-align: center; padding-top: 25px;'><label for='Work_Type'>Work Type</label></div><div class='col-md-1 ' style='text-align: center; padding-top: 25px;'><label for='Work_Item'>Work Item</label></div><div class='col-md-1 divTableCell'><label id='label1'></label><input type='hidden' id='hdtext1'><br><label id='date1'></label></div><div class='col-md-1 divTableCell'><label id='label2'></label><input type='hidden' id='hdtext2'><br><label id='date2'></label></div><div class='col-md-1 divTableCell'><label id='label3'></label><input type='hidden' id='hdtext3'><br><label id='date3'></label></div><div class='col-md-1 divTableCell'><label id='label4'></label><input type='hidden' id='hdtext4'><br><label id='date4'></label></div><div class='col-md-1 divTableCell'><label id='label5'></label><input type='hidden' id='hdtext5'><br><label id='date5'></label></div><div class='col-md-1 divTableCell'><label id='label6'></label><input type='hidden' id='hdtext6'><br><label id='date6'></label></div><div class='col-md-1 divTableCell'><label id='label7'></label><input type='hidden' id='hdtext7'><br><label id='date7'></label></div><div class='col-md-1 divTableCell'><label for='Total'>Total</label><br><label for='Hours'>Hours</label></div><div class='col-md - 2'></div >";
    $('.divTableBody').html(html);
}

function CrerateBAProjectRow() {
    var projecrList = "";
    for (i = 0; i < projectArray.length; i++) {

        projecrList += "<option value='" + projectArray[i].ProjectId + "'>" + projectArray[i].ProjectName + "</option>";

    };

    var strWorkType = "";
    for (i = 0; i < workTypeArray.length; i++) {

        strWorkType += "<option value='" + workTypeArray[i].WorkTypeId + "'>" + workTypeArray[i].WorkTypeName + "</option>";

    };

    var html = "<div class='projectRow_" + productRowCount + " divTableRow'>" +
        "<div class='row'>" +
        "<div class='col-md-2 divTableCell' style='display: none'>" +
        "<select class='form-control clsSelectApplication ddValue ApplicationId'  title=Select Application' name='selectApplication_" + productRowCount + "'><option value=''>Select Application</option>" +
        projecrList +
        "</select > " +
        "</div>" +
        "<div class='col-md-2 divTableCell'>" +
        "<select class='form-control clsSelectProjectType ddValue ProjectId'  title='Select Project' name='selectProjectType_" + productRowCount + "'><option value=''>Select Project</option>" + projecrList + "</select>" +
        "</div>" +
        "<div class='col-md-2 divTableCell'>" +
        "<select class='form-control clsSelectWorkType ddValue WorkTypeId' title='Select WorkType' name='selectWorkType_" + productRowCount + "'>" +
        "<option value=''>Select Work Type</option>" +
        strWorkType +
        "</select > " +
        "</div>" +
        "<div class='col-md-1 divTableCell'>" +
        "<input class='form-control hour clsSunday readValue Sunday allowNumericWithDecimal' maxlength='5' value='0' name='txtSunday_" + productRowCount + "'  type='text'>" +
        "</div>" +
        "<div class='col-md-1 divTableCell'>" +
        "<input class='form-control hour clsMonday readValue Monday allowNumericWithDecimal' maxlength='5' value='0' name='txtMonday_" + productRowCount + "' type='text' >" +
        "</div>" +
        "<div class='col-md-1 divTableCell'>" +
        "<input class='form-control hour clsTuesday readValue Tuesday allowNumericWithDecimal' maxlength='5' value='0' name='txtTuesday_" + productRowCount + "' type='text'>" +
        "</div>" +
        "<div class='col-md-1 divTableCell'>" +
        "<input class='form-control hour clsWednesday readValue Wednesday allowNumericWithDecimal' maxlength='5' value='0' name='txtWednesday_" + productRowCount + "'  type='text'>" +
        "</div>" +
        "<div class='col-md-1 divTableCell'>" +
        "<input class='form-control hour clsThursday readValue Thursday allowNumericWithDecimal' maxlength='5' value='0' name='txtThursday_" + productRowCount + "'  type='text'>" +
        "</div>" +
        " <div class='col-md-1 divTableCell'>" +
        "<input class='form-control hour clsFriday readValue Friday allowNumericWithDecimal' maxlength='5' value='0' name='txtFriday_" + productRowCount + "''  type='text'>" +
        "</div>" +
        "<div class='col-md-1 divTableCell'>" +
        "<input class='form-control hour clsSaturday readValue Saturday allowNumericWithDecimal'  maxlength='5' value='0' name='txtSaturday_" + productRowCount + "' type='text'>" +
        "</div>" +
        "<div class='col-md-1 divTableCell'>" +
        "<input class='form-control hour Total readValue' data-val='true' data-val-number='The field texttotal_p1 must be a number.' data-val-regex='Enter Only Numbers' data-val-regex-pattern='^\d+$' id='texttotal_p1' maxlength='5' name='texttotal_p1' readonly='readonly' type='text' value='0'>" +
        "</div>" +
        "<div class='col-md-1' style='width: 20px; padding-left: 5px; height: 30px; padding-top: 5px'>" +
        "<button type='button' class='btn btn-default btn-sm' id='btnDeleterow'><span class='glyphicon glyphicon-remove'></span> Remove</button>" +
        "</div>" +
        "</div>" +
        "<div style='margin:10px'></div>" +
        "</div>";


    $(".parentProject").append(html);
    $("input:text").focus(function () { $(this).select(); });
    productRowCount++;

    $(".allowNumericWithDecimal").on("keypress keyup blur", function (event) {
        //this.value = this.value.replace(/[^0-9\.]/g,'');
        $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
        if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });

    $(".allowNumericWithComma").on("keypress keyup blur", function (event) {
        var theEvent = event || window.event;
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
        var regex = /[^0-9]+$/;
        if (regex.test(key)) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault) theEvent.preventDefault();
        }
    });

    $(".clsSunday").each(function () {
        $(this).rules('add', {
            //required: true,
            max: 23.59,
            number: true,
            maxlength: 5

        });
    });

    $(".clsMonday").each(function () {
        $(this).rules('add', {
            //required: true,
            maxlength: 5,
            max: 23.59,
            number: true
        });
    });
    $(".clsTuesday").each(function () {
        $(this).rules('add', {
            //required: true,
            maxlength: 5,
            max: 23.59,
            number: true
        });
    });
    $(".clsWednesday").each(function () {
        $(this).rules('add', {
            //required: true,
            maxlength: 5,
            max: 23.59,
            number: true
        });
    });
    $(".clsThursday").each(function () {
        $(this).rules('add', {
            //required: true,
            maxlength: 5,
            max: 23.59,
            number: true
        });
    });
    $(".clsFriday").each(function () {
        $(this).rules('add', {
            //required: true,
            maxlength: 5,
            max: 23.59,
            number: true
        });
    });
    $(".clsSaturday").each(function () {
        $(this).rules('add', {
            //required: true,
            maxlength: 5,
            max: 23.59,
            number: true

        });
    });

    $(".clsSelectWorkType").each(function () {
        var ctrl = 'selectWorkType_' + productRowCount;
        $(this).rules('add', {
            required: true,
            messages: {
                ctrl: "Please fix the error(s) highlighted in red"
            }
        });
    });

    $(".clsSelectProjectType").each(function () {
        var ctrl = 'selectProjectType_' + productRowCount;
        $(this).rules('add', {
            required: true,
            messages: {
                ctrl: "Please fix the error(s) highlighted in red"
            }
        });
    });
}


function GetAllApplication() {



    $.ajax({
        type: "Get",
        //url: "/SBS.IT.Utilities.Web.TimeTrackerWeb/timesheet/GetAllApplication",
        //url: "/TimeTracker/timesheet/GetAllApplication",
        url: "GetAllApplication",
        dataType: "json",
        cache: false,
        async: false,
        contentType: "application/json",

        success: function (successData) {
            for (i = 0; i < successData.length; i++) {
                var projectList = { "ApplicationId": successData[i].ApplicationId, "ApplicationName": successData[i].ApplicationName, "ApplicationCode": successData[i].ApplicationCode, "Description": successData[i].Description };
                applicationArray.push(projectList);



            }
            closeModal();
        },
        error: function (error) {

        }
    });






}


function GetProjectType(applicationId) {

    var response = null;
    $.ajax({
        type: "Get",
        //url: "/TimeTracker/timesheet/GetAllProjectItem?applicationId=" + applicationId+"",
        //url: "/SBS.IT.Utilities.Web.TimeTrackerWeb/timesheet/GetAllProjectItem?applicationId=" + applicationId + "",
        url: "GetAllProjectItembyProjectId?projectId=" + applicationId + "",
        dataType: "json",
        cache: false,
        async: false,
        contentType: "application/json",

        success: function (successData) {
            response = successData;

            //for (i = 0; i < successData.length; i++) {
            //    var projectType = { "ProjectId": successData[i].ProjectId, "ProjectName": successData[i].ProjectName,  "ProjectDescription": successData[i].Description };
            //    projectArray.push(projectType);

            //    return successData



            //}
            closeModal();
        },
        error: function (error) {

        }
    });

    return response;






}




function GetAllWorkType() {

    $.ajax({
        type: "Get",
        //url: "/TimeTracker/timesheet/GetAllWorkType",
        //url: "/SBS.IT.Utilities.Web.TimeTrackerWeb/timesheet/GetAllWorkType",
        url: "GetAllWorkType",
        dataType: "json",
        contentType: "application/json",
        cache: false,
        async: false,
        success: function (successData) {
            for (i = 0; i < successData.length; i++) {
                var workType = { "WorkTypeId": successData[i].WorkTypeId, "WorkTypeName": successData[i].WorkTypeName, "WorkTypeCode": successData[i].WorkTypeCode, "Description": successData[i].Description };
                workTypeArray.push(workType);



            }
            closeModal();
        },
        error: function (error) {

        }
    });
}

function GetAllEmployee() {
    $.ajax({
        type: "Get",
        url: "GetAllEmployee",
        dataType: "json",
        contentType: "application/json",
        cache: false,
        async: false,
        success: function (successData) {
            for (i = 0; i < successData.length; i++) {
                var employee = { "EmployeeId": successData[i].EmployeeId, "EmployeeName": successData[i].EmployeeName };
                employeeArray.push(employee);
            }
            closeModal();
        },
        error: function (error) {

        }
    });
}

function BindInitialSelectBox() {

    var applicationList = "";
    for (i = 0; i < applicationArray.length; i++) {

        applicationList += "<option value='" + applicationArray[i].ApplicationId + "'>" + applicationArray[i].ApplicationName + "</option>";

    };

    var strWorkType = "";
    for (i = 0; i < workTypeArray.length; i++) {

        strWorkType += "<option value='" + workTypeArray[i].WorkTypeId + "'>" + workTypeArray[i].WorkTypeName + "</option>";

    };

    var strEmployee = "";
    for (i = 0; i < employeeArray.length; i++) {

        strEmployee += "<option value='" + employeeArray[i].EmployeeId + "'>" + employeeArray[i].EmployeeName + "</option>";

    };

    var strProject = "";
    for (i = 0; i < projectArray.length; i++) {

        strProject += "<option value='" + projectArray[i].ProjectId + "'>" + projectArray[i].ProjectName + "</option>";

    };

    $("#selectWorkType ").empty();

    $("#selectProject").append(strProject);
    $("#selectWorkType").append(strWorkType);
    $("#selectEmployee").append(strEmployee);
    if (tempuserId != null && tempuserId != undefined && tempuserId != '')
        $('#selectEmployee').val(tempuserId);

}

function initializeGrid() {

    var selectedIndex = GetSelectDayRecordIndex(selectedApplicationId, selectedProjectTypeId, selectedWorkTypeId);
    var selectedRecord = projectHoursArry[selectedIndex];
    if (selectedRecord == null || selectedRecord == 'null' || selectedRecord == undefined) {
        if ($("#TimeSheetForm").valid()) {
            var projectRecord = { "applicationId": selectedApplicationId, "ProjectTypeId": selectedProjectTypeId, "WorkTypeId": selectedWorkTypeId, "Sun": null, "Mon": null, "Tue": null, "Wed": null, "Thu": null, "Fri": null, "Sat": null };
        }
        projectHoursArry.push(projectRecord);
        selectedIndex = GetSelectDayRecordIndex(selectedApplicationId, selectedProjectTypeId, selectedWorkTypeId)
    }
    selectedRecord = projectHoursArry[selectedIndex];
    return selectedRecord;
}




function ToggleDayCommentTextArea(selectedDay, record, control) {
    $("#txtSunALM").hide();
    $("#txtMonALM").hide();
    $("#txtTueALM").hide();
    $("#txtWedALM").hide();
    $("#txtThuALM").hide();
    $("#txtFriALM").hide();
    $("#txtSatALM").hide();
    $("#txtAreaSat").hide();
    $("#txtAreaSun").hide();
    $("#txtAreaMon").hide();
    $("#txtAreaTue").hide();
    $("#txtAreaWed").hide();
    $("#txtAreaThu").hide();
    $("#txtAreaFri").hide();

    var updateVal = $(control).attr("alt");
    var almValue = $(control).attr("almId");
    var projectItem = $(control).parent().parent().children().find(".clsSelectProjectType option:selected").text();

    switch (selectedDay) {
        case "clsSunday":
            selectedHours = "sun";
            if (updateVal != null && updateVal != '' && updateVal != undefined && updateVal != 'null')
                $("#txtAreaSun").val(updateVal);
            else {
                if (record == undefined) {
                    $("#txtAreaSun").val(null);
                } else {

                    if (record.Sun != null) {
                        $("#txtAreaSun").val(record.Sun.comment);
                    }
                }
            }
            $("#txtAreaSun").show();
            //if (projectItem != null && projectItem != undefined) {
            //    if (projectItem.toLowerCase().indexOf(ALMprojectType) != -1) {
            //        $("#txtSunALM").show();
            //        $("#txtSunALM").attr('maxlength', '4');
            //        $("#txtSunALM").addClass("allowNumericWithComma");
            //    }
            //    else if (projectItem.toLowerCase().indexOf(productionProjectType) != -1) {
            //        $("#txtSunALM").show();
            //        $("#txtSunALM").attr('maxlength', '4');
            //        $("#txtSunALM").removeClass('allowNumericWithComma');
            //    }
            //}

            if (almValue != null && almValue != '' && almValue != undefined && almValue != 'null')
                $("#txtSunALM").val(almValue);
            else
                $("#txtSunALM").val(null);

            $("#txtAreaSat").hide();
            $("#txtAreaMon").hide();
            $("#txtAreaTue").hide();
            $("#txtAreaWed").hide();
            $("#txtAreaThu").hide();
            $("#txtAreaFri").hide();

            //Hide Alm box
            $("#txtMonALM").hide();
            $("#txtTueALM").hide();
            $("#txtWedALM").hide();
            $("#txtThuALM").hide();
            $("#txtFriALM").hide();
            $("#txtSatALM").hide();
            break;
        case "clsMonday":

            selectedHours = "mon";
            if (updateVal != null && updateVal != '' && updateVal != undefined && updateVal != 'null')
                $("#txtAreaMon").val(updateVal);
            else {
                if (record == undefined) {
                    $("#txtAreaMon").val(null);
                }

                else {

                    if (record.Mon != null) {
                        $("#txtAreaMon").val(record.Mon.comment);
                    }
                }
            }
            $("#txtAreaSun").hide();
            $("#txtAreaSat").hide();
            $("#txtAreaMon").show();
            $("#txtAreaTue").hide();
            $("#txtAreaWed").hide();
            $("#txtAreaThu").hide();
            $("#txtAreaFri").hide();

            //if (projectItem != null && projectItem != undefined) {
            //    if (projectItem.toLowerCase().indexOf(ALMprojectType) != -1) {
            //        $("#txtMonALM").show();
            //        $("#txtMonALM").attr('maxlength', '4');
            //        $("#txtMonALM").addClass("allowNumericWithComma");
            //    }
            //    else if (projectItem.toLowerCase().indexOf(productionProjectType) != -1) {
            //        $("#txtMonALM").show();
            //        $("#txtMonALM").attr('maxlength', '4');
            //        $("#txtMonALM").removeClass('allowNumericWithComma');
            //    }
            //}


            if (almValue != null && almValue != '' && almValue != undefined && almValue != 'null')
                $("#txtMonALM").val(almValue);
            else
                $("#txtMonALM").val(null);

            //Hide Alm box
            $("#txtSunALM").hide();
            $("#txtTueALM").hide();
            $("#txtWedALM").hide();
            $("#txtThuALM").hide();
            $("#txtFriALM").hide();
            $("#txtSatALM").hide();
            break;

        case "clsTuesday":
            selectedHours = "tue";

            if (updateVal != null && updateVal != '' && updateVal != undefined && updateVal != 'null')
                $("#txtAreaTue").val(updateVal);
            else {
                if (record == undefined) {
                    $("#txtAreaTue").val(null);
                }

                else {

                    if (record.Tue != null) {
                        $("#txtAreaTue").val(record.Tue.comment);
                    }
                }
            }
            //if (!control.readOnly) {
            $("#txtAreaTue").show();
            //}
            $("#txtAreaSun").hide();
            $("#txtAreaSat").hide();
            $("#txtAreaMon").hide();

            $("#txtAreaWed").hide();
            $("#txtAreaThu").hide();
            $("#txtAreaFri").hide();
            //if (projectItem != null && projectItem != undefined) {
            //    if (projectItem.toLowerCase().indexOf(ALMprojectType) != -1) {
            //        $("#txtTueALM").show();
            //        $("#txtTueALM").attr('maxlength', '4');
            //        $("#txtTueALM").addClass("allowNumericWithComma");
            //    }
            //    else if (projectItem.toLowerCase().indexOf(productionProjectType) != -1) {
            //        $("#txtTueALM").show();
            //        $("#txtTueALM").attr('maxlength', '4');
            //        $("#txtTueALM").removeClass('allowNumericWithComma');
            //    }
            //}

            if (almValue != null && almValue != '' && almValue != undefined && almValue != 'null')
                $("#txtTueALM").val(almValue);
            else
                $("#txtTueALM").val(null);

            //Hide Alm box
            $("#txtSunALM").hide();
            $("#txtMonALM").hide();
            $("#txtWedALM").hide();
            $("#txtThuALM").hide();
            $("#txtFriALM").hide();
            $("#txtSatALM").hide();
            break;
        case "clsWednesday":

            selectedHours = "wed";
            if (updateVal != null && updateVal != '' && updateVal != undefined && updateVal != 'null')
                $("#txtAreaWed").val(updateVal);
            else {
                if (record == undefined) {
                    $("#txtAreaWed").val(null);
                }

                else {

                    if (record.Wed != null) {
                        $("#txtAreaWed").val(record.Wed.comment);
                    }
                }
            }
            $("#txtAreaTue").hide();
            $("#txtAreaSun").hide();
            $("#txtAreaSat").hide();
            $("#txtAreaMon").hide();

            //if (!control.readOnly) {
            $("#txtAreaWed").show();
            //}
            $("#txtAreaThu").hide();
            $("#txtAreaFri").hide();
            //if (projectItem != null && projectItem != undefined) {
            //    if (projectItem.toLowerCase().indexOf(ALMprojectType) != -1) {
            //        $("#txtWedALM").show();
            //        $("#txtWedALM").attr('maxlength', '4');
            //        $("#txtWedALM").addClass("allowNumericWithComma");
            //    }
            //    else if (projectItem.toLowerCase().indexOf(productionProjectType) != -1) {
            //        $("#txtWedALM").show();
            //        $("#txtWedALM").attr('maxlength', '4');
            //        $("#txtWedALM").removeClass('allowNumericWithComma');
            //    }
            //}

            if (almValue != null && almValue != '' && almValue != undefined && almValue != 'null')
                $("#txtWedALM").val(almValue);
            else
                $("#txtWedALM").val(null);
            //Hide Alm box
            $("#txtSunALM").hide();
            $("#txtMonALM").hide();
            $("#txtTueALM").hide();
            $("#txtThuALM").hide();
            $("#txtFriALM").hide();
            $("#txtSatALM").hide();
            break;
        case "clsThursday":
            selectedHours = "thu";
            if (updateVal != null && updateVal != '' && updateVal != undefined && updateVal != 'null')
                $("#txtAreaThu").val(updateVal);
            else {
                if (record == undefined) {
                    $("#txtAreaThu").val(null);
                }
                else {

                    if (record.Thu != null) {
                        $("#txtAreaThu").val(record.Thu.comment);
                    }
                }
            }
            $("#txtAreaTue").hide();
            $("#txtAreaSun").hide();
            $("#txtAreaSat").hide();
            $("#txtAreaMon").hide();
            $("#txtAreaWed").hide();
            //if (!control.readOnly) {
            $("#txtAreaThu").show();
            //}
            $("#txtAreaFri").hide();

            //if (projectItem != null && projectItem != undefined) {
            //    if (projectItem.toLowerCase().indexOf(ALMprojectType) != -1) {
            //        $("#txtThuALM").show();
            //        $("#txtThuALM").attr('maxlength', '4');
            //        $("#txtThuALM").addClass("allowNumericWithComma");
            //    }
            //    else if (projectItem.toLowerCase().indexOf(productionProjectType) != -1) {
            //        $("#txtThuALM").show();
            //        $("#txtThuALM").attr('maxlength', '4');
            //        $("#txtThuALM").removeClass('allowNumericWithComma');
            //    }
            //}

            if (almValue != null && almValue != '' && almValue != undefined && almValue != 'null')
                $("#txtThuALM").val(almValue);
            else
                $("#txtThuALM").val(null);

            //Hide Alm box
            $("#txtSunALM").hide();
            $("#txtMonALM").hide();
            $("#txtTueALM").hide();
            $("#txtWedALM").hide();
            $("#txtFriALM").hide();
            $("#txtSatALM").hide();
            break;
        case "clsFriday":
            selectedHours = "fri";
            if (updateVal != null && updateVal != '' && updateVal != undefined && updateVal != 'null')
                $("#txtAreaFri").val(updateVal);
            else {
                if (record == undefined) {
                    $("#txtAreaFri").val(null);
                }
                else {

                    if (record.Fri != null) {
                        $("#txtAreaFri").val(record.Fri.comment);
                    }
                }
            }
            $("#txtAreaTue").hide();
            $("#txtAreaSun").hide();
            $("#txtAreaSat").hide();
            $("#txtAreaMon").hide();
            $("#txtAreaWed").hide();
            $("#txtAreaThu").hide();
            //if (!control.readOnly) {
            $("#txtAreaFri").show();
            //}

            //if (projectItem != null && projectItem != undefined) {
            //    if (projectItem.toLowerCase().indexOf(ALMprojectType) != -1) {
            //        $("#txtFriALM").show();
            //        $("#txtFriALM").attr('maxlength', '4');
            //        $("#txtFriALM").addClass("allowNumericWithComma");
            //    }
            //    else if (projectItem.toLowerCase().indexOf(productionProjectType) != -1) {
            //        $("#txtFriALM").show();
            //        $("#txtFriALM").attr('maxlength', '4');
            //        $("#txtFriALM").removeClass('allowNumericWithComma');
            //    }
            //}

            if (almValue != null && almValue != '' && almValue != undefined && almValue != 'null')
                $("#txtFriALM").val(almValue);
            else
                $("#txtFriALM").val(null);

            //Hide Alm box
            $("#txtSunALM").hide();
            $("#txtMonALM").hide();
            $("#txtTueALM").hide();
            $("#txtWedALM").hide();
            $("#txtThuALM").hide();
            $("#txtSatALM").hide();

            break;
        case "clsSaturday":
            selectedHours = "sat";
            if (updateVal != null && updateVal != '' && updateVal != undefined && updateVal != 'null')
                $("#txtAreaSat").val(updateVal);
            else {
                if (record == undefined) {
                    $("#txtAreaSat").val(null);
                }
                else {

                    if (record.Sat != null) {
                        $("#txtAreaSat").val(record.Sat.comment);
                    }
                }
            }

            //if (!control.readOnly) {
            $("#txtAreaSat").show();
            // };
            $("#txtAreaSun").hide();
            $("#txtAreaMon").hide();
            $("#txtAreaTue").hide();
            $("#txtAreaWed").hide();
            $("#txtAreaThu").hide();
            $("#txtAreaFri").hide();

            //if (projectItem != null && projectItem != undefined) {
            //    if (projectItem.toLowerCase().indexOf(ALMprojectType) != -1) {
            //        $("#txtSatALM").show();
            //        $("#txtSatALM").attr('maxlength', '4');
            //        $("#txtSatALM").addClass("allowNumericWithComma");
            //    }
            //    else if (projectItem.toLowerCase().indexOf(productionProjectType) != -1) {
            //        $("#txtSatALM").show();
            //        $("#txtSatALM").attr('maxlength', '4');
            //        $("#txtSatALM").removeClass('allowNumericWithComma');
            //    }
            //}

            if (almValue != null && almValue != '' && almValue != undefined && almValue != 'null')
                $("#txtSatALM").val(almValue);
            else
                $("#txtSatALM").val(null);

            //Hide Alm box
            $("#txtSunALM").hide();
            $("#txtMonALM").hide();
            $("#txtTueALM").hide();
            $("#txtWedALM").hide();
            $("#txtThuALM").hide();
            $("#txtFriALM").hide();
            break;

    }
}

function GetProject() {

    $.ajax({
        type: "Get",
        url: "getProjectList",
        dataType: "json",
        contentType: "application/json",
        cache: false,
        async: false,
        success: function (successData) {
            for (i = 0; i < successData.length; i++) {
                var projectList = { "ProjectId": successData[i].ProjectId, "ProjectName": successData[i].ProjectName };
                projectArray.push(projectList);
            }
            closeModal();
        },
        error: function (error) {
        }
    });
}

function deleteRow(timeentryIds) {

    $.ajax({
        type: "Get",
        url: "DeleteRows?timeentryIds=" + timeentryIds,
        dataType: "json",
        contentType: "application/json",
        cache: false,
        async: false,
        success: function (successData) {
            if (successData && successData.success === false) {
                closeModal();
                swal("Error!", successData.error || "Failed to delete time entries. Please try again.", "error");
                return;
            }
            if (successData != null && successData != undefined && successData != "") {
                swal({
                    title: "Success!",
                    text: "Record(s) deleted successfully!",
                    type: "success",
                    icon: "success",
                }).then(function () {
                    // Redirect the user
                    //window.location.href = "WeeklyTimeSheet";

                });
            }
            closeModal();
        },
        error: function (error) {
            closeModal();
            swal("Error!", "Failed to delete time entries. Please try again.", "error");
        }
    });
}
function BindTotalVertical(currentControlClass) {
    if (currentControlClass == '') {
        var arrClasses = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
        arrClasses.forEach(function (item) {
            BindTotalVertical(item);
        });
    }
    else {
        var totalHrsColumn = 0;
        $("input." + currentControlClass + ":text").each(function (index, element) {
            if (element.value != "") {
                var hr = parseFloat(element.value);
                if (isNaN(hr)) {
                    hr = 0;
                }
                totalHrsColumn += hr;
            }
        });
        $('#txt' + currentControlClass + "Total").val(totalHrsColumn);
    }
}