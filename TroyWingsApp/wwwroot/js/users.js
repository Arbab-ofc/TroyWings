(() => {
  const pageEl = document.getElementById('usersPage');
  if (!pageEl) {
    return;
  }

  const listUrl = pageEl.dataset.listUrl;
  const updateUrl = pageEl.dataset.updateUrl;
  const basePageSize = Number.parseInt(pageEl.dataset.pageSize || '4', 10);

  const gridEl = document.getElementById('usersGrid');
  const pagerEl = document.getElementById('usersPager');
  const statusEl = document.getElementById('usersStatus');
  const modalEl = document.getElementById('editUserModal');
  const formEl = document.getElementById('editUserForm');
  const alertEl = document.getElementById('editUserAlert');

  const modal = modalEl ? new bootstrap.Modal(modalEl) : null;
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
    if (statusEl) {
      statusEl.textContent = message;
    }
  };

  const renderCards = (items) => {
    if (!gridEl) {
      return;
    }

    if (!items.length) {
      gridEl.innerHTML = '<div class="text-soft">No users found.</div>';
      return;
    }

    gridEl.innerHTML = items
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
      .join('');
  };

  const renderPager = (page, totalPages) => {
    if (!pagerEl) {
      return;
    }

    if (totalPages <= 1) {
      pagerEl.innerHTML = '';
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

    pagerEl.innerHTML = buttons.join('');
  };

  const loadUsers = async (page = 1) => {
    if (!listUrl) {
      setStatus('Users endpoint missing.');
      return;
    }

    setStatus('Refreshing users...');

    try {
      const response = await fetch(`${listUrl}?page=${page}&pageSize=${currentPageSize}`, {
        headers: { Accept: 'application/json' }
      });

      if (!response.ok) {
        throw new Error('Failed to load users.');
      }

      const payload = await response.json();
      currentItems = payload.items || [];
      currentPage = payload.page || page;
      renderCards(currentItems);
      renderPager(currentPage, payload.totalPages || 1);
      setStatus(`Showing ${currentItems.length} of ${payload.totalCount || 0} users`);
    } catch (error) {
      setStatus('Unable to load users right now.');
      gridEl.innerHTML = '<div class="text-soft">Try refreshing the page.</div>';
    }
  };

  const openEditModal = (userId) => {
    if (!modal || !formEl) {
      return;
    }

    const user = currentItems.find((item) => item.id === userId);
    if (!user) {
      return;
    }

    formEl.querySelector('#editUserId').value = user.id;
    formEl.querySelector('#editName').value = user.name || '';
    formEl.querySelector('#editFatherName').value = user.fatherName || '';
    formEl.querySelector('#editDob').value = user.dateOfBirth || '';
    formEl.querySelector('#editContact').value = user.contactNumber || '';
    formEl.querySelector('#editAddress').value = user.address || '';

    if (alertEl) {
      alertEl.classList.add('d-none');
      alertEl.textContent = '';
    }

    modal.show();
  };

  const submitEdit = async (event) => {
    event.preventDefault();
    if (!formEl || !updateUrl) {
      return;
    }

    const tokenInput = formEl.querySelector('input[name="__RequestVerificationToken"]');
    const token = tokenInput ? tokenInput.value : '';

    const payload = {
      id: Number.parseInt(formEl.querySelector('#editUserId').value, 10),
      name: formEl.querySelector('#editName').value.trim(),
      fatherName: formEl.querySelector('#editFatherName').value.trim(),
      dateOfBirth: formEl.querySelector('#editDob').value,
      contactNumber: formEl.querySelector('#editContact').value.trim(),
      address: formEl.querySelector('#editAddress').value.trim()
    };

    if (alertEl) {
      alertEl.classList.add('d-none');
      alertEl.textContent = '';
    }

    try {
      const response = await fetch(updateUrl, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          RequestVerificationToken: token
        },
        body: JSON.stringify(payload)
      });

      const data = await response.json();
      if (!response.ok) {
        throw new Error(data.message || 'Unable to update user.');
      }

      modal.hide();
      await loadUsers(currentPage);
    } catch (error) {
      if (alertEl) {
        alertEl.textContent = error.message || 'Unable to update user.';
        alertEl.classList.remove('d-none');
      }
    }
  };

  if (pagerEl) {
    pagerEl.addEventListener('click', (event) => {
      const target = event.target.closest('button[data-page]');
      if (!target) {
        return;
      }
      const page = Number.parseInt(target.dataset.page, 10);
      if (!Number.isNaN(page)) {
        loadUsers(page);
      }
    });
  }

  if (gridEl) {
    gridEl.addEventListener('click', (event) => {
      const button = event.target.closest('.edit-user-btn');
      if (!button) {
        return;
      }
      const userId = Number.parseInt(button.dataset.userId, 10);
      if (!Number.isNaN(userId)) {
        openEditModal(userId);
      }
    });
  }

  if (formEl) {
    formEl.addEventListener('submit', submitEdit);
  }

  updatePageSize();
  loadUsers(currentPage);

  let resizeTimer;
  window.addEventListener('resize', () => {
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
})();
