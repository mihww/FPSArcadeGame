using UnityEngine;

public class IconManager : MonoBehaviour
{
    public static Sprite Pistol1911_Weapon;
    public static Sprite M16_Weapon;

    public static Sprite Pistol_Ammo;
    public static Sprite Rifle_Ammo;
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
    }
}