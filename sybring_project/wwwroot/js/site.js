document.addEventListener("DOMContentLoaded", function () {
    const themeToggleBtn = document.getElementById('theme-toggle');
    const themeLink = document.getElementById('theme-link');

    themeToggleBtn.addEventListener('click', function () {
        // Toggle between light and dark themes
        if (themeLink.getAttribute('href') === '/css/light-theme.css') {
            themeLink.setAttribute('href', '/css/dark-theme.css');
        } else {
            themeLink.setAttribute('href', '/css/light-theme.css');
        }
    });
});