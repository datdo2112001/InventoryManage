function loadNote(code) {
    $.ajax({
        url: '/App/GetNote',
        type: "GET",
        data: { productCode: code },
        dataType: "json",
        success: function (data) {
            $('.modal-body').html(data);
            $('#myModal1').modal();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("err");
        },
    });

}

function loadImage(code) {
    $.ajax({
        url: '/App/GetImage',
        type: "GET",
        data: { productCode: code },
        dataType: "json",
        success: function (data) {
            $('.modal-body').html('<img src="' + data + '" alt="Image" style="max-height:400px;max-width:400px;overflow:hidden;"/>');
            $('#myModal2').modal();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("err");
        },
    });

}