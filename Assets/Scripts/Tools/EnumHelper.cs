using System.Collections.Generic;

public class EnumHelper
{
    public static T Random<T>()
    {
        System.Array values = System.Enum.GetValues(typeof(T));
        System.Random random = new System.Random();
        T randomT = (T)values.GetValue(random.Next(values.Length));
        return randomT;
    }

    public static int IndexOf<T>(T value)
    {
        return System.Array.IndexOf(System.Enum.GetValues(typeof(T)), value);
    }

    public static T GetAtIndex<T>(int index)
    {
        System.Array values = System.Enum.GetValues(typeof(T));
        return (T)values.GetValue(index);
    }

    public static T RandomExcluding<T>(List<T> exclusions)
    {
        System.Array values = System.Enum.GetValues(typeof(T));

        HashSet<int> exclusionIndices = new HashSet<int>();
        foreach(T exclusion in exclusions)
        {
            exclusionIndices.Add(IndexOf<T>(exclusion));
        }

        int randomIndex = RandomHelper.Exclude(0, values.Length, exclusionIndices);
        return GetAtIndex<T>(randomIndex);
    }
}
