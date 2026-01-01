@echo off
::  ============================
::  Azure services Environment Variables
::  Windows CMD terminal only
::  ============================
echo BOOT FILE: [%~f0]

REM Make Azure CLI non-interactive
set "AZURE_EXTENSION_USE_DYNAMIC_INSTALL=yes_without_prompt"
set "AZURE_CORE_NO_COLOR=true"
REM Disable interactive prompts
set "AZURE_CORE_DISABLE_INTERACTIVE_PROMPTS=1"


::  Check to see if passed in subscription ID, if not then AZ_SUBSCRIPTION must be set in environment. If both are null, fail and bail.
if not "%~1"=="" (
    set "AZ_SUBSCRIPTION=%~1"
) else if not "%AZ_SUBSCRIPTION%"=="" (
	echo Using existing AZ_SUBSCRIPTION environment variable
) else (
    echo ERROR: You must provide a subscription ID as a parameter or set AZ_SUBSCRIPTION in the environment
    exit /b 1
)

echo "Using subscription ID: %AZ_SUBSCRIPTION%"

call az account set --subscription "%AZ_SUBSCRIPTION%" --only-show-errors
if errorlevel 1 (
  echo "ERROR: Could not set subscription %AZ_SUBSCRIPTION%"
  exit /b 1
)

echo "============================"
echo "Boot loaded for subscription %AZ_SUBSCRIPTION%"
echo "============================"

::  Azure specifics
set "LOCATION=westus2"

::  Core resource group
set "RG=rc_group_vision_01"

:: Azure Vision OCR Service
set "RC_VISION_SERVICE_NAME=rc-vision-ocr-v001"
set "RC_VISION_SERVICE_SKU=F0"
set "RC_VISION_ANALYSIS_SERVICE_REGION=%LOCATION%"

::  Log Analytics workspace - logging under Azure Monitor 
set "LAW=rc-logs-v001"
:: Keep logs for 30 days -default is 30 days, max 730 days
set "RETENTION_LOG_TIME=30"

::  Optional app publish output in the project dir
set "APP_PUBLISH_DIR=.\bin\x64\Release\net9.0\publishprod"

echo "============================"
echo "Boot loader initialized for subscription %AZ_SUBSCRIPTION% in location %LOCATION%"
echo "============================"
exit /b 0
