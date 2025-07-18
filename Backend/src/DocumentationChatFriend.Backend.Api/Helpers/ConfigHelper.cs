namespace DocumentationChatFriend.Backend.Api.Helpers;

public class ConfigHelper
{
    public static T MustBeSet<T>(T? value, string varName) where T : struct
    {
        if (!value.HasValue)
        {
            throw new InvalidOperationException($"{varName} must be set in appsettings.json");
        }

        return value.Value;
    }

    public static T MustBeSet<T>(T? value, string varName) where T : class
    {
        if (IsNullOrEmpty(value))
        {
            throw new InvalidOperationException($"{varName} must be set in appsettings.json");
        }

        return value!;
    }

    private static bool IsNullOrEmpty<T>(T? value)
    {
        if (value is null)
        {
            return true;
        }
        return value is string s && string.IsNullOrWhiteSpace(s);
    }
}