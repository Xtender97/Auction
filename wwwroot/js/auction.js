
var connection = new signalR.HubConnectionBuilder().withUrl("/hub").build();

// //Disable send button until connection is established
// document.getElementById("sendButton").disabled = true;

connection.on("Update", function (auctionId) {
    let pageNumber = $("#pageNumber").text();
    pageNumber--;
    console.log("Page number " + pageNumber);
    $(".auctionId").each(
        function (index) {
            let pageId = $(this).text();
            if (pageId == auctionId) {
                console.log("Reload...");
                findAuctions(pageNumber);

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
    pageNumber--;
    let oldPrice = +startPrice + +increment;

    let newIncrement = oldPrice * 0.05;

    //CONVERT BACK TO STRINGS

    $.ajax({
        type: "GET",
        url: "/Bid/bid?auctionId=" + id + "&oldPrice=" + oldPrice + "&increment=" + newIncrement,
        dataType: "text",
        success: function (response) {
            console.log(response);
            if(response.includes("Error")){
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

