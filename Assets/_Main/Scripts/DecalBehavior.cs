using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Common;
using Lean.Touch;
using UnityEngine;

public class DecalBehavior : MonoBehaviour
{
    [Header("Lean objects")]
    [SerializeField] private LeanMultiUpdate _leanMultiUpdate;
    [SerializeField] private LeanManualTranslate _leanManualTranslate;
    [SerializeField] private LeanRotateToPosition _leanRotateToPosition;
    [SerializeField] private LeanManualRescale _leanManualRescale;
    [Header("Indicator objects")]
    [SerializeField] private GameObject _moveArrows;
    [SerializeField] private GameObject _rotateArrows;
    [SerializeField] private GameObject _scaleArrows;
    
    [SerializeField] private Sprite _sprite;

    private void OnEnable()
    {
        SwitchToMoveMode();
    }

    public void SwitchToMoveMode()
    {
        ResetAllBehavior();
        
        _leanMultiUpdate.OnDelta.AddListener(_leanManualTranslate.TranslateAB);
        _moveArrows.SetActive(true);
    }
    
    public void SwitchToRotateMode()
    {
        ResetAllBehavior();
        
        _leanMultiUpdate.OnWorldFrom.AddListener(_leanRotateToPosition.SetPosition);
        _rotateArrows.SetActive(true);
    }
    
    public void SwitchToScaleMode()
    {
        ResetAllBehavior();
        
        _leanMultiUpdate.OnDelta.AddListener(_leanManualRescale.AddScaleAB);
        _scaleArrows.SetActive(true);
    }

    public void ResetAllBehavior()
    {
        _leanMultiUpdate.OnFingers.RemoveAllListeners();
        _leanMultiUpdate.OnDelta.RemoveAllListeners();
        _leanMultiUpdate.OnDistance.RemoveAllListeners();
        _leanMultiUpdate.OnWorldFrom.RemoveAllListeners();
        _leanMultiUpdate.OnWorldTo.RemoveAllListeners();
        _leanMultiUpdate.OnWorldFromTo.RemoveAllListeners();
        _leanMultiUpdate.OnWorldDelta.RemoveAllListeners();
        
        _moveArrows.SetActive(false);
        _rotateArrows.SetActive(false);
        _scaleArrows.SetActive(false);
        
    }
    
}