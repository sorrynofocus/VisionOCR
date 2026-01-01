@echo off
@REM Teardown script to remove the Vision Service resources

setlocal EnableExtensions EnableDelayedExpansion

echo Calling "%~dp0..\000_boot_env.cmd"
call "%~dp0..\000_boot_env.cmd" "%~1"
IF ERRORLEVEL 1 GOTO :fail

:: Delete Vision service
echo.
echo "== Deleting Vision service %RC_VISION_SERVICE_NAME% in resource group %RG% =="

call az cognitiveservices account show --resource-group "%RG%" --name "%RC_VISION_SERVICE_NAME%" --only-show-errors >nul 2>&1

if errorlevel 1 (
  echo "Vision service %RC_VISION_SERVICE_NAME% not found. Skipping."
) else (
  call az cognitiveservices account delete --resource-group "%RG%" --name "%RC_VISION_SERVICE_NAME%" --only-show-errors
  IF ERRORLEVEL 1 GOTO :fail
)

  :: Purge Vision service (because the service is soft-deleted -discovery!)
  call az cognitiveservices account purge --location "%LOCATION%" --resource-group "%RG%" --name "%RC_VISION_SERVICE_NAME%" --only-show-errors
  IF ERRORLEVEL 1 GOTO :fail

echo "============================================================"
echo "COMPLETE: VISION SERVICE TEARDOWN"
echo "============================================================"

endlocal
exit /b 0

:fail
set "ERR=%ERRORLEVEL%"
endlocal
exit /b %ERR%
