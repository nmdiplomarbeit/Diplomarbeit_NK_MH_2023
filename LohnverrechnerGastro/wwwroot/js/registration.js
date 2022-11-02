$(document).ready(() => {
    $("#Email").blur(() => {
        $.ajax({
            url: "/user/checkEmail",
            methode: "GET",
            data: { email: $("#Email").val() }
        })
            .done((dataFromServer) => {
                if (dataFromServer === true) {
                    $("#emailmessage").css("visibility", "visible");
                    $("#Email").addClass("redBorder");

                } else {
                    $("#emailmessage").css("visibility", "hidden");
                    $("#Email").removeClass("redBorder");

                }
            })
            .fail(() => {
                alert("Server/URL nicht erreichbar")
            });
    });

});

$(document).ready(() => {
    $("#Name").blur(() => {
        $.ajax({
            url: "/user/checkName",
            methode: "GET",
            data: { email: $("#Name").val() }
        })
            .done((dataFromServer) => {
                if (dataFromServer === true) {
                    $("#namemessage").css("visibility", "visible");
                    $("#Name").addClass("redBorder");

                } else {
                    $("#namemessage").css("visibility", "hidden");
                    $("#Name").removeClass("redBorder");

                }
            })
            .fail(() => {
                alert("Server/URL nicht erreichbar")
            });
    });

});



