const listSong = document.querySelector('#result-box');
const inputSong = document.getElementById('keyword');
const getDataSearchDebounce = userDebounce(getDataSearch, 200);

function getDataSearch(keyword) {
    if (keyword.trim() === "") {
        listSong.innerHTML = "";
        return;
    }
    $.ajax({
        type: "GET",
        url: "/MTHome/AutoSearch",
        data: { key: keyword },
        dataType: "json",
        success: function (data) {
            loadData(data, listSong);
        },
        error: function (xhr, status, error) {
            console.error("Error: ", status, error);
        }
    });
}

function loadData(data, element) {
    element.innerHTML = "";
    let innerElement = "";
    data.forEach((item) => {
        innerElement += `
            <li>
                <a class="item-search" href ="/MTHome/SongDetail?id=${item.id}">${item.name}</a>
            </li>
        `;
    });
    element.innerHTML = innerElement; 
}

inputSong.addEventListener("input", () => {
    const keyword = inputSong.value;
    getDataSearchDebounce(keyword);
});

function userDebounce(callback, delay) {
    let timeout;
    return (input) => {
        clearTimeout(timeout);
        timeout = setTimeout(() => {
            callback(input);
        }, delay);
    }
}