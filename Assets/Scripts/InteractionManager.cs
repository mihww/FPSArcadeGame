using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour // Singleton
{
    public static InteractionManager Instance { get; private set; }

    public Weapon hoveredWeapon = null;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;

            if (objectHitByRaycast.GetComponent<Weapon>() && objectHitByRaycast.GetComponent<Weapon>().isActiveWeapon==false)
            {
                hoveredWeapon = objectHitByRaycast.GetComponent<Weapon>();
                Highlight(hoveredWeapon);

                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupWeapon(objectHitByRaycast.gameObject);
                    Unhighlight(hoveredWeapon);
                    //AmmoManager.Instance.UpdateAmmoDisplay();
                }
            }
            else
            {
                if (hoveredWeapon)
                {
                    Unhighlight(hoveredWeapon);
                }
            }
        }


    }

    /* --------------------------------------------------------------------------------------------------- */

    #region Handling Highlighting for code readability
    private void Highlight(Weapon obj)
    {
        obj.GetComponent<Outline>().enabled = true;
    }
    private void Unhighlight(Weapon obj)
    {
        obj.GetComponent<Outline>().enabled = false;
    }
    #endregion

}

