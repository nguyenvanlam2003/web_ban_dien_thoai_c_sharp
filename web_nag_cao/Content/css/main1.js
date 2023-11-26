// Slider
var kichthuoc = document.getElementsByClassName("slider")[0].clientWidth;
var chuyenslider = document.getElementsByClassName("next-slides")[0];
var chuyen = 0;
var Img = chuyenslider.getElementsByClassName("slider__img");
var max = kichthuoc * Img.length;
max -= kichthuoc;
function next() {
    if (chuyen < max) chuyen += kichthuoc;
    else chuyen = 0
    chuyenslider.style.marginLeft = '-' + chuyen + 'px';
}
function prev() {
    if (chuyen == 0) chuyen = max;
    else chuyen -= kichthuoc;
    chuyenslider.style.marginLeft = '-' + chuyen + 'px';
}
setInterval("next()", 5000)