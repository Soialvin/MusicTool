function updateDownloadCount(songId) {
    $.ajax({
        type: "POST",
        url: '/MTHome/UpdateDownloadCount',
        data: { songId: songId },
        success: function (data) {
            console.log('Cập nhật lượt tải thành công');
        },
        error: function (xhr, status, error) {
            console.error("Error: ", status, error);
        }
    });
}