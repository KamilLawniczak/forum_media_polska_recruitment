"use strict";

let data = $.parseJSON($("#chat_data").html());

let users = [];
let conversations = [];


data.onlineUsers.forEach(x => users.push(new onlineUser(x.id, x.name)));
data.conversations.forEach(x => conversations.push(new conversation(x.id, x.messages)));

conversations.forEach(x => x.messages().forEach(m => matchUserName(m, users)));

let model = new chatViewModel(users, conversations);



let signalRConnection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

signalRConnection.on("ReceivePublicMessage", message => {
    matchUserName(message, model.onlineUsers());
    model.addMessage(message);
});

signalRConnection.start()
    .then(() => console.log("started signalr connection"))
    .catch(err => console.log(err));


ko.applyBindings(model);