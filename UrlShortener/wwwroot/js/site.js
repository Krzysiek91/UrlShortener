function copyShortUrl() {
    var copyShortUrl = document.getElementById("shortUrlInput");

    copyShortUrl.select();
    navigator.clipboard.writeText(copyShortUrl.value);
}