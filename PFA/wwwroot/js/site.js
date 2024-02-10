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

// Additional code for responsiveness
window.addEventListener('resize', () => {
    if (window.innerWidth > 768) {
        // If the window width is greater than 768 pixels, ensure the navigation menu is open
        navMenu.classList.add('open');
    } else {
        // If the window width is 768 pixels or less, close the navigation menu
        navToggler.classList.remove('toggler-open');
        navMenu.classList.remove('open');
    }
});


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
/* ddd*/
$(function () {
    if ($('div.alert.notification').length) {
        setTimeout(() => {
            $('div.alert.notification').fadeOut();

        }, 3000)

    }
});



function displayBusyIndication() {
    $('.loading').show();
}

function hideBusyIndication() {
    $('.loading').hide();
}

$(document).ready(function () {
    $(window).on('beforeunload', function () {
        displayBusyIndication();
    });

    $(document).on('submit', 'form', function () {
        displayBusyIndication();
    });

    window.setTimeout(function () {
        hideBusyIndication();
    }, 3000);
});

$(window).on('load', function () {
    hideBusyIndication();
});


//Popup to show image
$('#CustomImg img').on('mouseenter', function () {
    $('#ImgModel').show();
    var html = '<img src="' + $(this).attr('src') + '" style="width: 450px; height: 450px;">';
    $('#ImgBody').html(html);
});

$('#CustomImg img').on('mouseleave', function () {
    $('#ImgModel').hide();
});

$('#CloseBtn').on('click', function () {
    $('#ImgModel').hide();
});


//Show Password

    //function togglePasswordVisibility() {
    //        var passwordInput = document.getElementById('password');
    //var eyeIcon = document.getElementById('eyeIcon');

    //if (passwordInput.type === 'password') {
    //    passwordInput.type = 'text';
    //eyeIcon.classList.remove('bi-eye');
    //eyeIcon.classList.add('bi-eye-slash');
    //        } else {
    //    passwordInput.type = 'password';
    //eyeIcon.classList.remove('bi-eye-slash');
    //eyeIcon.classList.add('bi-eye');
    //        }
    //    }


    function togglePasswordVisibility(passwordId, eyeIconId) {
        var passwordInput = document.getElementById(passwordId);
    var eyeIcon = document.getElementById(eyeIconId);

    if (passwordInput.type === 'password') {
        passwordInput.type = 'text';
    eyeIcon.classList.remove('bi-eye');
    eyeIcon.classList.add('bi-eye-slash');
        } else {
        passwordInput.type = 'password';
    eyeIcon.classList.remove('bi-eye-slash');
    eyeIcon.classList.add('bi-eye');
        }
    }

