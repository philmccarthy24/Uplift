![](images/logo.png)

# Uplift

C++ SOAP web service library for Windows 7+

## Prototype build instructions

Build in VS2013. Amend source/IsapiExtDll/Debug32IIS.config so that the physical location specified in line 163 is your $(TargetDir), ie
probably the solution Debug or Release dirs.

Build and run, IISExpress should load, go to http://localhost:8080/test/IsapiExtDll.dll to get the simple rendered web page
to prove that it's working.

## TO DO

- Make Isapi binding layer use asynchronous I/O (looks horrible which is why I left it!)
- Add exception handling to Isapi func, tidy and allow return of different status codes and content types
- Choose a first simple web service client, get soap messages from it and write hard coded serialisers to go from xml to params and back. Write a simple hard coded C++ service class
- Decide whether wsdl belongs hard coded in dll or whether it should be generated
- Create a pretty simple single web page for service status (ie GET without query string), use embedded images. load from dll resource?
- Start on C# generator app hpp2soap. Work out how to template code (could use embedded IronPython, see "cog" idea)
- Decide on data types we will support in different project phases. Eg might decide to just do simple literals and base64 encoded binary data without MTOM for now
- Understand how to generate wsdl


