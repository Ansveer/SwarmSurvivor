using UnityEngine;

public class Meat : Pickup, ICollectible
{
    public int healthToRestore;

    public void Collect()
    {
        PlayerStats player = FindAnyObjectByType<PlayerStats>();
        player.RestoreHealth(healthToRestore);
    }
}
