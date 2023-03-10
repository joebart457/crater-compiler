

#define RUNTIME_FN int
#define RUNTIME_SUCCESS 0
#define RUNTIME_FAILURE -1


#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include <stdint.h>
#include <stdbool.h>

typedef struct {
	char* data;
	int32_t length;
} string;

string construct_string(char* src) {
	string self;

	if (src == NULL) {
		self.data = NULL;
		self.length = 0;
	}
	else {
		size_t size = sizeof(char) * (strlen(src) + 1);
		self.data = (char*)malloc(size);
		if (self.data == NULL) {
			self.length = 0;
		}
		else {
			strcpy(self.data, src);
			self.length = strlen(src);
		}
	}
	return self;
}


typedef struct {
	bool had_error;
	string error_message;
} error_result;


RUNTIME_FN println(void* _unused, error_result* err, string message) {
	printf(message.data);
	return RUNTIME_SUCCESS;
}