using UnityEngine;

public class IconManager : MonoBehaviour
{
    [Header("Weapon Icons")]
    public static Sprite Pistol1911_Weapon;
    public static Sprite M16_Weapon;

    [Header("Weapon Ammo Icons")]
    public static Sprite Pistol_Ammo;
    public static Sprite Rifle_Ammo;

    [Header("Lethal Icons")]
    public static Sprite Grenade;

    [Header("Tactical Icons")]
    public static Sprite Smoke_Grenade;


    public static IconManager Instance { get; set; }

    private void Awake()    
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        Pistol1911_Weapon = Resources.Load<GameObject>("Pistol1911_Weapon").GetComponent<SpriteRenderer>().sprite;
        M16_Weapon = Resources.Load<GameObject>("M16_Weapon").GetComponent<SpriteRenderer>().sprite;

        Pistol_Ammo = Resources.Load<GameObject>("Pistol_Ammo").GetComponent<SpriteRenderer>().sprite;
        Rifle_Ammo = Resources.Load<GameObject>("Rifle_Ammo").GetComponent<SpriteRenderer>().sprite;

        Grenade = Resources.Load<GameObject>("Grenade").GetComponent<SpriteRenderer>().sprite;
        Smoke_Grenade = Resources.Load<GameObject>("Smoke_Grenade").GetComponent<SpriteRenderer>().sprite;
    }
}