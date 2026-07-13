/*

to use, make sure:

1) heading element needs a class called "toggle_heading"
2) heading element needs an ID. (ie, "UPLOAD_LEADS")
3) next adjacent element needs to be a div.
4) make sure an IMG element is in the heading element.
5) requires G_APP_NAME to be set. set in Global.js

*/
jQuery(document).ready(function() {
        
    $('.toggle_heading').each(function(index) {
        
        // for each toggle heading, determine if we should open it or not.

        // our cookie prefix
        var g_toggle_prefix = G_APP_NAME + "_TOGGLE_";
        
        // the id of the header element
        var g_the_id = $(this).attr('id');
        
        // add cursor style.
        $(this).attr("style", "cursor:pointer");
        
        // set our cookie name
        var g_cookie_name = g_toggle_prefix + g_the_id;
        
        // attempt to fetch the cookie value
        var g_cookie_value = $.cookie(g_cookie_name);

        if( g_cookie_value == null ) {
        
            // cookie is not set, so we start off with a default value.
            // if the class "startclosed" is detected with the "toggle_heading" class then we default
            // it to be closed. otherwise, default it to open.
            
            var mystr = "#" + g_the_id + ".startclosed";
            
            if( $(mystr).length ) {
                $.cookie(g_cookie_name, "closed");
                g_cookie_value = "closed";
            } else {
                $.cookie(g_cookie_name, "opened");
                g_cookie_value = "opened";
            }
        } 
                
        // assumes that the content is always in the adjacent div
        var adjacent_content = "#" + g_the_id + " + div";
        
        // gets the selector for the image arrow
        var image_arrow = "#" + g_the_id + " > img";

        if( g_cookie_value == "opened" ) {
            // cookie set to open, so we slide down
            $(adjacent_content).slideDown(0);
            // and point arrow down
            $(image_arrow).attr('class', 'arrow_down').attr('src', '../Images/close.gif');
        } else if( g_cookie_value == "closed" ) {
            // closed, so we hide content
            $(adjacent_content).hide();
            // and point arrow right
            $(image_arrow).attr('class', 'arrow_right').attr('src', '../Images/open.gif');
        }

        jQuery("#" + g_the_id).click(function() {
            // called every time the header element is clicked.
            var cookie_value = $.cookie(g_cookie_name);
            
            var adjacent_content = "#" + g_the_id + " + div";
            var image_arrow = "#" + g_the_id + " > img";

            if( cookie_value == "closed" ) {
                jQuery(this).next(adjacent_content).slideToggle(500);
                $.cookie(g_cookie_name, "opened");
                $(image_arrow).attr('class', 'arrow_down').attr('src', '../Images/close.gif');
            } else if( cookie_value == "opened" ) {
                jQuery(this).next(adjacent_content).slideToggle(500);
                $.cookie(g_cookie_name, "closed");
                $(image_arrow).attr('class', 'arrow_right').attr('src', '../Images/open.gif');
            }
        });              
    });
});    