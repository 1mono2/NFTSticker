using MoralisUnity;
using MoralisUnity.Platform.Objects;
using MoralisUnity.Web3Api.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CongratView : MonoBehaviour
{
        [Header("Texts")]
        [SerializeField]
        private Text addressText;

        [SerializeField]
        private Text balanceText;

        [Header("Buttons")]
        [SerializeField]
        private Button backButton = null;

        // Start is called before the first frame update
        async void Start()
        {
            if (addressText == null)
            {
                Debug.LogError("Address Text not set.");
                return;
            }

            if (balanceText == null)
            {
                Debug.LogError("Balance Text not set.");
                return;
            }

            if (backButton == null)
            {
                Debug.LogError("Back Button not set.");
                return;
            }

            
                // Display User's wallet address.
                addressText.text = Web3WalletData.Instance.PublicKey;

                // Retrieve the user's native balance;
                NativeBalance balanceResponse = await Moralis.Web3Api.Account.GetNativeBalance(Web3WalletData.Instance.PublicKey, Web3WalletData.Instance.ChainEntry.EnumValue);

                double balance = 0.0;
                float decimals =  Web3WalletData.Instance.ChainEntry.Decimals * 1.0f;
                string sym =  Web3WalletData.Instance.ChainEntry.Symbol;

                // Make sure a response to the balance request was received. The 
                // IsNullOrWhitespace check may not be necessary ...
                if (balanceResponse != null && !string.IsNullOrWhiteSpace(balanceResponse.Balance))
                {
                    double.TryParse(balanceResponse.Balance, out balance);
                }

                // Display native token amount token in fractions of token.
                // NOTE: May be better to link this to chain since some tokens may have
                // more than 18 significant figures.
                balanceText.text = $"{(balance / (double)Mathf.Pow(10.0f, decimals)):0.####} {sym}";
            
        }

        private string FormatUserAddressForDisplay(string addr)
        {
            string resp = addr;

            if (resp.Length > 13)
            {
                resp = $"{resp.Substring(0, 6)}...{resp.Substring(resp.Length - 4, 4)}";
            }

            return resp;
        }
}
