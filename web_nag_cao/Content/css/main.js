// Slider
var kichthuoc= document.getElementsByClassName("slider")[0].clientWidth;
        var chuyenslider=document.getElementsByClassName("next-slides")[0];
        var chuyen=0;
        var Img=chuyenslider.getElementsByClassName("slider__img");
        var max=kichthuoc*Img.length;
        max-=kichthuoc;
        function next(){
        if(chuyen<max)chuyen+=kichthuoc;
        else chuyen=0
        chuyenslider.style.marginLeft='-'+chuyen+'px';
        }
        function prev(){
        if(chuyen==0)chuyen=max;
        else chuyen-=kichthuoc;
        chuyenslider.style.marginLeft='-'+chuyen+'px';
        }
        setInterval("next()",5000)

// Form
var email=document.querySelector('#email');
var password=document.querySelector('#password');
var confrimPassword=document.querySelector('#confrim-password');
var form=document.querySelector('form');


function showError(input,message){
    let parent=input.parentElement;
    let small=parent.querySelector('small');
    parent.classList.add('error');
    small.innerText=message
}
function showSucces(input){
    let parent=input.parentElement;
    let small=parent.querySelector('small');
    parent.classList.add('error');
    small.innerText=''
}
function checkEmptyError(listInput){
    let isEmptyError=false
     listInput.forEach(input => {
        input.value=input.value.trim()
        if(input.value==''){
           showError (input,'không được bỏ trống')
           isEmptyError=true
        }else{
            showSucces(input)
        }
     });
     return isEmptyError;
}
function checkEmail(input){
    const regexEmail =
  /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
    input.value=input.value.trim()
    let isEmailError=!regexEmail.test(input.value)
    if(regexEmail.test(input.value)){
        showSucces(input)
        return true
    }else{
        showError(input,'Email không hợp lệ')
    }
    return false
}
function checklength(input,min,max){
    input.value=input.value.trim()
    if(input.value.length<min){
        showError(input,`phải có ít nhất ${min} ký tự`)
        return true
    }
    if(input.value.length>max){
        showError(input,`không được quá  ${max} ký tự`)
        return true
    }
    showSucces(input)
    return false
}
function checkConfrim(pw,cfpw){
    if(pw.value!==cfpw.value){
        showError(cfpw,'không trùng password')
        return true
    }
    return false
}
form.addEventListener('submit',function(e){
    e.preventDefault();

     let isEmptyError = checkEmptyError([email,password,confrimPassword])
     let isEmailError
     let checkpasswordlength
     let checkCF
    if(email.value!=''){
        isEmailError=checkEmail(email)
    }
    if(password.value!=''){
        checkpasswordlength=checklength(password,3,16)
    }
    if(confrimPassword.value!=''){
        checkCF=checkConfrim(password,confrimPassword)
    }
   
})
email.addEventListener('blur', () => {
    checkEmptyError([email]);
    let isEmailError
    if(email.value!=''){
        isEmailError=checkEmail(email)
   }
  });
 password.addEventListener('blur', () => {
    checkEmptyError([password]);
    let checkpasswordlength
    if(password.value!=''){
        checkpasswordlength=checklength(password,3,16)
   }
  });
  confrimPassword.addEventListener('blur', () => {
    checkEmptyError([confrimPassword]);
    let checkCF
    if(confrimPassword.value!=''){
        checkCF=checkConfrim(password,confrimPassword)
    }
  });

const loginBtns = document.querySelectorAll('.js-login')
const modal = document.querySelector('.js-modal')
const modalBody = document.querySelector('.js-modal-body')
const modalClose = document.querySelector('.js-modal-close')

// Ham hien thi form dang nhap
function showLoginForm() {
    modal.classList.add('open')
}

// Ham an form dang nhap
function hideLoginForm() {
    modal.classList.remove('open')
}

for (const loginBtn of loginBtns) {
    loginBtn.addEventListener('click', showLoginForm)
}

modalClose.addEventListener('click', hideLoginForm)

modal.addEventListener('click', hideLoginForm)

modalBody.addEventListener('click', function (event) {
    event.stopPropagation() //Ngan chan noi bot su kien
})