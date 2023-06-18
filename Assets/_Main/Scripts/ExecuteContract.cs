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
using Newtonsoft.Json;
using Unity.VisualScripting;
using MoralisClient = MoralisUnity.Web3Api.MoralisClient;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json.Linq;

public class ExecuteContract : MonoBehaviour
{
    // Start is called before the first frame update
    private const string MoralisApiKey = "zb4sYWpTvVBbIoMHiuoAh4ejbEJgtwoAcRqWwVbrnY1NSgMIg6GBWrCS89ATvBQE";
    private string _myPublicAddress = "0x2410B80E6FaF3f56774c5F158bEC582f7bE016c4";
    private ChainList _chain = ChainList.sepolia;
    

    IAccountApi _accountApi;
    INativeApi _nativeApi;
    ITokenApi _tokenApi;
    
    public async void TestExecuteContract()
    {
        //_myPublicAddress = Web3WalletData.Instance.PublicKey;
        //var chainEntry = Web3WalletData.Instance.ChainEntry;
        //_chain = chainEntry.EnumValue;

        MoralisClient.Initialize(true, MoralisApiKey);
        _accountApi = MoralisClient.Web3Api.Account;
        NativeBalance balance = await _accountApi.GetNativeBalance(_myPublicAddress, _chain);
        Debug.Log(balance.Balance + " "  + "ETH");

        _nativeApi = MoralisClient.Web3Api.Native;

        //var block = await _nativeApi.GetBlock("17194417", _chain);
        //Debug.Log("Block: " + block.Number);

        // A function that calls the contract doesn't work because of Moralis' bugs 
        //var gachaNftContract = new GachaNFTContract(_chain, _nativeApi);
        //gachaNftContract.SelectRandomNft(20);
        //gachaNftContract.GetOffersByOwner();
    }

    private void Start()
    {
        TestExecuteContract();
    }
    
}