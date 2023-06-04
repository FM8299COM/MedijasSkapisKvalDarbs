function prepareDeleteButtons(deleteButtonId, confirmButtonId) {
    document.getElementById(deleteButtonId).addEventListener('click', function (event) {
        event.preventDefault();
        document.getElementById(deleteButtonId).style.display = 'none';
        document.getElementById(confirmButtonId).style.display = 'block';
    });
}