// main.js

// جلب العناصر
const governorateSelect = document.getElementById("governorate");
const adminSelect = document.getElementById("admin");

// أول ما يفتح الصفحة نملأ المحافظات
window.addEventListener("DOMContentLoaded", function () {
  for (const govKey in admins) {
    const option = document.createElement("option");
    option.value = govKey;
    option.textContent = getGovernorateName(govKey);
    governorateSelect.appendChild(option);
  }

  // خليه يعطل الإدارات لحد ما المستخدم يختار محافظة
  adminSelect.disabled = true;
});

// لما يختار محافظة
governorateSelect.addEventListener("change", function () {
  const selectedGov = this.value;

  // نفرغ حقل الإدارات
  adminSelect.innerHTML = "<option value='' disabled selected hidden> </option>"

  if (selectedGov && admins[selectedGov]) {
    admins[selectedGov].forEach(admin => {
      const option = document.createElement("option");
      option.value = admin;
      option.textContent = admin;
      adminSelect.appendChild(option);
    });

    adminSelect.disabled = false;
  } else {
    adminSelect.disabled = true;
  }
});

// دالة ترجمة أسماء المحافظات بالعربي
function getGovernorateName(key) {
  const names = {
    cairo: "القاهرة",
    giza: "الجيزة",
    alex: "الإسكندرية",
    qalyubia: "القليوبية",
    sharqia: "الشرقية",
    dakahlia: "الدقهلية",
    menoufia: "المنوفية",
    gharbia: "الغربية",
    kafrElsheikh: "كفر الشيخ",
    beniSuef: "بني سويف",
    minya: "المنيا",
    assiut: "أسيوط",
    sohag: "سوهاج",
    qena: "قنا",
    luxor: "الأقصر",
    aswan: "أسوان",
    redSea: "البحر الأحمر",
    matrouh: "مطروح",
    newValley: "الوادي الجديد",
    northSinai: "شمال سيناء",
    southSinai: "جنوب سيناء"
  };
  return names[key] || key;
}

// ________________________________________________________________________________
// -----------------------------------------------------------------------------
// ------------------------------------------------------------------------------------
document.addEventListener("DOMContentLoaded", () => {
  // كل الفورمات
  const forms = {
    student: document.querySelector("#registerForm"),
    parent: document.querySelector("#parentRegisterForm"),
    teacher: document.querySelector("#teacherRegisterForm"),
    login: document.querySelector("#form_login_m")
  };

  // اربط كل فورم مع التحقق
  Object.values(forms).forEach((form) => {
    if (!form) return;
    form.dataset.submitted = "false"; // ✅ flag مبدئي
    attachLiveValidation(form);
    form.addEventListener("submit", (e) => {
      e.preventDefault();
      form.dataset.submitted = "true"; // ✅ خلي الفورم في حالة submitted
      if (validateForm(form)) form.submit();
    });
  });
});

// ✅ تحقق مباشر لكل input و select
function attachLiveValidation(form) {
  form.querySelectorAll("input, select").forEach((field) => {
    field.addEventListener("blur", () => {
      if (form.dataset.submitted === "true") validateSingleField(field); 
    });
    field.addEventListener("input", () => clearError(getErrorSpan(field)));
  });
}

// ✅ دوال التحقق الأساسية
const isArabicText = (v) => /^[\u0600-\u06FF\s]+$/.test(v.trim());
const isNumeric = (v) => /^01\d{9}$/.test(v.trim());
const isValidEmail = (v) => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(v.trim());
const isStrongPassword = (v) => /^(?=.*[A-Z])(?=.*\d).{8,}$/.test(v);

const showError = (span, msg) => {
  if (!span) return;
  span.innerText = msg;
  span.style.display = "block";
  const input = span.closest(".form-group").querySelector("input, select");
  if (input) input.classList.add("input-error");
};

const clearError = (span) => {
  if (!span) return;
  span.innerText = "";
  span.style.display = "none";
  const input = span.closest(".form-group").querySelector("input, select");
  if (input) input.classList.remove("input-error");
};

function getErrorSpan(input) {
  return input.closest("div").parentElement.querySelector(".error-msg");
}

// ✅ دالة عامة للتحقق من حقل واحد
function validateSingleField(input) {
  const value = input.value.trim();
  const errorSpan = getErrorSpan(input);
  if (!errorSpan) return true;

  if (input.type === "text") {
    if (!value) return showError(errorSpan, "هذا الحقل مطلوب"), false;
    if (!isArabicText(value)) return showError(errorSpan, "أدخل الاسم باللغة العربية فقط"), false;
  }

  if (input.type === "tel") {
    if (!value) return showError(errorSpan, "رقم الهاتف مطلوب"), false;
    if (!isNumeric(value)) return showError(errorSpan, "أدخل رقم صحيح يبدأ بـ 01 ويكون مكون من 11 رقم"), false;
  }

  if (input.type === "email") {
    if (!value) return showError(errorSpan, "البريد الإلكتروني مطلوب"), false;
    if (!isValidEmail(value)) return showError(errorSpan, "أدخل بريد إلكتروني صحيح"), false;
  }

  if (input.type === "password" && input.id !== "confirm-password") {
    if (!value) return showError(errorSpan, "كلمة المرور مطلوبة"), false;
    if (!isStrongPassword(value)) return showError(errorSpan, "يجب أن تحتوي على 8 أحرف على الأقل، رقم وحرف كبير"), false;
  }

  if (input.id === "confirm-password") {
    const password = input.closest("form").querySelector("#password");
    if (password && value !== password.value) return showError(errorSpan, "كلمة المرور غير متطابقة"), false;
  }

  if (input.tagName.toLowerCase() === "select" && !value) {
    return showError(errorSpan, "اختيار هذا الحقل مطلوب"), false;
  }

  clearError(errorSpan);
  return true;
}

// ✅ دالة عامة للتحقق من كل الفورم
function validateForm(form) {
  let isValid = true;
  form.querySelectorAll("input, select").forEach((field) => {
    if (!validateSingleField(field)) isValid = false;
  });
  return isValid;
}

document.addEventListener("DOMContentLoaded", () => {
  const loginLinks = document.querySelectorAll(".subtitle_link_login_as_user");
  const loginForm = document.querySelector(".Login_page"); // لو ده ID خليه #Login_page

  // كل الصفحات اللي ممكن تكون ظاهرة
  const allSections = document.querySelectorAll(
    ".page_home1_m, .Student_registration_page_m, .loginaspartient_page, .form_login_m, .teacher_page_m"
  );

  if (loginLinks.length && loginForm) {
    loginLinks.forEach((link) => {
      link.addEventListener("click", (e) => {
        e.preventDefault();

        // اخفي كل الصفحات المعروضة
        allSections.forEach((sec) => (sec.style.display = "none"));

        // اظهر صفحة تسجيل الدخول فقط
        loginForm.style.display = "block";
      });
    });
  }
});


document.addEventListener("DOMContentLoaded", () => {
  const loginLinks = document.querySelectorAll("#link_create_m");
  const loginForm = document.querySelector(".Student_registration_page_m"); // لو ده ID خليه #Login_page

  // كل الصفحات اللي ممكن تكون ظاهرة
  const allSections = document.querySelectorAll(
    ".page_home1_m,.loginaspartient_page, .form_login_m, .teacher_page_m ,.Login_page"
  );

  if (loginLinks.length && loginForm) {
    loginLinks.forEach((link) => {
      link.addEventListener("click", (e) => {
        e.preventDefault();

        // اخفي كل الصفحات المعروضة
        allSections.forEach((sec) => (sec.style.display = "none"));

        // اظهر صفحة تسجيل الدخول فقط
        loginForm.style.display = "block";
      });
    });
  }
});


// ------------------------------------
document.addEventListener("DOMContentLoaded", () => {
  const alsaf = document.getElementById("alsaf");
  const shuebaDiv = document.querySelector(".shueba");
  const shuebaSelect = document.getElementById("shueba");

  // إخفاء الشعبة في البداية
  shuebaDiv.style.display = "none";

  alsaf.addEventListener("change", () => {
    const value = alsaf.value;

    if (value === "1") {
      // الصف الأول الثانوي → إخفاء الشعبة
      shuebaDiv.style.display = "none";
      shuebaSelect.value = "";
    } 
    else if (value === "2") {
      // الصف الثاني الثانوي → إظهار علمي وأدبي فقط
      shuebaDiv.style.display = "block";
      Array.from(shuebaSelect.options).forEach(opt => {
        if (["science", "arts"].includes(opt.value) || opt.value === "") {
          opt.style.display = "block";
        } else {
          opt.style.display = "none";
        }
      });
      shuebaSelect.value = "";
    } 
    else if (value === "3") {
      // الصف الثالث الثانوي → إظهار علمي علوم، علمي رياضة، أدبي فقط
      shuebaDiv.style.display = "block";
      Array.from(shuebaSelect.options).forEach(opt => {
        if (["science-sc", "science-ma", "arts"].includes(opt.value) || opt.value === "") {
          opt.style.display = "block";
        } else {
          opt.style.display = "none";
        }
      });
      shuebaSelect.value = "";
    }
  });
});












document.addEventListener("DOMContentLoaded", function () {
  const genderSelect = document.getElementById("gender");
  const studentForm = document.getElementById("registerForm");
  const teacherForm = document.getElementById("teacherRegisterForm");
  const parentForm = document.getElementById("parentRegisterForm");

  // دالة لإخفاء كل الفورمات
  function hideAllForms() {
    studentForm.style.display = "none";
    teacherForm.style.display = "none";
    parentForm.style.display = "none";
  }

  // أول ما الصفحة تفتح: الإختيار الافتراضي = طالب
  genderSelect.selectedIndex = 1; // أول option بعد الفاضي
  hideAllForms();
  studentForm.style.display = "grid"; // أو block لو انت مش مستخدم grid

  // لما المستخدم يغير النوع
  genderSelect.addEventListener("change", function () {
    hideAllForms();

const selected = genderSelect.value;
if (selected === "student") studentForm.style.display = "grid";
else if (selected === "instructor") teacherForm.style.display = "grid";
else if (selected === "parent") parentForm.style.display = "grid";
  });
});

