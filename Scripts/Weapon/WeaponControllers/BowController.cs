using UnityEngine;

public class BowController : WeaponController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedBow = Instantiate(weaponData.Prefab);
        spawnedBow.transform.position = transform.position;
        spawnedBow.GetComponent<BowBehaviour>().DirectionChecker(pm.lastMoveDir);
    }
}
