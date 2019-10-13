/* Copyright 2018 the SumatraPDF project authors (see AUTHORS file).
   License: GPLv3 */

//#include "BaseUtil.h"
//#include "ScopedWin.h"
//#include "CmdLineParser.h"
//#include "FileUtil.h"
//#include "HtmlParserLookup.h"
//#include "Mui.h"
//#include "WinUtil.h"
#include "../../Dek.Bel.Interop/Dek.Bel.Interop.h"

//#include "BaseEngine.h"
//#include "EngineManager.h"
//
//#include "SettingsStructs.h"
//#include "Controller.h"
//#include "DisplayModel.h"
//#include "FileHistory.h"
//#include "Theme.h"
//#include "GlobalPrefs.h"
//#include "Colors.h"
//
//#include "TextSelection.h"
//
//#include "SumatraPDF.h"
//#include "WindowInfo.h"
//#include "TabInfo.h"
//#include "resource.h"
////#include "ExternalViewers.h"
//#include "Favorites.h"
//#include "FileThumbnails.h"
//#include "Menu.h"
//#include "Selection.h"
//#include "SumatraAbout.h"
//#include "SumatraDialogs.h"
//#include "Translations.h"
//#include "BitManip.h"
//#include "Dpi.h"

// Codez
#define DEKBELCODE_SHOWBEL                 9000
#define DEKBELCODE_ADDVOLUMETITLE          9100
#define DEKBELCODE_ADDBOOKTITLE            9110
#define DEKBELCODE_ADDCHAPTER              9120
#define DEKBELCODE_ADDSUBCHAPTER           9130
#define DEKBELCODE_ADDPARAGRAPH            9140
#define DEKBELCODE_ADDRAWCITATION          9200
#define DEKBELCODE_ADDANDSHOWCITATION      9300
#define DEKBELCODE_ADDCITATIONSILENT       9301
#define DEKBELCODE_EDITCITATION            9310
#define DEKBELCODE_STARTAUTOPAGINATION     9400

void BelHandleResult(ResultData* result);
void DoBel(WindowInfo* winIn, int code);
void BelHandleRequestFileStorageResult(ResultFileStorageData* result);
//ResultFileStorageData* BelRequestFileStorage(WindowInfo* winIn);
ResultFileStorageData* BelRequestFileStorage(LoadArgs& args);
void BelEditCitation(const WCHAR* path);
void BelHandleEditResult(ResultData* result);
