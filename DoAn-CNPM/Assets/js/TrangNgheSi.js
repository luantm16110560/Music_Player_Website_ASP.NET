



function followArtistInAllArtistPage(artistID,element) {

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


