using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Unity;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine.Events;
using MoralisUnity.Sdk.Exceptions;
using MoralisUnity.Kits.AuthenticationKit;
using TMPro;
using DG.Tweening;


/// <summary>
/// See <see cref="Authentication"/> comments for a feature overview.
///
/// This <see cref="AuthenticationView"/> populates and observes UI elements.
/// 
/// </summary>
[Serializable]
public class AuthenticationView : MonoBehaviour
{
    //  Properties ------------------------------------

    //  Fields ----------------------------------------
    [Header("Kit")] [SerializeField] private Authentication _authentication = null;

    [Header("Platforms")] [SerializeField] private GameObject _iosPlatform = null;

    [SerializeField] private GameObject _walletConnectPlatform = null;
    
    [Header("Canvas")] [SerializeField] private CanvasGroup _canvasGroup = null;

    [Header("Buttons")] [SerializeField] private Button _connectButton = null;

    [SerializeField] private Button _disconnectButton = null;

    [SerializeField] private Button _retryButton = null;

    [Header("Other")] [SerializeField] private TextMeshProUGUI _statusText = null;
    [SerializeField] GameObject _loadingIndicator = null;
    [SerializeField] GameObject _checkmark = null;

    //  Unity Methods ---------------------------------
    protected void Start()
    {
        _authentication.OnStateChanged.AddListener(AuthenticationKit_OnStateChanged);

        // Local scope is 'late', so rebroadcast the state
        AuthenticationKit_OnStateChanged(_authentication.State);

        _connectButton.onClick.AddListener(ConnectButton_OnClicked);
        _disconnectButton.onClick.AddListener(DisconnectButton_OnClicked);
        _retryButton.onClick.AddListener(RetryButton_OnClicked);
    }

    protected void OnValidate()
    {
        // This works at edit time and thus at runtime
        // Note: At edit time the image may be NOT active (to unclutter UI). That is ok.
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            return;
        }

        switch (_authentication.AuthenticationKitPlatform)
        {
            case AuthenticationKitPlatform.Android:
            case AuthenticationKitPlatform.iOS:

                switch (_authentication.State)
                {
                    case AuthenticationKitState.WalletConnecting:
                    case AuthenticationKitState.WalletSigning:
                        // Show Button "Retry" after 2 seconds
                        StartCoroutine(EnableAfterSeconds(_retryButton, true, 1));
                        break;
                }

                break;
        }
    }

    private void SetActiveUIJustPlatforms(bool isActive)
    {
        // 1. Hide everything...
        _iosPlatform.SetActive(false);
        _walletConnectPlatform.SetActive(false);

        if (!isActive)
        {
            return;
        }

        // 2. Show something...
        switch (_authentication.AuthenticationKitPlatform)
        {
            case AuthenticationKitPlatform.Android:
                break;
            case AuthenticationKitPlatform.iOS:
                _iosPlatform.SetActive(true);
                break;
            case AuthenticationKitPlatform.WebGL:
                break;
            case AuthenticationKitPlatform.WalletConnect:
                _walletConnectPlatform.SetActive(true);
                break;
            default:
                SwitchDefaultException.Throw(_authentication.AuthenticationKitPlatform);
                break;
        }
    }


    private void SetActiveUIAllParts(bool isActive)
    {
        // Platforms
        SetActiveUIJustPlatforms(isActive);

        // Buttons
        _connectButton.gameObject.SetActive(isActive);
        _disconnectButton.gameObject.SetActive(isActive);
        _retryButton.gameObject.SetActive(isActive);

        // Texts
        _statusText.gameObject.SetActive(isActive);
        _loadingIndicator.SetActive(isActive);

        // Keep as default. Expected, set again after method call.
        //_statusText.text = "Status";
    }


    //  Event Handlers --------------------------------
    private void ConnectButton_OnClicked()
    {
        _authentication.Connect();
    }


    private void DisconnectButton_OnClicked()
    {
        _authentication.Disconnect();
    }


    private void RetryButton_OnClicked()
    {
        // Disable the button to prevent multiple retries
        _retryButton.interactable = false;
        _authentication.Retry();
    }


    private async void AuthenticationKit_OnStateChanged(AuthenticationKitState authenticationKitState)
    {
        switch (authenticationKitState)
        {
            case AuthenticationKitState.None:
                break;
            case AuthenticationKitState.PreInitialized:

                SetActiveUIAllParts(false);

                break;
            case AuthenticationKitState.Initializing:
                break;
            case AuthenticationKitState.Initialized:

                // Show Button "Connect"
                SetActiveUIAllParts(false);
                _connectButton.gameObject.SetActive(true);

                break;
            case AuthenticationKitState.WalletConnecting:

                // Show QR (or platform specific stuff)
                SetActiveUIAllParts(false);
                SetActiveUIJustPlatforms(true);

                // Show custom more text for SOME platform(s)
                switch (_authentication.AuthenticationKitPlatform)
                {
                    case AuthenticationKitPlatform.iOS:
                        // Hide statusText first will be turned on by WalletConnect when a user click on a wallet
                        //_statusText.gameObject.SetActive(false);
                        //_statusText.text = "Connecting With Your Wallet";
                        // TODO show message if there is no wallet installed on the device
                        _loadingIndicator.SetActive(true);
                        break;
                    case AuthenticationKitPlatform.Android:
                        //_statusText.gameObject.SetActive(true);
                        //_statusText.text = "Connecting With Your Wallet";

                        // TODO show message if there is no wallet installed on the device
                        _loadingIndicator.SetActive(true);
                        break;
                    case AuthenticationKitPlatform.WebGL:
                        _statusText.gameObject.SetActive(true);
                        if (!Application.isEditor)
                        {
                            //_statusText.text = "Connecting With Your Wallet";
                            _loadingIndicator.SetActive(true);
                        }
                        else
                        {
                            // TODO Add WalletConnect option for easy play testing when developing for WebGL
                            _statusText.text = "Please build your WebGL project to connect";
                        }

                        break;
                    case AuthenticationKitPlatform.WalletConnect:
                        // Hide status text because the QR has it own status text
                        _statusText.gameObject.SetActive(false);
                        _loadingIndicator.SetActive(true);
                        break;
                    default:
                        //Do nothing for other states
                        break;
                }

                break;
            case AuthenticationKitState.WalletConnected:
                SetActiveUIAllParts(false);
                break;
            case AuthenticationKitState.WalletSigning:
                SetActiveUIAllParts(false);
                //_statusText.gameObject.SetActive(true);
                //_statusText.text = "Signing With Your Wallet";
                _loadingIndicator.SetActive(true);
                break;
            case AuthenticationKitState.WalletSigned:
                SetActiveUIAllParts(false);
                // hide authentication UI
                _checkmark.SetActive(true);
                await UniTask.Delay(1500);
                _canvasGroup.DOFade(0, 1f).OnComplete(() => { _canvasGroup.gameObject.SetActive(false); });
                break;
            case AuthenticationKitState.MoralisLoggingIn:
                // No UI changes here
                break;
            case AuthenticationKitState.MoralisLoggedIn:
                // Show Button "Disconnect"
                SetActiveUIAllParts(false);
                //_disconnectButton.gameObject.SetActive(true);
                break;
            case AuthenticationKitState.Disconnecting:
                // No UI changes here
                break;
            case AuthenticationKitState.Disconnected:
                // No UI changes here
                break;
            default:
                SwitchDefaultException.Throw(authenticationKitState);
                break;
        }
    }

    public async void OnWalletSigned()
    {
        SetActiveUIAllParts(false);
        // hide authentication UI
        _checkmark.SetActive(true);
        await UniTask.Delay(1500);
        _canvasGroup.DOFade(0, 1f).OnComplete(() => { _canvasGroup.gameObject.SetActive(false); });
    } 

    IEnumerator EnableAfterSeconds(Button button, bool isActive = false, int seconds = 0)
    {
        AuthenticationKitState rememberState = _authentication.State;

        // yield on a new YieldInstruction that waits for X seconds.
        yield return new WaitForSeconds(seconds);

        // Only change if the state hasn't changed
        if (rememberState == _authentication.State)
        {
            button.interactable = true;
            button.gameObject.SetActive(isActive);
        }
    }
}