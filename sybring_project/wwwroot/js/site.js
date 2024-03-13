//document.addEventListener("DOMContentLoaded", function () {
//    const themeToggleBtn = document.getElementById('theme-toggle');
//    const themeLink = document.getElementById('theme-link');

//    themeToggleBtn.addEventListener('click', function () {
//        // Toggle between light and dark themes
//        if (themeLink.getAttribute('href') === '/css/light-theme.css') {
//            themeLink.setAttribute('href', '/css/dark-theme.css');
//            console.log('Switched to Dark Theme');
//        } else {
//            themeLink.setAttribute('href', '/css/light-theme.css');
//            console.log('Switched to Light Theme');
//        }
//    });
//});


// ------------------sidobaren-------------
function toggleNav() {
    var sidenav = document.getElementById("mySidenav");
    var computedStyle = window.getComputedStyle(sidenav);

    if (computedStyle.width === "60px") {
       
        openNav();
    } else {
       
        closeNav();
    }
}

function openNav() {
    var sidenav = document.getElementById("mySidenav");
    sidenav.style.width = "250px";
    document.querySelectorAll('.nav-text').forEach(function (element) {
        element.style.display = 'inline';
    });
    document.querySelectorAll('.icon-span').forEach(function (element) {
        element.style.display = 'none';
    });
    
    localStorage.setItem("sidebarStatus", "open");
}

function closeNav() {
    var sidenav = document.getElementById("mySidenav");
    sidenav.style.width = "60px";
    document.querySelectorAll('.nav-text').forEach(function (element) {
        element.style.display = 'none';
    });
    document.querySelectorAll('.icon-span').forEach(function (element) {
        element.style.display = 'inline';
    });
   
    localStorage.setItem("sidebarStatus", "closed");
}


document.addEventListener("DOMContentLoaded", function() {
    var sidebarStatus = localStorage.getItem("sidebarStatus");
    if (sidebarStatus === "open") {
       
        openNav();
    } else {
        
        closeNav();
    }
});

// ------------------sidobaren---------------------

