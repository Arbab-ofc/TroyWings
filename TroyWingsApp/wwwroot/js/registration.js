$(function () {
  const $forms = $('.needs-validation');
  const $summary = $('#validationSummary');

  $forms.on('submit', function (event) {
    if (!this.checkValidity()) {
      event.preventDefault();
      event.stopPropagation();
      $summary.removeClass('d-none');
    } else {
      $summary.addClass('d-none');
    }

    $(this).addClass('was-validated');
  });

  const $successAlert = $('.alert.alert-success');
  if ($successAlert.length) {
    setTimeout(() => {
      $successAlert.addClass('d-none');
    }, 3000);
  }
});
