using System;
using System.Collections;
using System.Collections.Generic;
using MoralisUnity.Web3Api.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Attribute = System.Attribute;
using DG.Tweening;

public class NFTAsset : MonoBehaviour
{
    private Sprite _sprite;
    private string _name;
    private Metadata _metadata;

    public Sprite Sprite => _sprite;
    public string Name => _name;
    public Metadata Metadata => _metadata;
    
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Image _nftImage;
    [SerializeField] private Button _spawnButton;
    [SerializeField] private Button _removeButton;

    private void Start()
    {
        this.gameObject.transform.DOScale(Vector3.zero, 0f);
        this.gameObject.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.InCubic).SetRelative(true);
    }

    public void Initialize(Sprite sprite, string name, Metadata metadata = null)
    {
        _sprite = sprite;
        _name = name;
        _metadata = metadata;
        
        _nameText.text = _name;
        _nftImage.sprite = _sprite;
    }

    public void SpawnDecal()
    {
        if (_sprite == null)
        {
            Debug.LogWarning("Sprite is null");
            return;
        };
        DecalManager.Instance.GenerateDecal(this);
        _spawnButton.gameObject.SetActive(false);
        _removeButton.gameObject.SetActive(true);
    }
    
    public void RemoveDecal()
    {
        DecalManager.Instance.RemoveDecal(this);
        _spawnButton.gameObject.SetActive(true);
        _removeButton.gameObject.SetActive(false);
    }
}