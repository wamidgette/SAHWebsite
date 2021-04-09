//Ticket field validation
//If these conditions are not satisfied, the form cannot be submitted!
function verifyTicket() {

    var ticketForm = document.forms.createTicket;
    var numberPlate = document.getElementById("Ticket_NumberPlate");
    var entryTime = document.getElementById("Ticket_EntryTime");
    var duration = document.getElementById("Ticket_Duration");
	var fees = document.getElementById("Ticket_Fees");
	

    //name pattern, only normal characters
    var nameRegex = /^([A-Za-z]\s?)+$/;
    //Simplified Date pattern not fully respecting day counts!
    var dateRegex = /^(3[0-1]|2[0-9]|1[0-9]|0[0-9])-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)-\d{4}$/i; //Date
    
    var numberRegex = /^([A-Za-z0-9])+$/; 
    
	//Pattern for fees
	var feesRegex = /^\d{1,5}([.]\d{0,2})?$/; //fees

    //This function checks all the fields one by one
    function validation() {
        //Ticket number checking
        if (!numberRegex.test(numberPlate.value)) {
            numberPlate.style.backgroundColor = "red";
            numberPlate.focus();
            return false;
        } else {
            numberPlate.style.backgroundColor = "white";
        }
        //Entry date checking
        if (!dateRegex.test(entryTime.value)) {
            entryTime.style.backgroundColor = "red";
            entryTime.focus();
            return false;
        } else {
            entryTime.style.backgroundColor = "white";
        }
        //Duration in hours checking
        if (!feesRegex.test(duration.value)) {
            duration.style.backgroundColor = "red";
            duration.focus();
            return false;
        } else {
            duration.style.backgroundColor = "white";
        }
    }

    ticketForm.onsubmit = validation;

}


//If these conditions are not satisfied, the form cannot be submitted!
function verifyParkingSpot() {

    var spotForm = document.forms.createSpot;
    var zone = document.getElementById("Zone");
    var spotNumber = document.getElementById("SpotNumber");
    var status = document.getElementById("Status");
    
    //name pattern, only normal characters
    var nameRegex = /^([A-Za-z])+$/;
    //Spot number pattern
    var spotRegex = /^([A-Za-z0-9])+$/; 

    //This function checks all the fields one by one
    function validation() {
        //Zone name checking
        if (!nameRegex.test(zone.value)) {
            zone.style.backgroundColor = "red";
            zone.focus();
            return false;
        } else {
            zone.style.backgroundColor = "white";
        }
        //Spot number checking
        if (!spotRegex.test(spotNumber.value)) {
            spotNumber.style.backgroundColor = "red";
            spotNumber.focus();
            return false;
        } else {
            spotNumber.style.backgroundColor = "white";
        }
        //Status checking
        if (!status.checked) {
            status.style.border = "15px";
            status.style.borderColor = "red";
            status.focus();
            return false;
        } else {
            status.style.borderWidths = "0px";
            status.style.borderColor = "black";
        }
    }

    spotForm.onsubmit = validation;

}


//Deletion confirmation
function confirmation() {

    var deleteForm = document.forms.delete;

    function confirmDelete() {
        //If user clicks "Cancel", the deletion is cancelled
        var question = confirm("Do you really want to delete?");
        if (!question) {
            return false;
        }
    }
    deleteForm.onsubmit = confirmDelete;
}
