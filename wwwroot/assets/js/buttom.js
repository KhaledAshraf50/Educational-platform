
document.addEventListener("DOMContentLoaded", () => {
  // ==================== عناصر الصفحات ====================
  const homeSection    = document.querySelector(".page_home1_m");
  const studentSection = document.querySelector(".Student_registration_page_m");
  const loginSection   = document.querySelector(".Login_page");
  const parentSection  = document.querySelector(".loginaspartient_page");
  const teacherSection = document.querySelector(".Teacher_registration_page_m");
  const forgetPasswordPage = document.querySelector(".forget-password-page");

  // ==================== أزرار التنقل ====================
  const createBtns     = document.querySelectorAll("#create_account, #create_account_link");
  const loginBtns      = document.querySelectorAll(".buttomloginhome.loginbtn, .login-link");
  const parentBtns     = document.querySelectorAll(".loginparnt, .parant_page");
  const goHomeBtn      = document.getElementById("goHomeBtn");
  const joinNowBtn     = document.querySelector(".cta-btn");
  const forgotLink     = document.getElementById("link_forgot_m");

  const closeBtn       = document.querySelector(".close-btn");

  // ==================== دوال مساعدة ====================
  function hideAllSections() {
    [homeSection, studentSection, loginSection, parentSection, teacherSection, forgetPasswordPage].forEach(sec => {
      if (sec) sec.style.display = "none";
    });
  }

  function showSection(section) {
    hideAllSections();
    if (section) section.style.display = "block";
    window.scrollTo({ top: 0, behavior: "smooth" }); // رجوع لأعلى الصفحة
  }

  function closeMenuIfOpen() {
    if (closeBtn) closeBtn.click();
  }

  // ==================== الأحداث ====================
  createBtns.forEach(btn => btn.addEventListener("click", e => { 
    e.preventDefault(); 
    showSection(studentSection); 
    closeMenuIfOpen();
  }));

  loginBtns.forEach(btn => btn.addEventListener("click", e => { 
    e.preventDefault(); 
    showSection(loginSection); 
    closeMenuIfOpen();
  }));

  parentBtns.forEach(btn => btn.addEventListener("click", e => { 
    e.preventDefault(); 
    showSection(parentSection); 
    closeMenuIfOpen();
  }));

  if (goHomeBtn) goHomeBtn.addEventListener("click", () => showSection(homeSection));

  if (joinNowBtn) joinNowBtn.addEventListener("click", e => {
    e.preventDefault();
    showSection(teacherSection);
  });

  if (forgotLink) forgotLink.addEventListener("click", e => {
    e.preventDefault();
    showSection(forgetPasswordPage);
  });

  // ==================== نسيت كلمة المرور ====================
  const forgotPasswordForm = document.getElementById("forgotPasswordForm");
  const verifyCodeForm     = document.getElementById("verifyCodeForm");
  const resetPasswordForm  = document.getElementById("resetPasswordForm");
  let generatedCode = "";

  if (forgotPasswordForm) {
    forgotPasswordForm.addEventListener("submit", e => {
      e.preventDefault();
      const email = document.getElementById("resetEmail").value.trim();
      if (!email) return alert("من فضلك أدخل البريد الإلكتروني");

      generatedCode = "123456"; // كود تجريبي
      alert("تم إرسال الكود إلى بريدك الإلكتروني ✅");
      forgotPasswordForm.style.display = "none";
      verifyCodeForm.style.display = "grid";
    });
  }

  if (verifyCodeForm) {
    verifyCodeForm.addEventListener("submit", e => {
      e.preventDefault();
      const codeInput = document.getElementById("verificationCode").value.trim();
      if (codeInput === generatedCode) {
        alert("✅ تم التحقق من الكود بنجاح");
        verifyCodeForm.style.display = "none";
        resetPasswordForm.style.display = "grid";
      } else {
        alert("❌ الكود غير صحيح");
      }
    });
  }

  if (resetPasswordForm) {
    resetPasswordForm.addEventListener("submit", e => {
      e.preventDefault();
      const pass = document.getElementById("newPassword").value.trim();
      const confirmPass = document.getElementById("confirmNewPassword").value.trim();

      if (pass.length < 6) return alert("كلمة المرور يجب أن تكون على الأقل 6 أحرف");
      if (pass !== confirmPass) return alert("كلمة المرور غير متطابقة");

      alert("✅ تم تغيير كلمة المرور بنجاح");
      resetPasswordForm.reset();
      showSection(loginSection); // رجوع لصفحة تسجيل الدخول
    });
  }

  // ==================== القائمة الجانبية ====================
  const menu      = document.querySelector('.buttomsmallscreen');
  const iconMenu  = document.querySelector('.iconmenu');
  if (iconMenu && menu && closeBtn) {
    iconMenu.addEventListener('click', () => menu.classList.add('active'));
    closeBtn.addEventListener('click', () => menu.classList.remove('active'));
  }

  // ==================== البحث ====================
  const btnSearch   = document.querySelector('.btnsearch');
  const searchBox   = document.querySelector('.search-box');
  const closeSearch = document.querySelector('.close-search');

  if (btnSearch && searchBox && closeSearch) {
    btnSearch.addEventListener('click', () => searchBox.style.display = 'flex');
    closeSearch.addEventListener('click', () => searchBox.style.display = 'none');
  }

  // ==================== عرض المزيد للكروت ====================
  const cards = document.querySelectorAll(".card-courses");
  const toggleButton = document.getElementById("toggleCards");
  if (cards.length && toggleButton) {
    let showingAll = false;
    cards.forEach((card, index) => { if (index > 2) card.classList.add("hidden"); });

    toggleButton.addEventListener("click", () => {
      showingAll = !showingAll;
      if (showingAll) {
        cards.forEach(card => card.classList.remove("hidden"));
        toggleButton.textContent = "عرض أقل";
      } else {
        cards.forEach((card, index) => { if (index > 2) card.classList.add("hidden"); });
        toggleButton.textContent = "عرض المزيد";
      }
    });
  }

 
  
});




