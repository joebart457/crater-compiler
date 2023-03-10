

#define RUNTIME_FN int
#define RUNTIME_SUCCESS 0
#define RUNTIME_FAILURE -1


#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include <stdint.h>
#include <stdbool.h>


typedef struct {
	bool had_error;
	char* error_message;
} error_result;


RUNTIME_FN println(void* _unused, error_result* err, char* message) {
	printf(message);
	return RUNTIME_SUCCESS;
}