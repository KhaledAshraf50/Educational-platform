const labels = [
  "يناير",
  "فبراير",
  "مارس",
  "أبريل",
  "مايو",
  "يونيو",
  "يوليو",
  "أغسطس",
  "سبتمبر",
  "أكتوبر",
];
const earningsData = [
  12000, 15000, 10000, 18000, 22000, 20000, 24000, 26000, 21000, 85000,
];

new Chart(document.getElementById("earningsChart"), {
  type: "line",
  data: {
    labels: labels,
    datasets: [
      {
        label: "أرباح (EGP)",
        data: earningsData,
        tension: 0.3,
        fill: true,
        backgroundColor: "rgba(59,130,246,0.12)",
        borderColor: "rgba(59,130,246,0.95)",
        pointRadius: 3,
      },
    ],
  },
  options: {
    responsive: true,
    plugins: { legend: { display: false } },
    scales: { y: { beginAtZero: true } },
  },
});

new Chart(document.getElementById("usersChart"), {
  type: "doughnut",
  data: {
    labels: ["طلاب", "معلمين", "أولياء أمور"],
    datasets: [
      {
        data: [70, 25, 5],
        backgroundColor: ["#2F80ED", "#27AE60", "#E2B93B"],
      },
    ],
  },
  options: { responsive: true, plugins: { legend: { position: "bottom" } } },
});
// ---- Sidebar Drawer toggle (compatible with existing HTML) ----
function toggleSidebar() {
  const sidebar = document.getElementById("sidebar");
  const overlay = document.getElementById("overlay");

  if (!sidebar || !overlay) return;

  // if on desktop screens (>1023) we keep default hover/collapse behavior
  const isMobile = window.matchMedia("(max-width: 1023px)").matches;

  if (!isMobile) {
    // desktop: toggle 'active' to expand collapsed sidebar on hover-like interaction
    sidebar.classList.toggle("active");
    return;
  }

  // mobile/tablet: use drawer open/closed classes
  if (sidebar.classList.contains("open")) {
    sidebar.classList.remove("open");
    sidebar.classList.add("closed");
    overlay.classList.remove("show");
  } else {
    sidebar.classList.remove("closed");
    sidebar.classList.add("open");
    overlay.classList.add("show");
  }
}

// close sidebar when overlay clicked
document.getElementById("overlay")?.addEventListener("click", () => {
  const sidebar = document.getElementById("sidebar");
  const overlay = document.getElementById("overlay");
  if (!sidebar || !overlay) return;
  sidebar.classList.remove("open");
  sidebar.classList.add("closed");
  overlay.classList.remove("show");
});

// On load: ensure sidebar default state for mobile
document.addEventListener("DOMContentLoaded", () => {
  const sidebar = document.getElementById("sidebar");
  const overlay = document.getElementById("overlay");
  if (!sidebar || !overlay) return;

  // default: closed on mobile, keep desktop behavior intact
  if (window.matchMedia("(max-width: 1023px)").matches) {
    sidebar.classList.add("closed");
    overlay.classList.remove("show");
  } else {
    sidebar.classList.remove("closed");
    sidebar.classList.remove("open");
  }

  // adjust on resize: close drawer if switching to mobile
  window.addEventListener("resize", () => {
    if (window.matchMedia("(max-width: 1023px)").matches) {
      sidebar.classList.add("closed");
      overlay.classList.remove("show");
    } else {
      sidebar.classList.remove("closed");
      sidebar.classList.remove("open");
      overlay.classList.remove("show");
    }
  });
});
