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
using Attribute = MoralisUnity.Web3Api.Models.Attribute;
using Metadata = MoralisUnity.Web3Api.Models.Metadata;
using Newtonsoft.Json.Linq;

public class ExecuteContract : MonoBehaviour
{
    // Start is called before the first frame update
    private const string ApiKey = "zb4sYWpTvVBbIoMHiuoAh4ejbEJgtwoAcRqWwVbrnY1NSgMIg6GBWrCS89ATvBQE";
    private string _publicKey = "0x6d8b494901D0D3893646337887Adbe5c226A1985";
    private ChainList _chain = ChainList.eth;

    private string _contractAddress = "0x6B175474E89094C44Da98b954EedeAC495271d0F";

    IAccountApi _accountApi;
    INativeApi _nativeApi;
    ITokenApi _tokenApi;

    public async void TestExecuteContract()
    {
        //_publicKey = Web3WalletData.Instance.PublicKey;
        //var chainEntry = Web3WalletData.Instance.ChainEntry;
        //_chain = chainEntry.EnumValue;

        MoralisClient.Initialize(true, ApiKey);
        _accountApi = MoralisClient.Web3Api.Account;
        NativeBalance balance = await _accountApi.GetNativeBalance(_publicKey, _chain);
        Debug.Log(balance.Balance);

        _nativeApi = MoralisClient.Web3Api.Native;

        var block = await _nativeApi.GetBlock("17194417", _chain);
        Debug.Log("Block: " + block.Number);

        string functionName = "totalSupply";
        RunContractDto runContractDto = new();
        string abiString =
            "{\"inputs\":[],\"name\":\"totalSupply\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"}";
        var jObject = JObject.Parse(abiString);
        runContractDto.Abi = new List<JObject>(){jObject};
        string st_totalSupply =
            await _nativeApi.RunContractFunction<string>(_contractAddress, functionName, runContractDto, _chain);
        Debug.Log("totalSupply: " + st_totalSupply);
    }

    private void Start()
    {
        TestExecuteContract();
    }
}