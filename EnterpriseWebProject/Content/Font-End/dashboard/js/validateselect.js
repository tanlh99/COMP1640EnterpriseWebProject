//--------------------------Validate Select--------------------------//
var form = document.querySelector('form');
// Add a listener to our form to wait for its submission
if (form.addEventListener) {
  form.addEventListener("submit", validate, false); //Modern browsers
} else if (form.attachEvent) {
  form.attachEvent('onsubmit', validate); //Old IE
}

function validate(e) {
  var select = e.target.querySelector("select");

  // Get the value of our selected option
  var selectedOption = select.options[select.selectedIndex].value;

  // Compare the value of the default option to the selected option
  if (selectedOption === "--Select--") {
    // Trigger Error and prevent the form submission
    alert("Please select an option!")
    e.preventDefault();
  }
}
//--------------------------Validate Select--------------------------//
