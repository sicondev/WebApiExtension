# WebApiExtension
An example project that demonstrates how to extend the Sicon Web Api.

# Assembly Naming
The Assembly must end in .WebAPI for the Web API Updater Service to copy it to its local cache.

# Assembly References
The NuGet package references should be replaced with your references to the Sage / Sicon assemblies.

# Method Group Attribute
This attribute controls whether the Controller and its methods appear in the Auto Generated Documentation in the Web Api Portal Page.

# Authentication
To use the in built web api authentication, apply the `[API_BasicAuthentication]` attribute to the controller method and ensure `WebAPISecurityEnabled` is enabled in the web.config file.

When enabled, a basic authenication header needs to be included with each request with username:password Base64 encoded. Web Api users are configured within the addon in Sage 200.

# Logging
To log events to the Sicon Event Log, use `Sicon.Sage200.Architecture.BLL.Logging.Logger`.

# Web Api Controllers
When Web Api Website starts up, it will load any `System.Web.Http.ApiController` Controllers from the assembly and register the routes.

# Web Api Service Extensions
when the Web Api Service starts up, it will load any classes that implement `Sicon.API.Sage200.WebAPI.Common.IWebAPIServiceExtension`. A `StartServiceExtension` and `StopServiceExtension` method and spawn a non blocking task or thread to run until the service shuts down.

# Message Service Extensions
To register for Sage Message Service events in the Web Api, a class that implements `Sicon.API.Sage200.WebAPI.Common.IMessageServiceRegistration` should be created. This has a simple `SubscribeToEvents` method that is used to Register any message service events.

# Addon Package
The Assembly containing the `ApiContoller` or `IWebAPIServiceExtension` should be deployed on the Server, or Server and Client. The Web Api Updater Service will not copy client only assemblies.