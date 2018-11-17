// Dek.Bel.Interop.cpp : Defines the exported functions for the DLL application.

#include "stdafx.h"

#pragma once
#include <windows.h>
#include "Dek.Bel.Interop.h"

// And the exported function is defined as in the following.
// Note how the.NET delegate gets invoked through the function pointer.

INTEROPBRIDGE_API ResultData belInteropBridge(EventData data)
{
    DekManagedLib::ManagedClass^ c = gcnew DekManagedLib::ManagedClass();

    System::IntPtr p = System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(c->GetDelegate());

    NativeToManaged funcPointer = (NativeToManaged)p.ToPointer();

    // invoke the delegate
    return funcPointer(data);
}
