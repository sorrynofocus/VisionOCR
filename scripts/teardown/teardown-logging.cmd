 @echo off
:: ============================================================
:: CLEANUP SCRIPT FOR LOG ANALYTICS/DIAGNOSTIC SETTINGS
:: ============================================================

setlocal EnableExtensions EnableDelayedExpansion

echo Calling "%~dp0..\000_boot_env.cmd"
call "%~dp0..\000_boot_env.cmd" "%~1"
if errorlevel 1 exit /b 1

echo Subscription is: %AZ_SUBSCRIPTION%
echo Resource Group:  %RG%
echo Log Analytics Workspace: %LAW%
echo.

echo "============================================================"
echo "Remove diagnostic settings from resources in resource group %RG%"
echo "============================================================"

echo.
echo "== Removing diagnostic settings from service... =="
call az monitor diagnostic-settings list --resource "/subscriptions/%AZ_SUBSCRIPTION%/resourceGroups/%RG%/providers/Microsoft.CognitiveServices/accounts/%RC_VISION_SERVICE_NAME%" --query "[].name" -o tsv > tmp_diag.txt

for /f "tokens=* USEBACKQ" %%D in (tmp_diag.txt) do (
    echo   Deleting setting: %%D
    call az monitor diagnostic-settings delete ^
        --name %%D ^
        --resource "/subscriptions/%AZ_SUBSCRIPTION%/resourceGroups/%RG%/providers/Microsoft.CognitiveServices/accounts/%RC_VISION_SERVICE_NAME%"
)

del tmp_diag.txt 2>nul

echo.
echo ============================================================
echo Delete the Log Analytics Workspace
echo ============================================================

echo "== Deleting workspace: %LAW% =="
call az monitor log-analytics workspace delete ^
    --resource-group %RG% ^
    --workspace-name %LAW% ^
    --yes

echo.
echo "============================================================"
echo "COMPLETE: LOGGING TEARDOWN"
echo "============================================================"

endlocal
exit /b 0