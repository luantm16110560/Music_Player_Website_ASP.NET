$(document).ready(function () {

    $('#overviewBtn').css('color','orange');
    $('#overviewBtn').click(()=>{
        $('#overviewBtn').css('color','orange');
        $('#introBtn').css('color','white');
        $('#overviewBody').css('display','block');
        $('#introduceBody').css('display','none');
        return false;
    });

    $('#introBtn').click(()=>{
        $('#introBtn').css('color','orange');
        $('#overviewBtn').css('color','white');
        $('#introduceBody').css('display','block');
        $('#overviewBody').css('display','none');
        return false;
    })
	
	//nút thêm 
	$('#add').click(() => {
		if (logged) {
		  $('.addPlayListUL').slideDown(200);
		  $('#body').css({
			'opacity': '0.5'
		  })
		}
		else {
		  $('#annouce').text('Vui lòng đăng nhập để thực hiện chức năng này');
		  $('.addSongStatus').css({ 'display': 'block' });
		  $('.addSongStatus').fadeOut(1500);
		}
	});
  
	//khi click ra ngoài modal thì tắt modal
  $(document).mouseup(function (e) {
    var container = $('#addPlayList');
    // if the target of the click isn't the container nor a descendant of the container
    if (!container.is(e.target) && container.has(e.target).length === 0) {
      container.slideUp(200);
      $('#body').css({ 'opacity': '1' })
    }
  });
	
    
	
});
//tải danh sách các playlist của menu popup
function addsongtoplaylist(playList, song) {

  if (logged ) {
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

function openInputNewPlayList() {

  $('.addNewPLayListInModal').css('display', 'none');
  $('.inputNewPlayList').css('display', 'block');
}
//download mp3 file
function downloadMP3File(song) {
  if (!logged) {
    $('#annouce').text('Vui lòng đăng nhập để thực hiện chức năng này');
    $('.addSongStatus').css({ 'display': 'block' });
    $('.addSongStatus').fadeOut(1500);
    event.preventDefault();
    return false;
  }
}

// follow nghệ sĩ

function followArtist(artistID) {

    if (logged) {
        $.ajax({
            type: "Post",
            url: "/TrangNgheSi/FollowArtist",
            data: {
                artistID: artistID
            },
            success: function (response) {

                switch(response.result){
                    case 1:
                        $('.profile>.btnfollow>img').attr('src', response.src);
                        $('.profile>.btnfollow').css('color', 'white');
                        break;
                    case 2:
                        $('.profile>.btnfollow>img').attr('src', response.src);
                        $('.profile>.btnfollow').css('color', '#20b484');
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

function annouce(content) {
    $('#annouce').text(content);
    $('.addSongStatus').css({ 'display': 'block' });
    $('.addSongStatus').fadeOut(1500);
}




function playAllSong() {

    if (list.length > 0) {
        $('#data').val(JSON.stringify(list));

        $('#playAll').click();
    }
    else {
        annouce('Không có bài hát nào trong danh sách. Vui lòng thử lại sau');
    }

}