﻿var frames = [];
var currentAudio;
var image = document.getElementById('remoteVideo');
var pendingFrames = [];

function pushFrame() {
    context.drawImage(video, 0, 0, 120, 90);
    var data = canvas.toDataURL('image/png');
    //store the frame
    frames.push(data);
    setTimeout(function () {
        pushFrame();
    }, 200);
}

function prepareStream() {
    if (audioRecorder == undefined || audioRecorder == null)
        return;
    //stop the audio
    audioRecorder.stop();

    //Export the audio as a wav file
    audioRecorder.exportWAV(function (audio) {
        currentAudio = audio;
        //convert to url data
        var reader = new FileReader();
        reader.onload = function (event) {
            sendAudioAndVideo();
        };
        reader.readAsDataURL(audio);
    });
    //reset the audio and start recording again
    audioRecorder.clear();
    audioRecorder.record();
}

//Send frames and audio to the server
function sendAudioAndVideo() {
    var formData = new FormData();
    //set the frames
    for (var x in frames) {
        //only process first 10 frames, this is to sync video and audio
        if (x > 9)
            break;
        //append a new frame
        formData.append('frame' + x, frames[x]);
    }
    //clear the frames
    frames = [];
    //set the audio
    formData.append('edition[audio]', currentAudio);
    //Send the audio to the server as a file attachment
    $.ajax({
        type: 'POST',
        url: pushVideoFragmentUrl,
        data: formData,
        contentType: false,
        cache: false,
        processData: false,
    });
}

//this method is "invoked" from the HomeController in the server, it receives all the frames and some ID to get the audio
function updateVideoFragment(id, remoteFrames, user) {
    //update frames
    for (var x in remoteFrames) {
        if (remoteFrames[x].length == 0)
            continue;
        pendingFrames.push(remoteFrames[x]);
    }

    //set the user who send the video
    $('#remoteUser').html(user);
    //we are not allowed to send the audio file directly, so we just set the src of the stream
    //play new audio
    $("#audio").attr("src", updateAudioUrl + '/' + id);
    $("#audio")[0].play();
}

//process always frame 0
function updateVideo() {
    //remove frame 0
    if (pendingFrames.length > 0) 
        pendingFrames.shift();
    //show new image    
    if (pendingFrames[0] != undefined && pendingFrames[0]!= null)
        image.src = pendingFrames[0];
    setTimeout(function () {
        updateVideo();
    },200);
    
}

