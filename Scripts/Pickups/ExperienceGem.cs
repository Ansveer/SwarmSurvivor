using UnityEngine;

public class ExperienceGem : Pickup, ICollectible
{
    public int experienceGranted;

    public void Collect()
    {
        
    }

    private void OnDestroy()
    {
        PlayerStats player = FindAnyObjectByType<PlayerStats>();
        player.IncreaseExperience(experienceGranted);
    }
}
