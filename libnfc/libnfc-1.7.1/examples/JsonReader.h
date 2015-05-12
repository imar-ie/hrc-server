
#include <string.h>
#include <stdio.h>
#include <stdlib.h>
#include <stddef.h>

#include "./json/json.h"


/**
** Struct to store information about devices
**
**/
typedef  struct Devices {
	char* username;	// Device username to connect to API
	char* password;		// Device password ti connect to API
}Device ;


/**
** Struct to store information about the server response
**/
typedef  struct ServerResponse {
	char* msg;			// Message
	char* error;		// Error : true or false
	char* status;		// 200 OK or other
}ServerResponse ;


/**
** Function use to read config from the config file (config.xml)
**/
Device getConfig();

/**
** Function use to read response from the server
**/
ServerResponse getResponse();
