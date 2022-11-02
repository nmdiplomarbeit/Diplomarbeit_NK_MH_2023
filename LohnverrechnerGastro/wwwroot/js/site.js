////function Abfrage() {

////    if (fabo.checked)
////        alert('gewählt')
////   else
////        alert('nicht gewählt')

////}

//function Abfrage(fabo)
//    if (document.getElementById(fabo).checked) {
//        alert('gewählt')
//    }
//}

//if ($_POST["fabo"] == "checked") {
//    alert('gewählt')
//} 


//$(document).ready(() => {
//    $("#checkToggle").click(() => {
//        //$("#fabo").toggle(1000);
//        $("#fabo").css("visibility", "visible");
//    });
    

//});

// Quelle: https://www.w3schools.com/howto/howto_js_display_checkbox_text.asp

function togglefunc() {
    var checkTogglefabo = document.getElementById("checkTogglefabo");
    var zusatzfabo = document.getElementById("zusatzfabo");         

    if (checkTogglefabo.checked == true) {
        zusatzfabo.style.display = "block";
    } else {
        zusatzfabo.style.display = "none";
    }

    var checkToggleavabaeab = document.getElementById("checkToggleavabaeab");
    var zusatzavabaeab = document.getElementById("zusatzavabaeab");

    if (checkToggleavabaeab.checked == true) {
        zusatzavabaeab.style.display = "block";
    } else {
        zusatzavabaeab.style.display = "none";
    }

    var checkTogglependler = document.getElementById("checkTogglependler");
    var zusatzpendler = document.getElementById("zusatzpendler");

    if (checkTogglependler.checked == true) {
        zusatzpendler.style.display = "block";
    } else {
        zusatzpendler.style.display = "none";
    }
}

//function OpenPopup() {

//    alert('gewählt');
//    return false;
//}

$('#modal').on('shown.bs.modal', function () {
    $('#modal-body').trigger('focus')
})


