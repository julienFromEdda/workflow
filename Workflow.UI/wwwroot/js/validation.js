// Validation form handler
function setupFormValidation() {
    // Get all forms with validation
    const forms = document.querySelectorAll('form[data-val="true"]');

    forms.forEach(form => {
        // Get validation summary
        const validationSummary = form.querySelector('.validation-summary-errors');
        
        // Add submit handler
        form.addEventListener('submit', async (e) => {
            e.preventDefault();
            
            // Reset validation
            resetValidation(form);

            // Get validation fields
            const validationFields = form.querySelectorAll('[data-val="true"]');
            
            // Validate each field
            let isValid = true;
            validationFields.forEach(field => {
                if (!validateField(field)) {
                    isValid = false;
                }
            });

            // Display validation summary
            if (!isValid) {
                if (validationSummary) {
                    validationSummary.style.display = 'block';
                }
                return;
            }

            // If form is valid, submit it
            form.submit();
        });
    });
}

// Validate a single field
function validateField(field) {
    // Get validation attributes
    const validationAttributes = field.attributes;
    let isValid = true;

    // Check required
    if (field.required && !field.value.trim()) {
        isValid = false;
        addValidationMessage(field, 'Ce champ est requis.');
    }

    // Check email format
    if (field.type === 'email' && field.value && !isValidEmail(field.value)) {
        isValid = false;
        addValidationMessage(field, 'Veuillez entrer une adresse email valide.');
    }

    // Check other validation attributes
    if (validationAttributes.getNamedItem('data-val-length')) {
        const minLength = validationAttributes.getNamedItem('data-val-length-min').value;
        const maxLength = validationAttributes.getNamedItem('data-val-length-max').value;
        if (field.value.length < minLength || field.value.length > maxLength) {
            isValid = false;
            addValidationMessage(field, `La valeur doit contenir entre ${minLength} et ${maxLength} caractères.`);
        }
    }

    return isValid;
}

// Add validation message to field
function addValidationMessage(field, message) {
    const validationMessage = document.createElement('span');
    validationMessage.className = 'field-validation-error';
    validationMessage.textContent = message;
    field.parentElement.appendChild(validationMessage);
}

// Reset validation messages
function resetValidation(form) {
    // Remove all validation messages
    const validationMessages = form.querySelectorAll('.field-validation-error');
    validationMessages.forEach(msg => msg.remove());
    
    // Hide validation summary
    const validationSummary = form.querySelector('.validation-summary-errors');
    if (validationSummary) {
        validationSummary.style.display = 'none';
    }
}

// Email validation
function isValidEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

// Initialize validation when document is ready
document.addEventListener('DOMContentLoaded', () => {
    setupFormValidation();
});
