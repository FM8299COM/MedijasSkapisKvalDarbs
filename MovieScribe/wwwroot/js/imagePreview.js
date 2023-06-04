document.getElementById('ImageUpload').addEventListener('change', function (e) {
    var imagePreview = document.getElementById('ImagePreview');
    var reader = new FileReader();

    reader.onload = function (e) {
        imagePreview.src = e.target.result;
        imagePreview.style.display = 'block';
    }

    reader.readAsDataURL(e.target.files[0]);
});