$(document).ready(function () {
    $.ajax({
        url: '/MTHome/ItemCategory',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data && data.length > 0) {
                data.forEach(function (category) {
                    var name = category.name;
                    var id = category.id;
                    var listItem = $('<li></li>');
                    var link = `<a href="/MTHome/SongList/${id}" class="dropdown-item">${name}</a>`;
                    listItem.append(link);
                    $('#category').append(listItem);
                });
            } else {
                console.log('Không nhận được dữ liệu từ server.');
            }
        },
        error: function (xhr, status, error) {
            console.error('Lỗi khi gửi AJAX request:', error);
        }
    });
});