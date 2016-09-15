TemplateProject
===============

This project holds template web service project C++ files (originally from a Visual Studio Project Template export), and zips them as output.
It is done this way to make it easier to manage / edit the template files, and to keep the VSIX packaging process clean (as the VSIX project 
can accept as input another project's output, rather than holding/referencing a static zip file). 

If using Visual Studio 2013 you will need to install the [Visual Studio SDK](https://www.microsoft.com/en-us/download/confirmation.aspx?id=40758)
for the VSIX project support. Haven't tested on Visual Studio 2015.