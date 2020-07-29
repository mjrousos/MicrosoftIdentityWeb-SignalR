# Microsoft.Identity.Web + SignalR

This simple sample demonstrates how Microsoft.Identity.Web can be used to
secure a web API that has both controller and SignalR hub endpoints.

The key to notice is that ASP.NET Core's authentication subsystem works
the same with API controllers as it does with SignalR. By enabling
authentication and authorization middleware and registering an authentication
handler (like Microsoft.Identity.Web), incoming connections for both APIs and
SignalR hubs will have authentication performed. Then, controllers and hubs
alike can use `[Authorize]` attributes to mark endpoints that require
authentication.

The client project demonstrates how .NET clients can specify the JWT token
needed to authenticate with Microsoft.Identity.Web for both web API and
SignalR scenarios.
