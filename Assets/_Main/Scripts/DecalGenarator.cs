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

    private void Start()
    {
        GenerateDecal();
    }

    private void GenerateDecal()
    {
        var decalBehavior = Instantiate(_decalBehaviorTemplate, transform);
        decalBehavior.gameObject.SetActive(true);
        Material material = new Material(_decalMaterialTemplate);
        
        material.SetTexture("Base_Map", _sampleSprite.texture);
        var decalProjector = decalBehavior.GetComponentInChildren<DecalProjector>();
        decalProjector.material = material;
    }
}
