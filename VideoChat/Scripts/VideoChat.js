var frames = [];
var currentAudio;
var image = document.getElementById('remoteVideo');
var pendingFrames = [];

function pushFrame() {
    context.drawImage(video, 0, 0, 120, 90);
    var data = canvas.toDataURL();
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
        sendAudioAndVideo();
    });
    //reset the audio and start recording again
    audioRecorder.clear();
    audioRecorder.record();
}

//var flag = false;
//Send all the data
function sendAudioAndVideo() {
    //if (!flag)
    //    return;
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
    //flag = false;
}

function updateVideoFragment(id, remoteFrames, user) {
    //console.log(remoteFrames);
    //update frames
    for (var x in remoteFrames) {
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
    //remove framw 0
    if (pendingFrames.length > 0) {
        pendingFrames.shift();
        //show new image    
        image.src = pendingFrames[0];
    }
    setTimeout(function () {
        updateVideo();
    },200);
    
}

