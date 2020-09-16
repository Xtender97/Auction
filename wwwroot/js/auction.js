
var connection = new signalR.HubConnectionBuilder().withUrl("/hub").build();




connection.on("Update", function (auctionId) {
    let pageNumber = $("#pageNumber").text();
    pageNumber--;
    console.log("Page number " + pageNumber);
    let pageType = $("#pageType").text();
    $(".auctionId").each(
        function (index) {
            let pageId = $(this).text();
            if (pageId == auctionId) {
                console.log("Reload...");
                if (pageType == "search") {
                    findAuctions(pageNumber);
                }
                else {
                    console.log("DETAILS PAGE");
                    reloadDetailsPage();
                }

            }
        }
    )
});

function handleError(error) {
    console.log(error);
}


connection.start().then(function () {

    // var conversationId = $("#conversationId").val ( );
    // connection.invoke ( "AddToGroup", conversationId )
    //     .catch ( handleError )
    console.log("Opened connection");
}
)
    .catch(handleError);


function bid(id, increment, startPrice, pageNumber) {
    console.log("bid");
    let oldPrice = +startPrice + +increment;
    let newIncrement = oldPrice * 0.05;

    if (pageNumber == null) {
        console.log("page number je null");
        $.ajax({
            type: "GET",
            url: "/Bid/bid?auctionId=" + id + "&oldPrice=" + oldPrice + "&increment=" + newIncrement,
            dataType: "text",
            success: function (response) {
                console.log(response);
                if (response.includes("Error")) {
                    alert(response)
                }
                reloadDetailsPage();
                let oldTokens = $('#tokeni').text();
                if (oldTokens != 0 && !response.includes("Error")) {
                    oldTokens = +oldTokens - 1;
                    $('#tokeni').text(oldTokens);
                }
                connection.invoke("NotifyAll", "" + id).catch(handleError);
            },
            error: function (response) {
                console.log(response);
            }
        })


    }
    else {
        pageNumber--;


        //CONVERT BACK TO STRINGS

        $.ajax({
            type: "GET",
            url: "/Bid/bid?auctionId=" + id + "&oldPrice=" + oldPrice + "&increment=" + newIncrement,
            dataType: "text",
            success: function (response) {
                console.log(response);
                if (response.includes("Error")) {
                    alert(response)
                }
                findAuctions(pageNumber);
                let oldTokens = $('#tokeni').text();
                if (oldTokens != 0 && !response.includes("Error")) {
                    oldTokens = +oldTokens - 1;
                    $('#tokeni').text(oldTokens);
                }
                connection.invoke("NotifyAll", "" + id).catch(handleError);
            },
            error: function (response) {
                console.log(response);
            }
        })
    }




}

