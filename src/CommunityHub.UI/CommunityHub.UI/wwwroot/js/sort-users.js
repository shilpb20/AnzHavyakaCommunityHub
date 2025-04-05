document.addEventListener("DOMContentLoaded", function () {

    const usersData = @Html.Raw(Newtonsoft.Json.JsonConvert.SerializeObject(Model));

    toggleLocationSortOptions(usersData);

    const locationFilter = document.getElementById("locationFilter");
    locationFilter.addEventListener("change", function () {
        toggleLocationSortOptions(usersData);
    });
});

function toggleLocationSortOptions(usersData) {
    const locationFilter = document.getElementById("locationFilter");
    const sortDropdown = document.getElementById('sortDropdown');
    const locationSortOptions = Array.from(sortDropdown.options).filter(option => option.value === "Location_A_Z" || option.value === "Location_Z_A");

    const uniqueLocations = [...new Set(usersData.map(user => user.Location))];

    if (locationFilter.value === "" && uniqueLocations.length > 1)
    {
        locationSortOptions.forEach(option =>
        {
            option.style.display = "inline";
        });
    } else
    {
        locationSortOptions.forEach(option =>
        {
            option.style.display = "none";
        });
    }
}

function sortUsers() {
    const sortDropdown = document.getElementById('sortDropdown');
    const selectedSort = sortDropdown.value;

    let sortedUsers = [];

    if (selectedSort === "Location_A_Z")
    {
        sortedUsers = [...usersData].sort((a, b) => a.Location.localeCompare(b.Location));
    }
    else if (selectedSort === "Location_Z_A")
    {
        sortedUsers = [...usersData].sort((a, b) => b.Location.localeCompare(a.Location));
    }
    else if (selectedSort === "FullName_A_Z")
    {
        sortedUsers = [...usersData].sort((a, b) => a.FullName.localeCompare(b.FullName));
    }
    else if (selectedSort === "FullName_Z_A")
    {
        sortedUsers = [...usersData].sort((a, b) => b.FullName.localeCompare(a.FullName));
    }

    updateTable(sortedUsers);
}

function updateTable(users)
{
    const tableBody = document.getElementById('requestTableBody');
    tableBody.innerHTML = '';
    let serial = 1;

    users.forEach(user =>
    {
        const row = document.createElement('tr');
        row.classList.add('mainRow');
        row.innerHTML = `
            <td>${serial}</td>
            <td>
                ${user.FullName}
                ${user.SpouseInfo ? '<br />' + user.SpouseInfo.FullName : ''}
            </td>
            <td>
                ${user.Children.length > 0 ? user.Children.map(child => `<div>${child.Name}</div>`).join('') : ''}
            </td>
            <td><span class="compact-location">${user.Location}</span></td>
        `;
        tableBody.appendChild(row);
        serial++;
    });
}
