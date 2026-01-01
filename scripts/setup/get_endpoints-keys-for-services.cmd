@echo off

:: This file retrieves and sets the keys and endpoints for various Azure services
:: based on the resource group and service names defined in the environment.

:: All this inforamtion is needed to configure the application to connect to the services.
:: At time of the writing, this script shjould help generate the needed infromation to 
:: copy and paste (c/p) into your configuration 

:: The appsettings.json lives at %APPDATA% (C:\Users\{USER}\AppData\Roaming)

:: The appsettings.json should look like this:

:: {
::   "RC_VISION_ANALYSIS_SERVICE_KEY": "[REDACTED]",
::   "RC_VISION_ANALYSIS_SERVICE_ENDPOINT": "https://your_endpoint_name.cognitiveservices.azure.com/",
::   "RC_VISION_ANALYSIS_SERVICE_REGION": "location",
:: }

setlocal EnableExtensions EnableDelayedExpansion

echo === ==== ===== ====== 
call "%~dp0..\000_boot_env.cmd" %1
echo "Calling %~dp0..\000_boot_env.cmd"
if errorlevel 1 exit /b 1
echo === ==== ===== ====== 

set RG=%RG%

for /f "tokens=*" %%i in ('CALL az cognitiveservices account keys list --name %RC_VISION_SERVICE_NAME% --resource-group %RG% --query "key1" -o tsv') do set RC_VISION_ANALYSIS_SERVICE_KEY=%%i
for /f "tokens=*" %%i in ('CALL az cognitiveservices account show --name %RC_VISION_SERVICE_NAME% --resource-group %RG% --query "properties.endpoint" -o tsv') do set RC_VISION_ANALYSIS_SERVICE_ENDPOINT=%%i

REM Output the retrieved values
echo *****************************************
echo Retrieved Service Keys and Endpoints
echo UPDATE YOUR CONFIGURATIONS AS NEEDED
echo *****************************************
echo.
echo VISION ANALYSIS SERVICE
echo RC_VISION_ANALYSIS_SERVICE_KEY=%RC_VISION_ANALYSIS_SERVICE_KEY%
echo RC_VISION_ANALYSIS_SERVICE_ENDPOINT=%RC_VISION_ANALYSIS_SERVICE_ENDPOINT%
echo RC_VISION_ANALYSIS_SERVICE_REGION=%RC_VISION_ANALYSIS_SERVICE_REGION%
echo.

endlocal
exit /b 0