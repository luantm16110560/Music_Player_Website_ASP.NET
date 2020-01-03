


//function ClickTab(ele) {
//    $('.headerTab .list').each((index, element) => {
//        $(element).removeClass('currentTab');
//        console.log($(element).html());
//    });
//    $(ele).addClass('currentTab');
//}



var tab = [
    {
        name:'Tuần',
        value: 1
    },
    {
        name:'Tháng',
        value: 2
    },
    {
        name:'Nhiều lượt nghe nhất',
        value:3
    }
];

function getValByNameTab(name) {
    for (let i = 0; i < tab.length; i++) {
        if (tab[i].name == name)
            return tab[i].value;
    }
    return false;
}

$(document).ready(function () {
    $('.headerTab .list a').click((e) => {
        var name = $(e.target).text();
        var tabIndex = getValByNameTab(name);
        if (tabIndex) {
            $.ajax({
                type: "POST",
                url: "/TrangBXH/GetRank",
                data: {
                    tab: tabIndex
                },
                success: function (response) {
                    if (response != null && response.result==0) {
                        annouce(response.message);
                    }
                    else {
                        $('.listBXH').html(response);
                        $('.headerTab .list li').each((index, element) => {
                            $(element).removeClass('currentTab');
                        });
                        $(e.target).parent().addClass('currentTab');
                    }
                }
            });
        }

        


        
    });

});