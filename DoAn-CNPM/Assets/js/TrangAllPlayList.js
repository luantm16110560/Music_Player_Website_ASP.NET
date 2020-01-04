function delFavoriteSong(songID) {
    $.ajax({
        type: "post",
        url: "/TrangPlayList/delFavoriteSong",
        data: {
            songID: songID
        },
        success: function (response) {
            $('#annouce').text(response.message);
            $('.addSongStatus').css({ 'display': 'block' });
            $('.addSongStatus').fadeOut(1500);
            location.reload();
        }
    });
}

function downloadFile(name,url) {
    var link = document.createElement('a');
    link.href = url;
    link.download = name;
    link.click();
}
function startPlayList(playlistID){
    location.href='/TrangPlayList/Index?playlistID='+playlistID;
}

//$('.mainTab>li').click(() => {
//    $('.mainTab>li>p').css('color', 'white');
//    alert($(this).text());
//})
$('.allPlayListContent').css('display', 'none');
$('#privateSong').css('display', 'block');

$('.mainTab li p').on('click', function () {
    $('.mainTab li').removeClass('selectedMainTab');


    $(this).parent().addClass('selectedMainTab');
    $('.allPlayListContent').css('display', 'none');
    
    switch ($(this).text()) {
        case 'Nhạc cá nhân': {
            $('#privateSong').css('display', 'block');
            break;
        }
        case 'Upload nhạc': {
            refeshUploadTab();
            $('#uploadSong').css('display', 'block');
            break;
        }
        case 'Thông tin tài khoản': {
            $('#profile').css('display', 'block');
            break;
        }
        case 'Nghệ sĩ quan tâm': {
            $('#artistCare').css('display', 'block');
            break;
        }
        default: break;
    }
});


CKEDITOR.replace('uploadSongLyrics');

$(".custom-file-input").on("change", function () {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
});


function refeshUploadTab() {
    $('#uploadSong input').val('');
    CKEDITOR.instances.uploadSongLyrics.setData('');
    $('#fileName').text(null);
    $('#uploadArtistName').prop('selectedIndex', 0);
    $('#uploadComposer').prop('selectedIndex', 0);
    $('#uploadGenre').prop('selectedIndex', 0);

}



function GetFile(selector) {
    var formdata = new FormData()

    if ($('#'+selector).prop('files').length > 0) {
        var file = $('#' + selector).prop('files')[0];
        formdata.append('file', file);

        for (var key of formdata.entries()) {
            console.log(key[0] + ', ' + key[1])
        }
        return formdata;
    }
    else {
        return null;
    }
}

function uploadSong() {
    var formData = new FormData();
        
    var songName = $('#uploadSongName').val();
    var lyrics = CKEDITOR.instances.uploadSongLyrics.getData();
    //var songFile = GetFile('uploadSongFile');
    var songFile = $('#uploadSongFile').prop('files')[0];
    var artist = $('#uploadArtistName').val();
    var composer = $("#uploadComposer").val();
    var genre = $('#uploadGenre').val();
    

    formData.append("songName", songName);
    formData.append("file", songFile);
    formData.append("artist", artist);
    formData.append("lyrics", lyrics);
    formData.append("composer", composer);
    formData.append("genre", genre);

    if (songName && songFile && artist) {
        $.ajax({
            type: "post",
            url: "/TrangPlayList/UploadSong",
            enctype: 'multipart/form-data',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                refeshUploadTab();
                annouce(response.message);
            }
        });
    }
    else {
        annouce('Vui lòng điền các thông tin bắt buộc');
        //alert('Vui lòng điền các thông tin bắt buộc');
    }
}
 
$(document).ready(function () {
    //validate submit form profile
    $('#btnSubmitProfile').click(() => {
        var name = $('#name').val();
        var phone = $('#phone').val();
        var email = $('#email').val();
        var oldPass = $('#oldPass').val();
        var pass = $('#password').val();
        var phoneFilter = /[0-9 -()+]+$/;
        var verifyPass = $('#verifyPassword').val();
        if (name || phone || email || pass || verifyPass || oldPass) {
            
            if (pass != verifyPass) {
                annouce('Xác nhận mật khẩu không trùng');
                return false;
            }
            if (phone && (!phoneFilter.test(phone) || phone.length != 10)) {
                annouce('Vui lòng nhập số điện thoại đúng định dạng');
                return false;
            }
            if((pass && verifyPass && oldPass) || (!pass && !verifyPass && !oldPass))
            {
                $.ajax({
                    type: "post",
                    url: "/TrangDangNhap/UpdateProfile",
                    data: {
                        UserName: $('#username').val(),
                        Name: name,
                        Phone: phone,
                        Email: email,
                        Password: pass? md5(pass) : null,
                        OldPassword: oldPass? md5(oldPass) : null
                    },
                    success: function (response) {
                        if (response.result == 1) {
                            if (name) {
                                $('#name').attr("placeholder", name);
                                $('#name').val(null);
                            }
                                
                            if (phone) {
                                $('#phone').attr("placeholder", phone);
                                $('#phone').val(null);
                            }
                                
                            if (email) {
                                $('#email').attr("placeholder", email);
                                $('#email').val(null);
                            }
                        }
                        annouce(response.message);
                    }
                });
                $('#oldPass').val(null);
                $('#password').val(null);
                $('#verifyPassword').val(null);
                return false;
            }
            else {
                annouce('Vui lòng điền đủ thông tin để đổi mật khẩu');
                return false;
            }
        }
        else {
            annouce('Vui lòng điền thông tin cập nhật');
        }

        
        return false;
    })
})

function checkPlayList(count) {
    if (count ==0 ) {
        annouce('Không có bài hát trong playlist');
        event.preventDefault();
        return false;
    }

}




function followArtistInAllArtistPage(artistID, element) {

    if (logged) {
        $.ajax({
            type: "Post",
            url: "/TrangNgheSi/FollowArtist",
            data: {
                artistID: artistID
            },
            success: function (response) {

                switch (response.result) {
                    case 1:

                        $(element).css('color', 'white');
                        break;
                    case 2:
                        $(element).css('color', '#20b484');
                        break;
                    default:
                        break;
                }

                annouce(response.message);
            }
        });
    }
    else {

        annouce('Vui lòng đăng nhập để thực hiện chức năng này');
    }
    return false;
}
