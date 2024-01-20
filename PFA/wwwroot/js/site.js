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


// Add this script at the end of your view



var animateButton = function (e) {

    e.preventDefault;
    e.target.classList.remove('animate');
    e.target.classList.add('animate');
    setTimeout(function () {
        e.target.classList.remove('animate');
    }, 700);
};

var bubblyButtons = document.getElementsByClassName("bubbly-button");

for (var i = 0; i < bubblyButtons.length; i++) {
    bubblyButtons[i].addEventListener('click', animateButton, false);
}


//handel search
