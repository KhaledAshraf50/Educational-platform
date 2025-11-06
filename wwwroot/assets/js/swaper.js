var swiper = new Swiper(".mySwiper", {
  spaceBetween: 30,
  freeMode: true,
  loop: true, // يخلي السلايدر يلف دايرة
  autoplay: {
    delay: 2000,              // السرعة (كل 2 ثانية)
    disableOnInteraction: false, // يفضل شغال حتى لو المستخدم لمس السلايدر
  },
  pagination: {
    el: ".swiper-pagination",
    clickable: true,
  },
  breakpoints: {
    0: {
      slidesPerView: 1, // موبايل
    },
    768: {
      slidesPerView: 2, // تابلت
    },
    1024: {
      slidesPerView: 3, // ديسكتوب
    },
  },
});
document.getElementById("goNext").addEventListener("click", function() {
    swiper.slideNext();
});
document.getElementById("goPrev").addEventListener("click", function() {
    swiper.slidePrev();
});

var swiper2 = new Swiper(".swiper-subject", {
  spaceBetween: 30,
  freeMode: true,
  loop: true, // يخلي السلايدر يلف دايرة

  pagination: {
    el: ".swiper-pagination",
    clickable: true,
  },
  breakpoints: {
    0: {
      slidesPerView: 1, // موبايل
    },
    768: {
      slidesPerView: 2, // تابلت
    },
    1024: {
      slidesPerView: 3, // ديسكتوب
    },
  },
});
document.getElementById("goNext1").addEventListener("click", function() {
    swiper2.slideNext();
});
document.getElementById("goPrev1").addEventListener("click", function() {
    swiper2.slidePrev();
});











