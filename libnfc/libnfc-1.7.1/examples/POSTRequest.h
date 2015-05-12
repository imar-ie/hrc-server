#include <curl/curl.h>
#include <curl/easy.h>
#include <string.h>

/**
** Fonction used yo do the web request, it returns the response of the server
** url  is the url of the server and args is a string composed of each args like : "arg1=val1&arg2=val2"
**/
char *do_web_request(char *url, char* args, char* username_pwd);
