// notification
const notification = document.getElementById("notification");
const notificationBtn = document.getElementById("notification-btn");
notificationBtn.addEventListener("click", function () {
    notification.classList.toggle("d-none");
});
function toggleSidebar() {
    const sidebar = document.getElementById("sidebar");
    const overlay = document.getElementById("overlay");
    sidebar.classList.toggle("active");

    if (overlay.classList.contains("active")) {
        overlay.classList.remove("active");
        setTimeout(() => (overlay.style.display = "none"), 300);
    } else {
        overlay.style.display = "block";
        setTimeout(() => overlay.classList.add("active"), 10);
    }
}

document.querySelectorAll(".sidebar a").forEach((link) => {
    link.addEventListener("click", () => {
        if (window.innerWidth <= 768) {
            toggleSidebar();
        }
    });
});

// Cards animation on load + scroll
// const cards = document.querySelectorAll(".card");

// function showCardsOnScroll() {
//   const triggerBottom = window.innerHeight * 0.85;
//   cards.forEach((card) => {
//     const cardTop = card.getBoundingClientRect().top;
//     if (cardTop < triggerBottom) {
//       card.classList.add("show");
//     }
//   });
// }

// window.addEventListener("DOMContentLoaded", showCardsOnScroll);
// window.addEventListener("scroll", showCardsOnScroll);
// const sidebar = document.querySelector(".side");
// const main = document.getElementById("mainpage");
// const Subjectspage = document.getElementById("Subjectspage");
// const Reportspage = document.getElementById("Reportspage");
// const Billingpage = document.getElementById("Billingpage");
// const Notificationspage = document.getElementById("Notificationspage");
// const Settingspage = document.getElementById("Settingspage");
// console.log(Settingspage)

// let queryss = document.querySelectorAll(".dashbtn");
// let bodycontent = document.querySelectorAll(".bodycontent");
// bodycontent.forEach((btn) => {
//   btn.addEventListener("click",function () {
//     bodycontent.forEach((b) => b.classList.remove("active"));

//   })
// }

// );
// queryss.forEach((btn) => {
//   btn.addEventListener("click", function () {
//     queryss.forEach((b) => b.classList.remove("active", "text-white"));
//     bodycontent.forEach((b) => {
//       b.classList.remove("active");
//     });

//     this.classList.add("active", "text-white");
//     if (
//       this.classList.contains("#mainpage") &&
//       this.classList.contains("active", "text-white")
//     ) {
//       main.classList.add("active");
//     } else if (
//       this.classList.contains("#Subjectspage") &&
//       this.classList.contains("active", "text-white")
//     ) {
//       Subjectspage.classList.add("active");
//     } else if (
//       this.classList.contains("#Reportspage") &&
//       this.classList.contains("active", "text-white")
//     ) {
//       Reportspage.classList.add("active");
//     } else if (
//       this.classList.contains("#Billingpage") &&
//       this.classList.contains("active", "text-white")
//     ) {
//       Billingpage.classList.add("active");
//     } else if (
//       this.classList.contains("#Notificationspage") &&
//       this.classList.contains("active", "text-white")
//     ) {
//       Notificationspage.classList.add("active");
//     } else if (
//       this.classList.contains("#Settingspage") &&
//       this.classList.contains("active", "text-white")
//     ) {
//       Settingspage.classList.add("active");
//     }
//   });
// });

// let dropdownItems = document.querySelectorAll(".dropdown-item");
// let subjectInput = document.getElementById("studentName");
// let tables = document.querySelectorAll(".subject-table");

// dropdownItems.forEach((item) => {
//   item.addEventListener("click", function (e) {
//     e.preventDefault();

//     // النص في input
//     subjectInput.value = this.textContent.trim();

//     // أخفي كل الجداول
//     tables.forEach((t) => t.classList.add("d-none"));

//     // أظهر الجدول المختار
//     let subjectId = this.getAttribute("data-subject");
//     document.getElementById(subjectId).classList.remove("d-none");
//   });
// });
// // ---------------------------------------------

// // الملف الشخصي
// const modalOverlay = document.getElementById("modalOverlay");
// const modalInput = document.getElementById("modalInput");
// const modalTitle = document.getElementById("modalTitle");
// const modalLabel = document.getElementById("modalLabel");
// let currentField = null;

// // لما تدوس على أيقونة القلم
// document.querySelectorAll(".edit-btn").forEach((btn) => {
//   btn.addEventListener("click", function () {
//     currentField = this.closest(".feild").querySelector("h1");
//     const fieldTitle = this.closest(".feild").querySelector("p").innerText;

//     modalTitle.innerText = "تعديل " + fieldTitle;
//     modalLabel.innerText = fieldTitle;
//     modalInput.value = currentField.innerText;

//     modalOverlay.style.display = "flex"; // يظهر المودال
//     document.body.classList.add("modal-open");
//     modalOverlay.classList.add("active");
//     modalInput.focus();
//   });
// });

// لما تدوس حفظ
// document.getElementById("saveModal").addEventListener("click", () => {
//   if (currentField) {
//     currentField.innerText = modalInput.value;
//   }
//   closeModal();
// });

// // لما تدوس إلغاء
// document.getElementById("cancelModal").addEventListener("click", closeModal);

// // لما تدوس بره المودال
// modalOverlay.addEventListener("click", (e) => {
//   if (e.target === modalOverlay) closeModal();
// });

// function closeModal() {
//   modalOverlay.style.display = "none"; // يخفي المودال
//   document.body.classList.remove("modal-open");
// }
// ___________________________________
// XXXXXX

// const sidebar = document.querySelector(".side");
const closeBtn = document.querySelector(".X");
const menuBtn = document.querySelector(".menu-btn2");
// إغلاق القائمة
closeBtn.addEventListener("click", () => {
    sidebar.style.display = "none";
    menuBtn.style.display = "block";
});

// فتح القائمة
menuBtn.addEventListener("click", () => {
    sidebar.style.display = "block";
    menuBtn.style.display = "none";
});
// ----------------------------
// const anchorNav = document.querySelector(".navbar-brand");
// // دالة لحفظ الاختيار
// function setActive(text) {
//   anchorNav.innerText = text;
//   localStorage.setItem("activeNav", text); // حفظ في localStorage
// }
// document.querySelectorAll(".sidebar a").forEach((ele) => {
//   ele.onclick = function () {
//     setActive(ele.innerText);
//   };
// });
// window.onload = function () {
//   const saved = localStorage.getItem("activeNav");
//   if (saved) {
//     anchorNav.innerText = saved;
//   }
// };

// function filterCourse(subject) {
//   const rowsCourse = document.querySelectorAll("#courseTable tr");
//   rowsCourse.forEach((row) => {
//     if (subject === "all" || row.dataset.subject === subject) {
//       row.style.display = "";
//     } else {
//       row.style.display = "none";
//     }
//   });
// }

// ----------------------
// const tabs = document.querySelectorAll(".settings-tabs button");
// const forms = document.querySelectorAll(".settings-form");

// tabs.forEach((tab, index) => {
//   tab.onclick = function () {
//     // شيل active من كل الأزرار
//     tabs.forEach((t) => t.classList.remove("active"));
//     // ضيف active للزرار اللي اتداس
//     this.classList.add("active");

//     // شيل active من كل الفورمز
//     forms.forEach((f) => f.classList.remove("active"));
//     // ضيف active للفورم اللي ليه نفس الترتيب
//     forms[index].classList.add("active");
//   };
// });
