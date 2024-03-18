

// ------------------sidobaren-------------
//$(document).ready(function () {




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


    document.addEventListener("DOMContentLoaded", function () {
        var sidebarStatus = localStorage.getItem("sidebarStatus");
        if (sidebarStatus === "open") {

            openNav();
        } else {

            closeNav();
        }
    });

    // ------------------sidobaren---------------------


    function showUser(id) {
        $.ajax({
            type: "GET",
            url: "/User/UserVc",
            data: { userId: id },
            success: function (response) {

                $('#userEle').html(response);

            },
            error: function (error) {
                alert("operation failed...", error);
            }
        });
    }


/*});*/