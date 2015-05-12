/*-
 * Free/Libre Near Field Communication (NFC) library
 *
 * Libnfc historical contributors:
 * Copyright (C) 2009      Roel Verdult
 * Copyright (C) 2009-2013 Romuald Conty
 * Copyright (C) 2010-2012 Romain Tartière
 * Copyright (C) 2010-2013 Philippe Teuwen
 * Copyright (C) 2012-2013 Ludovic Rousseau
 * See AUTHORS file for a more comprehensive list of contributors.
 * Additional contributors of this file:
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *  1) Redistributions of source code must retain the above copyright notice,
 *  this list of conditions and the following disclaimer.
 *  2 )Redistributions in binary form must reproduce the above copyright
 *  notice, this list of conditions and the following disclaimer in the
 *  documentation and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 *
 * Note that this license only applies on the examples, NFC library itself is under LGPL
 *
 */

/**
 * @file nfc-poll.c
 * @brief Polling example
 */

#ifdef HAVE_CONFIG_H
#  include "config.h"
#endif // HAVE_CONFIG_H

#include <err.h>
#include <inttypes.h>
#include <signal.h>
#include <stdio.h>
#include <stddef.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>

#include <nfc/nfc.h>
#include <nfc/nfc-types.h>

// IMPORT WITH RELATIVE PATH NECESSARY 
#include "../utils/nfc-utils.h"
// USE TO READ FROM JSON FILES
#include "JsonReader.h"
// USE TO SEND POST REAUEST TO A SPECIFIC SERVER
#include "POSTRequest.h"

#define API_URL "http://hr-clocker.azurewebsites.net/api/checkin"
#define MAX_DEVICE_COUNT 16
#define DEBUG

static nfc_device *pnd = NULL;
static nfc_context *context;


// In case of bad usage 
static void print_usage(const char *progname){
  printf("usage: %s [-v]\n", progname);
  printf("  -v\t verbose display\n");
}



int main(int argc, const char *argv[]){


	//get the config of the NFC card reader by reading fronm "config.json"
	Device device = getConfig();
	
	// get the username and password read from the json file
	char *user_name = device.username;
	char *password = device.password;

	// Create string which follows the pattern "user:password"
	char *username_pwd = malloc((strlen(user_name)+strlen(password)+2)*sizeof(char));
    strcpy(username_pwd,user_name);
	strcat(username_pwd,":");
	strcat(username_pwd,password);
	#ifdef DEBUG
		printf("\nusn:pwd : %s\n\n",username_pwd);
	#endif
  	bool verbose = false;

	int i=0;  	

  	// Display libnfc version
 	const char *acLibnfcVersion = nfc_version();
  	printf("%s uses libnfc %s\n", argv[0], acLibnfcVersion);

	// if bad usage
  	if (argc != 1) {
		if ((argc == 2) && (0 == strcmp("-v", argv[1]))) {
	  	verbose = true;
		} else {
	  	print_usage(argv[0]);
	  	exit(EXIT_FAILURE);
		}
  	}

	// Number of polling, it's a big number in order to increase the scanning time approx 2min 
  	const int uiPollNr = 50000;

	// Period of polling
  	const uint8_t uiPeriod = 2;

	// Number of modulation
  	const nfc_modulation nmModulations[5] = {
    	{ .nmt = NMT_ISO14443A, .nbr = NBR_106 },
    	{ .nmt = NMT_ISO14443B, .nbr = NBR_106 },
    	{ .nmt = NMT_FELICA, .nbr = NBR_212 },
    	{ .nmt = NMT_FELICA, .nbr = NBR_424 },
    	{ .nmt = NMT_JEWEL, .nbr = NBR_106 },
 	};
	// Size of modulations
  	const size_t szModulations = 5;

	// NFC devices
  	nfc_target nt;

  	int res = 0;

	// init context
  	nfc_init(&context);
  	if (context == NULL) {
    	ERR("Unable to init libnfc (malloc)");
    	exit(EXIT_FAILURE);
  	}

	// open NFC device
  	pnd = nfc_open(context, NULL);
  	if (pnd == NULL) {
    		ERR("%s", "Unable to open NFC device.");

		for(i=0;i<15;i++){
			if(i%2 == 0){
				system("/usr/local/bin/gpio -g write 18 1");
			}else{
				system("/usr/local/bin/gpio -g write 18 0");
			}
			usleep(500);
		}
		system("/usr/local/bin/gpio -g write 18 0");			
	    	nfc_exit(context);
    		exit(EXIT_FAILURE);
  	}

	// init the device
  	if (nfc_initiator_init(pnd) < 0) {
		nfc_perror(pnd, "nfc_initiator_init");
		nfc_close(pnd);
		nfc_exit(context);
		exit(EXIT_FAILURE);
  	}		

	#ifdef DEBUG
	  	printf("NFC reader: %s opened\n", nfc_device_get_name(pnd));
	  	printf("NFC device will poll during %ld s (%u pollings of %lu ms for %" PRIdPTR " modulations)\n", (unsigned long) (uiPollNr * 	szModulations * uiPeriod * 150)/1000, uiPollNr, (unsigned long) uiPeriod * 150, szModulations);
	#endif
	
	// loop which permit to scan an infinite number of cards
	while(true){
		
		// check if a card is near the sensor
		res = nfc_initiator_poll_target(pnd, nmModulations, szModulations, uiPollNr, uiPeriod, &nt);
		
	  	if (res  < 0) {
			nfc_perror(pnd, "nfc_initiator_poll_target");
			nfc_close(pnd);
			nfc_exit(context);
			exit(EXIT_FAILURE);
	  	}

		// if a card is scanned
	  	if (res > 0) {
			
			// the char which will contain the ID of the current card scanned
			char cardId[10] = "";

			// the char which will contain the response from the server
			char *response = "" ;

			printf("\n\nCard read\n");

			// init both LED
			system("/usr/local/bin/gpio -g mode 18 out");
			system("/usr/local/bin/gpio -g mode 23 out");

			// Turn on the red LED
			system("/usr/local/bin/gpio -g write 18 1");

			#ifdef DEBUG
				printf("\n\n");
				printf("NAI abtUID : ");
			#endif

			// Put the card Id into a variable as a string
			for(i=0;i<10;i++){
				#ifdef DEBUG	
					printf("%X  ",nt.nti.nai.abtUid[i]);
				#endif
				sprintf(cardId,"%s%X",cardId,nt.nti.nai.abtUid[i]);	 
			}

			// standart output of nfc-poll
			//print_nfc_target(&nt, verbose);
			
			// Building string which contains POST Request args
			char *args = malloc((strlen("payload={\"cardID\" => \"")+strlen(cardId)+strlen("\"}")+strlen("&type=1")+1)*sizeof(char));
			strcpy(args,"payload={\"cardID\" => \"");
			strcat(args,cardId); 
			strcat(args,"\"}");
			strcat(args,"&type=1");

			#ifdef DEBUG
				printf("URL : %s\n", API_URL);
				printf("ARGUMENTS T : %s\n\n", args);
			#endif 

			// Do the web request and get/display the response
			response = do_web_request(API_URL,args,username_pwd);

			// make available the space from args variable
			free(args);

			#ifdef DEBUG
				printf("response : \n%s\n", response);
			#endif
			
			if(strlen(response) != 0){
				// parse the response of the server and get a struct
				ServerResponse server = getResponse(response);

				// if the response mentionned some errors
				if(strcmp(server.msg,"Success") != 0 || strcmp(server.error,"false") != 0 || strcmp(server.status,"200") != 0){

					// turn OFF the red LED then ON then OFF
					system("/usr/local/bin/gpio -g write 18 0");
					// wait 1 second
					sleep(1);
					system("/usr/local/bin/gpio -g write 18 1");
					sleep(1);
					system("/usr/local/bin/gpio -g write 18 0");

				}else{ // if everything is all right
					// turn on the green LED
					system("/usr/local/bin/gpio -g write 23 1");
					// turn off the red LED
					system("/usr/local/bin/gpio -g write 18 0");
					// wait 2 seconds
					sleep(2);
					// turn off the green LED					
					system("/usr/local/bin/gpio -g write 23 0");
				}	
			}else{// in case of bad authentication
				#ifdef DEBUG			
					printf("Null response from the server\n");
				#endif
				for(i=0;i<15;i++){
					if(i%2 == 0){
						system("/usr/local/bin/gpio -g write 18 0");
						system("/usr/local/bin/gpio -g write 23 1");
					}else{
						system("/usr/local/bin/gpio -g write 18 1");
						system("/usr/local/bin/gpio -g write 23 0");
					}
					usleep(500);
				}
				system("/usr/local/bin/gpio -g write 23 0");	

			}		
	  	} else {
			printf("No target found.\n");
	  	}

	  	//printf("Waiting for card removing...");
	  	//while (0 == nfc_initiator_target_is_present(pnd, NULL)) {}
	  	//nfc_perror(pnd, "nfc_initiator_target_is_present");
	  	printf("done.\n\n");
	}

  	nfc_close(pnd);
  	nfc_exit(context);
  	exit(EXIT_SUCCESS);
}
