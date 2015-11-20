#Unit Test project for General UI

The purpose of this project is to demonstrate a unit test that touches the
Google URI Shortener service.

##Projects

The projects in this solution are the ShortenUri class, and the GeneralUi
test project.  The test framework used is XUnit, using the Visual Studio
test integration.  Selenium was used for the web driver.

##Packages for the Project

The project requires the XUnit and the Selenium drivers.

###XUnit

While the XUnit Visual Studio framework is available for Visual Studio 2012
via NuGet, the console tools are not available.  These have been compiled from
the git source under VS2012 and added to the project.  The executables are
under the 'tools' directory.

###Selenium 2.48

The Selenium 2.48 drivers were used for interacting programatically with the
Firefox web browser.  The drivers are available at <a href="http://docs.seleniumhq.org/download/">Selenium downloads</a>.
Unzip the C# drivers into the GeneralUi solution directory.  The directory should now contain both the project and drivers
as subdirectories.

##Running the Command Line Tests

The tools directory contains the XUnit command line utility. Use it as follows:

    set GoogleShortenerApiKey=__Some Value Here for API key__
	set GoogleUserId=__Valid Google User Id__
	set GooglePassword=__Valid Password for User Id__
    tools\xunit.console\xunit.console.exe GeneralUi\bin\debug\GeneralUi.dll

The test results will be printed out on the command line.
