# OCR Function 5 Runbook: Azure Setup and Teardown
**File:** `ocr_function_5_runbook_azure_setup.md`  
**Project:** VisionOCR
**Codename:** function_5  
**Platform:** Windows CMD + Azure CLI  
**Purpose:** Provision and remove the Azure resources needed for this repo's Azure Vision OCR demo app.

---

## 1) Runbook basics

This runbook exists to prevent **script confusion**.

You do **not** need to manually run every script in `scripts\setup\` or `scripts\teardown\`.  
Instead, you run **two orchestrator scripts**:

- **Setup (creates everything):** `scripts\setup\buildAzureservicesDeployment.cmd`
- **Teardown (removes everything):** `scripts\teardown\buildTeardownAll.cmd`

There is also a **supplemental helper**:

- **Print endpoints/keys:** `scripts\setup\get_endpoints-keys-for-services.cmd`

That one does not create resources. It just prints the values you need to paste into the app.

---

## 2) Prerequisites

### Required
- Windows 11
- Azure CLI installed (`az --version`)
- Logged into Azure CLI (interactive or service principal)
- An Azure subscription where you can create:
  - Resource Group
  - Cognitive Services (Computer Vision)
  - Log Analytics Workspace + Diagnostic Settings

### Recommended
Run from a normal `cmd.exe` prompt (these scripts are Windows CMD style).

---

## 3) Authentication options

### Option A: Interactive login (simplest)
```cmd
az login
az account show --output table
```

### Option B: Service Principal login (Bundled authentication for SP and app)
If you prefer scripts to run without interactive login:

```cmd
az ad sp create-for-rbac --name "VisionOCR-automation" --role contributor --scopes /subscriptions/<your-subscription-id>
```

Then login:

```cmd
az login --service-principal -u <appId> -p <password> --tenant <tenant>
az account show --output table
```

> The scripts will still need your **Subscription ID** (see next sections).

---

## 4) The Boot Loader: `scripts\000_boot_env.cmd`

All setup/teardown scripts call the boot loader.  
This is where you set shared environment variables for the whole run.

### What it does
- Forces Azure CLI non-interactive behavior (no prompts/colors)
- Sets the subscription context (`az account set --subscription ...`)
- Defines the resource names used by the scripts:
  - Resource Group name
  - Vision service name/SKU
  - Log Analytics workspace + retention

### What you should edit
Open:

`scripts\000_boot_env.cmd`

Update these values to match your environment and naming preferences:

- `LOCATION` (ex: `westus2`)
- `RG` (resource group name)
- `RC_VISION_SERVICE_NAME` (Vision resource name)
- `RC_VISION_SERVICE_SKU` (ex: `F0` free tier, or paid SKU)
- `LAW` (Log Analytics workspace name)
- `RETENTION_LOG_TIME` (log retention days)

> Tip: If you plan to run this repeatedly, use unique names to avoid conflicts, especially for Cognitive Services resources.

### Subscription ID input (important)
Every orchestrator script accepts the subscription in **two ways**:

1) Pass it as the first argument  
2) Or set it in an environment variable `AZ_SUBSCRIPTION`

Examples:

```cmd
SET AZ_SUBSCRIPTION=<your-subscription-id>
```

or:

```cmd
buildAzureservicesDeployment.cmd <your-subscription-id>
```

---

## 5) Main operation: Setup (provision all resources)

### The one command you run
From the repo root:

```cmd
cd scripts
.\setup\buildAzureservicesDeployment.cmd <your-subscription-id> > buildAzureservicesDeployment.log 2>&1
```

or if you already set `AZ_SUBSCRIPTION`:

```cmd
cd scripts
SET AZ_SUBSCRIPTION=<your-subscription-id>
.\setup\buildAzureservicesDeployment.cmd %AZ_SUBSCRIPTION% > buildAzureservicesDeployment.log 2>&1
```

### What it creates (high level)
`buildAzureservicesDeployment.cmd` orchestrates the setup scripts:

- `001_group-create-verify.cmd`  
  Creates (or verifies) the resource group.

- `002_vision-service-create-verify.cmd`  
  Creates (or verifies) the **Computer Vision** resource used for OCR (READ).

- `006_logging-create.cmd`  
  Creates a Log Analytics workspace and enables diagnostic settings on the Vision resource.

At the end, it calls:

- `get_endpoints-keys-for-services.cmd`  
  Prints Vision **endpoint/key/region** for copy/paste into your app configuration.

> Note: Scripts came from my last project and were adapted for this demo.

### Success criteria
- The log ends with `Azure services build completed successfully.`
- You can see resources in the Azure Portal under the resource group you set (`RG`).

---

## 6) Supplemental: Print endpoints and keys (copy/paste helper)

If you already ran setup earlier and just need the keys/endpoint again:

```cmd
cd scripts
.\setup\get_endpoints-keys-for-services.cmd <your-subscription-id>
```

This prints:

- `RC_VISION_ANALYSIS_SERVICE_KEY`
- `RC_VISION_ANALYSIS_SERVICE_ENDPOINT`
- `RC_VISION_ANALYSIS_SERVICE_REGION`

Use those values in the WinForms app UI (Endpoint and Key config section).

---

## 7) Main operation: Teardown (remove all resources and stop charges)

### The one command you run
From repo root:

```cmd
cd scripts
.\teardown\buildTeardownAll.cmd <your-subscription-id> > buildTeardownAll.log 2>&1
```

### What it removes (high level)
`buildTeardownAll.cmd` orchestrates teardown scripts:

- `teardown-logging.cmd`  
  Deletes diagnostic settings on the Vision resource and deletes the Log Analytics workspace.

- `teardown-vision-service.cmd`  
  Deletes the Vision resource **and purges it** (Cognitive Services uses soft-delete).

- `teardown-rc-grp.cmd`  
  Deletes the **resource group** (final cleanup sweep).

Afterward it also lists:
- `az cognitiveservices account list-deleted`
- `az lock list --resource-group "%RG%"`

### Important notes about purge
Cognitive Services resources may be **soft-deleted**.  
The teardown script includes a purge call so you do not get stuck with a name that cannot be reused. F) tier only allows one resource per subscription. Best to purge it immediately for re-use.

---

## 8) Troubleshooting

### "Azure CLI is not logged in"
Run:

```cmd
az login
az account show
```

### "Resource already exists" / name conflicts
Change the names in `000_boot_env.cmd`:
- `RG`
- `RC_VISION_SERVICE_NAME`
- `LAW`

Then re-run setup.

### Teardown fails due to locks
If `az lock list` shows locks, remove them (Portal or CLI) then rerun teardown.

### Need to see what happened
Always run the orchestrators with logs:

```cmd
.\setup\buildAzureservicesDeployment.cmd <sub> > buildAzureservicesDeployment.log 2>&1
.\teardown\buildTeardownAll.cmd <sub> > buildTeardownAll.log 2>&1
```

---

## 9) Reminder! Avoid resource charges! 

When you're done demoing/testing:

1) Run teardown:
```cmd
cd scripts
.\teardown\buildTeardownAll.cmd <your-subscription-id> > buildTeardownAll.log 2>&1
```

2) Confirm in Azure Portal:
- Resource group `%RG%` is gone
- No remaining Cognitive Services resources
- No remaining Log Analytics workspace

3) If the script reports soft-deleted resources, purge them using the example it prints. It tries to remove them automatically, but sometimes you need to do it manually.

---

## 10) Summary

- **Boot loader** sets names, the location, and subscription context: `000_boot_env.cmd`
- **Setup** creates everything: `setup\buildAzureservicesDeployment.cmd`
- **Teardown** removes everything: `teardown\buildTeardownAll.cmd`
- **Keys/endpoints helper** prints app config values: `setup\get_endpoints-keys-for-services.cmd`


