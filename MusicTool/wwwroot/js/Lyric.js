function toggleText() {
    const container = document.querySelector('.lyric-content');
    const button = document.getElementById('toggleButton');
    container.classList.toggle('expanded');
    if (container.classList.contains('expanded')) {
        button.innerHTML = 'Thu gọn <i class="fa-solid fa-arrow-up"></i>';
    }
    else {
        button.innerHTML = 'Xem toàn bộ <i class="fa-solid fa-arrow-down"></i>';
    }
}

function formatLyric() {
    const lyrics = document.querySelector('.lyric-content .content').textContent.trim();
    const lines = lyrics.split(/(?<=[.!?])\s+/).map(line => line.trim()).filter(line => line.length > 0);
    const container = document.querySelector('.lyric-content .content');
    container.innerHTML = '';
    if (lines.length === 0) {
        const p = document.createElement('p');
        p.textContent = "Hiện tại chưa có lời bài hát. Hãy đợi chúng tôi cập nhật trong thời gian tới nhé!!!!";
        container.appendChild(p);
    }
    else {
        lines.forEach(line => {
            const p = document.createElement('p');
            p.textContent = line;
            container.appendChild(p);
        });
    }
    const toggleButton = document.getElementById('toggleButton');
    if (lines.length <= 10) {
        toggleButton.style.display = 'none';
    }
    else {
        toggleButton.style.display = 'block';
    }
}