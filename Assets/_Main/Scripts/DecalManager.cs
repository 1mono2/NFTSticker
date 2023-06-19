using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalManager : SingletonMonoBehavior<DecalManager>
{
    [SerializeField] private DecalBehavior _decalBehaviorTemplate;
    [SerializeField] private Material _decalMaterialTemplate;
    
    [SerializeField] private DecalUIPresenter _decalUIPresenter;

    private DecalBehavior _targetDecalBehavior;
    private Dictionary<NFTAsset, DecalBehavior> _decalDictionary = new();

    public override void Awake()
    {
        base.Awake();
    }

    public void GenerateDecal(NFTAsset nftAsset)
    {
        RemoveListenerFromButton();
        if(_targetDecalBehavior != null)
        {
            _targetDecalBehavior.FixPosition(); 
            _targetDecalBehavior = null;
        }
        
        // Generate decal form nft asset
        if (nftAsset.Sprite == null)
        {
            Debug.LogWarning("It's not have sprite");
            return;
        }else if (_decalDictionary.ContainsKey(nftAsset))
        {
            Debug.LogWarning("It's already have decal");
            return;
        }
        
        
        _targetDecalBehavior = Instantiate(_decalBehaviorTemplate, transform);
        _targetDecalBehavior.gameObject.SetActive(true);
        Material material = new Material(_decalMaterialTemplate);
        
        material.SetTexture("Base_Map", nftAsset.Sprite.texture);
        var decalProjector = _targetDecalBehavior.GetComponentInChildren<DecalProjector>();
        decalProjector.material = material;
        AddListenerToButton();
        
        // Add to dictionary
        _decalDictionary.Add(nftAsset, _targetDecalBehavior);
    }

    public void RemoveDecal(NFTAsset nftAsset)
    {
        if (_decalDictionary.ContainsKey(nftAsset))
        {
            RemoveListenerFromButton();
            Destroy(_decalDictionary[nftAsset].gameObject);
            _decalDictionary.Remove(nftAsset);
        }
    }

    public void AddListenerToButton()
    {
        _decalUIPresenter._onMoveButtonClickEvent.AddListener(() => _targetDecalBehavior.SwitchToMoveMode());
        _decalUIPresenter._onRotateButtonClickEvent.AddListener(() => _targetDecalBehavior.SwitchToRotateMode());
        _decalUIPresenter._onScaleButtonClickEvent.AddListener(() => _targetDecalBehavior.SwitchToScaleMode());
    }
    
    public void RemoveListenerFromButton()
    {
        _decalUIPresenter._onMoveButtonClickEvent.RemoveAllListeners();
        _decalUIPresenter._onRotateButtonClickEvent.RemoveAllListeners();
        _decalUIPresenter._onScaleButtonClickEvent.RemoveAllListeners();
    }
}
