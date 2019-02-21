# Running Typescript and Javascript using Node JS in .Net Core

Click [here](https://medium.com/@michaelceber/running-typescript-and-javascript-using-node-js-in-net-core-ab2e8ba7ff4d) for the Medium post.

### Introduction

There are many articles which focus on either using Javascript or Typescript in .NET Core for the purpose of the client interface, whether it be for Angular, React or general single page applications.
This is focussed purely on executing Javascript and Typescript from an ASP .Net Core application.
This is achieved using the Microsoft.AspNetCore.NodeServices Nuget package as part of the JavascriptServices toolset.

### Why do this?
At some point in the past you were probably deciding what server-side technology should I use for the server, namely a web or rest API. If you are from a .net background you would probably choose ASP .net core over Node JS. There are many other articles comparing the two.
So you chose .Net Core for good reasons and you were looking for some thirdparty packages but could not find any suitable ones on Nuget. There is a huge selection of javascript libraries available on the node package manager (NPM). 
For example in my instance I needed to generate PDF documents and although there were a couple options on Nuget those available for both server and client Javascript looked much more favourable. I want to seemlessly from my .net code make calls to such libraries. As well you might have written your own Typescript or Javascript libraries and would simply like to reuse the code alongside your c# / .net core project.
Secondly, as the rest of my code is in c# to keep my server side code robust a strongly typed option would be better. Therefore I would like to add Typescript files at will to my visual studio project and seemless call this code.
Hence, this post will focus on setting up a .net core project to do just this with examples of calling both Javascript directly and Typescript code.

### Prerequistes
| Prerequiste | Download |
| ------ | ------ |
| Node JS | https://nodejs.org/en/ |
| NET Core 2.1 | https://www.microsoft.com/net/download |

### Dependencies
| Dependency | Further Information |
| ------ | ------ |
| Microsoft.AspNetCore.NodeServices | https://www.nuget.org/packages/Microsoft.AspNetCore.NodeServices/|

### References
| Dependency | Further Information |
| ------ | ------ |
| JavaScriptServices | https://github.com/aspnet/JavaScriptServices |
| PDF Make (example JS dependency used) | http://pdfmake.org |

### Configure your Project
Simple add the following line to ConfigureServices in your [Startup.cs](https://github.com/MikeyFriedChicken/DotNetCoreTypeScript/blob/master/MikeyFriedChicken.DotNetCoreTypeScript/Startup.cs) file:
```csharp
Public void ConfigureServices(IServiceCollection services)
{
    //
    services.AddNodeServices();
    //
}
```

Next We create a wrapper class called [JavaScriptService.cs](MikeyFriedChicken.DotNetCoreTypeScript/Services/JavaScriptService.cs).  This contains the following constructor.  The NodeServices instance automatically gets injected.

```csharp
public JavaScriptService([FromServices]INodeServices nodeServices, string scriptFolder)
{
    _nodeServices = nodeServices;
    _scriptFolder = scriptFolder;
}
```
This is achieved by registering our new IJavaScriptService instance in the ConfigureServices method in [Startup.cs](https://github.com/MikeyFriedChicken/DotNetCoreTypeScript/blob/master/MikeyFriedChicken.DotNetCoreTypeScript/Startup.cs) which will now look like this:

```csharp
Public void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<IJavaScriptService, JavaScriptService>();

    // Add node js
    services.AddNodeServices(options =>
    {
        //options.LaunchWithDebugging = true;
        //options.UseSocketHosting();
    });
}
```

So to use our node js wrapper from our [ValuesController.cs](MikeyFriedChicken.DotNetCoreTypeScript/Controllers/ValuesController.cs) class we simply include the service as a dependency via the constructor:

```csharp
private IJavaScriptService _javaScriptService;
public ValuesController(IJavaScriptService javaScriptService)
{
    _javaScriptService = javaScriptService;
}
```
And to use the service for example the 'Hello' typescript function in [ValuesController.cs](MikeyFriedChicken.DotNetCoreTypeScript/Controllers/ValuesController.cs) we do the following:

```csharp
// GET api/values/hello
[HttpGet("hello")]
public async Task<IActionResult> Hello()
{
    string ret = await _javaScriptService.Hello("Michael");
    return Ok(ret);
}
```

Which calls the 'Hello' method inside [JavaScriptService.cs](MikeyFriedChicken.DotNetCoreTypeScript/Services/JavaScriptService.cs)
```csharp
    public async Task<string> Hello(string name)
    {
        string path = Path.Combine(_scriptFolder, "./scripts/hello");
        var result = await _nodeServices.InvokeAsync<string>(path, name);
        return result;
    }
```

And that is it!  

The [hello.ts](MikeyFriedChicken.DotNetCoreTypeScript/scripts/hello.ts) typescript file automatically gets compiled into a corresponding javascript file called [hello.js](MikeyFriedChicken.DotNetCoreTypeScript/scripts/hello.js) which also lives in the scripts folder.

On execution the node services picks the JS up from the ./scripts/hello path and executes accordingly.

### Building and Running The Project

Visual Studio should automatically install the node dependencies as per the Package.json file, but just to be sure you can run the follow from the project folder.
```sh
npm install
```
or
```sh
yarn
```

To install the nuget dependencies this should also be automatic, however, you can also do this from command line:
```sh
dotnet restore
```

To run the project you can click F5 in Visual Studio for debug mode or from command line in the project folder:
```sh
dotnet run
```

Finally once up and running test the addNumbers function from the browser:

```sh
http://localhost:5000/api/values/addNumbers
```

This should return '7'

To test the TypeScript Make PDF function try the following:

```sh
http://localhost:5000/api/values/makepdf
```
This should return the PDF straight into the browser (depending on your browser and configuration), or will return a PDF file to download.

### Unit Testing
Add the following to the unit test post build as this may be the best way to put the javascript files from the main project in to the target test folder:

```sh
copy $(SolutionDir)MikeyFriedChicken.DotNetCoreTypeScript\scripts\*.* $(TargetDir)scripts
```

When calling the JavaScriptService wrapper the following base paths were needed so that the scripts could be correctly resolved:

```csharp
private string SCRIPT_FOLDER_TS =  ".";  // Works for TS javascript files
private string SCRIPT_FOLDER_JS = "./bin/Debug/netcoreapp2.1";  // Works for pure javascript files
```

To initialise the the JavaScriptService from a unit test the following code is used:
you will note the project path / where the node_modules are located had to be specified


```csharp
var services = new ServiceCollection();
services.AddNodeServices(options => {
    // Set any properties that you want on 'options' here
    options.ProjectPath = "../../../";
});
var serviceProvider = services.BuildServiceProvider();
var nodeServices = serviceProvider.GetRequiredService<INodeServices>();
```

### Issues
Sometimes when building you might get the following error:

```sh
Error	TS2403	(TS) Subsequent variable declarations must have the same type.  Variable 'module' must be of type 'any', but here has type 'NodeModule'.
```
To resolve this change, double click on the error and it will take you to the index.d.ts file with the error. Simply change the following and save:

```javascript
declare var module: NodeModule;
```
to

```javascript
declare var module: any;
```

