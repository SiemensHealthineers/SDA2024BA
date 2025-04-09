# Authentication and Authorization

## Introduction

- API Authorization is based on Bearer token
- If an endpoint does not need to be protected, it should contain `[AllowAnonymous]` attribute (use wisely and only if absolutely necessary)
- If an endpoint should be protected, it must contain i.e. `[CustomRoleAuthorize(Roles.InstitutionGroup)]` attribute instead (see further info below)
    - Access token should be included in `Authorization` header - standard Bearer token - if not present or invalid (e.g. expired), returns 403 (default ASP.NET authorization)
    - Roles can be combined by bitwise operations

## API Authorization flow

We're using role-based authorization for our API endpoints. Each endpoint can be limited to a given set of roles. We're using a custom `CustomRoleAuthorize` attribute, which extends and builds on the standard `Authorize` attribute. Since authorization policies are looked up from the policy provider only by string, this authorization attribute creates its policy name based on a constant prefix and the user-supplied `Roles` parameter. A custom authorization policy provider (`CustomRolePolicyProvider`) can then produce an authorization policy with the necessary requirements based on this policy name.

### Roles enum

As mentioned, the `Roles` enum is used to pass information about allowed users on each endpoint. Thanks to marking the enum with `[Flags]` attribute, we can simply use `ToString` to get the string representation of the enum's values (combinations are separated with comma followed by a space), instead of the underlying integral values. Furthermore, declaring the values as powers of 2 allows us to apply bitwise operations (with combination of `&`, `|`, and `~` operators).
This enumeration contains all PIP roles along with some shorthands for often-used combinations (like `Shs` containing all non-external users and `All` for all known roles). Thanks to aforementioned ability to combine the values, it is easy to use this type in `CustomRoleAuthorize` constructor to create variations without the need for explicitly creating an entry in the enum class.

### CustomRoleAuthorize

Usage of this attribute is straight-forward. It should be applied to our Http methods. Pass the allowed roles in the constructor. If you want to allow multiple roles, use the bitwise OR (`|`). It is also possible to exclude individual roles (with bitwise complement and AND (`&` + `~` or with a XOR `^`).

Examples:

```csharp
[CustomRoleAuthorize(Roles.Admin)]
```
```csharp
[CustomRoleAuthorize(Roles.InstitutionGroup)]
```
```csharp
[CustomRoleAuthorize(Roles.Nurse | Roles.Doctor)]
```

## CustomRoleAuthorizationHandler

This class contains the logic for determining if the user contains the necessary roles. It does so by applying bitwise AND operation between the user's and the requirement roles. The roles are retrieved from the `roles` claim in the access token (`User.Claims` in C#).

## CustomRolesHelper

This class contains method mapping the user's roles (claims) from the token into Roles values. It can inject all roles if `UseDeveloperRoles` setting is present and the user calling the endpoint is added in a whitelist (in `Developers` setting).

## Azure Entra ID

Is the default identity provider and used to authenticate users. Generally, all users which are existing in Azure tenant can authenticate. Azure Entra ID provides also Authorization Information in form of claims in the Tokens.


Further dev info can be found here:

https://learn.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-8.0