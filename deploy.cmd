@echo off

IF "%SITE_FLAVOR%" == "release" (
  deploy.release.cmd
) ELSE (
  IF "%SITE_FLAVOR%" == "qa" (
    deploy.qa.cmd
  ) ELSE (
    echo You have to set SITE_FLAVOR setting to either "release" or "qa"
    exit /b 1
  )
)
