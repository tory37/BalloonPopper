using System.Collections.Generic;
using System.Linq;

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

    // TODO does not work
    //public static T RandomExcluding<T>(List<T> exclusions)
    //{
    //    System.Array values = System.Enum.GetValues(typeof(T));

    //    HashSet<int> exclusionIndices = new HashSet<int>();
    //    foreach(T exclusion in exclusions)
    //    {
    //        exclusionIndices.Add(IndexOf<T>(exclusion));
    //    }

    //    int randomIndex System.Random.Range(0, 
    //    T val = GetAtIndex<T>(randomIndex);
    //    return val;
    //}

    public static List<T> Without<T>(List<T> toRemove)
    {
        List<T> wantedValues = System.Enum.GetValues(typeof(T)).Cast<T>().ToList();
        foreach(T t in toRemove)
        {
            wantedValues.Remove(t);
        }

        return wantedValues;
    }
}
