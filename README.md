# VideoChat

<a href="http://pedro-ramirez-suarez.github.io/VideoChat">Home page</a>

<p>A video chat with HTML5 + javascript + Server Sent Events</p>

<p>Created with Asp.Net MVC 5 and <a href="https://github.com/pedro-ramirez-suarez/needletailtools/wiki/Using-Needletail.Mvc" target="_blank">Needletail.MVC</a> to facilitate SSE</p>

The application supports only two users, but with minor changes it can support many more.

Using the following open source projects:
- Bootstap
- JQuery
- Recorderjs
- libmp3lame-js
 
I modified the Recorderjs library to record mono audio instead of stereo and also to automatically convert the wav file to a mp3 file using the libmp3lame-js.

<b>Important </b> SSE is not supported on Internet Explorer(any version), also, sometimes  SSE does not work fine on IIS Express.

This is just a proof of concept, there are better technologies to create a video chat like WebRTC, but WebRTC works with web sockets, the idea behind this proyect was to test if SSE can be used for that kind of applications, the main limitations of using SSE for this kind of application are:
- SSE is not peer to peer, so it's slower.
- The ammount of data received and sent is bigger with SSE, to upload data to the server, you need to send it as a file, or as a string, if you send strings you need to be sure that those are Base64(which increase the size) otherwise the server may reject the request, also, when sending data to the client, you can only serve files or strings, as incomming connections, you need to convert those strings to Base64 or the server may not send the info that you want.
- Trial and error is the way you sicronize the image and audio.



