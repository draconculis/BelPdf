#include "pch.h"
#include <iostream>
#include "Dek.Bel.Interop.h"
#include <TCHAR.H>

#define DEKBELCODE_ADDVOLUMETITLE          9100
#define DEKBELCODE_ADDBOOKTITLE            9110
#define DEKBELCODE_ADDCHAPTER              9120
#define DEKBELCODE_ADDCITATION             9200
#define DEKBELCODE_ADDANDSHOWCITATION      9300
#define DEKBELCODE_STARTAUTOPAGINATION     9400


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
    //data.SelectionRectsString = (TCHAR*)L"1,2,3,4;";
    data.SelectionRects = 0;
    data.Len = 1;

    data.Text = (TCHAR*)L"WALLYHO";
    data.Code = DEKBELCODE_ADDCITATION;
    data.FilePath = (TCHAR*)L"c:\\28";
    
    std::cout << "doBel!\n";

    ResultData* res = doBel(data);

    std::cout << "res: " << res << "\n";

    std::cout << (TCHAR&)(res->Message) << "\n";
    std::cout << res->Code << "\n";
    std::cout << res->Cancel << "\n";


    return 0;
}
