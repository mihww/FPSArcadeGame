using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int HP = 100;
    public GameObject bloodyScreen;

    public TextMeshProUGUI playerHealthUI;
    public GameObject gameOverUI;

    private bool isTakingDamage = false;

    public bool isDead = false;



    public void TakeDamage(int damage)
    {
        if (isTakingDamage) return;

        isTakingDamage = true;

        HP -= damage;
        if (HP <= 0)
        {
            print("Player is dead");
            PlayerDead();
            isDead = true;


        }
        else
        {
            print("Player Hit");
            StartCoroutine(BloodyScreenEffect());
            playerHealthUI.text = $"Health: {HP}";
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurt);
        }

        StartCoroutine(ResetDamageFlag());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        {
            if (isDead == false)
            {
                TakeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
            }
        }
    }

    /* --------------------------------------------------------------------------------------------------- */
    private void PlayerDead()
    {
        SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDeath);

        SoundManager.Instance.playerChannel.clip = SoundManager.Instance.gameOverMusic;
        SoundManager.Instance.playerChannel.Play();


        GetComponent<MouseMovement>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;

        if (GetComponentInChildren<Weapon>() != null)
        {
            GetComponentInChildren<Weapon>().gameObject.SetActive(false);
        }
        
        if (GetComponentInChildren<Animator>() != null)
        {   
            // Dying anim
            GetComponentInChildren<Animator>().enabled = true;
        }

        DisableHUD();
        playerHealthUI.gameObject.SetActive(false);

        GetComponent<ScreenFader>().StartFade();
        StartCoroutine(ShowGameOverUI());
    }
    private void DisableHUD()
    {
        HUDManager.Instance.lethalAmountUI.gameObject.SetActive(false);
        HUDManager.Instance.tacticalAmountUI.gameObject.SetActive(false);
        HUDManager.Instance.ammoTypeUI.gameObject.SetActive(false);
        HUDManager.Instance.magazineAmmoUI.gameObject.SetActive(false);
        HUDManager.Instance.totalAmmoUI.gameObject.SetActive(false);
        HUDManager.Instance.activeWeaponUI.gameObject.SetActive(false);
        HUDManager.Instance.inactiveWeaponUI.gameObject.SetActive(false);
        HUDManager.Instance.lethalUI.gameObject.SetActive(false);
        HUDManager.Instance.tacticalUI.gameObject.SetActive(false);
        HUDManager.Instance.crosshair.SetActive(false);
    }

    #region IEnumerators
    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(1f);
        gameOverUI.SetActive(true);

        int waveSurvived = GlobalReferences.Instance.waveNumber;
        if (waveSurvived - 1 > SaveLoadManager.Instance.LoadHighScore())
        {
            SaveLoadManager.Instance.SaveHighScore(waveSurvived - 1);
        }

        StartCoroutine(ReturnToMainMenu());
    }

    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(3f);
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator ResetDamageFlag()
    {
        yield return new WaitForSeconds(0.3f);
        isTakingDamage = false;
    }

    private IEnumerator BloodyScreenEffect()
    {
        if (bloodyScreen.activeInHierarchy == false)
        {
            bloodyScreen.SetActive(true);
            playerHealthUI.color = new Color(150f / 255f, 41f / 255f, 41f / 255f);
        }

        var image = bloodyScreen.GetComponentInChildren<Image>();

        // Set the initial alpha value to 1 (fully visible).
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate the new alpha value using Lerp.
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            // Update the color with the new alpha value.
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            // Increment the elapsed time.
            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame.
        }


        if (bloodyScreen.activeInHierarchy)
        {
            bloodyScreen.SetActive(false);
            playerHealthUI.color = Color.white;

        }
    }
    #endregion

}
