using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class InteractionManager : MonoBehaviour // Singleton
{
    public static InteractionManager Instance { get; private set; }

    public Weapon hoveredWeapon = null;
    public AmmoBox hoveredAmmoBox = null;
    public Throwable hoveredThrowable = null;

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
        float maxInteractionDistance = 5f;

        if (Physics.Raycast(ray, out hit, maxInteractionDistance))
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;

            HandleWeaponInteraction(objectHitByRaycast);
            HandleAmmoInteraction(objectHitByRaycast);
            HandleThrowableInteraction(objectHitByRaycast);
        }
    }

    /* --------------------------------------------------------------------------------------------------- */

    #region Handling Highlighting
    private void Highlight<T>(T obj) where T : Component
    {
        obj.GetComponent<Outline>().enabled = true;
    }
    private void Unhighlight<T>(T obj) where T : Component
    {
        obj.GetComponent<Outline>().enabled = false;
    }
    #endregion

    #region Handling Interactions
    private void HandleWeaponInteraction(GameObject objectHitByRaycast)
    {
        if (objectHitByRaycast.GetComponent<Weapon>() && objectHitByRaycast.GetComponent<Weapon>().isActiveWeapon == false)
        {
            hoveredWeapon = objectHitByRaycast.GetComponent<Weapon>();
            Highlight(hoveredWeapon);

            if (Input.GetKeyDown(KeyCode.F))
            {
                WeaponManager.Instance.PickupWeapon(objectHitByRaycast.gameObject);
                Unhighlight(hoveredWeapon);
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

    private void HandleAmmoInteraction(GameObject objectHitByRaycast)
    {
        if (objectHitByRaycast.GetComponent<AmmoBox>())
        {
            hoveredAmmoBox = objectHitByRaycast.GetComponent<AmmoBox>();
            Highlight(hoveredAmmoBox);

            if (Input.GetKeyDown(KeyCode.F))
            {
                WeaponManager.Instance.PickupAmmo(hoveredAmmoBox);
                Destroy(objectHitByRaycast.gameObject);
            }
        }
        else
        {
            if (hoveredAmmoBox)
            {
                Unhighlight(hoveredAmmoBox);
            }
        }
    }

    private void HandleThrowableInteraction(GameObject objectHitByRaycast)
    {
        if (objectHitByRaycast.GetComponent<Throwable>())
        {
            hoveredThrowable = objectHitByRaycast.GetComponent<Throwable>();
            Highlight(hoveredThrowable);

            if (Input.GetKeyDown(KeyCode.F))
            {
                WeaponManager.Instance.PickupThrowable(hoveredThrowable);
            }
        }
        else
        {
            if (hoveredThrowable)
            {
                Unhighlight(hoveredThrowable);
            }
        }
    }
    #endregion




}

