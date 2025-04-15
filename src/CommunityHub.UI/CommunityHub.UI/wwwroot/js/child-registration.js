document.addEventListener("DOMContentLoaded", function () {
    const btnAddChild = document.getElementById("btnAddChild");
    const childInputSection = document.getElementById("childInputSection");
    const childNameInput = document.getElementById("childNameInput");
    const btnSaveChild = document.getElementById("btnSaveChild");
    const btnCancelChild = document.getElementById("btnCancelChild");
    const childrenList = document.getElementById("childrenList");
    let childIndex = childrenList ? childrenList.children.length : 0;

    if (btnAddChild) {
        btnAddChild.addEventListener("click", function () {
            childInputSection.style.display = "block";
            btnAddChild.disabled = true;
        });
    }

    if (btnSaveChild) {
        btnSaveChild.addEventListener("click", function () {
            const childName = childNameInput.value.trim();
            if (!childName) return;

            const existing = document.querySelectorAll('input[name^="Children["]');
            const isDup = Array.from(existing).some(i => i.value.trim().toLowerCase() === childName.toLowerCase());

            const dupMsg = document.getElementById("duplicateErrorMessage");
            if (isDup) {
                dupMsg.style.display = "block";
                return;
            } else {
                dupMsg.style.display = "none";
            }

            const li = document.createElement("li");
            li.className = "list-group-item d-flex justify-content-between align-items-center";
            li.innerHTML = `
                <span>${childName}</span>
                <input type="hidden" name="Children[${childIndex}].Name" value="${childName}" />
                <button type="button" class="btn btn-danger btn-sm btnDeleteChild">Delete</button>
            `;
            childrenList.appendChild(li);
            childIndex++;

            childNameInput.value = "";
            childInputSection.style.display = "none";
            btnAddChild.disabled = false;
        });
    }

    if (btnCancelChild) {
        btnCancelChild.addEventListener("click", function () {
            childInputSection.style.display = "none";
            btnAddChild.disabled = false;
            document.getElementById("duplicateErrorMessage").style.display = "none";
            childNameInput.value = "";
        });
    }

    if (childrenList) {
        childrenList.addEventListener("click", function (e) {
            if (!e.target.matches("button.btnDeleteChild")) return;
            const li = e.target.closest("li");
            li.remove();

            const items = childrenList.querySelectorAll("li");
            items.forEach((item, idx) => {
                const input = item.querySelector('input[type="hidden"]');
                input.name = `Children[${idx}].Name`;
            });
            childIndex = items.length;
        });
    }
});
