$(function () {
  const $toggleButton = $('.nav-toggle');
  const $navLinks = $('.nav-links');
  const $header = $('.top-brand');

  if (!$toggleButton.length || !$navLinks.length || !$header.length) {
    return;
  }

  const setPanelTop = () => {
    const rect = $header[0].getBoundingClientRect();
    document.documentElement.style.setProperty('--nav-panel-top', `${Math.round(rect.bottom)}px`);
  };

  const closeMenu = () => {
    $navLinks.removeClass('is-open');
    $toggleButton.attr('aria-expanded', 'false');
  };

  $toggleButton.on('click', () => {
    const isOpen = $navLinks.toggleClass('is-open').hasClass('is-open');
    $toggleButton.attr('aria-expanded', isOpen ? 'true' : 'false');
  });

  $navLinks.on('click', event => {
    if ($(event.target).closest('a').length) {
      closeMenu();
    }
  });

  $(document).on('click', event => {
    if (!$(event.target).closest('.top-nav').length) {
      closeMenu();
    }
  });

  setPanelTop();
  $(window).on('resize', () => {
    setPanelTop();
  });
});
