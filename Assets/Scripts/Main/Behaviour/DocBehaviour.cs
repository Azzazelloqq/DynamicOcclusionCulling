using System;
using UnityEngine;
using UnityEngine.Profiling;

namespace Main.Behaviour
{
[RequireComponent(typeof(Renderer))]
public abstract class DocBehaviour : MonoBehaviour
{
    protected bool isUseUpdate;
    protected bool isUseLateUpdate;
    protected bool disableObjectOnInvisible;
    protected bool disableAnimatorOnInvisible;
    private bool _isVisible;
    private Action _onVisible;
    private Action _onInvisible;
    private float _updateDeltaTime;
    private float _lateUpdateDeltaTime;
    private Renderer _renderer;
    private bool _lastVisible;

    protected DocBehaviour()
    {
        DocBehaviourExecutor.Behaviours.Add(this);
        _onVisible += OnVisible;
        _onInvisible += OnInvisible;
    }

    protected virtual void OnVisible()
    {
        if (disableObjectOnInvisible && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void OnInvisible()
    {
        if (disableAnimatorOnInvisible && !gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }

    public void UpdateTick(float deltaTime)
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
        }
        
        UpdateVisible(IsVisible(_renderer, DocSettingsContainer.Cameras));
        
        InvokeUpdate(deltaTime);
    }

    private void InvokeUpdate(float deltaTime)
    {
        if(!isUseUpdate) return;

        Profiler.BeginSample("Doc behaviour. InvokeUpdate()");
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
        Profiler.EndSample();
    }

    public void LateUpdateTick(float deltaTime)
    {
        if(!isUseLateUpdate) return;
        
        _lateUpdateDeltaTime += deltaTime;

        if (!_isVisible)
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

    //TODO: optimize GeometryUtility.CalculateFrustumPlanes. It's to much GC.Alloc
    private bool IsVisible(Renderer renderer, Camera[] cameras)
    {
        var wasActive = gameObject.activeSelf;

        if (!wasActive)
        {
            gameObject.SetActive(true);
        }

        var bounds = renderer.bounds;

        var isVisibleByAnyCamera = false;

        foreach (var cam in cameras)
        {
            var calculateFrustumPlanes = GeometryUtility.CalculateFrustumPlanes(cam);
            isVisibleByAnyCamera = GeometryUtility.TestPlanesAABB(calculateFrustumPlanes, bounds);

            if (isVisibleByAnyCamera)
            {
                break;
            }
        }

        if (!wasActive)
        {
            gameObject.SetActive(false);
        }
        
        return isVisibleByAnyCamera;
    }

    private void UpdateVisible(bool newVisibilityState)
    {
        if (_isVisible != newVisibilityState)
        {
            if (newVisibilityState)
            {
                _onVisible?.Invoke();
            }
            else
            {
                _onInvisible?.Invoke();
            }
        }

        _isVisible = newVisibilityState;
    }

    #region BehaviourLifeTimea

    protected virtual void OnUpdate(float deltaTime)
    {
    }

    protected virtual void OnLateUpdate(float deltaTime)
    {
    }
    
    protected virtual void OnDestroy()
    {
        _onVisible -= OnVisible;
        _onInvisible -= OnInvisible;
    }

    #endregion
}
}