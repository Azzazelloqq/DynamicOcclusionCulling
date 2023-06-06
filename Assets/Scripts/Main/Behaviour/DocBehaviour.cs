using System;
using UnityEngine;

namespace Main.Behaviour
{
[RequireComponent(typeof(Renderer))]
public abstract class DocBehaviour : MonoBehaviour
{
    protected bool isUseUpdate;
    protected bool isUseLateUpdate;
    protected Action onVisible;
    protected Action onInvisible;
    protected bool isVisible;
    private float _updateDeltaTime;
    private float _lateUpdateDeltaTime;
    private Renderer _renderer;
    private bool _lastVisible;

    protected DocBehaviour()
    {
        DocBehaviourExecutor.Behaviours.Add(this);
    }

    public void UpdateTick(float deltaTime)
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
        }
        
        UpdateVisible(_renderer.isVisible);
        
        InvokeUpdate(deltaTime);
    }

    private void InvokeUpdate(float deltaTime)
    {
        if(!isUseUpdate) return;

        _updateDeltaTime += deltaTime;

        if (!_renderer.isVisible)
        {
            if (Time.frameCount % DocSettingsContainer.FrameDivisor == 0)
            {
                OnUpdate(_updateDeltaTime);
                _updateDeltaTime = 0f;
            }
        }
        else
        {
            OnUpdate(_updateDeltaTime);
            _updateDeltaTime = 0f;
        }
    }

    public void LateUpdateTick(float deltaTime)
    {
        if(!isUseLateUpdate) return;
        
        _lateUpdateDeltaTime += deltaTime;

        if (!isVisible)
        {
            if (Time.frameCount % DocSettingsContainer.FrameDivisor == 0)
            {
                OnUpdate(_lateUpdateDeltaTime);
                _lateUpdateDeltaTime = 0f;
            }
        }
        else
        {
            OnUpdate(_lateUpdateDeltaTime);
            _lateUpdateDeltaTime = 0f;
        }
    }

    private void UpdateVisible(bool isVisible)
    {
        if (this.isVisible != isVisible)
        {
            if (isVisible)
            {
                onVisible?.Invoke();
            }
            else
            {
                onInvisible?.Invoke();
            }
        }

        this.isVisible = isVisible;
    }

    #region BehaviourLifeTimea

    protected virtual void OnUpdate(float deltaTime)
    {
    }

    protected virtual void OnLateUpdate(float deltaTime)
    {
    }
    //
    // protected virtual void OnVisible()
    // {
    //     _isVisible = true;
    //     Debug.Log("OnVisible");
    // }
    //
    // protected virtual void OnInvisible()
    // {
    //     _isVisible = false;
    //     Debug.Log("OnInvisible");
    //
    // }

    #endregion
}
}