/* Copyright 2018 the SumatraPDF project authors (see AUTHORS file).
   License: GPLv3 */

#include "utils/BaseUtil.h"
#include "utils/ScopedWin.h"
#include "utils/CmdLineParser.h"
#include "utils/FileUtil.h"
#include "utils/HtmlParserLookup.h"
#include "mui/Mui.h"
#include "utils/WinUtil.h"
#include "Dek.Bel.Interop.h"
#include <sstream>

#include "BaseEngine.h"
#include "EngineManager.h"

#include "SettingsStructs.h"
#include "Controller.h"
#include "DisplayModel.h"
#include "FileHistory.h"
#include "Theme.h"
#include "Colors.h"
#include "GlobalPrefs.h"
#include "ProgressUpdateUI.h"
#include "Notifications.h"
#include "TextSelection.h"

#include "SumatraPDF.h"
#include "WindowInfo.h"
#include "TabInfo.h"
#include "resource.h"
//#include "ExternalViewers.h"
#include "Favorites.h"
#include "FileThumbnails.h"
#include "Menu.h"
#include "Selection.h"
#include "SumatraAbout.h"
#include "SumatraDialogs.h"
#include "Translations.h"
#include "utils/BitManip.h"
#include "utils/Dpi.h"
#include "BelExt.h"

// ********************************************************************
//     Dek Bel Pdf
// ********************************************************************
// void BelHandleResult(ResultData* result);
// void DoBel(WindowInfo* winIn);
// ********************************************************************
// ********************************************************************
//     Dek Bel Pdf
// ********************************************************************

void DoBel(WindowInfo* winIn, int code) {
    WindowInfo* win = FindWindowInfoByHwnd(winIn->hwndCanvas);

    // if (!win->currentTab || !win->currentTab->selectionOnPage)
    //    return;
    if (!win->currentTab)
        return;

    // CrashIf(win->currentTab->selectionOnPage->size() == 0 && win->mouseAction != MouseAction::SelectingText);
    // if (win->currentTab->selectionOnPage->size() == 0)
    //    return;
    CrashIf(!win->AsFixed());
    if (!win->AsFixed())
        return;

    DisplayModel* dm = win->AsFixed();

    bool isTextSelection = dm->textSelection->result.len > 0;
    AutoFreeW selText;
    if (!dm->GetEngine()->IsImageCollection()) {
        if (isTextSelection) {
            selText.Set(dm->textSelection->ExtractText(L"\r\n"));
        } else {
            // Can be a selection rect, from which text may be extracted
            if (win->currentTab->selectionOnPage != NULL) {
                WStrVec selections;
                for (SelectionOnPage& sel : *win->currentTab->selectionOnPage) {
                    WCHAR* text = dm->GetTextInRegion(sel.pageNo, sel.rect);
                    if (text)
                        selections.Push(text);
                }
                selText.Set(selections.Join());
            }
        }

        // don't copy empty text
        // if (str::IsEmpty(selText.Get()))
        //    return;

        EventData data;
        int fromPage, fromGlyph, toPage, toGlyph;
        dm->textSelection->GetGlyphRange(&fromPage, &fromGlyph, &toPage, &toGlyph); // (From page, from glyph, to page, to glyph)

        data.Code = code; // Menu command, see BelExt.h
        if (fromPage > 0)
            data.StartPage = fromPage;
        else
            data.StartPage = dm->CurrentPageNo();
        data.StartGlyph = fromGlyph;
        data.StopPage = toPage;
        data.StopGlyph = toGlyph;
        if (isTextSelection) // Don't copy empty text
            data.Text = (TCHAR*)selText.Get();
        else
            data.Text = L"\0";

        data.FilePath = (TCHAR*)win->currentTab->GetOrigFilePath(); // Return original path if filePath points to BelPdf storage

        if (isTextSelection) {
            data.SelectionRects = new int[4 * (dm->textSelection->result.len)];
            data.Len = dm->textSelection->result.len;
            for (int i = 0; i < data.Len; i++) {
                data.SelectionRects[i * 4] = dm->textSelection->result.rects[i].x;
                data.SelectionRects[i * 4 + 1] = dm->textSelection->result.rects[i].y;
                data.SelectionRects[i * 4 + 2] = dm->textSelection->result.rects[i].dx;
                data.SelectionRects[i * 4 + 3] = dm->textSelection->result.rects[i].dy;
            }
        } else {
            data.SelectionRects = new int[1];
            data.SelectionRects[0] = 999;
            data.Len = 0;
        }

        // This is sometimes the case when nothing is selected
        if (data.StartPage < 0) {
            data.StartPage = win->currPageNo;
            data.StopPage = win->currPageNo;
            data.StartGlyph = 0;
            data.StopGlyph = 0;
        }

        ResultData* result = doBel(data);

        BelHandleResult(result);

        delete[] data.SelectionRects;
    }
}

void BelHandleResult(ResultData* result) {
    if (result->Cancel)
        return;


}

// ****************************************************************/
// Bel edit citation
// ****************************************************************/
void BelEditCitation(const WCHAR* path) {
    EventData data;

    data.Code = DEKBELCODE_EDITCITATION; // Menu command, see BelExt.h
    data.Text = (TCHAR*)path;
    data.SelectionRects = NULL; // Dummy value
    data.FilePath = L"\0"; // Dummy value

    ResultData* result = doBel(data);

    BelHandleEditResult(result);
}

void BelHandleEditResult(ResultData* result) {
    if (result->Cancel)
        return;
}


/*****************************************************************/
// Bel file storage
/*****************************************************************/

// ResultFileStorageData* BelRequestFileStorage(WindowInfo* winIn) {
ResultFileStorageData* BelRequestFileStorage(LoadArgs& args) {
    // WindowInfo* win = FindWindowInfoByHwnd(winIn->hwndCanvas);

    RequestFileStorageData data;
    // data.FilePath = (TCHAR*)win->currentTab->filePath;
    data.FilePath = (TCHAR*)args.fileName;

    ResultFileStorageData* result = doBelRequestFileStorage(data);

    BelHandleRequestFileStorageResult(result);
    return result;
}

void BelHandleRequestFileStorageResult(ResultFileStorageData* result) {
    if (result->Cancel)
        return;
}

// static void CopyFileToStorage(WindowInfo& win, ResultData* result) {
//    if (!HasPermission(Perm_DiskAccess))
//        return;
//    if (!win.IsDocLoaded())
//        return;
//
//    const WCHAR* srcFileName = win.ctrl->FilePath();
//    AssertCrash(srcFileName);
//    if (!srcFileName)
//        return;
//
//
//
//    //bool ok = CopyFile(srcFileName, srcFileName, FALSE);
//}

// static void OnMenuSaveAs(WindowInfo& win) {
//    if (!HasPermission(Perm_DiskAccess))
//        return;
//    if (!win.IsDocLoaded())
//        return;
//
//    const WCHAR* srcFileName = win.ctrl->FilePath();
//    AutoFreeW urlName;
//    if (gPluginMode) {
//        urlName.Set(url::GetFileName(gPluginURL));
//        // fall back to a generic "filename" instead of the more confusing temporary filename
//        srcFileName = urlName ? urlName : L"filename";
//    }
//
//    AssertCrash(srcFileName);
//    if (!srcFileName)
//        return;
//
//    BaseEngine* engine = win.AsFixed() ? win.AsFixed()->GetEngine() : nullptr;
//    bool canConvertToTXT = engine && !engine->IsImageCollection() && win.currentTab->GetEngineType() !=
//    EngineType::Txt; bool canConvertToPDF = engine && win.currentTab->GetEngineType() != EngineType::PDF;
//#ifndef DEBUG
//    // not ready for document types other than PS and image collections
//    if (canConvertToPDF && win.currentTab->GetEngineType() != EngineType::PostScript && !engine->IsImageCollection())
//        canConvertToPDF = false;
//#endif
//#ifndef DISABLE_DOCUMENT_RESTRICTIONS
//    // Can't save a document's content as plain text if text copying isn't allowed
//    if (engine && !engine->AllowsCopyingText())
//        canConvertToTXT = false;
//    // don't allow converting to PDF when printing isn't allowed
//    if (engine && !engine->AllowsPrinting())
//        canConvertToPDF = false;
//#endif
//    CrashIf(canConvertToTXT &&
//            (!engine || engine->IsImageCollection() || EngineType::Txt == win.currentTab->GetEngineType()));
//    CrashIf(canConvertToPDF && (!engine || EngineType::PDF == win.currentTab->GetEngineType()));
//
//    const WCHAR* defExt = win.ctrl->DefaultFileExt();
//    // Prepare the file filters (use \1 instead of \0 so that the
//    // double-zero terminated string isn't cut by the string handling
//    // methods too early on)
//    str::Str<WCHAR> fileFilter(256);
//    if (AppendFileFilterForDoc(win.ctrl, fileFilter))
//        fileFilter.AppendFmt(L"\1*%s\1", defExt);
//    if (canConvertToTXT) {
//        fileFilter.Append(_TR("Text documents"));
//        fileFilter.Append(L"\1*.txt\1");
//    }
//    if (canConvertToPDF) {
//        fileFilter.Append(_TR("PDF documents"));
//        fileFilter.Append(L"\1*.pdf\1");
//    }
//    fileFilter.Append(_TR("All files"));
//    fileFilter.Append(L"\1*.*\1");
//    str::TransChars(fileFilter.Get(), L"\1", L"\0");
//
//    WCHAR dstFileName[MAX_PATH];
//    str::BufSet(dstFileName, dimof(dstFileName), path::GetBaseName(srcFileName));
//    if (str::FindChar(dstFileName, ':')) {
//        // handle embed-marks (for embedded PDF documents):
//        // remove the container document's extension and include
//        // the embedding reference in the suggested filename
//        WCHAR* colon = (WCHAR*)str::FindChar(dstFileName, ':');
//        str::TransChars(colon, L":", L"_");
//        WCHAR* ext;
//        for (ext = colon; ext > dstFileName && *ext != '.'; ext--)
//            ;
//        if (ext == dstFileName)
//            ext = colon;
//        memmove(ext, colon, (str::Len(colon) + 1) * sizeof(WCHAR));
//    }
//    // Remove the extension so that it can be re-added depending on the chosen filter
//    else if (str::EndsWithI(dstFileName, defExt))
//        dstFileName[str::Len(dstFileName) - str::Len(defExt)] = '\0';
//
//    OPENFILENAME ofn = {0};
//    ofn.lStructSize = sizeof(ofn);
//    ofn.hwndOwner = win.hwndFrame;
//    ofn.lpstrFile = dstFileName;
//    ofn.nMaxFile = dimof(dstFileName);
//    ofn.lpstrFilter = fileFilter.Get();
//    ofn.nFilterIndex = 1;
//    ofn.lpstrDefExt = defExt + 1;
//    ofn.Flags = OFN_OVERWRITEPROMPT | OFN_PATHMUSTEXIST | OFN_HIDEREADONLY;
//    // note: explicitly not setting lpstrInitialDir so that the OS
//    // picks a reasonable default (in particular, we don't want this
//    // in plugin mode, which is likely the main reason for saving as...)
//
//    bool ok = GetSaveFileName(&ofn);
//    if (!ok)
//        return;
//
//    WCHAR* realDstFileName = dstFileName;
//    bool convertToTXT = canConvertToTXT && str::EndsWithI(dstFileName, L".txt");
//    bool convertToPDF = canConvertToPDF && str::EndsWithI(dstFileName, L".pdf");
//
//    // Make sure that the file has a valid ending
//    if (!str::EndsWithI(dstFileName, defExt) && !convertToTXT && !convertToPDF) {
//        if (canConvertToTXT && 2 == ofn.nFilterIndex) {
//            defExt = L".txt";
//            convertToTXT = true;
//        } else if (canConvertToPDF && (canConvertToTXT ? 3 : 2) == (int)ofn.nFilterIndex) {
//            defExt = L".pdf";
//            convertToPDF = true;
//        }
//        realDstFileName = str::Format(L"%s%s", dstFileName, defExt);
//    }
//
//    OwnedData pathUtf8(str::conv::ToUtf8(realDstFileName));
//    AutoFreeW errorMsg;
//    // Extract all text when saving as a plain text file
//    if (convertToTXT) {
//        str::Str<WCHAR> text(1024);
//        for (int pageNo = 1; pageNo <= win.ctrl->PageCount(); pageNo++) {
//            WCHAR* tmp = engine->ExtractPageText(pageNo, L"\r\n", nullptr, RenderTarget::Export);
//            text.AppendAndFree(tmp);
//        }
//
//        OwnedData textUTF8(str::conv::ToUtf8(text.LendData()));
//        AutoFree textUTF8BOM(str::Join(UTF8_BOM, textUTF8.Get()));
//        ok = file::WriteFile(realDstFileName, textUTF8BOM, str::Len(textUTF8BOM));
//    } else if (convertToPDF) {
//        // Convert the file into a PDF one
//        PdfCreator::SetProducerName(APP_NAME_STR L" " CURR_VERSION_STR);
//        ok = engine->SaveFileAsPDF(pathUtf8.Get(), gGlobalPrefs->annotationDefaults.saveIntoDocument);
//        if (!ok) {
//#ifdef DEBUG
//            // rendering includes all page annotations
//            ok = PdfCreator::RenderToFile(pathUtf8.Get(), engine);
//#endif
//        } else if (!gGlobalPrefs->annotationDefaults.saveIntoDocument) {
//            SaveFileModifictions(realDstFileName, win.AsFixed()->userAnnots);
//        }
//    } else if (!file::Exists(srcFileName) && engine) {
//        // Recreate inexistant files from memory...
//        ok = engine->SaveFileAs(pathUtf8.Get(), gGlobalPrefs->annotationDefaults.saveIntoDocument);
//    } else if (gGlobalPrefs->annotationDefaults.saveIntoDocument && engine && engine->SupportsAnnotation(true)) {
//        // ... as well as files containing annotations ...
//        ok = engine->SaveFileAs(pathUtf8.Get(), true);
//    } else if (!path::IsSame(srcFileName, realDstFileName)) {
//        // ... else just copy the file
//        WCHAR* msgBuf;
//        ok = CopyFile(srcFileName, realDstFileName, FALSE);
//        if (ok) {
//            // Make sure that the copy isn't write-locked or hidden
//            const DWORD attributesToDrop = FILE_ATTRIBUTE_READONLY | FILE_ATTRIBUTE_HIDDEN | FILE_ATTRIBUTE_SYSTEM;
//            DWORD attributes = GetFileAttributes(realDstFileName);
//            if (attributes != INVALID_FILE_ATTRIBUTES && (attributes & attributesToDrop)) {
//                SetFileAttributes(realDstFileName, attributes & ~attributesToDrop);
//            }
//        } else if (FormatMessage(
//                       FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
//                       nullptr, GetLastError(), 0, (LPWSTR)&msgBuf, 0, nullptr)) {
//            errorMsg.Set(str::Format(L"%s\n\n%s", _TR("Failed to save a file"), msgBuf));
//            LocalFree(msgBuf);
//        }
//    }
//    if (ok && win.AsFixed() && win.AsFixed()->userAnnots && win.AsFixed()->userAnnotsModified && !convertToTXT &&
//        !convertToPDF) {
//        if (!gGlobalPrefs->annotationDefaults.saveIntoDocument || !engine || !engine->SupportsAnnotation(true)) {
//            ok = SaveFileModifictions(realDstFileName, win.AsFixed()->userAnnots);
//        }
//        if (ok && path::IsSame(srcFileName, realDstFileName))
//            win.AsFixed()->userAnnotsModified = false;
//    }
//    if (!ok) {
//        MessageBoxWarning(win.hwndFrame, errorMsg ? errorMsg : _TR("Failed to save a file"));
//    }
//
//    if (ok && IsUntrustedFile(win.ctrl->FilePath(), gPluginURL) && !convertToTXT) {
//        file::SetZoneIdentifier(realDstFileName);
//    }
//
//    if (realDstFileName != dstFileName) {
//        free(realDstFileName);
//    }
//}
//
// static void OnMenuRenameFile(WindowInfo& win) {
//    if (!HasPermission(Perm_DiskAccess))
//        return;
//    if (!win.IsDocLoaded())
//        return;
//    if (gPluginMode)
//        return;
//
//    AutoFreeW srcFileName(str::Dup(win.ctrl->FilePath()));
//    // this happens e.g. for embedded documents and directories
//    if (!file::Exists(srcFileName)) {
//        return;
//    }
//
//    // Prepare the file filters (use \1 instead of \0 so that the
//    // double-zero terminated string isn't cut by the string handling
//    // methods too early on)
//    const WCHAR* defExt = win.ctrl->DefaultFileExt();
//    str::Str<WCHAR> fileFilter(256);
//    bool ok = AppendFileFilterForDoc(win.ctrl, fileFilter);
//    CrashIf(!ok);
//    fileFilter.AppendFmt(L"\1*%s\1", defExt);
//    str::TransChars(fileFilter.Get(), L"\1", L"\0");
//
//    WCHAR dstFileName[MAX_PATH];
//    str::BufSet(dstFileName, dimof(dstFileName), path::GetBaseName(srcFileName));
//    // Remove the extension so that it can be re-added depending on the chosen filter
//    if (str::EndsWithI(dstFileName, defExt)) {
//        dstFileName[str::Len(dstFileName) - str::Len(defExt)] = '\0';
//    }
//
//    AutoFreeW initDir(path::GetDir(srcFileName));
//
//    OPENFILENAME ofn = {0};
//    ofn.lStructSize = sizeof(ofn);
//    ofn.hwndOwner = win.hwndFrame;
//    ofn.lpstrFile = dstFileName;
//    ofn.nMaxFile = dimof(dstFileName);
//    ofn.lpstrFilter = fileFilter.Get();
//    ofn.nFilterIndex = 1;
//    // note: the other two dialogs are named "Open" and "Save As"
//    ofn.lpstrTitle = _TR("Rename To");
//    ofn.lpstrInitialDir = initDir;
//    ofn.lpstrDefExt = defExt + 1;
//    ofn.Flags = OFN_OVERWRITEPROMPT | OFN_PATHMUSTEXIST | OFN_HIDEREADONLY;
//
//    ok = GetSaveFileName(&ofn);
//    if (!ok) {
//        return;
//    }
//
//    UpdateTabFileDisplayStateForWin(&win, win.currentTab);
//    CloseDocumentInTab(&win, true, true);
//    SetFocus(win.hwndFrame);
//
//    DWORD flags = MOVEFILE_COPY_ALLOWED | MOVEFILE_REPLACE_EXISTING;
//    BOOL moveOk = MoveFileEx(srcFileName.Get(), dstFileName, flags);
//    if (!moveOk) {
//        LogLastError();
//        LoadArgs args(srcFileName, &win);
//        args.forceReuse = true;
//        LoadDocument(args);
//        win.ShowNotification(_TR("Failed to rename the file!"), NOS_WARNING);
//        return;
//    }
//
//    AutoFreeW newPath(path::Normalize(dstFileName));
//    RenameFileInHistory(srcFileName, newPath);
//
//    LoadArgs args(dstFileName, &win);
//    args.forceReuse = true;
//    LoadDocument(args);
//}
