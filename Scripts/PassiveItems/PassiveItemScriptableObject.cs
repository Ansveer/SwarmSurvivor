using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PassiveItem")]
public class PassiveItemScriptableObject : ScriptableObject
{
    [SerializeField]
    float multiplier;
    public float Multiplier { get => multiplier; private set => multiplier = value; }

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

    [System.NonSerialized] private float defMultiplier;
    [System.NonSerialized] private int defLevel;

    public void Initialize()
    {
        defMultiplier = Multiplier;
        defLevel = Level;
    }

    public void ResetToDefault()
    {
        multiplier = defMultiplier;
        level = defLevel;

    }

    public void UpdateMultiplier(float mlt)
    {
        //Debug.Log(mlt);
        multiplier += mlt;
        //Debug.Log("Ã”À‹“»œÀ≈≈– ¡Àﬂ“‹");
        //Debug.Log(multiplier);
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
