var fnAutoComplete = {
    /*
        usage: 
         fnAutoComplete.init(ControlId,AJAXurl,SearchParamName,ParamArray); 

         ControlId: Pass the control Id with # added in it.

         AJAXUrl: it is webmethod url.
         SearchParamName: It is Parameter name is AJAX Method which is used to search.

         ParamArray: It is used to pass extra parameter other than search parameter.
         if no extra parameter then pass [[]]
          if have two or more paramter then send 2-D array of paramerter name and Control id like key value pair.
         refer to page wucCompliance.ascx for Example
         Example:
        fnAutoComplete.init('#<%=CRMName.ClientID%>','../ajax/AjaxWebservice.asmx/GetTPPName','TPPNameFragment',[[]]);
        
    */
    //PXP-8237 Changes Sanidhya
    init: function (tbNameSearch, url, SearchParamName, ParamArray,updateTarget) {
        $(tbNameSearch).typeahead({
            source: function (query, process) {
                var mytags = [];
                if (query != '') {
                    var data = {};
                    if (ParamArray != undefined || ParamArray.length > 0) {
                        for (var i = 0; i < ParamArray.length; i++) {
                            data[ParamArray[i][0]] = $('#'+ParamArray[i][1]).val();
                        }
                    }
                    data[SearchParamName] = query;
                    $.ajax({
                        type: "POST",
                        url: url,
                        data: JSON.stringify(data),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var availableTags = msg.d;                            
                            for (var i = 0; i < availableTags.length; i++) {
                                mytags.push(availableTags[i]);
                            }
                            process(mytags);
                        }
                    });
                } else
                {
                    process(mytags);
                }
            }            ,
            matcher: function (item) {               
                return true;
            },
            updater: function (item) {
                if (updateTarget == true) {
                    return item;
                } else {
                    return "";
                }
               
                
            }
        });
    }
};