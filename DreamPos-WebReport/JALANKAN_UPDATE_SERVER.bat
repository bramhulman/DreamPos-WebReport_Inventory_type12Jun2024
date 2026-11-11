@echo off
title Update & Sync Server - DreamPos WebReport
echo =====================================================================
echo   MENJALANKAN UPDATE & SINKRONISASI SERVER (DREAM POS WEBREPORT)
echo =====================================================================
echo.
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0apply_server_update.ps1" -TargetDir "%~dp0"
echo.
echo =====================================================================
echo   PROSES UPDATE SELESAI
echo =====================================================================
pause
