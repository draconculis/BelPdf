#include "pch.h"
#include <iostream>
#include "Dek.Bel.Interop.h"
#include <TCHAR.H>


int __cdecl main()
{
    std::cout << "Marshal!\n"; 

    EventData data;

    data.Code = 50;

    std::cout << "data!\n";

    //data.Message = L"Hello from native code!";
    data.Text = (TCHAR*)L"WALLYHO";
    data.Code = 12;
    data.FilePath = (TCHAR*)L"filepath";
    std::cout << "data!\n";

    ResultData* res = doBel(data);

    std::cout << (TCHAR&)(res->Message) << "\n";
    std::cout << res->Code << "\n";
    std::cout << res->Cancel << "\n";


    return 0;
}
