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

function togglefunc(id) {

    document.getElementById("checkTogglearbeiter").checked = false;
    document.getElementById("checkToggleangestellter").checked = false;
    document.getElementById(id).checked = true;

    var checkTogglefabo = document.getElementById("checkTogglefabo");
    var zusatzfabo = document.getElementById("zusatzfabo");         

    if (checkTogglefabo.checked == true) {
        zusatzfabo.style.display = "inline";
    } else {
        zusatzfabo.style.display = "none";
    }

    var checkToggleavabaeab = document.getElementById("checkToggleavabaeab");
    var zusatzavabaeab = document.getElementById("zusatzavabaeab");

    if (checkToggleavabaeab.checked == true) {
        zusatzavabaeab.style.display = "inline";
    } else {
        zusatzavabaeab.style.display = "none";
    }

    var checkTogglependler = document.getElementById("checkTogglependler");
    var zusatzpendler = document.getElementById("zusatzpendler");

    if (checkTogglependler.checked == true) {
        zusatzpendler.style.display = "inline";
    } else {
        zusatzpendler.style.display = "none";
    }

    var checkTogglearbeiter = document.getElementById("checkTogglearbeiter");
    var arb = document.getElementById("arb");

    if (checkTogglearbeiter.checked == true) {
        arb.style.display = "inline";
    } else {
        arb.style.display = "none";
    }

    var checkToggleangestellter = document.getElementById("checkToggleangestellter");
    var ang = document.getElementById("ang");

    if (checkToggleangestellter.checked == true) {
        ang.style.display = "inline";
    } else {
        ang.style.display = "none";
    }

    
}


//function OpenPopup() {

//    alert('gewählt');
//    return false;
//}

$('#modal').on('shown.bs.modal', function () {
    $('#modal-body').trigger('focus')
})

function onInputChange() {
    let hours = document.querySelector("#hprowoche").value;
    let anz681_input = document.querySelector("#anz681");
    hours = hours.replace(",", ".")
    let anzahl_681 = (Math.round((hours - 40) * 4.33) - 10);


    if (hours >= 42.5 && hours <= 48) {
        //alert(anzahl_681)
        let value = prompt("Wert zwischen 0 und " + anzahl_681 + ":")

        anz681_input.value = value;
    }
}


