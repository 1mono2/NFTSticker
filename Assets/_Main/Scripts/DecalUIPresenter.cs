using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DecalUIPresenter : MonoBehaviour
{
    
    public UnityEvent _onMoveButtonClickEvent = new UnityEvent();
    public UnityEvent _onRotateButtonClickEvent = new UnityEvent();
    public UnityEvent _onScaleButtonClickEvent = new UnityEvent();
    
    public void OnMoveButtonClicked()
    {
        _onMoveButtonClickEvent.Invoke();
    }
    
    public void  OnRotateButtonClicked()
    {
        _onRotateButtonClickEvent.Invoke();
    }
    
    public void OnScaleButtonClicked()
    {
        _onScaleButtonClickEvent.Invoke();
    }
    
    
    
}
