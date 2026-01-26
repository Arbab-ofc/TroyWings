$(function () {
  const $pageEl = $('#usersPage');
  if (!$pageEl.length) {
    return;
  }

  const updateUrl = '/Users/Update';
  const $tableEl = $('#usersTable');
  const $modalEl = $('#editUserModal');
  const $formEl = $('#editUserForm');
  const $alertEl = $('#editUserAlert');

  const modal = $modalEl.length ? new bootstrap.Modal($modalEl[0]) : null;
  let $activeRow = null;

  const months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
  const showAlert = (message) => {
    if (!$alertEl.length) {
      return;
    }
    if (message) {
      $alertEl.text(message).removeClass('d-none');
      return;
    }
    $alertEl.addClass('d-none').text('');
  };

  const getInputValue = (selector) => $formEl.find(selector).val();
  const getTrimmedValue = (selector) => String(getInputValue(selector) || '').trim();

  const formatDateLabel = (value) => {
    if (!value) {
      return '-';
    }

    const parts = String(value).split('-');
    const monthIndex = Number(parts[1]) - 1;
    if (parts.length !== 3 || Number.isNaN(monthIndex) || monthIndex < 0 || monthIndex > 11) {
      return value;
    }

    return `${months[monthIndex]} ${parts[2]}, ${parts[0]}`;
  };

  const fillFormFromRow = ($row) => {
    $formEl.find('#editUserId').val($row.data('userId'));
    $formEl.find('#editName').val($row.data('name') || '');
    $formEl.find('#editFatherName').val($row.data('fatherName') || '');
    $formEl.find('#editDob').val($row.data('dob') || '');
    $formEl.find('#editContact').val($row.data('contactNumber') || '');
    $formEl.find('#editAddress').val($row.data('address') || '');
  };

  const updateRowFromPayload = ($row, payload) => {
    $row.data('name', payload.name);
    $row.data('fatherName', payload.fatherName);
    $row.data('dob', payload.dateOfBirth);
    $row.data('contactNumber', payload.contactNumber);
    $row.data('address', payload.address);
    $row.find('[data-field="name"]').text(payload.name);
    $row.find('[data-field="fatherName"]').text(payload.fatherName);
    $row.find('[data-field="dateOfBirth"]').text(formatDateLabel(payload.dateOfBirth));
    $row.find('[data-field="contactNumber"]').text(payload.contactNumber);
    $row.find('[data-field="address"]').text(payload.address);
  };

  const openEditModal = ($row) => {
    if (!modal || !$formEl.length) {
      return;
    }

    $activeRow = $row;
    fillFormFromRow($row);
    showAlert('');
    modal.show();
  };

  const submitEdit = (event) => {
    event.preventDefault();
    if (!$formEl.length || !updateUrl || !modal) {
      return;
    }

    const token = getInputValue('input[name="__RequestVerificationToken"]') || '';
    const payload = {
      id: Number.parseInt(getInputValue('#editUserId'), 10),
      name: getTrimmedValue('#editName'),
      fatherName: getTrimmedValue('#editFatherName'),
      dateOfBirth: getInputValue('#editDob'),
      contactNumber: getTrimmedValue('#editContact'),
      address: getTrimmedValue('#editAddress')
    };

    showAlert('');

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
          updateRowFromPayload($activeRow, payload);
        }
      })
      .fail((xhr) => {
        const message = xhr.responseJSON?.message || 'Unable to update user.';
        showAlert(message);
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
