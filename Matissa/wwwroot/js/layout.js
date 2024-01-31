function toggleSidebar() {
    const sidebar = document.querySelector(".sidebar");
    const content = document.querySelector(".content");
    const toggleBtn = document.getElementById("toggle-btn");
    const isOpen = sidebar.style.left === "0px";
    sidebar .style.left = isOpen ? "-250px" : "0";
    content .style.marginLeft = isOpen ? "0" : "250px";
    toggleBtn .style.left = isOpen ? "10px" : "250px";
}

// Establece el estilo inicial del sidebar para que esté abierto por defecto
document.addEventListener("DOMContentLoaded", function () {
    const sidebar = document.querySelector(".sidebar");
    const content = document.querySelector(".content");
    const toggleBtn = document.getElementById("toggle-btn");

    sidebar.style.left = "0";
    content.style.marginLeft = "250px";
    toggleBtn.style.left = "300px";
});
