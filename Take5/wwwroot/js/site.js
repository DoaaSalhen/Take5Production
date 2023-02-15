
var userId = document.getElementById("userId").value;
var connection = new signalR.HubConnectionBuilder().withUrl("/NotificationHub?userId=" + userId).build();
connection.on("sendToUser", (result2, message, notificationModelData) => {
    var notificationData = JSON.parse(notificationModelData);
    var notificationType = notificationData['NotificationTypeName'];
    var count = document.getElementById("notificationCount1").textContent;
    document.getElementById("notificationCount1").textContent = parseInt(count) + 1;
    document.getElementById("notificationCount").textContent = parseInt(count) + 1 +" Notifications";
    var notificationList = document.getElementById("test");
    var dividerDiv = document.createElement("div");
    dividerDiv.setAttribute("class", "dropdown-divider");
    var a = document.createElement("a");
    a.removeAttribute("href");
    a.setAttribute("onclick", "getTripDetails(" + notificationData['TripId'] + "," + notificationData['JobSiteId'] + ")");
    a.setAttribute("class", "dropdown-item");
    var mediaDiv = document.createElement("div");
    mediaDiv.setAttribute("class", "media");
    var img = document.createElement("img");
    img.setAttribute("src", notificationData['NotificationImageUrl']);
    img.setAttribute("class", "img-size-50 img-circle mr-3");
    img.setAttribute("alt", "User Avatar");
    var mediaBodyDiv = document.createElement("div");
    mediaBodyDiv.setAttribute("class", "media-body");
    var h5 = document.createElement("h5");
    h5.innerText = notificationType;
    var p = document.createElement("p");
    p.setAttribute("class", "text-sm");
    p.innerText = message;
    var dateP = document.createElement("p");
    dateP.setAttribute("class", "text-sm text-muted");
    dateP.innerText = notificationData['CreatedDateText'] + "," + notificationData['CreatedTimeText'];
    mediaBodyDiv.append(h5);
    mediaBodyDiv.append(p);
    mediaBodyDiv.append(dateP);
    mediaDiv.append(img);
    mediaDiv.append(mediaBodyDiv);
    a.append(mediaDiv);
    notificationList.append(dividerDiv);
    notificationList.prepend(a);
    if (notificationType == "TripStarted")
    {
        document.getElementById("pendingCount").textContent = parseInt(document.getElementById("pendingCount").textContent) - 1;
        document.getElementById("inprogressCount").textContent = parseInt(document.getElementById("inprogressCount").textContent) + 1;
    }
    else if (notificationType == "TripCompleted")
    {
        document.getElementById("CompletedTake5Count").textContent = parseInt(document.getElementById("CompletedTake5Count").textContent) + 1;
        document.getElementById("StepTwoCompletedCount").textContent = parseInt(document.getElementById("StepTwoCompletedCount").textContent) - 1;
        document.getElementById("inprogressCount").textContent = parseInt(document.getElementById("inprogressCount").textContent) - 1;

    }
    removeoldRow(notificationType, notificationData["TripId"]);
    var result = JSON.parse(result2);
    if (result === null) {

    }
    else {
        addNewRow(notificationType, result);
    }
    var sound = document.getElementById("myAudio");
    sound.autoplay = true;
    sound.load();
});

function removeoldRow(status, TripId) {
    if (status != "TripStarted") {
        var oldRow = document.getElementById(TripId);
        if (oldRow != null) {
            oldRow.remove();
            if (status == "DestinationArrived") {
                document.getElementById("TripStartedCount").textContent = parseInt(document.getElementById("TripStartedCount").textContent) - 1;
            }
            else if (status == "StepOneCompleted") {
                document.getElementById("DestinationArrivedCount").textContent = parseInt(document.getElementById("DestinationArrivedCount").textContent) - 1;
            }
            else if (status == "StepTwoRequest") {
                document.getElementById("StepOneCompletedCount").textContent = parseInt(document.getElementById("StepOneCompletedCount").textContent) - 1;
            }
            else if (status == "StepTwoCompleted") {
                document.getElementById("StepTwoRequestCount").textContent = parseInt(document.getElementById("StepTwoRequestCount").textContent) - 1;
            }
        }
    }
}

function addNewRow(status, result) {

    var stratingTable = document.getElementById(status);
    var row = stratingTable.insertRow(1);
    row.setAttribute("id", result['TripNumber']);
    var Number = row.insertCell(0);
    var Jobsite = row.insertCell(1);
    var Driver = row.insertCell(2);
    var Truck = row.insertCell(3);
    var Date = row.insertCell(4);
    var ago = row.insertCell(5);
    Number.innerHTML = result['TripNumber'];
    Number.setAttribute("onclick", "getTripDetails(" + result['TripNumber'] + ", " + result['JobsiteNumber'] + ")");
    Number.setAttribute("class", "btn-link");
    Number.style.cursor = "pointer";
    Jobsite.innerHTML = result['JobsiteName'];
    Jobsite.setAttribute("onclick", "getJobsiteDetails(" + result['JobsiteNumber'] + ")");
    Jobsite.setAttribute("class", "btn-link");
    Jobsite.style.cursor = "pointer";
    Driver.innerHTML = result['DriverName'];
    Truck.innerHTML = result['TruckNumber'];
    ago.innerHTML = "Just Now";
    if (status == "TripStarted")
    {
        Date.innerHTML = result['StatrtingDate'];
    }
    else if (status == "DestinationArrived")
    {
        Date.innerHTML = result['ArrivingDate'];
    }
    else if (status == "StepOneCompleted")
    {
        Date.innerHTML = result['StepOneCompletedDate'];
    }
    else if (status == "StepTwoCompleted")
    {
        Date.innerHTML = result['StepTwoCompletedDate'];
    }
    else if (status == "StepTwoRequest")
    {
        Date.innerHTML = result['RequestDate'];
        Truck.innerHTML = result['StepOneCompletedDate'];
        var div = row.insertCell(6);
        var Button = document.createElement("button");
        Button.setAttribute("class", "btn btn-success");
        Button.setAttribute("onclick", "approveStep2Request(" + result['TripNumber'] + "," + result['JobsiteNumber'] + ")");
        Button.textContent = "Approve";
        var divId = "AprroveButton" + result['TripNumber'];
        div.setAttribute("id", divId);
        div.append(Button);
    }
    var countId = status + "Count";
    if (document.getElementById(countId) != null)
    {
        document.getElementById(countId).textContent = parseInt(document.getElementById(countId).textContent) + 1;
    } 
}

connection.start().catch(function (err) {
    return console.error(err.toString());
}).then(function () {
    connection.invoke('GetConnectionId').then(function (connectionId) {
    })
});
