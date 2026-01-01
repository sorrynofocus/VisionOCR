@echo off
setlocal EnableExtensions EnableDelayedExpansion

echo Calling "%~dp0..\000_boot_env.cmd"
call "%~dp0..\000_boot_env.cmd" "%~1"
if errorlevel 1 exit /b 1

echo "============================================"
echo " Azure Cognitive Services Diagnostic Settings Logging Setup"
echo "============================================"

:: Notes: 
:: Don’t try to enable logging at the resource group level. Azure doesn’t support cascading diagnostic settings.
:: Don’t mix production logs with experimentation if you expand later. You can create a second workspace with production-grade retention rules.

REM Step 1 - Create a Log Analytics Workspace (LAW)
echo "Creating logs for subscription: %AZ_SUBSCRIPTION%"
call az monitor log-analytics workspace create --resource-group %RG% --workspace-name %LAW% --location %LOCATION%    

:: Step 2 - Enable diagnostics settings on Cognitive Services resource
:: Azure Cognitive Vision OCR Service
echo "Enabling diagnostic settings for Vision OCR Service..."
SET LOGS_COG_VISION=[{""category"":""Audit"",""enabled"":true},{""category"":""RequestResponse"",""enabled"":true},{""category"":""AzureOpenAIRequestUsage"",""enabled"":true},{""category"":""Trace"",""enabled"":true}]
SET METRICS__VISION=[{""category"":""AllMetrics"",""enabled"":true}]

:: test - gather categories leave commented out
:: call az monitor diagnostic-settings categories list --resource "/subscriptions/%AZ_SUBSCRIPTION%/resourceGroups/%RG%/providers/Microsoft.CognitiveServices/accounts/%RC_VISION_SERVICE_NAME%"

:: Vision Service
echo "Enabling diagnostic settings for Vision Service..."
echo az monitor diagnostic-settings create --name vision-logs --resource "/subscriptions/%AZ_SUBSCRIPTION%/resourceGroups/%RG%/providers/Microsoft.CognitiveServices/accounts/%RC_VISION_SERVICE_NAME%"  --workspace %LAW%   --logs "%LOGS_COG_VISION%" --metrics "%METRICS__VISION%"
call az monitor diagnostic-settings create --name vision-logs --resource "/subscriptions/%AZ_SUBSCRIPTION%/resourceGroups/%RG%/providers/Microsoft.CognitiveServices/accounts/%RC_VISION_SERVICE_NAME%"  --workspace %LAW%   --logs "%LOGS_COG_VISION%" --metrics "%METRICS__VISION%"


:: Step 3 -Add retention policies (optional)
echo "Setting log retention policy to 30 days..."
call az monitor log-analytics workspace update  --resource-group %RG%  --workspace-name %LAW%  --retention-time %RETENTION_LOG_TIME%

echo "COMPLETED: Azure Cognitive Services Diagnostic Settings Logging Setup."
endlocal
exit /b 0

echo.
echo DONE!
echo Now - create dashboards in the Azure Portal using Log Analytics queries.
echo.
REM =====================
REM Sample Log Analytics Queries for Vision Service
REM =====================

@REM Sample Log Analytics Queries for Vision Service
@REM 1. Count total Vision OCR requests
@REM Kusto Query:
@REM AzureDiagnostics
@REM | where ResourceType == "MICROSOFT.COGNITIVESERVICES/ACCOUNTS"
@REM   and Resource == "%RC_VISION_SERVICE_NAME%"
@REM   and Category == "RequestResponse"
@REM | summarize TotalRequests = count()

@REM 2. Count failed Vision OCR requests (non-200 responses)
@REM AzureDiagnostics
@REM | where ResourceType == "MICROSOFT.COGNITIVESERVICES/ACCOUNTS"
@REM   and Resource == "%RC_VISION_SERVICE_NAME%"
@REM   and Category == "RequestResponse"
@REM   and httpStatusCode_d != 200
@REM | summarize FailedRequests = count() by httpStatusCode_d

@REM 3. Average Vision OCR request latency (ms)
@REM AzureDiagnostics
@REM | where ResourceType == "MICROSOFT.COGNITIVESERVICES/ACCOUNTS"
@REM   and Resource == "%RC_VISION_SERVICE_NAME%"
@REM   and Category == "RequestResponse"
@REM | summarize AvgLatencyMs = avg(durationMs_d)

@REM 4. Vision OCR requests by operation name
@REM AzureDiagnostics
@REM | where ResourceType == "MICROSOFT.COGNITIVESERVICES/ACCOUNTS"
@REM   and Resource == "%RC_VISION_SERVICE_NAME%"
@REM   and Category == "RequestResponse"
@REM | summarize Count = count() by operationName_s

@REM 5. Metrics: Vision OCR calls per minute (last 24h)
@REM AzureMetrics
@REM | where Resource == "%RC_VISION_SERVICE_NAME%"
@REM   and TimeGenerated > ago(24h)
@REM   and MetricName == "TotalCalls"
@REM | summarize CallsPerMinute = sum(Total) by bin(TimeGenerated, 1m)


@REM =====================
@REM Go to Azure portal - Dashboard - New Dashboard
@REM Add tiles using Metrics or Log Analytics   
@REM For each tile:
@REM - Choose resource (Speech, Language, Search, OpenAI)
@REM - Pick a metric OR write a KQL query
@REM - Add visualization (line chart, bar, number)
@REM - Resize and arrange tiles
@REM - Save dashboard   
