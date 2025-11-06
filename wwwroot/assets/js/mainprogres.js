function getColorByValue(value) {
  if (value < 50) return "#eb5757"; // أحمر
  if (value < 75) return "#E2B93B"; // أصفر
  return "#27AE60"; // أخضر
}

function createSemiCircle(containerId, percentage) {
  var bar = new ProgressBar.SemiCircle(containerId, {
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

// إنشاء الثلاث دوائر:
var semiContainerOverall = createSemiCircle("#semiContainer", 0.85); // 85%
var semiContainerExam = createSemiCircle("#semiContainerEXAM", 0.7); // 70%
var semiContainerTask = createSemiCircle("#semiContainerTASK", 0.9); // 90%

// const notification = document.getElementById("notification");
// const notificationBtn = document.getElementById("notification-btn");
// notificationBtn.addEventListener("click", function () {
//   notification.classList.toggle("d-none");
// });
