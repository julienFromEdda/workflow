// Validation utils
function validateField(field) {
    // Validate field type
    if (!field || !field.value) {
        return {
            isValid: false,
            message: 'Champ invalide.'
        };
    }

    const validation = {
        isValid: true,
        message: ''
    };

    // Get validation attributes
    const attributes = field.attributes;

    // Check if field type is supported
    const supportedTypes = ['text', 'email', 'password', 'number', 'select-one', 'textarea'];
    const fieldType = field.type || (field.tagName === 'SELECT' ? 'select-one' : 'text');
    
    if (!supportedTypes.includes(fieldType)) {
        return {
            isValid: false,
            message: `Type de champ non supporté: ${fieldType}`
        };
    }

    // Required validation
    if (field.required && !field.value.trim()) {
        validation.isValid = false;
        validation.message = attributes.getNamedItem('data-val-required')?.value || 'Ce champ est requis.';
        return validation;
    }

    // Email validation
    if (field.type === 'email' && field.value) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(field.value)) {
            validation.isValid = false;
            validation.message = attributes.getNamedItem('data-val-email')?.value || 'Veuillez entrer une adresse email valide.';
            return validation;
        }
    }

    // Number validation
    if (field.type === 'number' && field.value) {
        const num = Number(field.value);
        if (isNaN(num)) {
            validation.isValid = false;
            validation.message = attributes.getNamedItem('data-val-number')?.value || 'Veuillez entrer un nombre valide.';
            return validation;
        }

        // Check min/max values
        const minAttr = attributes.getNamedItem('data-val-number-min');
        const maxAttr = attributes.getNamedItem('data-val-number-max');
        
        if (minAttr || maxAttr) {
            const min = minAttr ? parseFloat(minAttr.value) : -Infinity;
            const max = maxAttr ? parseFloat(maxAttr.value) : Infinity;
            
            if (num < min || num > max) {
                validation.isValid = false;
                validation.message = attributes.getNamedItem('data-val-number-range')?.value || 
                    `Le nombre doit être entre ${min} et ${max}.`;
                return validation;
            }
        }
    }

    // Select validation
    if (field.type === 'select-one' || field.tagName === 'SELECT') {
        if (field.required && !field.value) {
            validation.isValid = false;
            validation.message = attributes.getNamedItem('data-val-required')?.value || 'Veuillez sélectionner une valeur.';
            return validation;
        }

        // Check if selected value exists in options
        const selectedOption = field.querySelector(`option[value="${field.value}"]`);
        if (!selectedOption) {
            validation.isValid = false;
            validation.message = 'Valeur invalide sélectionnée.';
            return validation;
        }
    }

    // Length validation
    if (attributes.getNamedItem('data-val-length')) {
        const minLength = parseInt(attributes.getNamedItem('data-val-length-min')?.value || 0);
        const maxLength = parseInt(attributes.getNamedItem('data-val-length-max')?.value || Number.MAX_SAFE_INTEGER);

        if (field.value.length < minLength || field.value.length > maxLength) {
            validation.isValid = false;
            validation.message = attributes.getNamedItem('data-val-length')?.value ||
                `La valeur doit contenir entre ${minLength} et ${maxLength} caractères.`;
            return validation;
        }
    }

    // Pattern validation
    if (attributes.getNamedItem('data-val-regex')) {
        const patternAttr = attributes.getNamedItem('data-val-regex-pattern');
        if (patternAttr) {
            const pattern = patternAttr.value;
            try {
                const regex = new RegExp(pattern);
                if (!regex.test(field.value)) {
                    validation.isValid = false;
                    validation.message = attributes.getNamedItem('data-val-regex')?.value ||
                        'La valeur ne correspond pas au format attendu.';
                    return validation;
                }
            } catch (error) {
                console.error(`Invalid regex pattern: ${pattern}`);
                validation.isValid = false;
                validation.message = 'Le format de validation est invalide.';
                return validation;
            }
        }
    }

    // Custom validation using data-val-custom
    if (attributes.getNamedItem('data-val-custom')) {
        const customValidator = attributes.getNamedItem('data-val-custom-validator')?.value;
        if (customValidator && window[customValidator]) {
            const result = window[customValidator](field);
            if (!result.isValid) {
                validation.isValid = false;
                validation.message = result.message || attributes.getNamedItem('data-val-custom')?.value ||
                    "La valeur n'est pas valide.";
                return validation;
            }
        }
    }

    return validation;
}

// Role validation
function validateRoles(form) {
    const roleCheckboxes = form.querySelectorAll('[name="selectedRoles"]');
    if (roleCheckboxes.length === 0) {
        return { isValid: true, message: '' };
    }

    const selectedRoles = form.querySelectorAll('[name="selectedRoles"]:checked');
    if (selectedRoles.length === 0) {
        return {
            isValid: false,
            message: 'Veuillez sélectionner au moins un rôle.'
        };
    }

    return { isValid: true, message: '' };
}

// Reset validation
function resetValidation(form) {
    // Reset validation summary
    const validationSummary = form.querySelector('.validation-summary-errors');
    if (validationSummary) {
        validationSummary.textContent = '';
    }

    // Reset field validation messages
    const validationMessages = form.querySelectorAll('.field-validation-error');
    validationMessages.forEach(msg => msg.textContent = '');
}

// Show validation message
function showValidationMessage(field, message) {
    const errorSpan = field.nextElementSibling;
    if (errorSpan && errorSpan.classList.contains('field-validation-error')) {
        errorSpan.textContent = message;
    }
}

// Hide validation message
function hideValidationMessage(field) {
    const errorSpan = field.nextElementSibling;
    if (errorSpan && errorSpan.classList.contains('field-validation-error')) {
        errorSpan.textContent = '';
    }
}

// Export functions for use in other scripts
window.ValidationUtils = {
    validateField,
    validateRoles,
    resetValidation,
    showValidationMessage,
    hideValidationMessage
};
