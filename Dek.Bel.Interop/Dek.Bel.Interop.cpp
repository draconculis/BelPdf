// Dek.Bel.Interop.cpp : Defines the exported functions for the DLL application.

#include "stdafx.h"

#pragma once
#include <windows.h>
#include "Dek.Bel.Interop.h"

// And the exported function is defined as in the following.
// Note how the.NET delegate gets invoked through the function pointer.

DEKBELINTEROP_API ResultData* doBel(EventData data)
{
    BelManagedLib::BelManagedClass^ c = gcnew BelManagedLib::BelManagedClass();

    System::IntPtr p = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(c->GetBelDelegate());

    NativeToManaged funcPointer = (NativeToManaged)p.ToPointer();

    // invoke the delegate
    return funcPointer(data);
}
