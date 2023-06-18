using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using MoralisUnity.Web3Api.Interfaces;
using MoralisUnity.Web3Api.Models;
using Newtonsoft.Json;
using MoralisClient = MoralisUnity.Web3Api.MoralisClient;
using Nethereum.Web3;

public class GachaNFTContract
{
    private static string Fun_Deposit = "deposit";
    private static string Fun_GetOffer = "getOffer";
    private static string Fun_GetOffersByOwner = "getOffersByOwner";
    private static string Fun_Withdraw = "withdraw";
    private static string Fun_SelectRandomNFT = "selectRandomNFT";
    private static string Fun_Purchase = "purchase";
    private static string Fun_OnERC721Received = "onERC721Received";

    private const string ContractAddress = "0x5c156dBdAcFe756D1c28f9DA4d9927f5fbdaA92e";
    private static readonly List<string> Abis = new List<string>()
    {
        @"{""inputs"":[],""stateMutability"":""nonpayable"",""type"":""constructor""}",
        "{\"inputs\":[{\"internalType\":\"address\",\"name\":\"nftContract\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"price\",\"type\":\"uint256\"}],\"name\":\"deposit\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}",
        "{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"offerId\",\"type\":\"uint256\"}],\"name\":\"getOffer\",\"outputs\":[{\"components\":[{\"internalType\":\"address\",\"name\":\"nftContract\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"price\",\"type\":\"uint256\"}],\"internalType\":\"struct GachaNFT.Offer\",\"name\":\"\",\"type\":\"tuple\"}],\"stateMutability\":\"view\",\"type\":\"function\"}",
        "{\"inputs\":[],\"name\":\"getOffersByOwner\",\"outputs\":[{\"components\":[{\"internalType\":\"address\",\"name\":\"nftContract\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"price\",\"type\":\"uint256\"}],\"internalType\":\"struct GachaNFT.Offer[]\",\"name\":\"\",\"type\":\"tuple[]\"}],\"stateMutability\":\"view\",\"type\":\"function\"}",
        "{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"offers\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"nftContract\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"price\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"}",
        "{\"inputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"\",\"type\":\"bytes\"}],\"name\":\"onERC721Received\",\"outputs\":[{\"internalType\":\"bytes4\",\"name\":\"\",\"type\":\"bytes4\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}",
        "{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"offerId\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"maxPrice\",\"type\":\"uint256\"}],\"name\":\"purchase\",\"outputs\":[],\"stateMutability\":\"payable\",\"type\":\"function\"}",
        "{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"maxPrice\",\"type\":\"uint256\"}],\"name\":\"selectRandomNFT\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"}",
        "{\"inputs\":[{\"internalType\":\"address\",\"name\":\"nftContractAddress\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"tokenId\",\"type\":\"uint256\"}],\"name\":\"withdraw\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}"
    };
    private readonly ChainList _chain;
    private readonly INativeApi _nativeApi;

    public GachaNFTContract(ChainList chain, INativeApi nativeApi)
    {
        this._chain = chain;
        this._nativeApi = nativeApi;

    }
    
    
    public async void Deposit(string contractAddress, string tokenId, string price)
    {
        var runContractDto = CreateAbi();
        string prams = $"nftContract:{contractAddress}, tokenId:{tokenId}, price:{price}";
        runContractDto.Params = JObject.Parse(prams);
        
        string stResult =
            await _nativeApi.RunContractFunction<string>(ContractAddress, "deposit", runContractDto, _chain);
        Console.WriteLine(stResult);
    }
    
    public async void Withdraw(string contractAddress, string tokenId)
    {
        var runContractDto = CreateAbi();
        string prams = $"nftContract:{contractAddress}, tokenId:{tokenId}";
        runContractDto.Params = JObject.Parse(prams);
        
        string stResult =
            await _nativeApi.RunContractFunction<string>(ContractAddress, "withdraw", runContractDto, _chain);
        Console.WriteLine(stResult);
    }

    public async void GetOffer(string offerId)
    {
        var runContractDto = CreateAbi();
        string prams = $"{{offerId:{offerId}}}";
        runContractDto.Params = JObject.Parse(prams);
        
        string stResult =
            await _nativeApi.RunContractFunction<string>(ContractAddress, "getOffer", runContractDto, _chain);
        Console.WriteLine(stResult);
        Offer result = JsonConvert.DeserializeObject<Offer>(stResult);
        Console.WriteLine(result.price);
    }
    
    public async void GetOffersByOwner()
    {
        var runContractDto = CreateAbi();
        string stResult =
            await _nativeApi.RunContractFunction<string>(ContractAddress, "getOffersByOwner", runContractDto, _chain);
        Console.WriteLine(stResult);
        var result = JsonConvert.DeserializeObject<List<Offer>>(stResult);
        Console.WriteLine(result.Count);
    }

    public async void SelectRandomNft(float maxPriceEth)
    {
        var maxPriceWei = Web3.Convert.ToWei(maxPriceEth);
        var runContractDto = CreateAbi();
        string prams = $"{{\"maxPrice\":\"{maxPriceWei}\"}}";
        runContractDto.Params = JObject.Parse(prams);
        string stResult =
            await _nativeApi.RunContractFunction<string>(ContractAddress, "selectRandomNFT", runContractDto, _chain);
        Console.WriteLine(stResult);
    }

    public async void Purchase(string offerId, float maxPriceEth)
    {
        
        var maxPriceWei = Web3.Convert.ToWei(maxPriceEth);
        var runContractDto = CreateAbi();
        string prams = $"offerId:{offerId}, maxPrice:{maxPriceWei}";
        runContractDto.Params = JObject.Parse(prams);
        
        string stResult =
            await _nativeApi.RunContractFunction<string>(ContractAddress, "purchase", runContractDto, _chain);
        Console.WriteLine(stResult);
    }

    private RunContractDto CreateAbi()
    {
        var parsedAbi = new List<JObject>();
        foreach (var abi in Abis)
        {
            parsedAbi.Add(JObject.Parse(abi));
        }
        return new RunContractDto() { Abi = parsedAbi };
    }

}
