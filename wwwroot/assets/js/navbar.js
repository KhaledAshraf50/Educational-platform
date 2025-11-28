const btn = document.getElementById('avatarBtn');
const dropdown = document.getElementById('avatarDropdown');
const wrap = document.getElementById('avatarWrapper');

btn.addEventListener('click', function (e) {
    e.stopPropagation();
    dropdown.classList.toggle('show');
});

// إغلاق عند الضغط خارج القائمة
document.addEventListener('click', function (e) {
    if (!wrap.contains(e.target)) {
        dropdown.classList.remove('show');
    }
});