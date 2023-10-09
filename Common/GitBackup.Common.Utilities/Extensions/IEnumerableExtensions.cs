namespace GitBackup.Common.Utilities.Extensions;

public static class IEnumerableExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? list)
    {
        if (list == null)
        {
            return true;
        }

        return !list.Any();
    }
}
