## Merge Multiple PDF Files

This project demonstrates how to use the [iTextSharp](https://www.nuget.org/packages/iTextSharp/) library to merge multiple PDF files into one single file using C#. 

This code was taken from a Windows Forms application that would allow the user to specify the following input parameters;

* Where the files were stored.
* Should the source files be deleted once merging had completed.
* The output filename which would created **{Filename}-Merged.pdf** in the same directory as the source files.

The application will then make a number of checks before performing the merge which include;

* Checking for an existing output file with the same name as the one specified in the input filename
* Ensuring that a directory with PDF files has been specified
* Ensuring that the user has entered a filename for the final file to be named. 

There were some other features that were included; 

* Ability to minimize to the system tray.
* Ability to maximize from the system tray. 
* Balloon notification when the application is minimized to the system tray



