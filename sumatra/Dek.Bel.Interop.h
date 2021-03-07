#pragma once

#ifndef DEKBELINTEROP_H
#define DEKBELINTEROP_H

/***********************************************************************
    See: http://tigerang.blogspot.com/2008/09/reverse-pinvoke.html
 ***********************************************************************/
#include <windows.h>

#ifdef DEKBELINTEROP_EXPORTS
#define DEKBELINTEROP_API __declspec(dllexport)   
#else  
#define DEKBELINTEROP_API __declspec(dllimport)   
#endif  

// data structure for the callback functions
struct EventData {
    int Code;
    int CurrentPage;
    int StartPage;
    int StopPage;
    int StartGlyph;
    int StopGlyph;
    TCHAR* Text;
    TCHAR* FilePath;
    int* SelectionRects;
    int Len;
};

struct ResultData {
    int Code;
    bool Cancel;
	TCHAR* Message;
};

struct RequestFileStorageData {
    TCHAR* FilePath;
};

struct ResultFileStorageData {
    bool Cancel;
    TCHAR* StorageFilePath;
};

// callback function prototype
typedef ResultData* (*NativeToManaged)(EventData data);
typedef ResultFileStorageData* (*NativeToManagedF)(RequestFileStorageData data);

// And the exported function is defined as in the following.
// Note how the.NET delegate gets invoked through the function pointer.

//#define DEKBELINTEROP_API __declspec(dllexport)

DEKBELINTEROP_API ResultData* doBel(EventData data);
DEKBELINTEROP_API ResultFileStorageData* doBelRequestFileStorage(RequestFileStorageData data);

#endif