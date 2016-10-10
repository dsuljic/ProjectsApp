
function showModalPass()
{

    $('#passModal').modal('toggle');
}


function updatePass() {
    var old_pass = document.getElementById('oldPass').value.trim();
    var new_pass_1 = document.getElementById('newPass1').value.trim();
    var new_pass_2 = document.getElementById('newPass2').value.trim();
    if (new_pass_1 != new_pass_2)
        alert("Please retype your new password correctly!");
    else
        $.ajax({
            url: "/User/ChangePass",
            type: 'POST',
            dataType: 'json',
            data: { old_pass: old_pass, new_pass: new_pass_1},
            success: function (data) {
                if (data.ok = "ok")
                    $('#passModal').modal('hide');
                else
                    alert("Incorrect password!");
            },
            async: false
        });
}


function showModalEdit()
{
    document.getElementById('name').value = @ViewBag.Name;
    document.getElementById('address').value = @ViewBag.Address;
    document.getElementById('phone').value = @ViewBag.PhoneNumber;
    document.getElementById('email').value = @ViewBag.E_mail;

    $('#myModal').modal('toggle');
}

