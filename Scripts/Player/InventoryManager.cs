using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
using System;

public class InventoryManager : MonoBehaviour
{
    public List<WeaponController> weaponSlots = new List<WeaponController>(3);
    public int[] weaponLevels = new int[3];
    public List<Image> weaponUISlots = new List<Image>();
    public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(3);
    public int[] passiveItemLevels = new int[3];
    public List<Image> passiveItemUISlots = new List<Image>();

    [System.Serializable]
    public class WeaponUpgrade
    {
        public GameObject initialWeapon;
        public WeaponScriptableObject weaponData;
    }

    [System.Serializable]
    public class PassiveItemUpgrade
    {
        public GameObject initialPassiveItem;
        public PassiveItemScriptableObject passiveItemData;
    }

    [System.Serializable]
    public class UpgradeUI
    {
        public TMP_Text upgradeNameDisplay;
        public TMP_Text upgradeDescriptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton;
    }

    public List<WeaponUpgrade> weaponUpgradeOptions = new List<WeaponUpgrade>();
    public List<PassiveItemUpgrade> passiveItemUpgradeOptions = new List<PassiveItemUpgrade>();
    public List<UpgradeUI> upgradeUIOptions = new List<UpgradeUI>();

    PlayerStats player;

    void Start()
    {
        player = GetComponent<PlayerStats>();
    }

    public void AddWeapon(int slotIndex, WeaponController weapon)
    {
        weaponSlots[slotIndex] = weapon;
        weaponLevels[slotIndex] = weapon.weaponData.Level;
        weaponUISlots[slotIndex].enabled = true;
        weaponUISlots[slotIndex].sprite = weapon.weaponData.Icon;

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    public void AddPassiveItem(int slotIndex, PassiveItem passiveItem)
    {
        passiveItemSlots[slotIndex] = passiveItem;
        passiveItemLevels[slotIndex] = passiveItem.passiveItemData.Level;
        passiveItemUISlots[slotIndex].enabled = true;
        passiveItemUISlots[slotIndex].sprite = passiveItem.passiveItemData.Icon;

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    (KeyValuePair<string, float>, string, string, int) RandomWeaponLevelUp(int slotIndex)
    {
        KeyValuePair<string, float> upgrade = new KeyValuePair<string, float>("null", 0);
        string name = "";
        string description = "";
        int lvl = 0;
        if (weaponSlots.Count > slotIndex)
        {
            List<string> modifiers = new List<string>(4)
            {
                "Damage",
                "Speed",
                "CooldownDuration",
                "Pierce"
            };

            WeaponController weapon = weaponSlots[slotIndex];
            if (weapon.weaponData.Name == "Aura")
            {
                upgrade = new KeyValuePair<string, float>("Damage", UnityEngine.Random.Range(1.0f, 3.0f));
            }
            else
            {
                upgrade = new KeyValuePair<string, float>(modifiers[UnityEngine.Random.Range(0, 4)], UnityEngine.Random.Range(1.0f, 3.0f));
            }

            lvl = weapon.weaponData.Level + 1;
            name = weapon.weaponData.Name + " (" + lvl.ToString() + ")";
            description = upgrade.Key + " +" + String.Format("{0:0.00}", upgrade.Value);
            return (upgrade, name, description, lvl);
        }
        return (upgrade, name, description, lvl);
    }

    public void LevelUpWeapon(int slotIndex, KeyValuePair<string, float> upgrade)
    {
        if (weaponSlots.Count > slotIndex)
        {
            WeaponController weapon = weaponSlots[slotIndex];
            weapon.weaponData.UpdateLevel(1);
            switch (upgrade.Key)
            {
                case "Damage":
                    weapon.weaponData.UpdateDamage(upgrade.Value);
                    break;
                case "Speed":
                    weapon.weaponData.UpdateSpeed(upgrade.Value);
                    break;
                case "CooldownDuration":
                    weapon.weaponData.UpdateCooldownDuration(upgrade.Value / 100f);
                    break;
                case "Pierce":
                    weapon.weaponData.UpdatePierce((int)upgrade.Value);
                    break;

            }

            if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
            {
                GameManager.instance.EndLevelUp();
            }
        }
    }

    (float, string, string, int) RandomPassiveItemLevelUp(int slotIndex)
    {
        float upgrade = 0f;
        string name = "";
        string description = "";
        int lvl = 0;
        if (passiveItemSlots.Count > slotIndex)
        {
            PassiveItem passiveItem = passiveItemSlots[slotIndex];

            upgrade = UnityEngine.Random.Range(1.0f, 5.0f);
            lvl = passiveItem.passiveItemData.Level + 1;
            name = passiveItem.passiveItemData.Name + " (" + lvl.ToString() + ")";
            description = "Multiplier +" + String.Format("{0:0.00}", upgrade);
            return (upgrade, name, description, lvl);
        }
        return (upgrade, name, description, lvl);
    }

    public void LevelUpPassiveItem(int slotIndex, float multiplier)
    {
        if (passiveItemSlots.Count > slotIndex)
        {
            PassiveItem passiveItem = passiveItemSlots[slotIndex];
            passiveItem.passiveItemData.UpdateLevel(1);
            passiveItem.passiveItemData.UpdateMultiplier(multiplier);
            passiveItem.UpdateModifier();

            if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
            {
                GameManager.instance.EndLevelUp();
            }
        }
    }

    void ApplyUpgradeOptions()
    {
        KeyValuePair<string, float> upgrade;
        float multiplier;
        string name;
        string description;
        int lvl;

        List<WeaponUpgrade> availableWeaponUpgrades = new List<WeaponUpgrade>(weaponUpgradeOptions);
        List<PassiveItemUpgrade> availablePassiveItemUpgrades = new List<PassiveItemUpgrade>(passiveItemUpgradeOptions);

        foreach (var upgradeOption in upgradeUIOptions)
        {
            if (availableWeaponUpgrades.Count == 0 && availablePassiveItemUpgrades.Count == 0)
            {
                return;
            }

            int upgradeType;

            if (availableWeaponUpgrades.Count == 0)
            {
                upgradeType = 2;
            } else if (availablePassiveItemUpgrades.Count == 0)
            {
                upgradeType = 1;
            } else
            {
                upgradeType = UnityEngine.Random.Range(1, 3);
            }

            if (upgradeType == 1)
            {

                WeaponUpgrade chosenWeaponUpgrade = availableWeaponUpgrades[UnityEngine.Random.Range(0, availableWeaponUpgrades.Count)];

                availableWeaponUpgrades.Remove(chosenWeaponUpgrade);

                if (chosenWeaponUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);

                    bool newWeapon = false;
                    for (int i = 0; i < weaponSlots.Count; i++)
                    {
                        if (weaponSlots[i] != null && weaponSlots[i].weaponData == chosenWeaponUpgrade.weaponData)
                        {
                            newWeapon = false;
                            if (!newWeapon)
                            {
                                if (chosenWeaponUpgrade.weaponData.Level >= 40)
                                {
                                    DisableUpgradeUI(upgradeOption);
                                    break;
                                }

                                (upgrade, name, description, lvl) = RandomWeaponLevelUp(i);
                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i, upgrade));
                                upgradeOption.upgradeNameDisplay.text = name;
                                upgradeOption.upgradeDescriptionDisplay.text = description;
                            }
                            break;
                        }
                        else
                        {
                            newWeapon = true;
                        }
                    }
                    if (newWeapon)
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnWeapon(chosenWeaponUpgrade.initialWeapon));
                        upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.Name + " (" + chosenWeaponUpgrade.weaponData.Level.ToString() + ")";
                        upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.Description;
                    }

                    upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.weaponData.Icon;
                }
            }
            else if (upgradeType == 2)
            {
                PassiveItemUpgrade chosenPassiveItemUpgrade = availablePassiveItemUpgrades[UnityEngine.Random.Range(0, availablePassiveItemUpgrades.Count)];

                availablePassiveItemUpgrades.Remove(chosenPassiveItemUpgrade);

                if (chosenPassiveItemUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);

                    bool newPassiveItem = false;
                    for (int i = 0; i < passiveItemSlots.Count; i++)
                    {
                        if (passiveItemSlots[i] != null && passiveItemSlots[i].passiveItemData == chosenPassiveItemUpgrade.passiveItemData)
                        {
                            newPassiveItem = false;
                            if (!newPassiveItem)
                            {
                                if (chosenPassiveItemUpgrade.passiveItemData.Level >= 10)
                                {
                                    DisableUpgradeUI(upgradeOption);
                                    break;
                                }

                                (multiplier, name, description, lvl) = RandomPassiveItemLevelUp(i);
                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i, multiplier));
                                upgradeOption.upgradeNameDisplay.text = name;
                                upgradeOption.upgradeDescriptionDisplay.text = description;
                            }
                            break;
                        }
                        else
                        {
                            newPassiveItem = true;
                        }
                    }

                    if (newPassiveItem)
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnPassiveItem(chosenPassiveItemUpgrade.initialPassiveItem));
                        upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Name + " [" + chosenPassiveItemUpgrade.passiveItemData.Level.ToString() + "]";
                        upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Description;
                    }

                    upgradeOption.upgradeIcon.sprite = chosenPassiveItemUpgrade.passiveItemData.Icon;
                }
            }
        }
    }

    void RemoveUpgradeOptions()
    {
        foreach (var upgradeOption in upgradeUIOptions)
        {
            upgradeOption.upgradeButton.onClick.RemoveAllListeners();
            DisableUpgradeUI(upgradeOption);
        }
    }

    public void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOptions();
        ApplyUpgradeOptions();
    }

    void DisableUpgradeUI(UpgradeUI ui)
    {
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(false);
    }

    void EnableUpgradeUI(UpgradeUI ui)
    {
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(true);
    }
}
