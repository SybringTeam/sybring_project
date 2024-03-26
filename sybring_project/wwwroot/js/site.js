

// ------------------sidobaren-------------
//$(document).ready(function () {

$(document).ready(function () {
    $('#myTable').DataTable({
        "scrollY": "500px",
        "scrollCollapse": false,
        "paging": true,
        "order": [[0, "desc"]],
        "searching": true,
        
        


    });
});

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
    //function showUser(id) {
    //    $.ajax({
    //        type: "GET",
    //        url: "/User/UserVc",
    //        data: { userId: id },
    //        success: function (response) {

    //            $('#userEle').html(response);

    //        },
    //        error: function (error) {
    //            alert("operation failed...", error);
    //        }
    //    });
    //}

function showUser(userId) {
    fetch(`/User/UserVc?userId=${userId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(html => {
            document.getElementById('showUserContainer').innerHTML = html;
        })
        .catch(error => console.error('Error fetching user view component:', error));
}
function showUser(userId) {
    hideAllUsers();
    $('#showUserContainer_' + userId).show();
}

function hideAllUsers() {
    $('[id^=showUserContainer_]').hide();
}
