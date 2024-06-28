using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public int ammoAmount = 200;
    public EAmmoType ammoType;
    
    
    public enum EAmmoType
    {
        PistolAmmo,
        RifleAmmo
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
