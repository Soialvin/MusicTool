var sliderTrack = document.querySelector(".image-slider-track");

var slides = sliderTrack.querySelectorAll(".slide");
slides.forEach(function (slide) {
    sliderTrack.appendChild(slide.cloneNode(true));
});