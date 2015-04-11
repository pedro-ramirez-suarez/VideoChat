function pushVideo(data) {
    context.drawImage(video, 0, 0, 160, 120);
    $.post(
        pushVideoUrl,
        { video : data },
        function (data) {
            //do nothing
        });
}

function pushAudio() {
    if (audioRecorder == undefined || audioRecorder == null)
        return;
    //stop the audio
    audioRecorder.stop();

    //if there is much difference between audio and video, you can send both togehter here in the callback
    //Export the audio as a wav file
    audioRecorder.exportWAV(function (audio) {
        var formData = new FormData();
        formData.append('edition[audio]', audio)
        //Send the audio to the server as a file attachment
        $.ajax({
            type: 'POST',
            url: pushAudioUrl,
            data: formData,
            contentType: false,
            cache: false,
            processData: false,
        });

    });
    //reset the audio and start recording again
    audioRecorder.clear();
    audioRecorder.record();
}

//new video comming from the server
function updateVideo(data,user) {
    //show new image
    var image = document.getElementById('remoteVideo');
    image.src = data;
    //set the user who send the video
    $('#remoteUser').html(user);
}

//new audio comming from the server
function updateAudio(id) {
    //we are not allowed to send the audio file directly, so we just set the src of the stream
    //play new audio
    $("#audio").attr("src", updateAudioUrl + '/'+id);
    $("#audio")[0].play();
}