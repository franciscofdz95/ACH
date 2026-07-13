var MPSEquipmentAutoComplete = {

    /*
        usage: 

        MPSEquipmentAutoComplete.init("#Type", "#Maker", "#Model", "#ItemUID");



        // this helped me out a lot!!
        http://tatiyants.com/how-to-use-json-objects-with-twitter-bootstrap-typeahead/
    */

    init: function (tbEquipmentSearch, tbType, tbMaker, tbModel, tbItemUID, tbIsNewItem, tbEMVCompliance) {

        var map = {};

        $(tbEquipmentSearch).typeahead({
            source: function (query, process) {

               

                $.ajax({
                    type: "POST",
                    url: "../ajax/AjaxWebservice.asmx/GetEquipment",
                    data: "{ModelFragment: '" + query + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {

                        
                        var availableTags = msg.d;

                        var mytags = [];

                        for (var i = 0; i < availableTags.length; i++) {

                            var arr = availableTags[i].split('|');

                            mytags.push( arr[4] + ":" + arr[1] + ":" + arr[2] + ":" + arr[3]+ ":" + arr[5] );

                            map[arr[4]] = arr;
                        }
 
                        process(mytags);

                    }

                });

                

            },
            matcher: function (item) {
                /*
                if (item.toLowerCase().indexOf(this.query.trim().toLowerCase()) != -1) {
                    return true;
                }
                */

                // we always want to return true because we always want to display the list we get back from google.
                return true;
            },
            updater: function (item) {

                var myid = item.split(':')[0];

                //alert(myid);

                $(tbItemUID).val( map[myid][0] );
                $(tbType).val(map[myid][1]);
                $(tbMaker).val(map[myid][2]);
                $(tbModel).val(map[myid][3]);
                $(tbIsNewItem).val("1");
                $(tbEMVCompliance).val(map[myid][5]);
                
                //return map[myid][3];
                return "";
            }
        });

    }

};