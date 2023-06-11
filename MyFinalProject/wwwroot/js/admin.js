$(function () {

    $("#tabs").tabs();
    $("#cars-list").accordion();

    $("#add-car").click(function () {
        $("#add-car-form").toggle("blind", 500);
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

    $("#save-car-btn").submit(function (e) {
        e.preventDefault();
        var formdata = new FormData(this);
        // append FilePond files into the form data
        var pondFiles = pond.getFiles();
        for (var i = 0; i < pondFiles.length; i++) {
            // append the blob file
            formdata.append('photos', pondFiles[i].file);
        }

        $.ajax({
            url: "/Admin/ChangeCar",
            data: formdata,

            processData: false,
            contentType: false,
            method: "post"

        });

    });

});