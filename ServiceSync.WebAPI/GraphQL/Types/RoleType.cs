using ServiceSync.Core.Enums;

namespace ServiceSync.WebApi.GraphQL.Types;

/// <summary>
/// This class provides an explicit configuration for our Role enum in the GraphQL schema.
/// This is the definitive way to solve enum parsing issues.
/// </summary>
public class RoleType : EnumType<Role>
{
    protected override void Configure(IEnumTypeDescriptor<Role> descriptor)
    {
        // Set the name of the enum in the GraphQL schema.
        descriptor.Name("Role");

        // Explicitly map each C# enum value to a GraphQL enum value.
        // This forces the names to be "User" and "Admin", exactly matching what the frontend sends,
        // instead of the default "USER" and "ADMIN".
        descriptor.Value(Role.User).Name("User");
        descriptor.Value(Role.Admin).Name("Admin");
        descriptor.Value(Role.SuperUser).Name("SuperUser");
    }
}
