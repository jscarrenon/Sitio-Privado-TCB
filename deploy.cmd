@echo off

IF "%SITE_FLAVOR%" == "release" (
  deploy.release.cmd
) ELSE (
  IF "%SITE_FLAVOR%" == "qa" (
    deploy.qa.cmd
  ) ELSE (
	IF "%SITE_FLAVOR%" == "debug" (
		deploy.debug.cmd
	) ELSE (
      echo You have to set SITE_FLAVOR setting to either "release", "qa" or "debug"
      exit /b 1
	)
  )
)
