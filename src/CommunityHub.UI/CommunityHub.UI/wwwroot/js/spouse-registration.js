document.addEventListener("DOMContentLoaded", () => {
    const addBtn = document.getElementById("btnAddSpouse");
    const cancelBtn = document.getElementById("btnCancelSpouse");
    const inputSec = document.getElementById("spouseInputSection");

    // Show the spouse input section
    if (addBtn && inputSec) {
        addBtn.addEventListener("click", () => {
            inputSec.style.display = "block";
            addBtn.style.display = "none";
        });
    }

    // Cancel/Remove: clear inputs + hide form + show Add button
    if (cancelBtn && inputSec) {
        cancelBtn.addEventListener("click", () => {
            inputSec.querySelectorAll("input, select").forEach(el => {
                if (el.tagName === "SELECT") el.selectedIndex = 0;
                else el.value = "";
            });
            inputSec.style.display = "none";
            addBtn.style.display = "inline-block";
        });
    }
});
