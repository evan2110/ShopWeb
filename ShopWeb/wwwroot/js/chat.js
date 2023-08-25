"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var currentDate = new Date();
var formattedDate = `${currentDate.getFullYear()}-${(currentDate.getMonth() + 1).toString().padStart(2, '0')}-${currentDate.getDate().toString().padStart(2, '0')}`;
var formattedTime = `${currentDate.getHours().toString().padStart(2, '0')}:${currentDate.getMinutes().toString().padStart(2, '0')}:${currentDate.getSeconds().toString().padStart(2, '0')}`;

//Disable the send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    li.classList.add("message-item");
    document.getElementById("messageList").appendChild(li);
    //We can assign user-supplied strings to an element's textContent because it is not interpreted as markup.
    //If you're assigning in any other way, you should be aware of possible script injection concerns.
    // Get the current date and time
    
    // Combine user, message, date, and time
    var messageText = `${formattedDate} ${formattedTime} - ${user}: ${message}`;

    // Assign the combined message to the li element's textContent
    li.textContent = messageText;
    // Clear the message input field after sending a message
    var messageInput = document.getElementById("messageInput");
    messageInput.value = ""; // Thiết lập giá trị thành chuỗi rỗng
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });

    var messageData = {
        userSupport: user,
        message: message,
        timeSend: `${formattedDate} ${formattedTime}`,
    };
    console.log(JSON.stringify(messageData));

    fetch('http://localhost:5251/Support/SaveMessageInSession', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(messageData)
    })
        .then(response => response.json())
        .then(data => {
            console.log('Message saved in session:', data);
        })
        .catch(error => {
            console.error('Error saving message:', error);
        });
    event.preventDefault();
});