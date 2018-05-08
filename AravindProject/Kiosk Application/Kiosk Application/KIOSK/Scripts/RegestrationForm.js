jQuery.fn.preventDoubleSubmission = function () {
    $(this).on('submit', function (e) {
        var $form = $(this);

        if ($form.data('submitted') === true) {
            // Previously submitted - don't submit again
            e.preventDefault();
        } else {
            // Mark it so that the next submit can be ignored
            $form.data('submitted', true);
        }
    });

    // Keep chainability
    return this;
};
       
       

$(document).ready(function () {
           
    $('form').preventDoubleSubmission();
    $('#age').keyboard({

        layout: 'custom',

        customLayout: {
            'normal': [

                '1 2 3',
                '4 5 6',
                '7 8 9',
                '0 ',
                '{bksp} {accept} {cancel}',

            ]
        },
        validate: function (keyboard, value, isClosing) {

            // only allow a 3-digit number
            var valid = /^\d{1,3}$/g.test(value);

            if (isClosing && valid) {

                // *** closing and valid ***
                return true;
            } else if (isClosing && !valid) {

                // *** closing and not valid ***
                // add an indicator/popup here to tell the user the input is not valid
                keyboard.$preview.addClass('myclassborder') // needs css: .red-border { border: #f00 1px solid; }
                // remove indicator after 5 seconds
                setTimeout(function () {
                    keyboard.$preview.removeClass('myclassborder'); // no more red border
                }, 5000);
                // fire off a canceled event
                keyboard.$el.trigger('canceled', [keyboard, keyboard.el]);
                return false;
            }

            // *** not closing ***
            // continuous checking during input, so don't go nuts here
            // accept button is enabled/disabled automatically if "acceptValid" is true
            return valid;
        },
        accepted: function (event, keyboard, el) {
            var d = new Date();
            var month = d.getMonth() + 1;
            var day = d.getDate();

            var output = d.getFullYear() + '/' +
                (month < 10 ? '0' : '') + month + '/' +
                (day < 10 ? '0' : '') + day;
            var year = output.substring(0, 4);
            var query = el.value;
            //alert(year - query);
            var check = year - query;
            if (check > 0 && el.value <=150)
                $('#DOB').val('01/01/' + (year - query).toString());
            else {
                $("#age").val('');
                $('#DOB').val('');
            }
        },
        repeatRate: 0
    });

    $("#district").prop('readonly', true);
    $("#state").prop('readonly', true);
    $("#country").prop('readonly', true);
        
    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });
    /* $('#name').keypress(function (e) {

         if ((e.which < 97 || e.which > 122) && (e.which < 65 || e.which > 90)&&e.which!=8&&e.which!=0&&e.which!=32&&e.which!=46) { $("#errmsg2").html("Alphabets Only").show().fadeOut("slow"); return false; }
     });
     $('#locality').keypress(function (e) {

         if ((e.which < 97 || e.which > 122) && (e.which < 65 || e.which > 90) && e.which != 8 && e.which != 0 && e.which != 32 && e.which != 46) { $("#errmsg5").html("Alphabets Only").show().fadeOut("slow"); return false; }
     });
     $('#city').keypress(function (e) {

         if (e.which != 8 && e.which != 0 && (e.which < 97 || e.which > 122) && (e.which < 65 || e.which > 90) && e.which != 8 && e.which != 0 && e.which != 32 && e.which != 46) { $("#errmsg3").html("Alphabets Only").show().fadeOut("slow"); return false; }
     });
     $('#nextofkin').keypress(function (e) {

         if (e.which != 8 && e.which != 0 && (e.which < 97 || e.which > 122) && (e.which < 65 || e.which > 90) && e.which != 8 && e.which != 0 && e.which != 32 && e.which != 46&&e.which!=47) { $("#errmsg6").html("Alphabets Only").show().fadeOut("slow"); return false; }
     });
     $("#mobile").keypress(function (e) {
         //if the letter is not digit then display error and don't type anything
         if (e.which != 8 && e.which != 0&&e.which!=43&&e.which!=45 && (e.which < 48 || e.which > 57)) {
             //display error message
             $("#errmsg").html("Digits Only").show().fadeOut("slow");
             return false;
         }
     });
     $("#landline").keypress(function (e) {
         //if the letter is not digit then display error and don't type anything
         if (e.which != 8 && e.which != 0&&e.which!=43&&e.which!=45 && (e.which < 48 || e.which > 57)) {
             //display error message
             $("#errmsg1").html("Digits Only").show().fadeOut("slow");
             return false;
         }
     });
     $("#pin").keypress(function (e) {
         //if the letter is not digit then display error and don't type anything
         if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
             //display error message
             $("#errmsg4").html("Digits Only").show().fadeOut("slow");
             return false;
         }
     });
     $("#age").keypress(function (e) {
         //if the letter is not digit then display error and don't type anything
         if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
             //display error message
             $("#errmsg7").html("Digits Only").show().fadeOut("slow");
             return false;
         }
     });*/
           
});

function f2()
{
    b2.disabled = true;
}
  
  
function f1()
{
    var url = "@Html.Raw(HttpUtility.JavaScriptStringEncode(url1))";
    //alert(url);
    var district = "";
    var state = "";
    var country = "";
    var pos="";
    //alert("called");

    var conceptName = $('#taluk').val();
         
    // alert(conceptName);
    if (conceptName == "") {
        // alert(conceptName);
        //$('#district').attr("disabled", "disabled");
        $('#district').val("");
        //$('#state').attr("disabled", "disabled");
        $('#state').val("");
        // $('#country').attr("disabled", "disabled");
        $('#country').val("");
    }
    else {
        //alert(url+conceptName);

        $.ajax({
            url: url + conceptName,
            crossOrgin: true,	// use jsonp for cross origin request
            dataType: 'json',
            type: 'GET',
            success: function (data) {
                for (var i = 0; i < data.length; i++)
                {
                    if (data[i] != '$') {
                        district = district + data[i];

                    }
                    else
                    {
                        pos=i;
                        break;
                    }
                }
                for (var i = pos+1; i < data.length; i++)
                {
                    if (data[i] != '$') {
                        state = state + data[i];
                    }
                    else {
                        pos = i;
                        break;
                    }
                }
                for (var i = pos + 1; i < data.length; i++)
                {
                    country = country + data[i];
                }
                // alert('called');
                //alert(state);
                //  $('#district').removeAttr("disabled");
                // $('#state').removeAttr("disabled");
                //$('#country').removeAttr("disabled");
                $("#district").prop('readonly', false);
                $("#state").prop('readonly', false);
                $("#country").prop('readonly', false);
                $('#district').val(district.trim());
                $('#state').val(state.trim());
                $('#country').val(country.trim());
                $("#district").prop('readonly', true);
                $("#state").prop('readonly', true);
                $("#country").prop('readonly', true);
                console.log(data);
            },
            error: function ()
            {
                alert('not called');
            }
        });
        // $('#city').removeAttr("disabled");
    }
}
        
$(function () {

    $('#DOB').datepicker({
        format: 'mm/dd/yyyy',
        changeMonth: true, 
        changeYear: true, 
        yearRange: '1900:+0',
        maxDate: new Date,
        onSelect: function (selected, evnt) {
            //alert(selected);
            var v = $('#DOB').val();
            var today = new Date();
            var dd = Number(today.getDate());
            var mm = Number(today.getMonth() + 1);

            var yyyy = Number(today.getFullYear());


            var myBDM = Number(v.split("/")[0])
            var myBDD = Number(v.split("/")[1])
            var myBDY = Number(v.split("/")[2])
            var age = yyyy - myBDY;

            if (mm < myBDM) {
                age = age - 1;
            }
            else if (mm == myBDM && dd < myBDD) {
                age = age - 1;
            }

            $('#age').val(age);
        }
    }).on('changeDate', function(ev){       
        var v = $('#DOB').val();
        var today = new Date();
        var dd = Number(today.getDate());
        var mm = Number(today.getMonth() + 1);

        var yyyy = Number(today.getFullYear());


        var myBDM = Number(v.split("/")[0])
        var myBDD = Number(v.split("/")[1])
        var myBDY = Number(v.split("/")[2])
        var age = yyyy - myBDY;

        if (mm < myBDM) {
            age = age - 1;
        }
        else if (mm == myBDM && dd < myBDD) {
            age = age - 1;
        }

        $('#age').val(age);
    });

});
        
       

       
$("#target").keyup(function () {
    //Fetching the textbox value.
    var query = $(this).val();
    //Calling GetItems method.
    getItems(query);
});
$(document).ready(function () {
    $('input[type=radio]').click(function () {
        // alert(this.value);
        $("#targetUL").find("li").remove();
        $("#targetUL").remove();
    });
});
        
            
function GetDateDiff(date1, date2) {

    return Math.floor(Math.abs((date1.getTime() - date2.getTime()) / (1000 * 60 * 60 * 24)) / 365);

}
//This method appends the text oc clicked li element to textbox.
function appendTextToTextBox(e) {
    //alert(e.textContent);
    //Getting the text of selected li element.
    var textToappend = e.textContent;

    //setting the value attribute of textbox with selected li element.
    $("#target").val(textToappend);
    //Removing the ul element once selected element is set to textbox.
    $("#targetUL").remove();
}

    $(function () {

        /*  $('#email').mailtip({
              onselected: function (mail) {
                 
              }
          });
      });*/
        function f2() {

            $('#nextofkin').attr('placeholder', 'AAAA');
        }
        function f3() {
            $('#nextofkin').attr('placeholder', 'AAAA');
        }
        /*   $().ready(function () {
               // validate signup form on keyup and submit
               $("#commentForm").validate({
                   rules: {
                       name:
                       {
                           required:true
                       },
                       
                       mobile:
                       {
                           minlength: 10
                       }, messages: {
                           name:"This field is required",
                           email: "Please enter a valid email address",
                           mobile: "Please enter 10 numbers"
                       }
                   }
               });
           });*/
        function f5() {
            var url = '@Url.Action("Index1", "Home")';
            window.location.href = url;
        }
        function keyboarddisplay() {

            var txt;
            txt = document.getElementById('combobox').value;
            if (val != 'B') {
                txt = txt + '' + val;
            }
            else {
                txt = txt.substr(0, (txt.length) - 1);
            }
            document.getElementById('combobox').value = txt;


        }
    });