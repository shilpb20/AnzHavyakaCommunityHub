const itemsPerPage = 10;

function renderTable(page)
{
    const start = (page - 1) * itemsPerPage;
    const end = start + itemsPerPage;
    const pagedData = usersData.slice(start, end);

    const tableBody = document.getElementById('requestTableBody');
    tableBody.innerHTML = '';

    let serial = start + 1;

    pagedData.forEach(user => {
        const row = document.createElement('tr');
        row.classList.add('mainRow');

        row.innerHTML = `
            <td>${serial}</td>
            <td>
                ${user.FullName}
                ${user.SpouseInfo ? `<br />${user.SpouseInfo.FullName}` : ''}
            </td>
            <td>
                ${user.Children.map(child => `<div>${child.Name}</div>`).join('')}
            </td>
            <td><span class="compact-location">${user.Location}</span></td>
        `;

        tableBody.appendChild(row);
        serial++;
    });

    renderPagination(usersData.length, page);
}

function renderPagination(totalItems, currentPage) {
    const totalPages = Math.ceil(totalItems / itemsPerPage);
    const paginationContainer = document.getElementById('pagination-container');
    paginationContainer.innerHTML = '';

    const prevButton = document.createElement('button');
    prevButton.classList.add('btn', 'btn-primary');
    prevButton.innerText = 'Previous';
    prevButton.disabled = currentPage === 1;
    prevButton.addEventListener('click', () => {
        if (currentPage > 1) renderTable(currentPage - 1);
    });

    const nextButton = document.createElement('button');
    nextButton.classList.add('btn', 'btn-primary');
    nextButton.innerText = 'Next';
    nextButton.disabled = currentPage === totalPages;
    nextButton.addEventListener('click', () => {
        if (currentPage < totalPages) renderTable(currentPage + 1);
    });

    paginationContainer.appendChild(prevButton);
    paginationContainer.appendChild(nextButton);
}
