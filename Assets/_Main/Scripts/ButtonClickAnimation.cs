using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClickAnimation : MonoBehaviour
{
    
    [SerializeField] private Ease _ease = Ease.Unset;
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private float _scale = 0.1f;
    
    private RectTransform _rectTransform;
    private Button _button;
    
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(_scale, _duration).SetEase(_ease).SetRelative(true));
        sequence.Append(transform.DOScale(-_scale, _duration).SetEase(_ease).SetRelative(true));
    }


}
