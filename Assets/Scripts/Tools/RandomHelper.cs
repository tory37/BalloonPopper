using System.Collections.Generic;
using System.Linq;

public class RandomHelper
{
    public static int WithExclusions(int min, int max, List<int> exclusions) {
        // If the exclusions list is longer than max - min, then exclusions could be all possible numbers.  Return 0
        var exclude = new HashSet<int>(exclusions);
        var range = Enumerable.Range(min, max).Where(i => !exclude.Contains(i));

        var rand = new System.Random();
        int index = rand.Next(min, max - exclude.Count);
        return range.ElementAt(index);
    }
}
