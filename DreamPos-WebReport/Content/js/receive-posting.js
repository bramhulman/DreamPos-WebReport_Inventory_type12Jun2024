/* Prevent repeat UI submissions; database locking is the authoritative guard. */
(function (window, document) {
    'use strict';
    if (window.ReceivePostingUI) { return; }
    var busy = false;
    var controls = [];
    var activeButton = null;
    function reset() {
        busy = false;
        if (activeButton) { activeButton.removeAttribute('aria-busy'); activeButton = null; }
        controls.forEach(function (item) {
            item.control.disabled = item.disabled;
            item.control.removeAttribute('aria-busy');
            if (item.html !== null) { item.control.innerHTML = item.html; }
        });
        controls = [];
    }
    function begin(button) {
        if (busy) { return false; }
        busy = true;
        activeButton = button;
        button.setAttribute('aria-busy', 'true');
        // Let the native/WebForms submit capture the clicked button before disabling it.
        window.setTimeout(function () {
            if (!busy) { return; }
            var form = button.form;
            if (!form) { return; }
            Array.prototype.forEach.call(form.querySelectorAll('button, input[type="submit"], input[type="button"]'), function (control) {
                controls.push({ control: control, disabled: control.disabled, html: control === button ? control.innerHTML : null });
                control.disabled = true;
            });
            button.textContent = 'Memproses pembatalan…';
        }, 0);
        return true;
    }
    // A second click on any action in this form must not abort/replace the active AJAX request.
    document.addEventListener('click', function (event) {
        if (!busy) { return; }
        var node = event.target;
        while (node && node !== document) {
            if (node.tagName === 'BUTTON' || (node.tagName === 'INPUT' && /^(submit|button)$/i.test(node.type))) {
                event.preventDefault();
                event.stopImmediatePropagation();
                return;
            }
            node = node.parentNode;
        }
    }, true);
    window.ReceivePostingUI = { begin: begin, reset: reset };
    if (window.Sys && window.Sys.WebForms) {
        window.Sys.WebForms.PageRequestManager.getInstance().add_endRequest(reset);
    }
})(window, document);
