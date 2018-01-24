
function GetBarcode(barcode,file)
    {
      var datas = new FormData();
    var files = $(file).get(0).files;
    if (files.length > 0) {
        datas.append("HelpSectionImages", files[0]);
    }
      var file_data = $(file).prop("files")[0];   // Getting the properties of file from file field
	    var form_data = new FormData();                  // Creating object of FormData class
	    form_data.append("file", file_data)
        var data = function(){
            var tmp = null;
            $.ajax({
            type: "POST",
            data: form_data,
            async: false,
            url:  'http://localhost:5000/Products/GetBarcode',
            cache: false,
            contentType: false,
            processData: false,
            success: function (result)
            {
            
                tmp = result;
            }
            });
            return tmp;
        }();

        $(barcode).val(data);  



    }

    function ClickButton(file,barcode)
    {
        $(file).click();
        $(document).ready(function(){
            $(file).on('change',function(){
                GetBarcode(barcode,file);
            });
        });

    }