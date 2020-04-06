let model = {
    messages: ko.observableArray([{ text: "hahahah" }, { text: "lorem ipsum" }])
};

ko.applyBindings(model);