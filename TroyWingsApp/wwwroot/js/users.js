$(function () {
  const $pageEl = $('#usersPage');
  if (!$pageEl.length) {
    return;
  }

  const listUrl = $pageEl.data('list-url');
  const updateUrl = $pageEl.data('update-url');
  const basePageSize = Number.parseInt($pageEl.data('page-size') || '4', 10);

  const $gridEl = $('#usersGrid');
  const $pagerEl = $('#usersPager');
  const $statusEl = $('#usersStatus');
  const $modalEl = $('#editUserModal');
  const $formEl = $('#editUserForm');
  const $alertEl = $('#editUserAlert');

  const modal = $modalEl.length ? new bootstrap.Modal($modalEl[0]) : null;
  let currentPage = 1;
  let currentItems = [];
  let currentPageSize = basePageSize;

  const updatePageSize = () => {
    if (window.matchMedia('(max-width: 575.98px)').matches) {
      currentPageSize = 2;
      return;
    }

    currentPageSize = 4;
  };

  const escapeHtml = (value) => {
    return String(value)
      .replace(/&/g, '&amp;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/"/g, '&quot;')
      .replace(/'/g, '&#39;');
  };

  const setStatus = (message) => {
    if ($statusEl.length) {
      $statusEl.text(message);
    }
  };

  const renderCards = (items) => {
    if (!$gridEl.length) {
      return;
    }

    if (!items.length) {
      $gridEl.html('<div class="text-soft">No users found.</div>');
      return;
    }

    $gridEl.html(items
      .map((user) => {
        return `
          <article class="user-card">
            <div class="user-card__header">
              <div>
                <div class="user-name">${escapeHtml(user.name)}</div>
                <div class="user-meta">ID ${user.id}</div>
              </div>
              <button class="btn btn-sm edit-user-btn" type="button" data-user-id="${user.id}">
                Edit
              </button>
            </div>
            <div class="user-detail"><span>Father</span>${escapeHtml(user.fatherName)}</div>
            <div class="user-detail"><span>Date of Birth</span>${escapeHtml(user.dateOfBirth || '-')}</div>
            <div class="user-detail"><span>Contact</span>${escapeHtml(user.contactNumber)}</div>
            <div class="user-detail"><span>Address</span>${escapeHtml(user.address)}</div>
          </article>
        `;
      })
      .join(''));
  };

  const renderPager = (page, totalPages) => {
    if (!$pagerEl.length) {
      return;
    }

    if (totalPages <= 1) {
      $pagerEl.html('');
      return;
    }

    const windowSize = 2;
    const pages = [];
    const start = Math.max(1, page - windowSize);
    const end = Math.min(totalPages, page + windowSize);

    for (let i = start; i <= end; i += 1) {
      pages.push(i);
    }

    const buttons = [];
    buttons.push(`<button type="button" data-page="${page - 1}" ${page === 1 ? 'disabled' : ''}>Prev</button>`);
    pages.forEach((pageNumber) => {
      buttons.push(
        `<button type="button" data-page="${pageNumber}" class="${pageNumber === page ? 'active' : ''}">${pageNumber}</button>`
      );
    });
    buttons.push(
      `<button type="button" data-page="${page + 1}" ${page === totalPages ? 'disabled' : ''}>Next</button>`
    );

    $pagerEl.html(buttons.join(''));
  };

  const loadUsers = (page = 1) => {
    if (!listUrl) {
      setStatus('Users endpoint missing.');
      return;
    }

    setStatus('Refreshing users...');

    $.ajax({
      url: `${listUrl}?page=${page}&pageSize=${currentPageSize}`,
      method: 'GET',
      headers: { Accept: 'application/json' }
    })
      .done((payload) => {
        currentItems = payload.items || [];
        currentPage = payload.page || page;
        renderCards(currentItems);
        renderPager(currentPage, payload.totalPages || 1);
        setStatus(`Showing ${currentItems.length} of ${payload.totalCount || 0} users`);
      })
      .fail(() => {
        setStatus('Unable to load users right now.');
        if ($gridEl.length) {
          $gridEl.html('<div class="text-soft">Try refreshing the page.</div>');
        }
      });
  };

  const openEditModal = (userId) => {
    if (!modal || !$formEl.length) {
      return;
    }

    const user = currentItems.find((item) => item.id === userId);
    if (!user) {
      return;
    }

    $formEl.find('#editUserId').val(user.id);
    $formEl.find('#editName').val(user.name || '');
    $formEl.find('#editFatherName').val(user.fatherName || '');
    $formEl.find('#editDob').val(user.dateOfBirth || '');
    $formEl.find('#editContact').val(user.contactNumber || '');
    $formEl.find('#editAddress').val(user.address || '');

    if ($alertEl.length) {
      $alertEl.addClass('d-none').text('');
    }

    modal.show();
  };

  const submitEdit = (event) => {
    event.preventDefault();
    if (!$formEl.length || !updateUrl) {
      return;
    }

    const token = $formEl.find('input[name="__RequestVerificationToken"]').val() || '';

    const payload = {
      id: Number.parseInt($formEl.find('#editUserId').val(), 10),
      name: $formEl.find('#editName').val().trim(),
      fatherName: $formEl.find('#editFatherName').val().trim(),
      dateOfBirth: $formEl.find('#editDob').val(),
      contactNumber: $formEl.find('#editContact').val().trim(),
      address: $formEl.find('#editAddress').val().trim()
    };

    if ($alertEl.length) {
      $alertEl.addClass('d-none').text('');
    }

    $.ajax({
      url: updateUrl,
      method: 'POST',
      contentType: 'application/json',
      data: JSON.stringify(payload),
      headers: {
        RequestVerificationToken: token
      }
    })
      .done(() => {
        modal.hide();
        loadUsers(currentPage);
      })
      .fail((xhr) => {
        const message = xhr.responseJSON?.message || 'Unable to update user.';
        if ($alertEl.length) {
          $alertEl.text(message).removeClass('d-none');
        }
      });
  };

  if ($pagerEl.length) {
    $pagerEl.on('click', 'button[data-page]', (event) => {
      const page = Number.parseInt($(event.currentTarget).data('page'), 10);
      if (!Number.isNaN(page)) {
        loadUsers(page);
      }
    });
  }

  if ($gridEl.length) {
    $gridEl.on('click', '.edit-user-btn', (event) => {
      const userId = Number.parseInt($(event.currentTarget).data('user-id'), 10);
      if (!Number.isNaN(userId)) {
        openEditModal(userId);
      }
    });
  }

  if ($formEl.length) {
    $formEl.on('submit', submitEdit);
  }

  updatePageSize();
  loadUsers(currentPage);

  let resizeTimer;
  $(window).on('resize', () => {
    window.clearTimeout(resizeTimer);
    resizeTimer = window.setTimeout(() => {
      const previousSize = currentPageSize;
      updatePageSize();
      if (previousSize !== currentPageSize) {
        currentPage = 1;
        loadUsers(currentPage);
      }
    }, 150);
  });
});
