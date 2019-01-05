#include "pch.h"
#include <iostream>
#include "Dek.Bel.Interop.h"
#include <TCHAR.H>

#define DEKBELCODE_ADDVOLUMETITLE          100
#define DEKBELCODE_ADDBOOKTITLE            110
#define DEKBELCODE_ADDCHAPTER              120
#define DEKBELCODE_ADDCITATION             200
#define DEKBELCODE_ADDANDSHOWCITATION      300
#define DEKBELCODE_STARTAUTOPAGINATION     400


int __cdecl main()
{
    std::cout << "Marshal!\n"; 
    int dummy;
    std::cin >> dummy;

    EventData data;

    data.Code = 50;
    data.StartPage = 1;
    data.StopPage = 2;
    data.StartGlyph = 3;
    data.StopGlyph = 4;
    data.SelectionRectsString = (TCHAR*)L"1,2,3,4;";
    data.Len = 1;

    data.Text = (TCHAR*)L"WALLYHO";
    data.Code = DEKBELCODE_ADDANDSHOWCITATION;
    data.FilePath = (TCHAR*)L"c:\\28";
    
    std::cout << "doBel!\n";

    ResultData* res = doBel(data);

    std::cout << "res: " << res << "\n";

    std::cout << (TCHAR&)(res->Message) << "\n";
    std::cout << res->Code << "\n";
    std::cout << res->Cancel << "\n";


    return 0;
}
