document.addEventListener("DOMContentLoaded", function () {
    var maritalStatusSelector = document.getElementById("MaritalStatus");
    var spouseSection = document.getElementById("spouseSection");

    maritalStatusSelector.addEventListener("change", function () {
        if (maritalStatusSelector.value !== "Married") {
            spouseSection.style.display = "none";
            var spouseInputs = document.querySelectorAll("#spouseSection input, #spouseSection select");
            spouseInputs.forEach(function (input) {
                input.value = "";
                input.disabled = true;
            });
        } else {
            spouseSection.style.display = "block";
            var spouseInputs = document.querySelectorAll("#spouseSection input, #spouseSection select");
            spouseInputs.forEach(function (input) {
                input.disabled = false;
            });
        }
    });

    if (maritalStatusSelector.value !== "Married") {
        if (spouseSection != null) {
            spouseSection.style.display = "none";
            var spouseInputs = document.querySelectorAll("#spouseSection input, #spouseSection select");
            spouseInputs.forEach(function (input) {
                input.value = "";
                input.disabled = true;
            });
        }
    }

    var btnAddChild = document.getElementById("btnAddChild");
    var childInputSection = document.getElementById("childInputSection");
    var childNameInput = document.getElementById("childNameInput");
    var btnSaveChild = document.getElementById("btnSaveChild");
    var btnCancelChild = document.getElementById("btnCancelChild");
    var childrenList = document.getElementById("childrenList");
    var childIndex = 0;

    if (btnAddChild) {
        btnAddChild.addEventListener("click", function () {
            childInputSection.style.display = "block";
            btnAddChild.disabled = true;
        });
    }

    if (btnSaveChild) {
        btnSaveChild.addEventListener("click", function () {
            var childName = childNameInput.value.trim();

            if (childName !== "") {
                var existingChildren = document.querySelectorAll('input[name^="Children["]');
                var isDuplicate = false;

                existingChildren.forEach(function (input) {
                    if (input.value.trim().toLowerCase() === childName.toLowerCase()) {
                        isDuplicate = true;
                    }
                });

                if (isDuplicate) {
                    var errorMessage = document.getElementById("duplicateErrorMessage");
                    if (errorMessage) {
                        errorMessage.style.display = "block";
                    }
                } else {
                    var errorMessage = document.getElementById("duplicateErrorMessage");
                    if (errorMessage) {
                        errorMessage.style.display = "none";
                    }

                    var listItem = document.createElement("li");
                    listItem.classList.add("list-group-item", "d-flex", "justify-content-between", "align-items-center");
                    listItem.innerHTML = childName +
                        '<input type="hidden" name="Children[' + childIndex + '].Name" value="' + childName + '" />' +
                        '<button type="button" class="btn btn-danger btn-sm btnDeleteChild">Delete</button>';

                    childrenList.appendChild(listItem);
                    childNameInput.value = "";
                    childInputSection.style.display = "block";

                    btnAddChild.disabled = false;
                    childIndex++;
                }
            }
        });
    }

    if (btnCancelChild) {
        btnCancelChild.addEventListener("click", function () {
            childInputSection.style.display = "none";
            btnAddChild.disabled = false;
        });
    }

    childrenList.addEventListener("click", function (event) {
        if (event.target && event.target.matches("button.btnDeleteChild")) {
            var listItem = event.target.closest("li");
            childrenList.removeChild(listItem);

            var children = childrenList.querySelectorAll('li');
            childIndex = 0;
            children.forEach(function (child) {
                var hiddenInput = child.querySelector('input[type="hidden"]');
                hiddenInput.name = "Children[" + childIndex + "].Name";
                childIndex++;
            });
        }
    });
});

