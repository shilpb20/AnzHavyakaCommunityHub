let usersData = [];
let filteredData = [];
let currentPage = 1;
let rowsPerPage = 3;

document.addEventListener("DOMContentLoaded", function () {
    if (window.usersData && Array.isArray(window.usersData)) {
        usersData = window.usersData;
        filteredData = [...usersData];
        renderTable();
    } else {
        console.error("Error: usersData is not available or is not in the correct format.");
    }
});

function renderTable() {
    const tableBody = document.getElementById('requestTableBody');
    tableBody.innerHTML = '';

    const startIdx = (currentPage - 1) * rowsPerPage;
    const endIdx = currentPage * rowsPerPage;
    const paginatedData = filteredData.slice(startIdx, endIdx);

    paginatedData.forEach((user, index) => {
        const childrenHtml = (user.Children && user.Children.length > 0)
            ? user.Children.map(c => `<div>${c.Name}</div>`).join('')
            : 'N/A';

        const row = `<tr>
            <td>${startIdx + index + 1}</td>
            <td>${user.FullName || ''}</td>
            <td>${user.ContactNumber || ''}<br>${user.Email || ''}</td>
            <td>${user.Location || ''}</td>
            <td>${user.SpouseInfo?.FullName || ''}</td>
            <td>${user.SpouseInfo ? `${user.SpouseInfo.Email || ''}<br>${user.SpouseInfo.ContactNumber || ''}` : ''}</td>
            <td>${user.SpouseInfo?.Location || ''}</td>
            <td>${childrenHtml}</td>
        </tr>`;

        tableBody.innerHTML += row;
    });

    initializePagination();
}

function initializePagination() {
    const paginationContainer = document.getElementById('requestTableBody-pagination');
    paginationContainer.innerHTML = '';

    const totalPages = Math.ceil(filteredData.length / rowsPerPage);
    if (totalPages <= 1) {
        paginationContainer.style.display = 'none';
        return;
    } else {
        paginationContainer.style.display = 'flex';
    }

    const prev = document.createElement('button');
    prev.textContent = '<<';
    prev.disabled = currentPage === 1;
    prev.onclick = () => {
        if (currentPage > 1) {
            currentPage--;
            renderTable();
        }
    };
    paginationContainer.appendChild(prev);

    for (let i = 1; i <= totalPages; i++) {
        const btn = document.createElement('button');
        btn.textContent = i;
        btn.disabled = i === currentPage;
        btn.onclick = () => {
            currentPage = i;
            renderTable();
        };
        paginationContainer.appendChild(btn);
    }

    const next = document.createElement('button');
    next.textContent = '>>';
    next.disabled = currentPage === totalPages;
    next.onclick = () => {
        if (currentPage < totalPages) {
            currentPage++;
            renderTable();
        }
    };
    paginationContainer.appendChild(next);
}
