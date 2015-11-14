#Unit Test project for General UI

The purpose of this project is to demonstrate a unit test that touches the
Google URI Shortener service.

##Projects

The projects in this solution are the ShortenUri class, and the GeneralUi
test project.  The test framework used is XUnit, using the Visual Studio
test integration.

The packages directory in the solution root was not added, since it contains
16Mb of compiled code for XUnit.  Follow the instructions on the XUnit wiki
to add it to the project.

##Running the Command Line Tests

The tools directory contains the XUnit command line utility. Use it as follows:

    set GoogleShortenerApiKey=__Some Value Here for API key__
    tools\xunit.console\xunit.console.exe GeneralUi\bin\debug\GeneralUi.dll

The test results will be printed out on the command line.
