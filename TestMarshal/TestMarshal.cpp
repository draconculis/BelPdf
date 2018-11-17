// TestMarshal.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include "pch.h"
#include <iostream>
#include "Dek.Bel.Interop.h"
#include <TCHAR.H>

int main()
{
    std::cout << "Marshal!\n"; 

    EventData data;

    data.Code = 50;

    std::cout << "data!\n";

    //data.Message = L"Hello from native code!";
    data.Text = (TCHAR*)L"WALLYHO";

    std::cout << "data!\n";

    belInteropBridge(data);
    
    return 0;
}
