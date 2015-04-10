function pushVideo(data) {
    $.post(
        pushVideoUrl,
        { video : data },
        function (data) {
            //do nothing
        });
}

function updateVideo(data) {
    console.log(data);
    //show new image
    var image = document.getElementById('remoteVideo');
    image.src = data;
}


function updateAudio(id) {
    //play new audio
    $("#audio").attr("src", updateAudioUrl + '/'+id);
    $("#audio")[0].play();
}