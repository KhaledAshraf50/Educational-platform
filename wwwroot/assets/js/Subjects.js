function filterCourse(subject) {
    const rowsCourse = document.querySelectorAll("#courseTable tr");
    rowsCourse.forEach(row => {
        if (subject === "all" || row.dataset.subject === subject) {
            row.style.display = "";
        } else {
            row.style.display = "none";
        }
    });
}
const buttons = document.querySelectorAll(".btns button")
buttons.forEach(btn => {
    btn.addEventListener("click", () => {
        buttons.forEach(b => b.classList.remove("active"));
        btn.classList.add("active");
    })
})
document.addEventListener("DOMContentLoaded", function () {

    const searchInput = document.getElementById("courseSearch");
    const rows = document.querySelectorAll("#courseTable tr");

    searchInput.addEventListener("keyup", function () {

        const value = searchInput.value.toLowerCase();

        rows.forEach(row => {
            const rowText = row.textContent.toLowerCase();

            if (rowText.includes(value)) {
                row.style.display = "";
            } else {
                row.style.display = "none";
            }
        });

    });

});
document.addEventListener("DOMContentLoaded", function () {

    const searchInput = document.getElementById("reportSearch");
    const rows = document.querySelectorAll("#courseTable tr");

    searchInput.addEventListener("keyup", function () {

        const value = searchInput.value.toLowerCase();

        rows.forEach(row => {
            const text = row.textContent.toLowerCase();

            if (text.includes(value)) {
                row.style.display = "";
            } else {
                row.style.display = "none";
            }
        });

    });

});
