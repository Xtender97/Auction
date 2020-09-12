﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function findAuctions(pageNumber){
    let keyWord = $("#keyWord").val();
    let minPrice = $("#minPrice").val();
    let maxPrice = $("#maxPrice").val();
    let state = $("#state").val();
    let page = +pageNumber + 1;
    $.ajax ({
        type: "GET",
        url: "/Search/SearchAuctions?keyWord=" + keyWord + "&minPrice=" + minPrice + "&maxPrice" + maxPrice + "&state=" + state + "&page=" + page,
        dataType: "text",
        success: function ( response ) {
            $("#foundAuctions").html ( response )
        },
        error: function ( response ) {
            alert ( response );
            console.log(response);
        }
    })
}


function updateTimer ( string ) {
    var array = string.split ( ":" )
    var hours = parseInt ( array[0] )
    var minutes = parseInt ( array[1] )
    var seconds = parseInt ( array[2] )
    var timeInSeconds = hours * 3600 + minutes * 60 + seconds - 1
    seconds = timeInSeconds % 60
    minutes = Math.floor ( timeInSeconds / 60 ) % 60
    hours = Math.floor ( timeInSeconds / 3600 )
    if ( seconds < 10 ) {
        seconds = "0" + seconds
    }
    if ( minutes < 10 ) {
        minutes = "0" + minutes
    }
    if ( hours < 10 ) {
        hours = "0" + hours
    }
    return hours + ":" + minutes + ":" + seconds;
}

function timer(){
    const update = function updateTimers(){
        let listOfTimers = $(".timer").each(
            function(index)  {
                if(!$(this).text().includes("Opens")){
                    let timer = updateTimer($(this).text());
                    $(this).text(timer);
                }
            });
        

    }
    setInterval(update,1000);
}
function clearAll(windowObject) {
    var id = Math.max(
        windowObject.setInterval(noop, 1000),
        windowObject.setTimeout(noop, 1000)
    );

    while (id--) {
        windowObject.clearTimeout(id);
        windowObject.clearInterval(id);
    }

    function noop(){}
}


