Thought this would be a good starting point. Think we want to get an interface defined going from ISAPI / embedded web server to the lowest level HttpSoapRequestHandler which uses custom generated (can be hard coded for now) IMyServiceXmlDeserializer to get c++ variable params from soap message, then call the registered service interface, and with the return value (or exception) call IMyServiceXmlSerializer to generate response (possibly with Fault).
Also need to consider how the wsdl should be served, and what versions of soap and http to support.

[Useful blog about production running of ISAPI in IIS](http://chee-yang.blogspot.co.uk/2009/10/configure-windows-7-iis7-for-isapi-dll.html)

[Another article about setting up ISAPI dll with iisexpress](http://paul.klink.id.au/Software/Delphi/DebuggingIsapiWithIisExpress.htm)

[Good walkthrough for simplest ISAPI dll](http://www.informit.com/articles/article.aspx?p=605368&seqNum=2)

Nearly got this working, the url I'm using is http://localhost:8080/test/IsapiExtDll.dll after setting physical dir of iisexpress config to solution Debug folder. The Visual Studio project debug command is
```
$(ProgramFiles)\IIS Express\iisexpress.exe
```
and the command arguments are:
```
/config:$(ProjectDir)Debug32IIS.config
```

Now chrome is displaying a "connection was reset" error. HTTP buffering could be the culprit according to:
https://support.microsoft.com/en-gb/kb/840875


hmm setting MaxCopyThreshold to 65536 had no effect.

Maybe we should try getting asynchronous write working in the isapi test dll - it could be that synchronous writes effectively aren't supported anymore as they're too slow?

Putting in async I/O worked, not sure if it was that or sending back http headers which did the trick.

Anyway it's working, just need to refactor the (far too complex) isapi code into something a bit easier to manage, and work out how to plug soap deserialisers and service interface into it.

## Alternatives to IIS

I've also found that as of IIS 7.0, there is a [native C++ IIS module api](https://msdn.microsoft.com/en-us/library/ms692648(v=vs.90).aspx) along with the legacy? ISAPI interface. You can create C++ IIS modules which act as handlers.

That said, ISAPI is supported on several different web servers, which might give more flexibility if portability is a factor.

## module thread safety

GML thread safety is still an issue - confirmed with Jeremy Sallis that multiple instances of SprintSurfaceCreator do indeed execute simultaneously for speed / efficiency, so it is an issue for a single loaded service to serve several clients at once. :( Not yet sure what the solution is - should check with GML team what the situation is wrt thread safety.
That said, the morpher isn't exclusively using GML - the code taking longest to run might be made threadsafe fairly straight forwardly.

## Project template creation

VS2013 - will need to get hold of the [SDK](https://www.microsoft.com/en-us/download/confirmation.aspx?id=40758) to support Vix creation etc

[This seems like a good article](http://www.dotnetcurry.com/visualstudio/1243/create-project-template-visual-studio-2013-2015).

[This shows how to write the template wizard without needing to install to gac](http://stackoverflow.com/questions/16244185/using-iwizard-in-an-item-template-without-installing-assembly-in-gac)