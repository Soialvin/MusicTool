function statistical() {
    window.location.href = urls.statistical;
}
function song() {
    window.location.href = urls.song;
}

function account() {
    window.location.href = urls.account;
}

function singer() {
    window.location.href = urls.singer;
}

function category() {
    window.location.href = urls.category;
}
document.addEventListener("DOMContentLoaded", function () {
    var currentUrl = window.location.href.toLowerCase();

    if (currentUrl.includes('/singer')) {
        var singer = document.getElementById('singer');
        if (singer) {
            singer.classList.add('active');
        }
    }
    if (currentUrl.includes('/song')) {
        var song = document.getElementById('song');
        if (song) {
            song.classList.add('active');
        }
    }
    if (currentUrl.includes('/account')) {
        var account = document.getElementById('account');
        if (account) {
            account.classList.add('active');
        }
    }
    if (currentUrl.includes('/category')) {
        var category = document.getElementById('category');
        if (category) {
            category.classList.add('active');
        }
    }
    if (currentUrl.endsWith('/admin')) {
        var statistical = document.getElementById('statistical');
        if (statistical) {
            statistical.classList.add('active');
        }
    }
});