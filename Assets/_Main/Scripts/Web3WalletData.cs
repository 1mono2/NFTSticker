using System.Collections;
using System.Collections.Generic;
using MoralisUnity;
using MoralisUnity.Web3Api.Models;
using UnityEngine;

public class Web3WalletData : Singleton<Web3WalletData>
{
    
        public string PublicKey { get; private set; }
        public ChainEntry ChainEntry { get; private set; }
        public bool NFT { get; private set; }


        public Web3WalletData()
        {
            
        }
        
        public void SetPublicKey(string publicKey)
        {
            PublicKey = publicKey;
        }
        
        public void SetChainEntry(ChainEntry chainEntry)
        {
            ChainEntry = chainEntry;
        }
}
