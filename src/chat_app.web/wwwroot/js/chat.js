"use strict";

const current_userId = $("#user_id").html();
let data = $.parseJSON($("#chat_data").html());

let users = [];
let conversations = [];


data.onlineUsers.forEach(x => users.push(new onlineUser(x.id, x.name)));

data.conversations.forEach(x =>
{
    const name = x.id === "Public" ? "Public" : users.find(u => u.userId === x.id).name;
    return conversations.push(new conversation(x.id, name, current_userId, x.messages))
});

conversations.forEach(x => x.messages().forEach(m => matchUserName(m, users)));

let model = new chatViewModel(users, conversations, current_userId);



let signalRConnection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

signalRConnection.on("ReceivePublicMessage", message => {
    matchUserName(message, model.onlineUsers());
    model.addPublicMessage(message);
});

signalRConnection.on("ReceivePrivateMessage", message => {
    matchUserName(message, model.onlineUsers());
    model.addPrivateMessage(message);
});

signalRConnection.on("ChatUserConnected", user => model.addUser(user));

signalRConnection.on("ChatUserDisconnected", id => model.removeUser(id));

signalRConnection.on("Disconnect", () => signalRConnection.stop());

signalRConnection.start()
    .then(() => console.log("started signalr connection"))
    .catch(err => console.log(err));


ko.applyBindings(model);