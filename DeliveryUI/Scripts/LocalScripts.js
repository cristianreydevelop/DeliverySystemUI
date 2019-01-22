function submitform(frmName, method, action) {
    $("#" + frmName).attr("method", method);
    $("#" + frmName).attr("action", action);
    $("#" + frmName).submit();
}
