window.MyscriptCtrl = {
    LoadPartialViews: function (div_id, message) {
        var data = {
            //"X-CSRF-TOKEN": $('input[name="__RequestVerificationToken"]').val(),
            'PageTitle': message
        };
        $.ajax({
            'url': '/Home/_CommonPartialView',
            'type': 'POST',
            contentType: 'application/json',
            headers: {
                "X-CSRF-TOKEN": $('input[name="__RequestVerificationToken"]').val()
            },
            'data': JSON.stringify(data),

            'success': function (data) {
                $("#" + div_id).html(data);
            },
            'error': function (request, error) {
                alert('Request: ' + JSON.stringify(request));
            }
        });
    },
    EmployerLogin: function () {
        var data = {
            //"X-CSRF-TOKEN": $('input[name="__RequestVerificationToken"]').val(),
            'Email': $("#TxtEmployerEmail").val(),
            'Password': $("#TxtEmployerPassword").val()
        };
        $.ajax({
            'url': '/Login/Index',
            'type': 'POST',
            contentType: 'application/json',
            'data': JSON.stringify(data),

            'success': function (response) {
                setCookie("AccessToken", response.token, 1);
            },
            'error': function (request, error) {
                alert('Request: ' + JSON.stringify(request));
            }
        });
    }
}


// COOKIE HELPER
function setCookie(name, value, days) {
    let expires = "";
    if (days) {
        let date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + value + expires + "; path=/";
}

function getCookie(name) {
    let nameEQ = name + "=";
    let ca = document.cookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i].trim();
        if (c.indexOf(nameEQ) === 0)
            return c.substring(nameEQ.length);
    }
    return null;
}


// SIDEBAR SAVE STATE 

document.addEventListener("DOMContentLoaded", function () {

    let sidebarState = getCookie("sidebar");

    if (sidebarState === "collapsed") {
        document.body.classList.remove("sidebar-open");
        document.body.classList.add("sidebar-collapse");
    }

    document.querySelectorAll('[data-lte-toggle="sidebar"]').forEach(btn => {
        btn.addEventListener("click", function () {

            setTimeout(() => {
                if (document.body.classList.contains("sidebar-collapse")) {
                    setCookie("sidebar", "collapsed", 30);
                } else {
                    setCookie("sidebar", "open", 30);
                }
            }, 300);

        });
    });

});

//CHANGE COLOR MODE

function toggleTheme() {

    let currentTheme = document.documentElement.getAttribute("data-bs-theme");

    if (currentTheme === "dark") {
        document.documentElement.setAttribute("data-bs-theme", "light");
        setCookie("theme", "light", 30);
    }
    else {
        document.documentElement.setAttribute("data-bs-theme", "dark");
        setCookie("theme", "dark", 30);
    }

}

