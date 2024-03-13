// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Initialization for ES Users
import { Modal, Input, Ripple, initMDB } from "mdb-ui-kit";

initMDB({ Modal, Input, Ripple });
//(() => {
//    'use strict';

//    // Fetch all the forms we want to apply custom Bootstrap validation styles to
//    const forms = document.querySelectorAll('.needs-validation');

//    // Loop over them and prevent submission
//    Array.prototype.slice.call(forms).forEach((form) => {
//        form.addEventListener('submit', (event) => {
//            if (!form.checkValidity()) {
//                event.preventDefault();
//                event.stopPropagation();
//            }
//            form.classList.add('was-validated');
//        }, false);
//    });
//})();