class conversation {
    constructor(id, name, currentUserId, messages) {
        this.id = ko.observable(id);
        this.currentUserId = ko.observable(currentUserId);
        this.name = ko.observable(name);
        this.messages = ko.observableArray(messages);
    }

    getMessagesPage = (onlineUsers) => {
        const count = 20;
        const skip = this.messages().length;

        let url = new URL("/Chat/GetPrivateMessagesPage", window.location.origin);
        url.searchParams.append("interlocutorId", this.id());
        url.searchParams.append("skip", skip.toString());
        url.searchParams.append("count", count.toString());

        fetch(url).then(response => response.json().then(data => {
            const mesages = data;
            mesages.forEach(m => {
                matchUserName(m, onlineUsers)
                this.messages.push(m);
            });
        })).catch(err => console.log(err));
    }
}

class onlineUser {
    constructor(userId, name) {
        this.userId = ko.observable(userId);
        this.name = ko.observable(name);
    }
}

class chatViewModel {
    constructor(onlineUsers, conversations, currentUserId) {
        this.onlineUsers = ko.observableArray(onlineUsers);
        this.conversations = ko.observableArray(conversations);
        this.typedMessage = ko.observable("");
        this.selectedConversation = ko.observable();
        this.selectedConversation(this.conversations()[0]);
        this.conversations().forEach((x) => {
            x.selectedConversation = this.selectedConversation
            x.isSelected = ko.computed(() => x.id() === this.selectedConversation().id(), x);
        });

        this.currentUserId = ko.observable(currentUserId);
        this.canSendMessage = true;//ko.computed(() => !this.typedMessage() || this.typedMessage().length === 0);
    }

    sendMessage = () => {
        if (this.selectedConversation().id() === "Public") {
            this.sendPublicMessage();
        } else {
            this.sendPrivateMessage();
        }
    }

    sendPublicMessage = () => {
        const data = { Content: this.typedMessage() }
        fetch("/Chat/SendPublicMessage", {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        }).then(data => console.log("sended public message"))
            .catch(err => console.log(err));
    }

    sendPrivateMessage = () => {
        const data = {
            ReceiverId: this.selectedConversation().id(),
            Sended: new Date().toISOString(),
            Content: this.typedMessage()
        };

        fetch("/Chat/SendPrivateMessage", {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        }).then(data => console.log("sended private message"))
            .catch(err => console.log(err));
    }

    addPublicMessage = (message) => {
        const conversation = this.conversations().find(x => x.id() === "Public");
        this.selectedConversation = conversation;
        conversation.messages.push(message);
        $("#messages").scroll();
    }

    addPrivateMessage = (message) => {
        let conversation = this.conversations().find(x => x.id() === message.conversationId);

        if (conversation === undefined) {
            const name = this.onlineUsers.find(x => x.id() === message.conversationId);
            conversation = new conversation(message.conversationId, name, this.currentUserId(), []);
            conversation.getMessagesPage();
        }
        conversation.messages.push(message);
        this.selectedConversation = conversation;
    }

    addUser = (usr) => {
        let id = usr.id;
        let name = usr.name;

        if (this.onlineUsers().some(x => x.userId() === id)) return;

        let user = new onlineUser(id, name);
        this.onlineUsers.push(user);
    }

    removeUser= (id) => {
        const toRemove = this.onlineUsers().find(x => x.userId() === id);

        if (toRemove === undefined) return;

        this.onlineUsers.remove(toRemove);
    }

    openConversation = (interLocutor) => {
        const conv = new conversation(interLocutor.userId(), interLocutor.name(), this.currentUserId(), []);
        conv.getMessagesPage(this.onlineUsers());
        conv.isSelected = ko.computed(() => conv.id() === this.selectedConversation().id(), conv)
        this.conversations.push(conv);
    }
}

function matchUserName(message, onlineUsers) {
    let user = onlineUsers.find(x => x.userId() === message.senderId);

    if (user === undefined) {
        message.userName = message.senderId;
    } else {
        message.userName = user.name;
    }

}