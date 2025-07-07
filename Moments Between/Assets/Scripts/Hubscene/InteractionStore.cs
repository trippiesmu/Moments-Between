using System.Collections.Generic;

public static class InteractionStore
{
    static HashSet<string> interacted = new HashSet<string>();

    public static void Add(string id)
    {
        interacted.Add(id);
    }

    public static bool Has(string id)
    {
        return interacted.Contains(id);
    }
}