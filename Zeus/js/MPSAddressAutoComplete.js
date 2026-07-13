var MPSAddressAutoComplete = {

    /*
        usage: 

        MPSAddressAutoComplete.init("#ContentPlaceHolder1_WucBusinessInfo1_BusinessAddress",
                                    "#ContentPlaceHolder1_WucBusinessInfo1_BusinessCity",
                                    "#ContentPlaceHolder1_WucBusinessInfo1_BusinessState",
                                    "#ContentPlaceHolder1_WucBusinessInfo1_BusinessZip",
                                    "#ContentPlaceHolder1_WucBusinessInfo1_BusinessMailingCountry"
                                    ,"#ContentPlaceHolder1_WucBusinessInfo1_BusinessMailingProvince");

        MPSAddressAutoComplete.init("#mystreet",
                                    "#mycity",
                                    "#mystate",
                                    "#myzip",
                                    "#mycountry",
                                    "#myProvince");

        // this helped me out a lot!!
        http://tatiyants.com/how-to-use-json-objects-with-twitter-bootstrap-typeahead/
    */

    init: function (tbAddress, tbCity, ddlState, tbZipcode,ddlCountry,tbProvince) {

        var map = {};

        $(tbAddress).typeahead({
            source: function (query, process) {

                /*var goog_req = "http://maps.googleapis.com/maps/api/geocode/json?sensor=false&region=US&components=country:US&address=" + query;*/
                var goog_req = "http://maps.googleapis.com/maps/api/geocode/json?sensor=false&address=" + query;


                var myaddy = [];

                var myvar = $.get(goog_req, function (data) {

                    //var count = 0;
                    $.each(data["results"], function (index, value) {
                        //var cleanaddy = count + " [" + value["formatted_address"].replace("'", " ") + "]";
                        var cleanaddy = value["formatted_address"].replace("'", " ");
                        map[cleanaddy] = value;
                        myaddy.push(cleanaddy);
                        //count = count + 1;
                    });

                   // alert(myaddy);

                    process(myaddy);
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

                var streetnumber = ""; // street_number
                var streetaddress = ""; // route
                var city = ""; //locality
                var state = ""; //administrative_area_level_1
                var zipcode = ""; //postal_code
                var country = "";//country
                var first_result = map[item];

                $.each(first_result["address_components"], function (index, item) {

                    if ($.inArray("street_number", item["types"]) >= 0) {
                        streetnumber = item["short_name"];
                    }

                    if ($.inArray("route", item["types"]) >= 0) {
                        streetaddress = item["short_name"];
                    }

                    if ($.inArray("locality", item["types"]) >= 0) {
                        city = item["short_name"];
                    }

                    if ($.inArray("administrative_area_level_1", item["types"]) >= 0) {
                        state = item["short_name"];
                    }

                    if ($.inArray("postal_code", item["types"]) >= 0) {
                        zipcode = item["short_name"];
                    }
                    if ($.inArray("country", item["types"]) >= 0) {
                        country = item["short_name"];
                    }
                });

                $(tbAddress).val(streetnumber + " " + streetaddress);
                $(tbCity).val(city);

                //This is because there are different controls for State when country is US and Rest of the world.
                if (country == "US") {
                    $(ddlState).val(state);
                    $(tbProvince).hide();
                    $(ddlState).show();
                }

                else if (country != "US")
                {
                    $(tbProvince).val(state);
                    $(tbProvince).show();
                    $(ddlState).hide();
                }
                $(tbZipcode).val(zipcode);
                $(ddlCountry).val(country);

                return streetnumber + " " + streetaddress;
            }
        });

    }

};