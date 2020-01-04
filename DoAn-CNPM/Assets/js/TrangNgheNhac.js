$(document).ready(function () {

  //set min max input range

  //init
  var audio = document.getElementById("player");
  var slider = $("#slider");


  //check nội dung lời bài hát và ẩn bớt
  $('#readMore').click(function () {
    var text = $('#readMore').text();
    if (text === 'Xem thêm') {
      $('#lyric').removeClass('contentLyric');
      $('#readMore').text('Rút gọn');
    }
    else {
      $('#lyric').addClass('contentLyric');
      $('#readMore').text('Xem thêm');
    }
    return false;
  })

  //xử lý file audio
  audio.load();
  audio.onloadedmetadata = function () {
    //get duration and set max input range
    var tam = audio.duration;
    var durationTime = parseInt(tam, 10);

    slider.attr("max", durationTime);

    var minutes = String(parseInt(durationTime / 60)).padStart(2, "0");
    var seconds = String(durationTime % 60).padStart(2, "0");

    $("#durationTime").html(minutes + ":" + seconds);

    $("#slider").on("change input", function () {
      audio.pause();
      audio.currentTime = $(this).val();
      audio.play();
      var val =
        ($(this).val() - $(this).attr("min")) /
        ($(this).attr("max") - $(this).attr("min"));

      $(this).css(
        "background-image",
        "-webkit-gradient(linear, left top, right top, " +
        "color-stop(" +
        val +
        ", #0ba14b), " +
        "color-stop(" +
        val +
        ", #C5C5C5)" +
        ")"
      );
    });

    // tí nhở mở lại
    var play = audio.play();
    if (play != undefined) {
      play
        .then(_ => { })
        .catch(err => {
          //alert(err);
          //alert(audio.paused);
          $("#play").attr('src', '/Assets/test/play.png');
        });
    }
  };

  //nút yêu thích
  $('#favorite').click(() => {
    $.ajax({
      type: "post",
      url: "/TrangNgheNhac/LikeSong",
      data: {
        song: currentSong
      },
      success: function (response) {
        switch (response.result) {
          case 1: {
            $('#annouce').text(response.message);
            $('.addSongStatus').css({ 'display': 'block' });
            $('.addSongStatus').fadeOut(1500);
            $('#imgLike').attr('src', response.imgsrc);
            if (response.like != null) {
              $('#likeCount').text(response.like);
            }
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
  })


  //khi click ra ngoài modal thì tắt modal
  $(document).mouseup(function (e) {
    var container = $('#addPlayList');
    // nếu click ra ngoài thì tắt popup modal add playlist
    if (!container.is(e.target) && container.has(e.target).length === 0) {
      container.slideUp(200);
      $('#body').css({ 'opacity': '1' })
    }
  });

  //nút thêm 
  $('#add').click(() => {
      if (logged) {
          $.ajax({
              type: "Post",
              url: '/TrangPlayList/GetPlayList',
              data:{
                  songID: currentSong
              },
              success: function (response) {
                  $('#addPlayList').html(response);
                  $('.addPlayListUL').slideDown(200);
                  $('#body').css({
                      'opacity': '0.5'
                  })
              }
          });
    }
    else {
      $('#annouce').text('Vui lòng đăng nhập để thực hiện chức năng này');
      $('.addSongStatus').css({ 'display': 'block' });
      $('.addSongStatus').fadeOut(1500);
    }
  });

    //nghe bài tiếp theo
  $('#forward').click(() => {
      
      
      var now = new Date();
      now.setTime(now.getTime() + 1 * 3600 * 1000);
      document.cookie = "previousSong=" + currentSong + "; expires= " + now.toUTCString();

      if (typePage == 1) {

          
          location.href = '/TrangNgheNhac/Index?id=' + nextSong;
      }
      else if (typePage==4) {
          changeSongInList(nextSong);
      }
      else {
          changeSongInPlayList(nextSong);
      }
  })
    //nghe bài trước
  $('#backward').click(() => {
      var previousSong = getCookie('previousSong');
      if (previousSong && previousSong!='-1') {
          if (typePage == 1) {
              location.href = '/TrangNgheNhac/Index?id=' + previousSong+"&&preID="+currentSong;
          }
          else if (typePage == 4) {
              changeSongInList(previousSong);
          }
          else {
              changeSongInPlayList(previousSong);
          }
      }
      else {
          annouce('Bài hát trước đó không tồn tại');
      }
  })
});

//del cookie
function deleteCookie(name) { setCookie(name, '', -1); }

//set cookie
function setCookie(name, value, days) {
    var d = new Date;
    d.setTime(d.getTime() + 24 * 60 * 60 * 1000 * days);
    document.cookie = name + "=" + value + ";path=/;expires=" + d.toGMTString();
}

//get cookie
function getCookie(name) {
    var v = document.cookie.match('(^|;) ?' + name + '=([^;]*)(;|$)');
    return v ? v[2] : null;
}





function changeValueVolume(slider) {

  var audio = document.getElementById("player");
  audio.volume = slider.value / 10;
  if (slider.value == 0) {
    $('#volume').attr('src', '/Assets/test/volume-mute.png');
  }
  else {
    $('#volume').attr('src', '/Assets/test/volume.png');
  }
  var val = slider.value;


}

function PlayTimeSlider() {
  $("#play").attr('src', '/Assets/test/pause.png');

}

$('#replay').click(function () {
  if ($(this).attr('src') == '/Assets/test/reload-off.png') {
    $(this).attr('src', '/Assets/test/reload-on.png');
    $('#replayValue').prop('checked', true);


  }
  else {
    $(this).attr('src', '/Assets/test/reload-off.png');
    $('#replayValue').prop('checked', false);

  }
})

function PauseTimeSlider() {
  $("#play").attr('src', '/Assets/test/play.png');
}

// auto play
function endTimeslider(audio) {

  if ($('#replayValue').is(':checked')) {

    $("#play").click();
  }
  else {
    if (typePage == 1) {
      location.href = '/TrangNgheNhac/Index?id=' + nextSong;
    }
	else if(typePage==4){
		changeSongInList(nextSong);
	}
    else {
      changeSongInPlayList(nextSong);
    }

  }

  var myAudio = document.getElementById("player");
  myAudio.currentTime = 0;
}

//xử lý nút volume

$('#volume').click(function () {
  var slider = document.getElementById('valueVolume');
  var myAudio = document.getElementById("player");
  if (slider.value == 0) {
    slider.value = 10;
    myAudio.volume = 1;
    $('#volume').attr('src', '/Assets/test/volume.png');
  }
  else {
    slider.value = 0;
    myAudio.volume = 0;
    $('#volume').attr('src', '/Assets/test/volume-mute.png');
  }
})


//xử lý nút pause và play

// function play() {
//   var myAudio = document.getElementById("player");

//   $('#play').css('display', 'none');
//   $('#pause').css('display', 'block');
//   myAudio.play();
// }
// function pause() {
//   var myAudio = document.getElementById("player");

//   $('#play').css('display', 'block');
//   $('#pause').css('display', 'none');
//   myAudio.pause();
// }

function playorpause(img) {
  var myAudio = document.getElementById('player');
  if (myAudio.paused) {
    myAudio.play();
    $(this).attr('src', '/Assets/test/pause.png');
  }
  else {
    myAudio.pause();
    $(this).attr('src', '/Assets/test/play.png');
  }
}

function UpdateTimeSlider(audio) {
  $("#slider").val(parseInt(audio.currentTime, 10));

  var slider = $("#slider");
  var val =
    (slider.val() - slider.attr("min")) /
    (slider.attr("max") - slider.attr("min"));

  slider.css(
    "background-image",
    "-webkit-gradient(linear, left top, right top, " +
    "color-stop(" +
    val +
    ", #0ba14b), " +
    "color-stop(" +
    val +
    ", #C5C5C5)" +
    ")"
  );

  var valueHover = slider.val();

  var minutes = String(parseInt(valueHover / 60)).padStart(2, "0");
  var seconds = String(valueHover % 60).padStart(2, "0");
  $("#currentTime").html(minutes + ":" + seconds);
}

//attach to slider and fire on mousemove
document.getElementById("slider").addEventListener("mousemove", function (e) {
  valueHover = parseInt(calcSliderPos(e).toFixed(2), 10);

  var minutes = String(parseInt(valueHover / 60)).padStart(2, "0");
  var seconds = String(valueHover % 60).padStart(2, "0");

  $(this).attr("title", minutes + ":" + seconds);
});

function calcSliderPos(e) {
  return (
    (e.offsetX / e.target.clientWidth) *
    parseInt(e.target.getAttribute("max"), 10)
  );
}
//get specified cookie 
function getCookie(cname) {
  var name = cname + "=";
  var decodedCookie = decodeURIComponent(document.cookie);
  var ca = decodedCookie.split(';');
  for (var i = 0; i < ca.length; i++) {
    var c = ca[i];
    while (c.charAt(0) == ' ') {
      c = c.substring(1);
    }
    if (c.indexOf(name) == 0) {
      return c.substring(name.length, c.length);
    }
  }
  return "";
}






function changeSongInPlayList(songID) {
    
    var previousSongID = currentSong;
	
	
    $.ajax({
        type: "post",
        url: "/TrangPlayList/GetSongInPlayList",
        data: {
            song: songID,
            type: typePage
        },
        success: function (response) {
 
            $('#songModal').html(response);

            $('#faSong' + previousSongID).find('.songName').css({
                color: 'white'
            })

            //đổi màu bài hát đang play
            $('#faSong' + currentSong).find('.songName').css({
                color: '#20b484'
            })
        }
    });
}

function changeSongInList(songID) {
    var previousSongID = currentSong;
    $.ajax({
        type: "post",
        url: "/TrangNgheNhac/GetSongPartial",
        data: {
            songID: songID
        },
        success: function (response) {

            if (typeof response.result !== 'undefined' && response.result ==-1) {
                annouce(response.message);
                console.log(response.message);
            }
            else {
                $('#songModal').html(response);

                $('#faSong' + previousSongID).find('.songName').css({
                    color: 'white'
                })

                //đổi màu bài hát đang play
                $('#faSong' + currentSong).find('.songName').css({
                    color: '#20b484'
                })
            }

            
        }
    });
}