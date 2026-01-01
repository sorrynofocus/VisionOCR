@echo off
setlocal EnableExtensions EnableDelayedExpansion

echo Calling "%~dp0..\000_boot_env.cmd"
call "%~dp0..\000_boot_env.cmd" "%~1"
if errorlevel 1 exit /b 1

echo "============================================"
echo " Azure Resource Setup -COGNITIVE SERVICE- VISION OCR"
echo "============================================"

if not defined RG (
  echo "ERROR: RG is not set."
  exit /b 1
)
if not defined LOCATION (
  echo "ERROR: LOCATION is not set."
  exit /b 1
)


call az cognitiveservices account show --name "%RC_VISION_SERVICE_NAME%" --resource-group "%RG%" --only-show-errors >nul 2>&1
if %errorlevel%==0 (
  echo "OK: Vision account %RC_VISION_SERVICE_NAME% already exists. No action needed."
  goto :done
)

echo "Creating Cognitive Services Vision OCR account %RC_VISION_SERVICE_NAME% in resource group %RG%..."

call az cognitiveservices account create ^
  --name %RC_VISION_SERVICE_NAME% ^
  --resource-group %RG% ^
  --kind ComputerVision ^
  --sku %RC_VISION_SERVICE_SKU% ^
  --location %LOCATION% ^
  --yes ^
  --only-show-errors ^
  --output table  

if errorlevel 1 (
  echo "ERROR: Could not create Cognitive Services Vision OCR account %RC_VISION_SERVICE_NAME% in resource group %RG%."
  GOTO :fail
)

echo "SUCCESS: Created Cognitive Services Vision OCR account %RC_VISION_SERVICE_NAME% in resource group %RG%."
GOTO :done


:fail
endlocal
exit /b 1

:done
endlocal
echo "COMPLETED: COGNITIVE SERVICE VISION OCR CREATE VERIFY."
exit /b 0