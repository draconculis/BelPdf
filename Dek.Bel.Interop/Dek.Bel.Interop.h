#pragma once
#include <windows.h>
/*
    See: http://tigerang.blogspot.com/2008/09/reverse-pinvoke.html
*/


// data structure for the callback function
struct EventData
{
    int Code;
    TCHAR* Text;
    TCHAR* FilePath;
};

struct ResultData
{
    int Code;
    bool Cancel;
    TCHAR* Message;
};

// callback function prototype
typedef ResultData(*NativeToManaged)(EventData data);

// And the exported function is defined as in the following.
// Note how the.NET delegate gets invoked through the function pointer.

#define INTEROPBRIDGE_API __declspec(dllexport)

INTEROPBRIDGE_API ResultData belInteropBridge(EventData data);
