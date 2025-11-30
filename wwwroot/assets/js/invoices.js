document.addEventListener("DOMContentLoaded", function () {

    const searchInput = document.getElementById("invoiceSearch");
    const tableRows = document.querySelectorAll("table tbody tr");

    searchInput.addEventListener("keyup", function () {

        const searchValue = searchInput.value.toLowerCase();

        tableRows.forEach(row => {
            const rowText = row.textContent.toLowerCase();

            if (rowText.includes(searchValue)) {
                row.style.display = "";
            } else {
                row.style.display = "none";
            }
        });

    });

});
