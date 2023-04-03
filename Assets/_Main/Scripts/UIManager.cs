using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject authenticationKitObject = null;

    [SerializeField]
    private GameObject congratulationUiObject = null;

    [SerializeField]
    private GameObject fireworksObject = null;

    private Authentication auth = null;

    private void Start()
    {
        auth = authenticationKitObject.GetComponent<Authentication>();
    }
    public void Authentication_OnConnect()
    {
        authenticationKitObject.SetActive(false);
        congratulationUiObject.SetActive(true);
        fireworksObject.SetActive(true);
        var fireworks = fireworksObject.GetComponentsInChildren<ParticleSystem>();
        foreach (var firework in fireworks)
        {
            firework.Play();
        }
    }

    public void LogoutButton_OnClicked()
    {
        // Logout the Moralis User.
        auth.Disconnect();

        authenticationKitObject.SetActive(true);
        congratulationUiObject.SetActive(false);
        fireworksObject.SetActive(false);
    }
}
