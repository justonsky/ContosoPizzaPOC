// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// WebSocket = undefined;
//EventSource = undefined;
//, signalR.HttpTransportType.LongPolling

let connection = null;

setupConnection = () => {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/customerhub")
        .build();

    connection.on("ReceiveOrderUpdate", (update) => {
            const statusDiv = document.getElementById("status");
            statusDiv.innerHTML = update;
        }
    );

    connection.on("ReceiveMessage", (update) => {
            console.log(connection.connectionId);
            console.log(update);
            const pizzaList = document.getElementById("pizza-list");
            var node = document.createElement('li');
            node.appendChild(document.createTextNode(update.name));
            pizzaList.appendChild(node);
        }
    );

    connection.on("finished", function () {
            connection.stop();
        }
    );

    connection.start()
        .catch(err => console.error(err.toString()));
};

setupConnection();

document.getElementById("submit").addEventListener("click", e => {
    e.preventDefault();
    let id = Math.floor(Math.random() * 1000) + 1;
    const name = document.getElementById("Name").value;
    const isGlutenFree = document.getElementById("IsGlutenFree").value == "on";

    fetch("/pizza",
        {
            method: "POST",
            body: JSON.stringify({ id, name, isGlutenFree }),
            headers: {
                'x-HubConnectionId': connection.connectionId,
                    'content-type': 'application/json'
                }
            })
        .then(response => response.text());
});
