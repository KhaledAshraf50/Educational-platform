document.addEventListener("DOMContentLoaded", function () {

    const progressBars = {};

    function initProgress(containerId, percentage) {
        const el = document.getElementById(containerId);
        if (!el || isNaN(percentage)) return;

        // إزالة أي semi-circle موجود
        if (progressBars[containerId]) {
            progressBars[containerId].destroy();
        }

        progressBars[containerId] = new ProgressBar.SemiCircle(el, {
            strokeWidth: 10,
            trailColor: "#eee",
            trailWidth: 10,
            easing: "easeInOut",
            duration: 1400,
            text: { value: "", alignToBottom: false },
            step: function (state, bar) {
                const value = Math.round(bar.value() * 100);
                bar.path.setAttribute("stroke", value < 50 ? "#eb5757" : value < 75 ? "#E2B93B" : "#27AE60");
                bar.setText(value + "%");
            }
        });

        progressBars[containerId].text.style.fontFamily = "DIN Next LT Arabic";
        progressBars[containerId].animate(percentage);
    }

    // ===== حالة ولي الأمر =====
    const dropdown = document.getElementById("childrenDropdown");
    const childPages = document.querySelectorAll(".child-page");

    if (dropdown) {
        // event listener عند اختيار طالب
        dropdown.addEventListener("change", function () {
            const studentId = this.value;

            childPages.forEach(page => page.style.display = "none");
            const selectedPage = document.getElementById(studentId);
            if (!selectedPage) return;

            selectedPage.style.display = "block";

            // استخدام dict لكل طالب
            initProgress(`semiContainer_${studentId}`, overallDict[studentId] / 100);
            initProgress(`semiContainerEXAM_${studentId}`, examDict[studentId] / 100);
            initProgress(`semiContainerTASK_${studentId}`, taskDict[studentId] / 100);
        });
    }

    // ===== حالة الطالب الفردي =====
    const overallContainer = document.querySelector("[id^='semiContainer_']");
    if (overallContainer && typeof overallProgress !== 'undefined') {
        const studentId = overallContainer.id.split("_")[1];

        initProgress(`semiContainer_${studentId}`, overallProgress / 100);
        initProgress(`semiContainerEXAM_${studentId}`, examProgress / 100);
        initProgress(`semiContainerTASK_${studentId}`, taskProgress / 100);
    }

});
function renderSemiProgress(value) {
    const container = document.getElementById("semiContainer");
    container.innerHTML = ""; // Clear old chart

    // إنشاء div داخلي عشان الـ progressbar.js
    const child = document.createElement("div");
    child.id = "semi_single";
    child.style.width = "200px";
    child.style.margin = "auto";
    container.appendChild(child);

    // رسم السيمي سيركل
    var bar = new ProgressBar.SemiCircle("#semi_single", {
        strokeWidth: 8,
        trailWidth: 8,
        duration: 1400,
        easing: "easeInOut",
        text: {
            value: "",
            alignToBottom: false
        },
        step: function (state, bar) {
            const v = Math.round(bar.value() * 100);
            bar.path.setAttribute("stroke", v < 50 ? "#eb5757" : v < 75 ? "#E2B93B" : "#27AE60");
            bar.setText(v + "%");
        }
    });

    bar.text.style.fontFamily = "DIN Next LT Arabic";
    bar.animate(value / 100);
}
