const childrenDropdown = document.getElementById('childrenDropdown');
const childPages = document.querySelectorAll('.child-page');
const options = document.querySelectorAll("option");
childrenDropdown.addEventListener('change', function () {
    childPages.forEach(page => page.style.display = 'none');

    const selectedChild = this.value;
    console.log(selectedChild)
    if (selectedChild) {
        document.getElementById(selectedChild + 'Page').style.display = 'block';
    }
});
options.forEach(opt => {
    opt.onmouseover = function () {
        opt.style.backgroundColor = "red";
    }
})