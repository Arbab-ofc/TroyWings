document.addEventListener('DOMContentLoaded', () => {
  const toggleButton = document.querySelector('.nav-toggle');
  const navLinks = document.querySelector('.nav-links');
  const header = document.querySelector('.top-brand');

  if (!toggleButton || !navLinks || !header) {
    return;
  }

  const setPanelTop = () => {
    const rect = header.getBoundingClientRect();
    document.documentElement.style.setProperty('--nav-panel-top', `${Math.round(rect.bottom)}px`);
  };

  const closeMenu = () => {
    navLinks.classList.remove('is-open');
    toggleButton.setAttribute('aria-expanded', 'false');
  };

  toggleButton.addEventListener('click', () => {
    const isOpen = navLinks.classList.toggle('is-open');
    toggleButton.setAttribute('aria-expanded', isOpen ? 'true' : 'false');
  });

  navLinks.addEventListener('click', event => {
    if (event.target.closest('a')) {
      closeMenu();
    }
  });

  document.addEventListener('click', event => {
    if (!event.target.closest('.top-nav')) {
      closeMenu();
    }
  });

  setPanelTop();
  window.addEventListener('resize', () => {
    setPanelTop();
  });
});
