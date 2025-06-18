function loadModalContent(modalId, url, title, onLoadCallback = null) {
    const modal = new bootstrap.Modal(document.getElementById(modalId));
    const titleEl = document.getElementById(modalId + 'Label');
    const bodyEl = document.querySelector(`#${modalId} .modal-body`);

    titleEl.innerHTML = title || 'Chargement';
    bodyEl.innerHTML = '<div class="text-center">Chargement...</div>';
    modal.show();

    fetch(url)
        .then(response => {
            if (response.redirected && response.url.includes("/Identity/Account/Login")) {
                window.location.href = response.url;
                return;
            }
            return response.text();
        })
        .then(html => {
            if (html) {
                bodyEl.innerHTML = html;
                if (typeof onLoadCallback === 'function') {
                    onLoadCallback();
                }
            }
        });
}

function bindUploadForm(modalId, formId) {
    const form = document.getElementById(formId);
    if (!form) return;

    form.addEventListener('submit', function (e) {
        e.preventDefault();
        const data = new FormData(form);
        const action = form.getAttribute('action');

        fetch(action, {
            method: 'POST',
            body: data
        }).then(response => {
            if (response.redirected && response.url.includes('/Login')) {
                window.location.href = response.url;
                return;
            }
            return response.text();
        }).then(html => {
            if (html) {
                document.querySelector(`#${modalId} .modal-body`).innerHTML = html;

                bindUploadForm(modalId, formId);
                bindDeleteDocumentForms(modalId);
            }
        });
    });
}

function bindDeleteDocumentForms(modalId) {
    document.querySelectorAll(`#${modalId} .delete-doc-form`).forEach(form => {
        form.addEventListener('submit', function (e) {
            e.preventDefault();

            if (!confirm('Confirmer la suppression de ce document ?')) return;

            const formData = new FormData(form);
            const action = form.getAttribute('action');

            fetch(action, {
                method: 'POST',
                body: formData
            }).then(response => {
                if (response.redirected && response.url.includes('/Login')) {
                    window.location.href = response.url;
                    return;
                }
                return response.text();
            }).then(html => {
                if (html) {
                    document.querySelector(`#${modalId} .modal-body`).innerHTML = html;
                    bindUploadForm(modalId, 'upload-doc-form');
                    bindDeleteDocumentForms(modalId);
                }
            });
        });
    });
}

document.addEventListener('DOMContentLoaded', () => {
    const modalEl = document.getElementById('modal');
    if (modalEl) {
        modalEl.addEventListener('hidden.bs.modal', function () {
            location.reload();
        });
    }
});