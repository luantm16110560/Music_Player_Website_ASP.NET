//quy định
//page: 0 là song, 1 là album, 2 là artist, 3 là thể loại, 4 là người dùng
//isAdd: true là thêm, false là cập nhật
const PAGES = [
    '/TrangAdmin/GetSongDetails',
    '/TrangAdmin/GetAlbumDetails',
    '/TrangAdmin/GetArtistDetails',
    '/TrangAdmin/GetGenreDetails',
    '/TrangAdmin/GetUserDetails',
]

const GET_PAGE= [
    '/TrangAdmin/GetSong',
    '/TrangAdmin/GetAlbum',
    '/TrangAdmin/GetArtist',
    '/TrangAdmin/GetGenre',
    '/TrangAdmin/GetUser',
]

var selectedObj=[];


$(document).ready(function () {
    $('#accordionSidebar li').each((index, element) => {
        $(element).click(() => {
            
            var tab = $(element).find('span').text();

     
                selectedObj = [];
                $('.generalClass').css('display', 'none');
                switch (tab) {
                    case 'Bài hát': {
                        Get(0);

                        break;
                    }
                    case 'Album': {
                        Get(1);

                        break;
                    }
                    case 'Nghệ sĩ': {
                        Get(2);

                        break;
                    }
                    case 'Thể loại': {
                        Get(3);

                        break;
                    }
                    case 'Người dùng': {
                        Get(4);
                        break;
                    }
                }
         })
    });

    $('#AddUserAdmin').on('show.bs.modal', function (e) {
        $('#AddUserAdmin').find('.form-group input').val(null);
    });
});




function AddSong() {

        $.ajax({
            type: "POST",
            url: "/TrangAdmin/GetSongDetails",
            data: {
                id:-1
            },
            success: function (response) {
                $('#SongDetailsModal').find('.modal-body').html(response);
                $('#SongDetailsModal').modal('show');
            }
        });

}

function UpdateSong() {
    var id = selectedObj[0];

    if (selectedObj.length > 0) {
        $.ajax({
            type: "POST",
            url: "/TrangAdmin/GetSongDetails",
            data: {
                id:id
            },
            success: function (response) {
                $('#SongDetailsModal').find('.modal-body').html(response);
                $('#SongDetailsModal').modal('show');
            }
        });
    }
    else {
        alert('Vui lòng chọn bài hát để cập nhật');
    }

    
}



function AddAlbum() {
    $.ajax({
        type: "POST",
        url: "/TrangAdmin/GetAlbumDetails",
        data: {
            id: -1
        },
        success: function (response) {
            $('#SongDetailsModal').find('.modal-body').html(response);
            $('#SongDetailsModal').modal('show');
        }
    });
}

function UpdateAlbum() {
    var id = selectedObj[0];

    if (selectedObj.length > 0) {
        $.ajax({
            type: "POST",
            url: "/TrangAdmin/GetAlbumDetails",
            data: {
                id: id
            },
            success: function (response) {
                $('#SongDetailsModal').find('.modal-body').html(response);
                $('#SongDetailsModal').modal('show');
            }
        });
    }
    else {
        alert('Vui lòng chọn bài hát để cập nhật');
    }
}




function AddOrEdit(indexPage, isAdd) {
    if (isAdd) {
        $.ajax({
            type: "POST",
            url: PAGES[indexPage],
            data: {
                id: -1
            },
            success: function (response) {
                $('#AllModal').find('.modal-body').html(response);
                $('#AllModal').modal('show');
            }
        });
    }
    else {
        if (selectedObj.length > 0) {
            $.ajax({
                type: "POST",
                url: PAGES[indexPage],
                data: {
                    id: selectedObj[0]
                },
                success: function (response) {
                    $('#AllModal').find('.modal-body').html(response);
                    $('#AllModal').modal('show');
                }
            });
        }
        else {
            alert('Vui lòng chọn đối tượng');
        }
    }
}

function Get(indexPage) {
    $.ajax({
        type: "Get",
        url: GET_PAGE[indexPage],
        success: function (response) {
            $('#Content').html(response);
            $('#Content').css('display', 'block');
        }
    });
}

function LockOrUnlockUser() {
    if (selectedObj.length > 0) {
        //var result = $('#isLockUser').val();
        var isLock = selectedObj[2];
        var message = isLock ? 'khóa' : 'mở khóa';
        var result = confirm('Bạn muốn ' + message + ' tài khoản này?');
        if (result) {
            location.href = "/TrangAdmin/LockOrUnlockUser?username=" + selectedObj[0] + '&&Status=' + !isLock;
        }
    }
    
}

function signUpAdmin() {
    var username = $('#newUsername').val();
    var newpass = $('#newPassword').val();
    var verify = $('#verifyPassword').val();
    if (newpass != verify) {
        alert('Xác nhận mật khẩu không đúng');
    }
    else{
        $.ajax({
            type: "Post",
            url: "/TrangAdmin/SignUpAdmin",
            data: {
                username: username,
                password: md5(newpass)
            },
            success: function (response) {
                if (response.result == 1) {
                    alert('Tạo thành công');
                    $('#AddUserAdmin').modal('hide');
                }
                else{
                    alert('Tạo thất bại');   
                }
            }
        });
    }
}

