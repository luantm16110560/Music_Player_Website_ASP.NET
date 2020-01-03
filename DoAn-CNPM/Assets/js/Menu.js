
// object song để tương tác trong các function, ví dụ như createNewPlayList

var songObject = {}


$(document).ready(function () {
    $('.menu li').hover(function () {
        // over
        $(this).find('ul:first').slideDown(300);
    }, function () {
        // out
        $(this).find('ul:first').slideUp(100);
    }
    );

    $('.user').hover(function () {
        // over
        $(this).find('ul:first').slideDown(300);
    }, function () {
        // out
        $(this).find('ul:first').slideUp(100);
    }
    );


    $('#userName').click(() => {
        var pageURL = "/TrangDangNhap/Index";
        var title="Login";
        // newwindow = window.open(url, 'name', 'height=500,width=500, location=0');
        // if (window.focus) { newwindow.focus() }
        OpenPopupCenter(pageURL, title, 500, 400);
        return false;
    })

    $('#txtSearch').focus(() => {
        $('.menu').hide(0);
        $('.search').css({
            'margin-left':'82%',
            'width':'0'
        });
        $('.search').animate({
            
            'margin-left':'20%',
            'width': '40%',
            'right':'5%'

        }, 500);
        $('.search>a').css('width', '10%');
        $('.search>input').css('width', '80%');
        
     
    })
    $('#txtSearch').blur(function (e) { 
        $('.menu').show(500);
        $('.search').animate({
            'margin-left':'1%',
            'width': '15%',
            'right':'0%'
        }, 500);

        $('.search>a').css('width', '20%');
        $('.search>input').css('width', '80%');
        
   
    });


});

function OpenPopupCenter(pageURL, title, w, h) {
    var left = (screen.width - w) / 2;
    var top = (screen.height - h) / 4;  // for 25% - devide by 4  |  for 33% - devide by 3
    var targetWin = window.open(pageURL, title, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
}


function annouce(content) {
    $('#annouce').text(content);
    $('.addSongStatus').css({ 'display': 'block' });
    $('.addSongStatus').fadeOut(1500);
}

// gọi modal add playlist
function addPlayListGeneral(songID) {
    $.ajax({
        type: "Post",
        url: '/TrangPlayList/GetPlayList',
        data: {
            songID: songID
        },
        success: function (response) {
            if (response != null && response.result != 0) {
                $('#addPlayList').html(response);
                $('.addPlayListUL').slideDown(200);
                $('#body').css({
                    'opacity': '0.5'
                })
                songObject.ID = songID;
            }
            else {
                annouce(response.message);
            }
            
        }
        
    });
}
//nút thêm playlist mới
function openInputNewPlayList() {

    $('.addNewPLayListInModal').css('display', 'none');
    $('.inputNewPlayList').css('display', 'block');
}

$(document).mouseup(function (e) {
    var container = $('#addPlayList');
    // nếu click ra ngoài thì tắt popup modal add playlist
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        container.slideUp(200);
        $('#body').css({ 'opacity': '1' })
    }
});


function createNewPlayList() {
    if (logged) {
        var name = $('#txtPlayListName').val();
        $.ajax({
            type: "post",
            url: "/TrangNgheNhac/AddNewPlayList",
            data: {
                playlistName: name,
                currentSong: songObject.ID
            },
            success: function (response) {
                console.log(response);
                if (response) {
                    $('.listPlayListInModal').html(response);
                    $('.addNewPLayListInModal').css('display', 'block');
                    $('.inputNewPlayList').css('display', 'none');
                }
                else {
                    annouce('Có lỗi xảy ra. Vui lòng thử lại sau!');
                }
            }
        });
    }
    else {
        annouce('Vui lòng đăng nhập để thực hiện chức năng này!');
    }

    event.preventDefault();
    return false;
}

//tải danh sách các playlist của menu popup
function addsongtoplaylist(playList, song) {

    if (logged) {
        $.ajax({
            type: "post",
            url: "/TrangNgheNhac/AddPlayList",
            data: {
                'playlist': playList,
                'song': song
            },
            success: function (response) {
                console.log(response);
                switch (response.result) {
                    case 1: {

                        $('#img' + playList).attr('src', response.imgsrc);
                        $('#annouce').text(response.message);
                        $('.addSongStatus').css({ 'display': 'block' });
                        $('.addSongStatus').fadeOut(1500);
                        break;
                    }
                    default: {
                        $('#annouce').text(response.message);
                        $('.addSongStatus').css({ 'display': 'block' });
                        $('.addSongStatus').fadeOut(1500);
                        break;
                    }
                }
            }
        });

    }
    else {
        $('#annouce').text('Vui lòng đăng nhập để thực hiện chức năng này');
        $('.addSongStatus').css({ 'display': 'block' });
        $('.addSongStatus').fadeOut(1500);
    }
    event.preventDefault();
    return false;
}

//check đăng nhập trước khi tải nhạc
function downloadMP3File(song) {
    if (!logged) {
        $('#annouce').text('Vui lòng đăng nhập để thực hiện chức năng này');
        $('.addSongStatus').css({ 'display': 'block' });
        $('.addSongStatus').fadeOut(1500);
        event.preventDefault();
        return false;
    }
}

function checkLogin(val) {
    if (!val) {
        annouce('Vui lòng đăng nhập để thực hiện chức năng này');
        event.preventDefault();
        return false;
    }
}