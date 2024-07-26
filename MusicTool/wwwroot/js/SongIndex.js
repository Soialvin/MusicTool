function openModal(id) {
    const song = list.find(item => item.Id == id);
    if (song) {
        let Id = song.Id;
        let Name = song.Name;
        let PhotoName = song.PhotoName;
        let SongFile = song.SongFile;
        let Lyrics = song.Lyrics;

        
        const modal = document.getElementById('myModal');
        const modalTitle = modal.querySelector('.modal-title');
        const modalBody = modal.querySelector('.modal-body');

        const modalFooter = modal.querySelector('.modal-footer');
        const deleteOk = modal.querySelector('.modal-footer .delete-ok');

        if (deleteOk) {
            modalFooter.classList.remove('d-flex', 'justify-content-between');
            deleteOk.remove();
        }

        modalTitle.textContent = `Bài hát: ${Name}`;
        modalBody.innerHTML = `
                    <div class="demo">
                        <img class="img-fluid" src="../../images/Songs/${PhotoName}" alt="${PhotoName}"/>
                        <audio controls class="ms-3">
                            <source src="/songs/${SongFile}" type="audio/mpeg" type="audio/mpeg">
                        </audio>
                    </div>
                    
                    <div class="lyric-content mt-3">
                        <div class="lyric-name">
                            Lời bài hát:
                        </div>
                        <div class="content" id="content">
                            ${Lyrics}
                        </div>
                        <div class="text-center" id="toggleButton" onclick="toggleText()">Xem toàn bộ <i class="fa-solid fa-arrow-down"></i></div>
                    </div>
            `;
        let lyrics;
        let lines;
        if (Lyrics === null) {
            lines = [];
        }
        else {
            lyrics = document.querySelector('.lyric-content .content').textContent.trim();
            lines = lyrics.split(/(?<=[.!?])\s+/).map(line => line.trim()).filter(line => line.length > 0);
        }
        const container = document.querySelector('.lyric-content .content');
        container.innerHTML = '';
        if (lines.length === 0) {
            const p = document.createElement('p');
            p.textContent = "Hiện tại chưa có lời bài hát.";
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
        new bootstrap.Modal(modal).show();
    }
    else {
        console.log(`Lỗi không có biến, song: ${song}`);
    }
}

function Delete(id,action) {
    if (id && action) {
        
        const modal = document.getElementById('myModal');
        const modalTitle = modal.querySelector('.modal-title');
        const modalBody = modal.querySelector('.modal-body');
        const modalFooter = modal.querySelector('.modal-footer');
        const deleteOk = modal.querySelector('.modal-footer .delete-ok');

        
        modalTitle.textContent = `Thông báo`;
        modalBody.innerHTML = `
                <form class="d-flex frm-delete" id="frm-delete" method="post" action="/Admin/${action}/Delete">
                    <div class="delete-content">
                        Bạn có chắc chắn muốn xóa không?
                    </div>
                    <input name='txt_id' class="form-control me-2" value="${id}" readonly hidden>
                </form>
            `;

        if (!deleteOk) {
            modalFooter.classList.add('d-flex', 'justify-content-between');
            var submitButton = document.createElement('button');
            submitButton.type = 'submit';
            submitButton.className = 'btn btn-primary delete-ok';
            submitButton.innerText = 'OK';
            submitButton.id = 'btn-ok';
            submitButton.addEventListener('click', function (event) {
                event.preventDefault();
                let form = document.getElementById('frm-delete');
                if (form) {
                    form.submit();
                }
            });
            modalFooter.appendChild(submitButton);
        }
        new bootstrap.Modal(modal).show();
    }
    else {
        console.log(`Lỗi không có biến, id: ${id}`);
    }
}