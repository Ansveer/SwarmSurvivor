using UnityEngine;

public class AuraController : WeaponController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedAura = Instantiate(weaponData.Prefab);
        spawnedAura.transform.position = transform.position;
        spawnedAura.transform.parent = transform;
    }
}
