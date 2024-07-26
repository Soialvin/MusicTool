let songfile= document.getElementById('songFile').innerText;
let songId = document.getElementById('songId').innerText;

const music = new Audio(`/Songs/${songfile}`);



let hasUpdatedListenCount = false;

let listenDuration = 0;

let play = document.getElementById('play');
let ware = document.getElementsByClassName('ware')[0];
play.addEventListener('click', () => {
    songfile = document.getElementById('songFile').innerText;
    if (music.paused || music.currentTime <= 0) {
        music.play().catch(error => {
            console.error('Lỗi khi phát âm thanh:', error);
        });
        play.classList.remove('fa-play');
        play.classList.add('fa-pause');
        ware.classList.add('active2');
        Array.from(document.getElementsByClassName('playlist-play')).forEach((e) => {
            if (e.id === songId) {
                e.querySelector('.fa-regular').classList.remove('fa-circle-play');
                e.querySelector('.fa-regular').classList.add('fa-circle-pause');
                document.getElementById(`${e.id}`).style.backgroundColor = "#E9F3F7";
            }
        });
    }
    else {
        music.pause();
        play.classList.add('fa-play');
        play.classList.remove('fa-pause');
        ware.classList.remove('active2');
        Array.from(document.getElementsByClassName('playlist-play')).forEach((e) => {
            if (e.id === songId) {
                e.querySelector('.fa-regular').classList.add('fa-circle-play');
                e.querySelector('.fa-regular').classList.remove('fa-circle-pause');
                document.getElementById(`${e.id}`).style.backgroundColor = "rgb(105,105,170,0)";
            }
        });
    }
});

let ID = '';
let mp3 = '';
let img = '';
let Name = '';
let lyric = '';

let idSong = document.getElementById('songId');
let songFile = document.getElementById('songFile');
let imgSong = document.getElementById('img-song');
let lyricSong = document.getElementById('content');
let nameSong = document.getElementById('name');
let nameLyric = document.getElementById('lyric-name');

const makeAllBackgrounds = () => {
    Array.from(document.getElementsByClassName('playlist-play')).forEach((element) => {
        element.style.backgroundColor = "rgb(105,105,170,0)";
    });
}

const makeAllPlays = () => {
    Array.from(document.getElementsByClassName('playlist-play')).forEach((element) => {
        element.querySelector('.fa-regular').classList.add('fa-circle-play');
        element.querySelector('.fa-regular').classList.remove('fa-circle-pause');
    });
}

Array.from(document.getElementsByClassName('playlist-play')).forEach((element) => {
    element.addEventListener('click', (e) => {
        ID = e.currentTarget.closest('.playlist-play').id;
        mp3 = e.currentTarget.closest('.playlist-play').querySelector('.song-file').innerText;;
        idSong.innerHTML = '';
        idSong.innerHTML = ID;
        songFile.innerHTML = '';
        songFile.innerHTML = mp3;
        makeAllPlays();
        img = e.currentTarget.closest('.playlist-play').querySelector('.img-song').innerText;
        Name = e.currentTarget.closest('.playlist-play').querySelector('.song-name-playlist').innerText;
        lyric = e.currentTarget.closest('.playlist-play').querySelector('.lyric-song').innerText;
        e.currentTarget.querySelector('.fa-regular').classList.remove('fa-circle-play');
        e.currentTarget.querySelector('.fa-regular').classList.add('fa-circle-pause');
        music.src = `/Songs/${mp3}`;
        imgSong.src = `/images/Songs/${img}`;
        lyricSong.innerHTML = '';
        nameSong.innerHTML = '';
        nameLyric.innerHTML = '';
        lyricSong.innerHTML = lyric;
        formatLyric()
        nameSong.innerHTML = Name;
        nameLyric.innerHTML = Name;
        music.addEventListener('canplaythrough', () => {
            music.play().catch(error => {
                console.error('Lỗi khi phát âm thanh:', error);
            });
            play.classList.remove('fa-play');
            play.classList.add('fa-pause');
            ware.classList.add('active2');
            music.addEventListener('ended', () => {
                play.classList.add('fa-play');
                play.classList.remove('fa-pause');
                ware.classList.remove('active2');
            });
            makeAllBackgrounds();
            document.getElementById(`${ID}`).style.backgroundColor = "#E9F3F7";
        }, { once: true });
    });
})

let currentStart = document.getElementById('currentStart');
let currentEnd = document.getElementById('currentEnd');
let seek = document.getElementById('seek');
let bar2 = document.getElementById('bar2');
let dot = document.getElementsByClassName('dot')[0];

music.addEventListener('timeupdate', () => {
    let music_curr = music.currentTime;
    let music_dur = music.duration;

    let min = Math.floor(music_dur / 60);
    let sec = Math.floor(music_dur % 60);
    if (sec < 10) {
        sec =`0${sec}`
    }
    currentEnd.innerText = `${min}:${sec}`;

    let min1 = Math.floor(music_curr / 60);
    let sec1 = Math.floor(music_curr % 60);
    if (sec1 < 10) {
        sec1 = `0${sec1}`
    }
    currentStart.innerText = `${min1}:${sec1}`;

    let progressbar = parseInt((music.currentTime/music.duration) * 100);
    seek.value = progressbar;
    let seekbar = seek.value;
    bar2.style.width = `${seekbar}%`;
    dot.style.left = `${seekbar}%`;

    listenDuration += (music.currentTime - listenDuration);

    //Setup time
    if (listenDuration >= 60 && !hasUpdatedListenCount) {
        updateListenCount(songId);
        hasUpdatedListenCount = true;
    }
});

seek.addEventListener('change', () => {
    music.currentTime = seek.value * music.duration / 100;
});

music.addEventListener('ended', () => {
    play.classList.add('fa-play');
    play.classList.remove('fa-pause');
    ware.classList.remove('active2');
    Array.from(document.getElementsByClassName('playlist-play')).forEach((e) => {
        if (e.id === songId) {
            e.querySelector('.fa-regular').classList.add('fa-circle-play');
            e.querySelector('.fa-regular').classList.remove('fa-circle-pause');
            document.getElementById(`${e.id}`).style.backgroundColor = "rgb(105,105,170,0)";
        }
    });
});

let vol_icon = document.getElementById('vol_icon');
let vol = document.getElementById('vol');
let vol_dot = document.getElementById('vol_dot');
let vol_bar = document.getElementsByClassName('vol_bar')[0];

function setInitialVolume() {
    let vol_value = vol.value;

    if (vol_value == 0) {
        vol_icon.classList.remove('fa-volume-low');
        vol_icon.classList.add('fa-volume-xmark');
        vol_icon.classList.remove('fa-volume-high');
    } else if (vol_value > 0 && vol_value <= 50) {
        vol_icon.classList.add('fa-volume-low');
        vol_icon.classList.remove('fa-volume-xmark');
        vol_icon.classList.remove('fa-volume-high');
    } else if (vol_value > 50) {
        vol_icon.classList.remove('fa-volume-low');
        vol_icon.classList.remove('fa-volume-xmark');
        vol_icon.classList.add('fa-volume-high');
    }

    vol_bar.style.width = `${vol_value}%`;
    vol_dot.style.left = `${vol_value}%`;
    music.volume = vol_value / 100;
}

document.addEventListener('DOMContentLoaded', setInitialVolume);

vol.addEventListener('change', () => {
    if (vol.value == 0) {
        vol_icon.classList.remove('fa-volume-low');
        vol_icon.classList.add('fa-volume-xmark');
        vol_icon.classList.remove('fa-volume-high');
    }
    if (vol.value > 0) {
        vol_icon.classList.add('fa-volume-low');
        vol_icon.classList.remove('fa-volume-xmark');
        vol_icon.classList.remove('fa-volume-high');
    }
    if (vol.value > 50) {
        vol_icon.classList.remove('fa-volume-low');
        vol_icon.classList.remove('fa-volume-xmark');
        vol_icon.classList.add('fa-volume-high');
    }
    let vol_a = vol.value;
    vol_bar.style.width = `${vol_a}%`;
    vol_dot.style.left = `${vol_a}%`;
    music.volume = vol_a / 100;
});

let back = document.getElementById('back');
let next = document.getElementById('next');

back.addEventListener('click', () => {
    const arr = Array.from(document.getElementsByClassName('playlist-play'));
    songId = document.getElementById('songId').innerText;
    let index = 0;
    index = arr.findIndex((e) => e.id === songId);
    resetListen();
    if (index !== -1) {
        if (index === 0) {
            index = arr.length - 1;
            mp3 = arr[index].querySelector('.song-file').innerText;
            ID = arr[index].id;
            idSong.innerHTML = '';
            idSong.innerHTML = ID;
            songFile.innerHTML = '';
            songFile.innerHTML = mp3;
            makeAllPlays();
            img = arr[index].querySelector('.img-song').innerText;
            Name = arr[index].querySelector('.song-name-playlist').innerText;
            lyric = arr[index].querySelector('.lyric-song').innerText;
            arr[index].querySelector('.fa-regular').classList.remove('fa-circle-play');
            arr[index].querySelector('.fa-regular').classList.add('fa-circle-pause');
            music.src = `/Songs/${mp3}`;
            imgSong.src = `/images/Songs/${img}`;
            lyricSong.innerHTML = '';
            nameSong.innerHTML = '';
            nameLyric.innerHTML = '';
            lyricSong.innerHTML = lyric;
            formatLyric()
            nameSong.innerHTML = Name;
            nameLyric.innerHTML = Name;
            music.addEventListener('canplaythrough', () => {
                music.play().catch(error => {
                    console.error('Lỗi khi phát âm thanh:', error);
                });
                play.classList.remove('fa-play');
                play.classList.add('fa-pause');
                ware.classList.add('active2');
                music.addEventListener('ended', () => {
                    play.classList.add('fa-play');
                    play.classList.remove('fa-pause');
                    ware.classList.remove('active2');
                });
                makeAllBackgrounds();
                document.getElementById(`${ID}`).style.backgroundColor = "#E9F3F7";
            }, { once: true });
        }
        else {
            index -= 1;
            mp3 = arr[index].querySelector('.song-file').innerText;
            ID = arr[index].id;
            idSong.innerHTML = '';
            idSong.innerHTML = ID;
            songFile.innerHTML = '';
            songFile.innerHTML = mp3;
            makeAllPlays();
            img = arr[index].querySelector('.img-song').innerText;
            Name = arr[index].querySelector('.song-name-playlist').innerText;
            lyric = arr[index].querySelector('.lyric-song').innerText;
            arr[index].querySelector('.fa-regular').classList.remove('fa-circle-play');
            arr[index].querySelector('.fa-regular').classList.add('fa-circle-pause');
            music.src = `/Songs/${mp3}`;
            imgSong.src = `/images/Songs/${img}`;
            lyricSong.innerHTML = '';
            nameSong.innerHTML = '';
            nameLyric.innerHTML = '';
            lyricSong.innerHTML = lyric;
            formatLyric()
            nameSong.innerHTML = Name;
            nameLyric.innerHTML = Name;
            music.addEventListener('canplaythrough', () => {
                music.play().catch(error => {
                    console.error('Lỗi khi phát âm thanh:', error);
                });
                play.classList.remove('fa-play');
                play.classList.add('fa-pause');
                ware.classList.add('active2');
                music.addEventListener('ended', () => {
                    play.classList.add('fa-play');
                    play.classList.remove('fa-pause');
                    ware.classList.remove('active2');
                });
                makeAllBackgrounds();
                document.getElementById(`${ID}`).style.backgroundColor = "#E9F3F7";
            }, { once: true });
        }
    }
});
next.addEventListener('click', () => {
    const arr = Array.from(document.getElementsByClassName('playlist-play'));
    songId = document.getElementById('songId').innerText;
    let index = 0;
    index = arr.findIndex((e) => e.id === songId);
    resetListen();
    if (index !== -1) {
        if (index === arr.length - 1) {
            index = 0;
            mp3 = arr[index].querySelector('.song-file').innerText;
            ID = arr[index].id;
            idSong.innerHTML = '';
            idSong.innerHTML = ID;
            songFile.innerHTML = '';
            songFile.innerHTML = mp3;
            makeAllPlays();
            img = arr[index].querySelector('.img-song').innerText;
            Name = arr[index].querySelector('.song-name-playlist').innerText;
            lyric = arr[index].querySelector('.lyric-song').innerText;
            arr[index].querySelector('.fa-regular').classList.remove('fa-circle-play');
            arr[index].querySelector('.fa-regular').classList.add('fa-circle-pause');
            music.src = `/Songs/${mp3}`;
            imgSong.src = `/images/Songs/${img}`;
            lyricSong.innerHTML = '';
            nameSong.innerHTML = '';
            nameLyric.innerHTML = '';
            lyricSong.innerHTML = lyric;
            formatLyric()
            nameSong.innerHTML = Name;
            nameLyric.innerHTML = Name;
            music.addEventListener('canplaythrough', () => {
                music.play().catch(error => {
                    console.error('Lỗi khi phát âm thanh:', error);
                });
                play.classList.remove('fa-play');
                play.classList.add('fa-pause');
                ware.classList.add('active2');
                music.addEventListener('ended', () => {
                    play.classList.add('fa-play');
                    play.classList.remove('fa-pause');
                    ware.classList.remove('active2');
                });
                makeAllBackgrounds();
                document.getElementById(`${ID}`).style.backgroundColor = "#E9F3F7";
            }, { once: true });
        }
        else {
            index += 1;
            mp3 = arr[index].querySelector('.song-file').innerText;
            ID = arr[index].id;
            idSong.innerHTML = '';
            idSong.innerHTML = ID;
            songFile.innerHTML = '';
            songFile.innerHTML = mp3;
            makeAllPlays();
            img = arr[index].querySelector('.img-song').innerText;
            Name = arr[index].querySelector('.song-name-playlist').innerText;
            lyric = arr[index].querySelector('.lyric-song').innerText;
            arr[index].querySelector('.fa-regular').classList.remove('fa-circle-play');
            arr[index].querySelector('.fa-regular').classList.add('fa-circle-pause');
            music.src = `/Songs/${mp3}`;
            imgSong.src = `/images/Songs/${img}`;
            lyricSong.innerHTML = '';
            nameSong.innerHTML = '';
            nameLyric.innerHTML = '';
            lyricSong.innerHTML = lyric;
            formatLyric()
            nameSong.innerHTML = Name;
            nameLyric.innerHTML = Name;
            music.addEventListener('canplaythrough', () => {
                music.play().catch(error => {
                    console.error('Lỗi khi phát âm thanh:', error);
                });
                play.classList.remove('fa-play');
                play.classList.add('fa-pause');
                ware.classList.add('active2');
                music.addEventListener('ended', () => {
                    play.classList.add('fa-play');
                    play.classList.remove('fa-pause');
                    ware.classList.remove('active2');
                });
                makeAllBackgrounds();
                document.getElementById(`${ID}`).style.backgroundColor = "#E9F3F7";
            }, { once: true });
        }
    }
});

function resetListen() {
    hasUpdatedListenCount = false;
    listenDuration = 0;
}

function updateListenCount(songId) {
    var xhr = new XMLHttpRequest();

    xhr.open("POST", "/MTHome/UpdateListenCount", true);
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

    xhr.onreadystatechange = function () {
        if (xhr.readyState === XMLHttpRequest.DONE) {
            if (xhr.status === 200) {
                console.log("Đã cập nhật lượt nghe thành công.");
            } else {
                console.error("Lỗi khi cập nhật lượt nghe.");
            }
        }
    };

    xhr.send("songId=" + encodeURIComponent(songId));
}