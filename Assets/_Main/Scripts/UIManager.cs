using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject authenticationKitObject = null;

    [SerializeField]
    private CongratView congratulationUiObject = null;

    [SerializeField]
    private GameObject fireworksObject = null;

    private Authentication auth = null;
    
    [SerializeField]
    private Canvas NFTCanvas = null;

    private void Start()
    {
        auth = authenticationKitObject.GetComponent<Authentication>();
    }
    public void Authentication_OnConnect()
    {
        authenticationKitObject.SetActive(false);
        congratulationUiObject.gameObject.SetActive(true);
        congratulationUiObject.Initialize();
        fireworksObject.SetActive(true);
        var fireworks = fireworksObject.GetComponentsInChildren<ParticleSystem>();
        foreach (var firework in fireworks)
        {
            firework.Play();
        }
        NFTCanvas.gameObject.SetActive(true);
    }

    public void LogoutButton_OnClicked()
    {
        // Logout the Moralis User.
        auth.Disconnect();

        authenticationKitObject.SetActive(true);
        congratulationUiObject.gameObject.SetActive(false);
        fireworksObject.SetActive(false);
        NFTCanvas.gameObject.SetActive(false);
    }
}
