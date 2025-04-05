window.filterByLocation = function () {
    console.log("filterByLocation() function executed!");
    const selectedLocation = document.getElementById('locationFilter').value;
    const tableBody = document.getElementById('requestTableBody');
    const rows = Array.from(tableBody.querySelectorAll('.mainRow'));
    let count = 1;

    // Filter rows based on the selected location
    const filteredRows = rows.filter(row => {
        const locationCell = row.querySelector('.compact-location').textContent.trim();
        if (selectedLocation === "" || selectedLocation === "all locations" || locationCell === selectedLocation) {
            row.style.display = "";
            return true;
        } else {
            row.style.display = "none";
            return false;
        }
    });

    filteredRows.sort((rowA, rowB) => {
        const nameA = rowA.cells[1].textContent.trim();
        const nameB = rowB.cells[1].textContent.trim();
        return nameA.localeCompare(nameB);
    });

    filteredRows.forEach(row => {
        row.cells[0].textContent = count++;
        tableBody.appendChild(row);
    });
};
