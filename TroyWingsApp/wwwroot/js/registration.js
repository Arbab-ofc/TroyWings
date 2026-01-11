document.addEventListener('DOMContentLoaded', () => {
  const forms = document.querySelectorAll('.needs-validation');
  const summary = document.getElementById('validationSummary');

  forms.forEach(form => {
    form.addEventListener(
      'submit',
      event => {
        if (!form.checkValidity()) {
          event.preventDefault();
          event.stopPropagation();
          summary?.classList.remove('d-none');
        } else {
          summary?.classList.add('d-none');
        }

        form.classList.add('was-validated');
      },
      false
    );
  });

  const successAlert = document.querySelector('.alert.alert-success');
  if (successAlert) {
    setTimeout(() => {
      successAlert.classList.add('d-none');
    }, 3000);
  }
});
