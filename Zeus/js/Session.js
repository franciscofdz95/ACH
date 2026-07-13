// JScript File

    var sessionTimeout = 90; //length of time until logged out (minutes)
    var warningTime = 1; //length of time popup stays up for (minutes)
    var howLong = 60; 
    //popup opens sessionTimeout minus warningTime minutes after the page is opened
    var thisTimer = null; //stop/start session timer
    var thisWarningTimer = null; //stop/start warning timer
    var a = null;
    var b = null;
    var c= null;

    function startTimeoutTimer()
    {
        howLong = 60; 
        
        clearTimeout(a);
        clearTimeout(b);
        clearInterval(c);
        c=null;
        clearTimeout(thisTimer);
        clearTimeout(thisWarningTimer); 

        thisTimer = setTimeout("closeSession()", sessionTimeout*60000);
        thisWarningTimer = setTimeout("popupAsk()", (sessionTimeout - warningTime)*60000);
    }
    
    function tick()
    {
        //subtract one second
        howLong -= 1;

        var timeValue = "";
        var timeLeft = howLong //- second;

        //if at least a minute, parse number of minutes
        if ( timeLeft >= 60 )
        {
            timeValue = (timeLeft-(timeLeft % 60)) / 60 + " minute and ";
        }

        //append number of seconds
        timeValue += ( howLong % 60 );

        //show message
        if(document.all)
        {
            document.all['msg'].innerHTML = "There has been no server activity for an extended period of time.<BR><BR>Your session will time out in <b><font color='red'>" + timeValue + " seconds</font></b>.";
            window.focus();
        }
        else  
        {
            document.getElementById('msg').innerHTML = "There has been no server activity for an extended period of time.<BR><BR>Your session will time out in <b><font color='red'>" + timeValue + " seconds</font></b>.";            
            logout = document.getElementById("modalPage");		
            setTimeout(function () { logout.focus(); },0);
        }
    }

    function stillHere()
    {
        //resets timer on parent page and closes this window
        window.refreshSession();
    }

    function goAway()
    {
        //logout on parent page and close this window   
        window.closeSession();
    }
    
    if (document) 
    {
        document.onclick= startTimeoutTimer;
        document.onkeypress = startTimeoutTimer;
        document.onselectstart = startTimeoutTimer;
    }       

    function refreshSession()
    {   
        window.location.href = window.location;               
        //restart timers
        clearTimeout(thisTimer);
        clearTimeout(thisWarningTimer);
        startTimeoutTimer();        
    }

    function closeSession()
    {
        //redirect to login page with logged out message
        logout = document.getElementById("modalPage");		
        logout.style.display = "none";
        document.location.href = '../frmLogin.aspx?relogin=false';         
    }
    
    function popupAsk()
    {
        //open popup window
		logout = document.getElementById("modalPage");		
	    var a = setTimeout("closeSession()", howLong * 1000);
	    var b = setInterval("tick()", 1000);
        logout.style.display = "block";
        window.focus();
        setTimeout(function () { logout.focus(); },0);
	 }
