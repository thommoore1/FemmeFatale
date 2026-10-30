using UnityEngine;

public class Gift : Item
{
    [Tooltip("0 is hated, 1 is neutral, 2 is loved")] 
    public int likedByEyeris;
    [Tooltip("0 is hated, 1 is neutral, 2 is loved")]
    public int likedByBramble;
    [Tooltip("0 is hated, 1 is neutral, 2 is loved")]
    public int likedByFinnigan;
}
