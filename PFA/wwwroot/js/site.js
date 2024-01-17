// Define UI variables
const navToggler = document.querySelector('.nav-toggler');
const navMenu = document.querySelector('.site-navbar ul');
const navLinks = document.querySelectorAll('.site-navbar a');

// Load event listeners
navToggler.addEventListener('click', togglerClick);
navLinks.forEach(elem => elem.addEventListener('click', navLinkClick));

// Toggle click function
function togglerClick() {
    navToggler.classList.toggle('toggler-open');
    navMenu.classList.toggle('open');
}

// Nav link click function
function navLinkClick() {
    if (navMenu.classList.contains('open')) {
        navToggler.click();
    }
}

// Toggle password visibility
$(document).ready(function () {
    $("#togglePassword").click(function () {
        var passwordField = $("#password");
        var passwordFieldType = passwordField.attr("type");

        if (passwordFieldType == "password") {
            passwordField.attr("type", "text");
        } else {
            passwordField.attr("type", "password");
        }

        // Toggle eye icon
        $(this).find("i").toggleClass("bi-eye bi-eye-slash");
    });
});

// Add this script at the end of your view

   

