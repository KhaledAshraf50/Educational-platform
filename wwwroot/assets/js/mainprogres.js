function getColorByValue(value) {
  if (value < 50) return "#eb5757"; // أحمر
  if (value < 75) return "#E2B93B"; // أصفر
  return "#27AE60"; // أخضر
}

function createSemiCircle(el, percentage) {
  var bar = new ProgressBar.SemiCircle(el, {
    strokeWidth: 10,
    trailColor: "#eee",
    trailWidth: 10,
    easing: "easeInOut",
    duration: 1400,
    svgStyle: null,
    text: {
      value: "",
      alignToBottom: false,
    },
    step: function (state, bar) {
      var value = Math.round(bar.value() * 100);
      bar.path.setAttribute("stroke", getColorByValue(value));
      bar.setText(value + "%");
    },
  });

  bar.text.style.fontFamily = "DIN Next LT Arabic";
  bar.animate(percentage); // يبدأ التحريك للنسبة المطلوبة
  return bar;
}
function initProgress(containerId, percentage) {
    const el = document.querySelector(containerId);
    if (el) {
        createSemiCircle(el, percentage);
    }
}
// Child 1
initProgress("#semiContainer", 0.85);         // 85%
initProgress("#semiContainerEXAM", 0.70);     // 70%
initProgress("#semiContainerTASK", 0.90);     // 90%
// Child 2
initProgress("#semiContainer2", 0.60);        // 60%
initProgress("#semiContainerEXAM2", 0.40);    // 40%
initProgress("#semiContainerTASK2", 0.75);    // 75%



// const notification = document.getElementById("notification");
// const notificationBtn = document.getElementById("notification-btn");
// notificationBtn.addEventListener("click", function () {
//   notification.classList.toggle("d-none");
// });
