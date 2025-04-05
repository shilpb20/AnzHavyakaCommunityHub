document.addEventListener("DOMContentLoaded", function () {
    // Initialize Bootstrap Tooltip
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Success message fade-out
    var successMessage = document.getElementById("successMessage");
    if (successMessage) {
        setTimeout(function () {
            $(successMessage).fadeOut();
        }, 5000);
    }
});