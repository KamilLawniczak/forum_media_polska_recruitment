class chatViewModel {
    constructor(onlineUsers, conversations) {
        this.onlineUsers = ko.observableArray(onlineUsers);
        this.conversations = ko.observableArray(conversations);
        this.typedMessage = ko.observable("");
        this.selectedConversation = ko.observable();
        this.selectedConversation(this.conversations()[0]);
        this.conversations().forEach((x) => {
            x.selectedConversation = this.selectedConversation
            x.isSelected = ko.computed(() => x.id() === this.selectedConversation().id(), x);
        });
        this.canSendMessage = true;//ko.computed(() => !this.typedMessage() || this.typedMessage().length === 0);
    }

    sendMessage() {
        if (this.selectedConversation().id() === "Public") {
            this.sendPublicMessage();
        }
    }

    sendPublicMessage() {
        const data = { Content: this.typedMessage() }
        const response = fetch("/Chat/SendPublicMessage", {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        }).then(data => console.log("sended public message"))
          .catch(err => console.log(err));
    }

    addMessage(message) {
        const conversation = this.conversations().find(x => x.id() === "Public");
        conversation.messages.push(message);
        $("#messages").scroll();
    }
}

class conversation {
    constructor(id, messages) {
        this.id = ko.observable(id);
        this.messages = ko.observableArray(messages);
    }   
}

class onlineUser {
    constructor(userId, name) {
        this.userId = ko.observable(userId);
        this.name = ko.observable(name);
    }
}

function matchUserName(message, onlineUsers) {
    var user = onlineUsers.find(x => x.userId() == message.senderId);

    if (user === undefined) {
        message.userName = message.senderId;
    } else {
        message.userName = user.name;
    }
    
}