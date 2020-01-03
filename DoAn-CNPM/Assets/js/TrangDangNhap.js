$(document).ready(function () {
    $('#txtUser').focus();
    $('#Signin').click(() => {
        $('.content').css('display', 'block');
        $('#Signup').removeClass('currentTab');
        $('.contentSignup').css('display', 'none');
        $('#Signin').addClass('currentTab');
        $('#txtUser').focus();
        return false;
    });

    $('#Signup').click(() => {
        $('.content').css('display', 'none');
        $('#Signup').addClass('currentTab');
        $('.contentSignup').css('display', 'block');
        $('#Signin').removeClass('currentTab');
        $('#txtUserSignUp').focus();
        return false;
    })


    $('#btnLogin').click(() => {

        if($('#txtUser').val()==='' || $('#txtPass').val()===''){
            $('.errorLogin').text('Tài khoản hoặc mật khẩu không được để trống');
                        $('.errorLogin').show();
        }
        else{
            $.ajax({
                type: "post",
                url: "/TrangDangNhap/Login",
                data: {
                    user: $('#txtUser').val(),
                    pass: md5($('#txtPass').val())
                },
                success: function (response) {
                    if (response === "1") {
                        window.opener.location.reload(true);
                        window.close();
                    }
                    else {
                        $('.errorLogin').text(response);
                        $('.errorLogin').show();
                        $('#txtUser').val('');
                        $('#txtPass').val('');
                    }
                }
            });
        }

        
        return false;
    })

    $('#btnSignUp').click(()=>{
        var username = $('#txtUserSignUp').val();
        var pass = $('#txtPassSignUp').val();
        var name = $('#txtName').val();
        var phone = $('#txtPhone').val();
        var email = $('#txtEmail').val();
        var repass = $('#txtRePassSignUp').val();

        if(pass!=repass){
            $('.errorSignUp').text('mật khẩu và xác nhận mật khẩu không đúng');
            $('.errorSignUp').show();
        }
        else{
            if(username=='' || pass=='' || name=='' || phone=='' || email=='' || repass=='' ){
                $('.errorSignUp').text('Không được để trống bất kì thông tin nào');
                $('.errorSignUp').show();
            }
            else{
                $.ajax({
                    type: "post",
                    url: "/TrangDangNhap/SignUp",
                    data: {
                        user: username,
                        pass: md5(pass),
                        phone: phone,
                        email: email,
                        name: name
                    },
                    success: function (response) {
                        if (response === "1") {
                            window.opener.location.reload(true);
                            window.close();
                        }
                        else {
                            $('.errorSignUp').text(response);
                            $('.errorSignUp').show();
                            $('#txtPass').val('');
                            $('#txtRePass').val('');
                        }
                    }
                });
            }
        }
    })

    $('.content').keypress((event)=>{
        var keycode = (event.keyCode ? event.keyCode : event.which);
	    if(keycode == '13'){
		    $('#btnLogin').click();
	    }
    })
    $('.contentSignup').keypress((event)=>{
        var keycode = (event.keyCode ? event.keyCode : event.which);
	    if(keycode == '13'){
		    $('#btnSignUp').click();
	    }
    })
});