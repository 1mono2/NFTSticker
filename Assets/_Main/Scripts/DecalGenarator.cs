using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalGenarator : MonoBehaviour
{
    [SerializeField] private DecalBehavior _decalBehaviorTemplate;
    [SerializeField] private Material _decalMaterialTemplate;
    
    [SerializeField] private Sprite _sampleSprite;
    [SerializeField] private DecalUIPresenter _decalUIPresenter;

    private DecalBehavior _targetDecalBehavior;

    public void GenerateDecal(Sprite sprite)
    {
        RemoveListenerFromButton();
        
        _targetDecalBehavior = Instantiate(_decalBehaviorTemplate, transform);
        _targetDecalBehavior.gameObject.SetActive(true);
        Material material = new Material(_decalMaterialTemplate);
        
        material.SetTexture("Base_Map", sprite.texture);
        var decalProjector = _targetDecalBehavior.GetComponentInChildren<DecalProjector>();
        decalProjector.material = material;
        AddListenerToButton();
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
