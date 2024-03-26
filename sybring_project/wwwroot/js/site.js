//var editor = new DataTable.Editor({
//    ajax: '../php/staff.php',
//    fields: [
//        {
//            label: 'First name:',
//            name: 'first_name'
//        },
//        {
//            label: 'Last name:',
//            name: 'last_name'
//        },
//        {
//            label: 'Position:',
//            name: 'position'
//        },
//        {
//            label: 'Office:',
//            name: 'office'
//        },
//        {
//            label: 'Extension:',
//            name: 'extn',
//            multiEditable: false
//        },
//        {
//            label: 'Start date:',
//            name: 'start_date',
//            type: 'datetime'
//        },
//        {
//            label: 'Salary:',
//            name: 'salary'
//        }
//    ],
//    table: '#example'
//});

//new DataTable('#example', {
//    ajax: '../php/staff.php',
//    columns: [
//        {
//            data: null,
//            render: (data) => data.first_name + ' ' + data.last_name
//        },
//        { data: 'position' },
//        { data: 'office' },
//        { data: 'extn' },
//        { data: 'start_date' },
//        { data: 'salary', render: DataTable.render.number(null, null, 0, '$') }
//    ],
//    layout: {
//        topStart: {
//            buttons: [
//                { extend: 'create', editor: editor },
//                { extend: 'edit', editor: editor },
//                { extend: 'remove', editor: editor }
//            ]
//        }
//    },
//    select: true
//});

// ------------------sidobaren-------------
//$(document).ready(function () {

$(document).ready(function () {
    $('#myTable').DataTable({
        "scrollY": "500px",
        "scrollCollapse": false,
        "paging": true,
        "order": [[0, "desc"]],
        "searching": true,
        "select": true
        
        


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
