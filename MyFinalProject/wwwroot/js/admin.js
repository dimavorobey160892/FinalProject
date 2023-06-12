$(function () {

    $("#tabs").tabs();
    $("#cars-list").accordion({ collapsible: true, active: false });

    $("#add-car").click(function () {
        $("#add-car-div").toggle("blind", 500);
    });

    FilePond.registerPlugin(
        FilePondPluginImagePreview,
        FilePondPluginImageExifOrientation,
        FilePondPluginFileValidateSize
    );
    const fieldsetElement = $(".filepond").get(0);
    const pond = FilePond.create(fieldsetElement);


    //$("#save-car-btn").click(function () {
    //    if ($("#add-car-form")[0].checkValidity()) {
    //        var files = pond.getFiles();
    //    }

    //})

    //var files = pond.getFiles();

    $("#add-car-form").submit(function (e) {
        e.preventDefault();
        var formdata = new FormData(this);
        var pondFiles = pond.getFiles();
        for (var i = 0; i < pondFiles.length; i++) {
            formdata.append('Images', pondFiles[i].file);
        }

        $.ajax({
            url: "/Admin/ChangeCar",
            data: formdata,

            processData: false,
            contentType: false,
            method: "post"

        })
        .done(function (data) {
            $("#add-car-form")[0].reset();
            pond.removeFiles();
            $("#add-car-div").toggle("blind", 500);
            //$("#cars-list").append(
        })
        .fail(function (data) {
            $("body").html(
                '<div class="alert alert-danger">Could not reach server, please try again later.</div>'
            );
        });

    });

});