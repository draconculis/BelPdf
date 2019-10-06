rem "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\msbuild.exe" VS2017\SumatraPDF.sln /p:Configuration=Release /p:Platform=Win32 /t:BelSumatraPdf

echo %date%
set CUR_YYYY=%date:~0,4%
set CUR_MM=%date:~5,2%
set CUR_DD=%date:~8,2%
set CUR_HH=%time:~0,2%
if %CUR_HH% lss 10 (set CUR_HH=0%time:~1,1%)
set CUR_MI=%time:~3,2%
if %CUR_MI% lss 10 (set CUR_HH=0%time:~1,1%)

set targetDir=.\rel_%CUR_YYYY%%CUR_MM%%CUR_DD%%CUR_HH%%CUR_MI%
ROBOCOPY .\rel %targetDir% /E /XF *.pdb *.lib *.ipdb *.iobj *.xml System.Data.SQLite.dll.config /XD obj

pause
