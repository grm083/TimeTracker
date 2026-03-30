var windowResizeChartRedrawTimer;
var windowResizeChartRedrawTimerContentWidth;

$(document).ready(function () {
    windowResizeChartRedrawTimerContentWidth = $('.master-wrapper-content').width();
    $(window).resize(function () {
        var newWidth = $('.master-wrapper-content').width();
        if (newWidth != windowResizeChartRedrawTimerContentWidth) {
            windowResizeChartRedrawTimerContentWidth = newWidth;
            window.clearTimeout(windowResizeChartRedrawTimer);
            windowResizeChartRedrawTimer = window.setTimeout(RedrawCharts, 500);
        }
    });
});

function htmlEncode(value) {
    return $('<div/>').text(value).html();
}
function htmlDecode(value) {
    return $('<div/>').html(value).text();
}
function addTopPagerToKendoUIAjaxGrid(name) {
    var grid = $("#" + name).data("kendoGrid");
    var wrapper = $('<div class="k-pager-wrap k-grid-pager pagerTop"/>').insertBefore(grid.element.children("table"));
    grid.pagerTop = new kendo.ui.Pager(wrapper, $.extend({}, grid.options.pageable, { dataSource: grid.dataSource }));
    grid.element.height("").find(".pagerTop").css("border-width", "0 0 1px 0");
}
function selectKendoUIGridFirstRow(name) {
    var grid = $("#" + name).data("kendoGrid");
    row = grid.tbody.find(">tr:not(.k-grouping-row)").eq(0);
    grid.select(row);
}
function AjaxExceptionWindow_Refresh(e) {
    this.center();
}
function AjaxExceptionWindow_Open(e) {
    $('#AjaxExceptionWindow').html('<div class="ChartLoading"></div>');
}
function RedrawCharts(e) {
    $(".k-chart").each(function (i) {
        $(this).data("kendoChart").redraw();
    });
}
$.ajaxPrefilter(function (options, originalOptions, jqXHR) {
    jqXHR.setRequestHeader('__RequestVerificationToken', '@WebUtilityExtension.GenerateRequestVerificationToken()');
});

function showAjaxProgress(innerdata) {
    var maskContainer = $("<div id='mask' />");
    var innerText = "<div class='text-center'><p>Please wait while we are processing your request...</p></div>";
    var innerContainer = $("<div id='mask-inner' />").append(innerdata == '' || innerdata == null ? innerText : innerdata);
    $("body").prepend(maskContainer).prepend(innerContainer);
    $("#mask").addClass("show-mask");
}

function hideAjaxProgress(){
    $("#mask, #mask-inner").remove();
}

function GetURLParameter() {
    var sPageURL = window.location.href;
    var indexOfLastSlash = sPageURL.lastIndexOf("/");

    if (indexOfLastSlash > 0 && sPageURL.length - 1 != indexOfLastSlash)
        return sPageURL.substring(indexOfLastSlash + 1);
    else
        return 0;
}

function DeleteNotificationPopUpWindow_Refresh(e) {
    this.center();
}
function ClearNotifications() {
    $('#notification-error').empty();
    $('#notification-error').hide();
    $('#notification-success').empty();
    $('#notification-success').hide();
}
function displayNotoficationSuccess(messagesArray) {
    var Index;
    if (messagesArray.length > 0) {
        $('#notification-error').empty();
        $('#notification-error').hide();
        $('#notification-success').empty();
        $('#notification-success').append("<ul></ul>");
        for (Index = 0; Index < messagesArray.length; Index++) {
            $('#notification-success > ul').append('<li>' + messagesArray[Index] + '</li>');
        }
        $('#notification-success').show();
        setTimeout(showNotifications, 5000);
    }
}
function showNotifications() {
    ClearNotifications();
}
function displayNotoficationError(messagesArray) {
    var Index;
    if (messagesArray.length > 0) {
        $('#notification-success').empty();
        $('#notification-success').hide();
        $('#notification-error').empty();
        $('#notification-error').append("<ul></ul>");
        for (Index = 0; Index < messagesArray.length; Index++) {
            $('#notification-error > ul').append('<li>' + messagesArray[Index] + '</li>');
        }
        $('#notification-error').show();
    }
}
function displayNotoficationModelError(e) {
    var Index;
    if (e.Errors) {
        var messagesArray = [];
        $.each(e.Errors, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () {
                    messagesArray.push(this);
                });
            }
        });
        if (messagesArray.length > 0) {
            $('#notification-success').empty();
            $('#notification-success').hide();
            $('#notification-error').empty();
            $('#notification-error').append("<ul></ul>");
            for (Index = 0; Index < messagesArray.length; Index++) {
                $('#notification-error > ul').append('<li>' + messagesArray[Index] + '</li>');
            }
            $('#notification-error').show();
        }
    }
}
function displayKendoUIModelErrors(e) {
    if (e.errors) {
        var messagesArray = [];
        $.each(e.errors, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () {
                    messagesArray.push(this);
                });
            }
        });
        displayNotoficationError(messagesArray);
    }
}
function displayPartialViewErrors(ErrorPanelId, e) {
    if (e.Errors) {
        var messagesArray = [];
        $.each(e.Errors, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () {
                    messagesArray.push(this);
                });
            }
        });
        var Index;
        if (messagesArray.length > 0) {
            $('#' + ErrorPanelId).empty();
            $('#' + ErrorPanelId).append("<ul></ul>");
            for (Index = 0; Index < messagesArray.length; Index++) {
                $('#' + ErrorPanelId + ' > ul').append('<li>' + messagesArray[Index] + '</li>');
            }
            $('#' + ErrorPanelId).show();
        }
    }
}
function displayPartialNotoficationError(ErrorPanelId, messagesArray) {
    var Index;
    if (messagesArray.length > 0) {
        $('#' + ErrorPanelId).empty();
        $('#' + ErrorPanelId).append("<ul></ul>");
        for (Index = 0; Index < messagesArray.length; Index++) {
            $('#' + ErrorPanelId + ' > ul').append('<li>' + messagesArray[Index] + '</li>');
        }
        $('#' + ErrorPanelId).show();
    }
}
function displayPopupNotificationSuccess(messagesArray) {
    var Index;
    if (messagesArray.length > 0) {
        $('#notification-success').empty();
        $('#notification-success').hide();
        $('#notification-error').empty();
        $('#notification-error').hide();
        var htmlcode = '';
        for (Index = 0; Index < messagesArray.length; Index++) {
            htmlcode = htmlcode + '<p>' + messagesArray[Index] + '</p>';
        }
        $("#NotificationPopupWindow").data("kendoWindow").title('@Vantive.MainView.Notification_Success_Title').content(htmlcode).center().open();
    }
}
function displayPopupNotificationError(messagesArray) {
    var Index;
    if (messagesArray.length > 0) {
        $('#notification-success').empty();
        $('#notification-success').hide();
        $('#notification-error').empty();
        $('#notification-error').hide();
        var htmlcode = '';
        for (Index = 0; Index < messagesArray.length; Index++) {
            htmlcode = htmlcode + '<p>' + messagesArray[Index] + '</p>';
        }
        $("#NotificationPopupWindow").data("kendoWindow").title('@Vantive.MainView.Notification_Error_Title').content(htmlcode).center().open();
    }
}