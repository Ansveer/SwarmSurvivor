using UnityEngine;

public class BowBehaviour : ProjectileWeaponBehaviour
{

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        transform.position += currentSpeed * Time.deltaTime * direction;
    }
}
