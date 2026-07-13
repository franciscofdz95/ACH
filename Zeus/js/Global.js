// JScript File

// used to identify the app. very useful when you're setting cookie that are specific to different apps.
var G_APP_NAME = "ZEUS";

function Field2Str(fieldvalue) {
    if (fieldvalue == null) {
        return '';
    }
    else {
        return fieldvalue;
    }
}

function OpenNewWindow(page) {
    window.open(page, null, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no');
}

// opens a new ticket window with data from exiistng ticket
function CopyTicket(ticketid) {

    var randomno = Math.floor((Math.random() * 10000) + 1);
    if (ticketid != '0')
        window.open('../SecureTicketForms/frmTicketPopup.aspx?WindowCallID=' + randomno + '&Adding=true&CopyTicketUID=' + ticketid, randomno, 'width=1200,height=650, toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no');
}

// opens a new ticket
//function openticket() {
function OpenTicket() {

    var randomno = Math.floor((Math.random() * 10000) + 1);
    window.open('../SecureTicketForms/frmTicketPopup.aspx?WindowCallID=' + randomno + '&Adding=true', randomno, 'width=1200,height=650, toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no');
}

// opens an existing ticket
//function Openticket(ticketid) {
function OpenTicket(ticketid) {

    // assign a random number to the window ID so that it does not replace itself.
    var randomno = Math.floor((Math.random() * 10000) + 1);
    if (ticketid != '0')
        window.open('../SecureTicketForms/frmTicketPopup.aspx?WindowCallID=' + randomno + '&Adding=false&TicketUID=' + ticketid, randomno, 'width=1200,height=650, toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no');
}

function NewTicketAgent(agentuid) {

    var randomno = Math.floor((Math.random() * 10000) + 1);

    if (agentuid != '') {
        window.open('../SecureTicketForms/frmTicketPopup.aspx?Adding=true&WindowCallID=' + randomno + '&RequestOrigin=Agent&AgentUID=' + agentuid, randomno, 'width=1200,height=650, toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no');
    }
}


function NewTicketMerchant(merchantappuid) {

    var randomno = Math.floor((Math.random() * 10000) + 1);

    if (merchantappuid != '') {
        window.open('../SecureTicketForms/frmTicketPopup.aspx?Adding=true&WindowCallID=' + randomno + '&RequestOrigin=Merchant&MerchantAppUID=' + merchantappuid, randomno, 'width=1200,height=650, toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no');
    }

}



function force_min() {

    var minimized_elements = $('p.minimize');

    minimized_elements.each(function () {

        var mymax = 120;

        var t = $(this).text();

        if (t.length < mymax) {
            return;
        }

        $(this).html(t.slice(0, mymax) + '<span>... </span><a href="#" class="more">More</a><span style="display:none;">' + t.slice(mymax, t.length) + ' <a href="#" class="less">Less</a></span>');

    });

    $('a.more', minimized_elements).click(function (event) {
        event.preventDefault();
        $(this).hide().prev().hide();
        $(this).next().show();
    });

    $('a.less', minimized_elements).click(function (event) {
        event.preventDefault();
        $(this).parent().hide().prev().show().prev().show();
    });
}

function ToggleHeadMoreLess(myobj, event, myparentid) {

    var moreless = $(myobj).text(); // More or Less

    if (moreless == "More") {

        event.preventDefault();
        $(myobj).text("Less");

        $('#' + myparentid).find('.minimize a.more').each(function () {
            $(this).hide().prev().hide();
            $(this).next().show();
        });

    } else if (moreless == "Less") {
        event.preventDefault();

        $(myobj).text("More");
        $('#' + myparentid).find('.minimize a.less').each(function () {
            $(this).parent().hide().prev().show().prev().show();
        });


    }


}

// pulled from: http://blog.stevenlevithan.com/archives/faster-trim-javascript
function trim11(str) {
    str = str.replace(/^\s+/, '');
    for (var i = str.length - 1; i >= 0; i--) {
        if (/\S/.test(str.charAt(i))) {
            str = str.substring(0, i + 1);
            break;
        }
    }
    return str;
}

// pulled from: http://blog.stevenlevithan.com/archives/faster-trim-javascript
function trim1(str) {
    return str.replace(/^\s\s*/, '').replace(/\s\s*$/, '');
}


function timezonechangeHandler(myobj) {
    var val = $(":selected", myobj).text();
    $(myobj).prop('title', val);
}

//jQuery(function () {

//    force_min();

//});
$(document).ready(function () {
    $('.NoCopyPaste').on("cut copy paste", function (e) {
        e.preventDefault();
    });
});

function RemoveSpaces(ClientId) {
    $(ClientId).on("keydown", function (e) {
        if (e.keyCode === 32) {
            return false;
        }
    });
    $(ClientId).on("change", function (e) {
        $(e.target).val(function (i, v) { return v.replace(/ /g, ""); });
    });
}
