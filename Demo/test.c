#include "crater_runtime.h"
typedef struct {
	bool had_error;
	char* error_message;
	int32_t extra;
} my_error;
RUNTIME_FN printntimes(int32_t* _result_printntimes_0, my_error* _error_result_printntimes_0, int32_t n)
{
	int32_t rc = -1;
bool condition = true;
while (condition)
	{
		int32_t _Subtraction_result_0 = n - 1;
		n = _Subtraction_result_0;
		bool _GreaterThanEqual_result_1 = _Subtraction_result_0 >= 0;
		condition = _GreaterThanEqual_result_1;
		error_result _error_result_println_1;
		int32_t _result_println_1;
		rc = println(&_result_println_1, &_error_result_println_1,  "test");
		if (rc != RUNTIME_SUCCESS) return rc;
		
	}
	
*_result_printntimes_0 = 1;
	return RUNTIME_SUCCESS;
	
	return -1;
}

int main(int argc, char** argv) {


	printf(strcat("hello ", "world!"));
	int32_t res;
	error_result err;
	printntimes(&res, &err, 42);
	return 0;
}