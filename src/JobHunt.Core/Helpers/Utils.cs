
namespace JobHunt.Core.Helpers;

public class Utils
{
    public static string ToStringArray<T>(List<T>? arr)
    {
        string res = "[";
        if (arr != null)
        {
            for (int idx = 0; idx < arr.Count; ++idx)
            {
                res += arr[idx]?.ToString();
                if (idx < arr.Count - 1) res += ", ";
            }

            return res + "]";
        }
        return "[]";
    }

    public static List<string> RemoveKeywordDuplication(List<string>? arr)
    {
        HashSet<string> set = [];
        if (arr != null)
        {
            foreach (string element in arr)
            {
                if (!set.Contains(element.ToUpper())) set.Add(element.ToUpper());
            }

            return [.. set];
        }
        return [];
    }

    public static bool CompareArrayOfString(List<string>? arr1, List<string>? arr2)
    {
        return (arr1 ?? new List<string>()).SequenceEqual(arr2 ?? new List<string>());
    }
}