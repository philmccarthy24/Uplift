![](images/logo.png)

# Uplift

C++ SOAP web service library for Windows 7+

## Prototype build instructions

Open source/Uplift.sln in VS2013.

Build and run, IISExpress should load, go to http://localhost:8080/test/IsapiExtDll.dll to see the test service status page.

## TO DO

- Make Isapi binding layer use asynchronous I/O (looks horrible which is why I left it!)
- Choose a first simple web service client, get soap messages from it and write hard coded serialisers to go from xml to params and back. Write a simple hard coded C++ service class
- Start on C# generator app hpp2soap. Work out how to template code (could use embedded IronPython, see "cog" idea)
- Decide on data types we will support in different project phases. Eg might decide to just do simple literals and base64 encoded binary data without MTOM for now
- Understand how to generate wsdl


