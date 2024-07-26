function updateUserTypeSelect() {
    var accountType = document.getElementById('accountType').value;
    var userType = document.getElementById('ctn-userType');

    if (accountType === "false") {
        userType.hidden = false;
    } else {
        userType.hidden = true;
    }
}

function updateUserTypeVip() {
    var userType = document.getElementById('userType').value;
    var userTypeVip = document.getElementById('ctn-userTypeVip');

    if (userType === "normal") {
        userTypeVip.hidden = true;
    } else {
        userTypeVip.hidden = false;
    }
}

document.addEventListener('DOMContentLoaded', function () {
    updateUserTypeSelect();
    updateUserTypeVip();
    document.getElementById('accountType').addEventListener('change', updateUserTypeSelect);
    document.getElementById('userType').addEventListener('change', updateUserTypeVip);
});