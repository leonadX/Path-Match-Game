using UnityEngine;

public class ItemDatabase
{
    public static Item[] items { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]private static void Initialize() => items = Resources.LoadAll<Item>("items/");
}
