(function () {
    const tabsContainer = document.querySelector(".settings-tabs");
<<<<<<< HEAD
    if (!tabsContainer) return;
=======
    if (!tabsContainer) return; 
>>>>>>> origin/dev

    const tabs = Array.from(tabsContainer.querySelectorAll("button"));
    const forms = Array.from(document.querySelectorAll(".settings-form"));

    function activate(index) {
        tabs.forEach(t => t.classList.remove("active"));
        forms.forEach(f => f.classList.remove("active"));

        if (tabs[index]) tabs[index].classList.add("active");
        if (forms[index]) forms[index].classList.add("active");
    }

    tabs.forEach((tab, i) => {
        tab.addEventListener("click", function (e) {
            e.preventDefault();
            activate(i);
        }, { passive: true });
    });
<<<<<<< HEAD
})();
=======
})();
>>>>>>> origin/dev
