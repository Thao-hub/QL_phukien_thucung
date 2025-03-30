/**
 * Upload Image
 */
function previewImage(input, previewElementId) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            document.getElementById(previewElementId).src = e.target.result;
        };
        reader.readAsDataURL(input.files[0]);
    }
}

function resetImage(previewElementId, defaultImagePath, fileInputId) {
    document.getElementById(previewElementId).src = defaultImagePath;
    document.getElementById(fileInputId).value = ""; // Reset input file
}
