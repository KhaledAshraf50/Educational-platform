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