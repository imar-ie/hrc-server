RASPBERRY PI + NFC READER PROJECT
__________________________________


__________________________________
INSTALL
----------------------------------

To install the source code of this project on your Raspberry Pi, you need to : 
	- clone the repo
	- Go to libnfc/libnfc 1.7.1 and, in a terminal, run :	
		? ./configure
		? make
		? make install
		(? make tests)

	- Go to libnfc/libnfc 1.7.1/examples/curl and, in a terminal, run :	
		? ./configure
		? make
		? make install
		(? make tests)

	- Go to libnfc/libnfc 1.7.1/examples/json and, in a terminal, run :	
		? ./configure
		? make
		? make install
		(? make tests)

	- Compile "nfc-poll.c" with the following command line : 
		gcc nfc-reader.c ../utils/nfc-utils.c POSTRequest.c JsonReader.c -ljson -lcurl -lnfc -o nfc_reader

	- Execute with : 
		./nfc_reader

__________________________________
DEBUG
----------------------------------

By default, debug mode is not enabled but if you need to debug the program you can just uncomment the line in "nfc-poll.C" where DEBUG constant is defined.
Then you will need to recompile the file with this line : 
		gcc nfc-reader.c ../utils/nfc-utils.c POSTRequest.c JsonReader.c -ljson -lcurl -lnfc -o nfc_reader
You can now execute it and it will show you some information (card ID, request structure, server response, ...) which can help you to debug your program.

__________________________________
USAGE
----------------------------------

The two LEDs are here to give you a feedback on the result of your scan. 

• When The LEDs are red then green : Everything works.
• When the red LED is blinking : NFC Device cannot be found.
• When the red and green LEDs are blinking : There is no network access, the server may be down or the request structure cannot be understood by the server.