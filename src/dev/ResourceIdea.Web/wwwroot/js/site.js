// Set the initial size of the planner.
resizeWindow();

// Add event listener to resize the planner when the window is resized.
window.addEventListener('resize', resizeWindow);

// Resize the planner.
function resizeWindow() {
    const plannerElements = document.querySelectorAll('.planner');
    const vw = Math.max(document.documentElement.clientWidth - 50 || 0, window.innerWidth - 50 || 0);
    const vh = Math.max(document.documentElement.clientHeight - 350 || 0, window.innerHeight - 350 || 0);

    plannerElements.forEach(planner => {
        planner.style.width = `${vw}px`;
        planner.style.height = `${vh}px`;
    });
}