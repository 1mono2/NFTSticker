
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine.UI;

public class LoadNftUIManager : MonoBehaviour
{
    [Header("Progress bar")]
    [SerializeField] private RectTransform _progressBar;
    [SerializeField] private Image _loadingIndicator;
    [SerializeField] private TextMeshProUGUI _percentageText;

    [Header("Load NFT Button")]
    [SerializeField] private GameObject _loadNftButton;
    
    [SerializeField] private LoadNFT _loadNft;

    private Tween _loadingTween;
    private Sequence _loadingSequence;

    public void ActivateProgressBar()
    {
        // Pop up animation
        _progressBar.gameObject.SetActive(true);
        _progressBar.DOScale(Vector3.zero, 0f);
        _progressBar.DOScale(Vector3.one, 0.4f).SetEase(Ease.InCubic).SetRelative(true);
        
        _loadNftButton.transform.DOScale(Vector3.zero, 0.4f).SetEase(Ease.InBack).OnComplete(() => _loadNftButton.SetActive(false));
        
        _loadNft.LoadingProgressPercentage.Subscribe(ProgressLoading).AddTo(this);
        _loadingIndicator.ObserveEveryValueChanged(x => x.fillAmount)
            .Subscribe(fillAmount =>
            {
                _percentageText.text = (fillAmount * 100).ToString("F0") + "%";
            }).AddTo(this);
        _loadingIndicator.ObserveEveryValueChanged(x => x.fillAmount)
            .Subscribe(fillAmount =>
            {
                // Animation
                if (fillAmount > 0.99f)
                {
                    _progressBar.DOScale(Vector3.zero, 0.4f).SetEase(Ease.InCubic);
                }
            }).AddTo(this);
    }

    public void ProgressLoading(float percentage)
    {
        _loadingSequence?.Kill();
        _loadingSequence = DOTween.Sequence();
        // _loadingTween = DOTween.To(() => _loadingIndicator.fillAmount, x => _loadingIndicator.fillAmount = x, percentage, 1f)
        //     .SetEase(Ease.InOutQuad);

        _loadingSequence.Append(DOTween.To(() => _loadingIndicator.fillAmount, x => _loadingIndicator.fillAmount = x, percentage, 1f)
            .SetEase(Ease.InOutQuad));
        _loadingSequence.Append(DOTween
            .To(() => _loadingIndicator.fillAmount, x => _loadingIndicator.fillAmount = x, 1, 120f)
            .SetEase(Ease.Linear));
        _loadingSequence.Play();
    }
    
    public void StopLoading()
    {
        // アニメーションを止める
        _loadingTween.Kill();

        // fillAmountの値をリセット
        _loadingIndicator.fillAmount = 0;
    }
}
