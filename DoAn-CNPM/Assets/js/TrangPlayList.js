


function delSongInPlayList() {
  $.ajax({
    type: "post",
    url: "/TrangPlayList/delSongInPlayList",
    success: function (response) {
      switch (response.result) {
        case 1: {
          $('#annouce').text(response.message);
          $('.addSongStatus').css({ 'display': 'block' });
          $('.addSongStatus').fadeOut(1500);
          location.reload();
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
