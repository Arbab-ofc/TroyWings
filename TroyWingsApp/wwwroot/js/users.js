$(function () {
  const $pageEl = $('#usersPage');
  if (!$pageEl.length) {
    return;
  }

  const updateUrl = $pageEl.data('update-url');
  const $tableEl = $('#usersTable');
  const $modalEl = $('#editUserModal');
  const $formEl = $('#editUserForm');
  const $alertEl = $('#editUserAlert');

  const modal = $modalEl.length ? new bootstrap.Modal($modalEl[0]) : null;
  let $activeRow = null;

  const formatDateLabel = (value) => {
    if (!value) {
      return '-';
    }

    const parts = String(value).split('-');
    if (parts.length !== 3) {
      return value;
    }

    const monthIndex = Number(parts[1]) - 1;
    const day = parts[2];
    const months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    if (Number.isNaN(monthIndex) || monthIndex < 0 || monthIndex > 11) {
      return value;
    }

    return `${months[monthIndex]} ${day}, ${parts[0]}`;
  };

  const openEditModal = ($row) => {
    if (!modal || !$formEl.length) {
      return;
    }

    $activeRow = $row;

    $formEl.find('#editUserId').val($row.data('userId'));
    $formEl.find('#editName').val($row.data('name') || '');
    $formEl.find('#editFatherName').val($row.data('fatherName') || '');
    $formEl.find('#editDob').val($row.data('dob') || '');
    $formEl.find('#editContact').val($row.data('contactNumber') || '');
    $formEl.find('#editAddress').val($row.data('address') || '');

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
        if ($activeRow) {
          $activeRow.data('name', payload.name);
          $activeRow.data('fatherName', payload.fatherName);
          $activeRow.data('dob', payload.dateOfBirth);
          $activeRow.data('contactNumber', payload.contactNumber);
          $activeRow.data('address', payload.address);
          $activeRow.find('[data-field="name"]').text(payload.name);
          $activeRow.find('[data-field="fatherName"]').text(payload.fatherName);
          $activeRow.find('[data-field="dateOfBirth"]').text(formatDateLabel(payload.dateOfBirth));
          $activeRow.find('[data-field="contactNumber"]').text(payload.contactNumber);
          $activeRow.find('[data-field="address"]').text(payload.address);
        }
      })
      .fail((xhr) => {
        const message = xhr.responseJSON?.message || 'Unable to update user.';
        if ($alertEl.length) {
          $alertEl.text(message).removeClass('d-none');
        }
      });
  };

  if ($tableEl.length) {
    $tableEl.on('click', '.edit-user-btn', (event) => {
      const $row = $(event.currentTarget).closest('tr');
      if ($row.length) {
        openEditModal($row);
      }
    });
  }

  if ($formEl.length) {
    $formEl.on('submit', submitEdit);
  }
});
