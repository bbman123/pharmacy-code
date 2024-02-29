function exportFile(reportName, byteArray) {
    //if(/(iP)/g.test(navigator.userAgent)) {
    //    alert('Your device do not support files downloading.\n Please try again in desktop browser.')
    //}
    //else {
    //}
    var link = document.createElement('a');
    link.download = reportName;
    link.href = "data:application/octet-stream;base64," + byteArray;
    link.target = '_blank';
    document.body.appendChild(link); // Needed for Firefox
    link.click();
    document.body.removeChild(link);
}       