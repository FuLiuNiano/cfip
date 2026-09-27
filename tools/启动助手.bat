@echo off
rem nodesCatch 助手:真 subconverter(25501) + 剥BOM代理(25500)
rem nodesCatch.exe 启动前先双击本脚本;代理会自动剥掉 temp.txt 的 BOM,避免 400
cd /d "%~dp0"
taskkill /F /IM subconverter.exe >nul 2>&1
start "" /min cmd /c "cd subconverter && subconverter.exe"
start "" /min cmd /c "python bom_proxy.py"
timeout /t 2 /nobreak >nul
echo 助手已启动(25500=代理, 25501=subconverter),现在可以打开 nodesCatch.exe
pause
