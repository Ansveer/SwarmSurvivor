using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    [SerializeField]
    GameObject prefab;
    public GameObject Prefab { get => prefab; private set => prefab = value; }

    [SerializeField]
    float damage;
    public float Damage { get => damage; private set => damage = value; }

    [SerializeField]
    float speed;
    public float Speed { get => speed; private set => speed = value; }

    [SerializeField]
    float cooldownDuration;
    public float CooldownDuration { get => cooldownDuration; private set => cooldownDuration = value; }

    [SerializeField]
    int pierce;
    public int Pierce { get => pierce; private set => pierce = value; }

    [SerializeField]
    int level;
    public int Level { get => level; private set => level = value; }

    [SerializeField]
    Sprite icon;
    public Sprite Icon { get => icon; private set => icon = value; }

    [SerializeField]
    new string name;
    public string Name { get => name; private set => name = value; }

    [SerializeField]
    string description;
    public string Description { get => description; private set => description = value; }

    [System.NonSerialized] private float defDamage;
    [System.NonSerialized] private float defSpeed;
    [System.NonSerialized] private float defCooldownDuration;
    [System.NonSerialized] private int defPierce;
    [System.NonSerialized] private int defLevel;

    public void Initialize()
    {
        defDamage = Damage;
        defSpeed = Speed;
        defCooldownDuration = CooldownDuration;
        defPierce = Pierce;
        defLevel = Level;
    }

    public void ResetToDefault()
    {
        damage = defDamage;
        speed = defSpeed;
        cooldownDuration = defCooldownDuration;
        pierce = defPierce;
        level = defLevel;
    }

    public void UpdateDamage(float dmg)
    {
        damage += dmg;
    }

    public void UpdateSpeed(float spd)
    {
        speed += spd;
    }

    public void UpdateCooldownDuration(float cd)
    {
        cooldownDuration -= cd;
    }

    public void UpdatePierce(int prc)
    {
        pierce += prc;
    }

    public void UpdateLevel(int lvl)
    {
        level += lvl;
    }

    public void UpdateDescription(string text)
    {
        description = text;
    }
}
