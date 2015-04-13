# HR clocker code dump from IMaR

### About

There was 3 main parts to this project:

* Clockite integration
* NFC reader
* API demo ( this is currently used by the NFC reader to post checking to the db


### About this code repo

This is just a dump of the code and applications.

**NOTE - as the API demo is deployed via git this was added to a separate branch *``api_server``*, so this can be deployed by  pushing this branch. ** 

**NOTE - due to auth issue discussed during the project, the clockwrite clockin's and those from the API are store to different table within the DB ** 



### Clockite integration

This consists of 2 part:
* client code 
* server code 

##### Client code
This runs as a service on the client PC, it watches for file to be transfered from the clockwrite device and posts these to the cloud server
Documention of this is conatined in the *Installation Guide*

##### Server code ####
This process runs on the cloud server, it waits for file (blobs) added as described by the previous step, processes these and inputs them into the server.

  
##### NFC reader

TO be uploaded


### API demo

This is a server endpoint which device can call in order to post checkins to the DB

The server is located at:

``hr-clocker.azurewebsites.net``

It has only one route:

``http://hr-clocker.azurewebsites.net/api/checkin``

Devices can make **``POST``** requests to this with checkin info in the payload this will then be added to the DB.

##### Auth
The endpoint requires authentication, this is provided via *HTTP Basic Auth* using the device **reference** and **hash**,  these are current taken from the ``devices`` table, **hash** is an MD5 of the the device secret.  

#### format 

The endpoint expects the following form data:

``payload : {"cardID" => "1244343"} ``  
this is a a json object, at the moment this only contains the the id of the card scanned  

``type :  1``  
This is for the device type, or now it does not matter but could be used to between different device types eg NFC reader, phone etc.

** The only device in the devices table at present is ``reader1:toor`` **

##### Sample CURL request:  

``curl -X POST -H "Authorization: Basic cmVhZGVyMTp0b29y" -H "Cache-Control: no-cache" -H "Postman-Token: e32a5360-51bf-7102-3159-a9550e191354" -H "Content-Type: multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW" -F "payload={"cardID" => "007"}" -F "type=1" http://hr-clocker.azurewebsites.net/api/checkin``












