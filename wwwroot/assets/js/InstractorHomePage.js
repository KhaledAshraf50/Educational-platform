let courses = [
    { title: "كورس اللغة العربية", grade: "الصف الثالث الثانوي", price: 250, desc: "هذا الكورس يشرح قواعد النحو بالتفصيل مع تمارين تفاعلية. uuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuu", img: "https://via.placeholder.com/250x150" }
];

const coursesContainer = document.getElementById('coursesContainer');
const noCoursesDiv = document.getElementById('noCourses');
const totalCoursesEl = document.getElementById('totalCourses');
const availableCoursesEl = document.getElementById('availableCourses');

function renderCourses(filterGrade = '', searchTerm = '') {
    coursesContainer.innerHTML = '';
    let filtered = courses.filter(c =>
        (filterGrade === '' || c.grade === filterGrade) &&
        (searchTerm === '' || c.title.includes(searchTerm))
    );

    if (filtered.length === 0) noCoursesDiv.style.display = 'block';
    else noCoursesDiv.style.display = 'none';

    filtered.forEach((c, index) => {
        let card = document.createElement('div');
        card.className = 'col-lg-3 col-md-4 col-sm-6';
        card.innerHTML = `
          <div class="card course-card">
            <img src="${c.img}" class="card-img-top" alt="${c.title}">
            <div class="card-body">
              <h6 class="fw-bold">${c.title}</h6>
              <p class="mb-1"><strong>الصف:</strong> ${c.grade}</p>
              
              <p class="mb-1"><strong>السعر:</strong> ${c.price} جنيه</p>

              <!-- قسم الوصف منفصل -->
              <p><strong>الوصف:</strong></p>
              <p class="course-desc" id="desc-${index}">${c.desc}</p>
              <span class="read-more" id="read-more-${index}" onclick="toggleDesc(${index})">قراءة المزيد</span>

              <div class="d-flex justify-content-center mt-3">
               <button class="btn btn-sm text-white px-4" 
               style="background-color: #092C4C; border: none;" 
               onclick="openCourseDetails(${index})">
               <i class="fa-solid fa-eye me-1"></i> الدخول لعرض التفاصيل
               </button>
             </div>

            </div>
          </div>
        `;
        coursesContainer.appendChild(card);
    });

    updateStats();
    updateClassCounts();
}

function updateStats() {
    totalCoursesEl.innerText = courses.length;
    availableCoursesEl.innerText = courses.length;
}

function updateClassCounts() {
    document.querySelectorAll('.class-card').forEach(card => {
        const grade = card.querySelector('h6').innerText;
        const count = courses.filter(c => c.grade === grade).length;
        card.querySelector('.course-count').innerText = count;
    });
}

document.getElementById('saveCourseBtn').addEventListener('click', () => {
    const title = document.getElementById('courseTitle').value;
    const grade = document.getElementById('courseGrade').value;
    const subject = document.getElementById('courseSubject').value;
    const price = document.getElementById('coursePrice').value;
    const desc = document.getElementById('courseDesc').value;
    const img = document.getElementById('courseImg').value || 'https://via.placeholder.com/250x150';

    if (title && grade) {
        courses.push({ title, grade, subject, price, desc, img });
        renderCourses();
        new bootstrap.Toast(document.getElementById('successToast')).show();
        document.getElementById('addCourseForm').reset();
        bootstrap.Modal.getInstance(document.getElementById('addCourseModal')).hide();
    }
});

document.getElementById('searchInput').addEventListener('input', e => {
    renderCourses('', e.target.value);
});

document.querySelectorAll('.browse-btn').forEach(btn => {
    btn.addEventListener('click', e => {
        const grade = e.target.closest('.class-card').querySelector('h6').innerText;
        renderCourses(grade);
    });
});
document.querySelectorAll('.add-course-grade-btn').forEach(btn => {
    btn.addEventListener('click', e => {
        const grade = e.target.closest('.class-card').querySelector('h6').innerText;
        document.getElementById('courseGrade').value = grade; // يضبط الصف تلقائيًا
        const modal = new bootstrap.Modal(document.getElementById('addCourseModal'));
        modal.show();
    });
});


function deleteCourse(index) {
    if (confirm('هل أنت متأكد من حذف هذا الكورس؟')) {
        courses.splice(index, 1);
        renderCourses();
    }
}

function viewCourse(index) {
    const c = courses[index];
    document.getElementById('viewCourseTitle').innerText = c.title;
    document.getElementById('viewCourseImg').src = c.img;
    document.getElementById('viewCourseGrade').innerText = c.grade;
    document.getElementById('viewCourseSubject').innerText = c.subject;
    document.getElementById('viewCoursePrice').innerText = c.price;
    document.getElementById('viewCourseDesc').innerText = c.desc;
    const modal = new bootstrap.Modal(document.getElementById('viewCourseModal'));
    modal.show();
}

function editCourse(index) {
    alert('ميزة التعديل سيتم إضافتها لاحقًا مع MVC.');
}

// دالة توسيع/تصغير الوصف
function toggleDesc(index) {
    const descEl = document.getElementById(`desc-${index}`);
    const readMoreEl = document.getElementById(`read-more-${index}`);
    descEl.classList.toggle('expanded');
    readMoreEl.innerText = descEl.classList.contains('expanded') ? 'قراءة أقل' : 'قراءة المزيد';
}

// Initial render
renderCourses();