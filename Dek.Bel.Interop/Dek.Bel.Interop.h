/***********************************************************************
    See: http://tigerang.blogspot.com/2008/09/reverse-pinvoke.html
 ***********************************************************************/
#pragma once
#include <windows.h>

#ifdef DEKBELINTEROP_EXPORTS
#define DEKBELINTEROP_API __declspec(dllexport)   
#else  
#define DEKBELINTEROP_API __declspec(dllimport)   
#endif  

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
typedef ResultData* (*NativeToManaged)(EventData data);

// And the exported function is defined as in the following.
// Note how the.NET delegate gets invoked through the function pointer.

//#define DEKBELINTEROP_API __declspec(dllexport)

DEKBELINTEROP_API ResultData* doBel(EventData data);
