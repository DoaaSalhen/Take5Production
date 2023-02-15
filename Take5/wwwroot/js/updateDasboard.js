var userId = document.getElementById("userId").value;
var connection = new signalR.HubConnectionBuilder().withUrl("/NotificationHub?userId=" + userId).build();
connection.on("updateDashboard", (result2, tripStatus) =>
{
    var result = JSON.parse(result2);
    addNewRow(tripStatus, result);
});


function addNewRow(status, result)
{
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
    if (status == "Started") {
        Date.innerHTML = result['StatrtingDate'];
    }
    else if (status == "DestinationArrived")
    {
        Date.innerHTML = result['ArrivingDate'];

    }
    else if (status == "SurveyStepOneCompleted") {
        Date.innerHTML = result['StepOneCompletedDate'];
    }
    else if (status == "StepTwoRequested") {
        Date.innerHTML = result['RequestDate'];
        var StepOneDate = row.insertCell(6);
        StepOneDate.innerHTML = result['StepOneCompletedDate'];
        var Button = row.insertCell(7);
        Button.setAttribute("class", "btn btn-success");
        Button.setAttribute("onclick", "approveStep2Request(" + result['TripNumber'] + "," + result['JobsiteNumber']+")");
        Button.textContent = "Approve";
        var buttonId = "AprroveButton" + result['TripNumber'];
        Button.setAttribute("id", buttonId);
    }
    else if (status == "SurveyStepTwoCompleted") {
        Date.innerHTML = result['StepTwoCompletedDate'];
    }

    if (status != "Started") {
        var oldRow = document.getElementById(result['TripNumber']);
        if (oldRow != null)
        {
            oldRow.remove();
            if (status == "DestinationArrived")
            {
                document.getElementById("StartedCount").textContent = parseInt(document.getElementById("StartedCount").textContent) - 1;
            }
            else if (status == "SurveyStepOneCompleted") {
                document.getElementById("DestinationArrivedCount").textContent = parseInt(document.getElementById("DestinationArrivedCount").textContent) - 1;
            }
            else if (status == "StepTwoRequested")
            {
                document.getElementById("SurveyStepOneCompletedCount").textContent = parseInt(document.getElementById("SurveyStepOneCompletedCount").textContent) - 1;
            }
            else if (status == "TripCompleted") {
                document.getElementById("SurveyStepTwoCompletedCount").textContent = parseInt(document.getElementById("SurveyStepTwoCompletedCount").textContent) - 1;
            }
        }
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
