using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using MoralisUnity;
using UnityEngine;
using MoralisUnity.Web3Api;
using MoralisUnity.Web3Api.Api;
using MoralisUnity.Web3Api.Client;
using MoralisUnity.Web3Api.Interfaces;
using MoralisUnity.Web3Api.Models;
using Unity.VisualScripting;
using MoralisClient = MoralisUnity.Web3Api.MoralisClient;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using Attribute = MoralisUnity.Web3Api.Models.Attribute;
using Metadata = MoralisUnity.Web3Api.Models.Metadata;


public class GetNFTImage : MonoBehaviour
{
    // Start is called before the first frame update
    private const string ApiKey = "zb4sYWpTvVBbIoMHiuoAh4ejbEJgtwoAcRqWwVbrnY1NSgMIg6GBWrCS89ATvBQE";
    private string _publicKey = "0x6d8b494901D0D3893646337887Adbe5c226A1985";
    private ChainList _chain = ChainList.eth;
    
    [SerializeField] private GameObject _attributeTemplate;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private Texture2D _defaultImage;

    [SerializeField] private GameObject _gridContainer;
    [SerializeField] private GameObject _nftContainerTemplate;
    
    IAccountApi _accountApi;

    public async void GetNftImage()
    {
        //_publicKey = Web3WalletData.Instance.PublicKey;
        //var chainEntry = Web3WalletData.Instance.ChainEntry;
        //_chain = chainEntry.EnumValue;

        MoralisClient.Initialize(true, ApiKey);
        _accountApi = MoralisClient.Web3Api.Account;
        
        GetBalance();

        // Get NFTs from the account using Moralis API 
        var nfts = await _accountApi.GetNFTs(_publicKey, _chain);
        if (nfts.Result.Count == 0)
        {
            Debug.LogError("No NFTs found");
            return;
        }
        
        for (var i = 0; i < nfts.Result.Count; i ++)
        {
            if (i > 10 ) break;
            try
            {
                // Get the tokenUri from the first NFT
                var tokenUri = nfts.Result[i].TokenUri;
                string imageApi = await CallApi(tokenUri);
                Metadata metadata = (Metadata)ApiClient.Deserialize(imageApi, typeof(Metadata));
                
                // Get texture from the image url
                var imageURL = metadata.Image;
                // Coordinate url
                if (imageURL.StartsWith("ipfs://"))
                {
                    imageURL = imageURL.Replace("ipfs://", "https://ipfs.io/ipfs/");
                }
                var texture = await GetTextureFromUrl(imageURL);

                var nftContainer =  Instantiate(_nftContainerTemplate, _gridContainer.transform);
                var nftImage = nftContainer.GetComponentInChildren<Image>();
                var nftName = nftContainer.GetComponentInChildren<TextMeshProUGUI>();
                if (texture == null)
                {
                    nftImage.sprite = Sprite.Create(_defaultImage, new Rect(0.0f, 0.0f, _defaultImage.width, _defaultImage.height), new Vector2(0.5f, 0.5f), 100.0f);
                }
                else
                {
                    nftImage.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                    nftName.text = metadata.Name;
                }
                nftContainer.SetActive(true);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
            
            Debug.Log("Finished " + (i +1) +" / " + nfts.Result.Count);
        }
        
    }
    
    

    public static async UniTask<string> CallApi(string uri)
    {
        using var webRequest = UnityWebRequest.Get(uri);
        webRequest.method = "GET";
        try
        {
            await webRequest.SendWebRequest();
        }
        catch (Exception exp)
        {
            Debug.LogError($"Error: {exp.Message}");
        }
            
        string responseText = webRequest.result == UnityWebRequest.Result.ConnectionError ? webRequest.error : webRequest.downloadHandler.text;
        return responseText;
    }
    
    // A function that takes a string type as an URL argument and return a Unity Texture2D type using Task
    public static async Task<Texture2D> GetTextureFromUrl(string url)
    {
        // Create a web request object with the URL
        using UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url);
        // Send the request and wait for a response asynchronously
        await webRequest.SendWebRequest();

        // Check if the request was successful
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            // Get the downloaded texture as a Texture2D object and return it
            return DownloadHandlerTexture.GetContent(webRequest);
        }
        else
        {
            // Throw an exception with the error message
            throw new System.Exception(webRequest.error);
        }
    }

    public void CreateAttributeText(List<Attribute> attributes)
    {
        if (attributes == null)
        {
            Debug.LogError("No attributes found");
            return;
        }
        foreach (var attribute in attributes)
        {
            var attributeText = Instantiate(_attributeTemplate, _canvas.transform);
            attributeText.SetActive(true);
            TextMeshProUGUI[] texts = attributeText.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = attribute.Key;
            texts[1].text = attribute.Value;
        }
    }

    private async void GetBalance()
    {
        NativeBalance balance = await _accountApi.GetNativeBalance(_publicKey, _chain);
        Debug.Log(balance.Balance + " "  + "ETH");
    }
}