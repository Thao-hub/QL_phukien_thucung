
/*
Example Data:
var config = {
    selector: "selector of checkbox",
    modalId: "confirmModal",
    formId: "confirmForm",
    selectorType: "checkbox",
    selectorEvent: "change",
    checkBoxData: {
        id: "data-id",
        name: "data-ten"
    },
    dynamicData: {
        id: "SanPhamId",
        status: "TrangThai"
    },
    messageId: "confirmMessage",
    message: "message in popup",
    updateUrl: "Action of Controller to update status"
}
*/
function setupToggleStatus(config) {
    let currentSelector;
    document.querySelectorAll(config.selector).forEach(function (selector) {
        selector.addEventListener(config.selectorEvent, function () {
            currentSelector = this;
            let newStatus = currentSelector?.checked;

            let id = currentSelector.getAttribute(config.checkBoxData.id);
            let name = currentSelector.getAttribute(config.checkBoxData.name);
            

            // Update modal dynamically
            document.getElementById(config.messageId).innerHTML =
                `${config.message} <b>${name}</b>?`;

            // Dynamically update hidden inputs
            let idElements = document.getElementsByName(config.dynamicData.id);
            if (idElements.length > 0) {
                idElements.forEach(el => el.value = id);  // Set value for all matching elements
            }

            if (config.selectorType === "checkbox") {
                let statusElements = document.getElementsByName(config.dynamicData.status);
                if (statusElements.length > 0) {
                    statusElements.forEach(el => el.value = newStatus);
                }
            }

            // Show confirmation modal
            let confirmModal = new bootstrap.Modal(document.getElementById(config.modalId));
            confirmModal.show();

            // Handle modal close event (Reset checkbox state)
            document.getElementById(config.modalId).addEventListener("hidden.bs.modal", function () {
                if (!document.getElementById(config.formId).dataset.submitted && config.selectorType == "checkbox") {
                    currentSelector.checked = !newStatus; // Revert to original state
                }
            });

            // Set form action dynamically
            document.getElementById(config.formId).setAttribute("action", config.updateUrl);

            // Prevent multiple form submissions
            document.getElementById(config.formId).addEventListener("submit", function () {
                this.dataset.submitted = "true";
            });
        });
    });
}