@echo off
title Update ^& Sync Server - DreamPos WebReport
set "APP_DIR=%~dp0"
if "%APP_DIR:~-1%"=="\" set "APP_DIR=%APP_DIR:~0,-1%"
echo =====================================================================
echo   MENJALANKAN UPDATE ^& SINKRONISASI SERVER (DREAM POS WEBREPORT)
echo =====================================================================
echo.
powershell -NoProfile -ExecutionPolicy Bypass -File "%APP_DIR%\apply_server_update.ps1" -TargetDir "%APP_DIR%"
echo.
echo =====================================================================
echo   PROSES UPDATE SELESAI
echo =====================================================================
pause
