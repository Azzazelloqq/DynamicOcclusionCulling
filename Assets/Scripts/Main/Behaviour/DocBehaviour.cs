using System;
using UnityEngine;

namespace Main.Behaviour
{
[RequireComponent(typeof(Renderer))]
public abstract class DocBehaviour : MonoBehaviour
{
    public bool ObjectDisabledByDoc { get; private set; }
    protected bool isUseUpdate;
    protected bool isUseLateUpdate;
    protected bool disableObjectOnInvisible;
    protected bool disableAnimatorOnInvisible;
    private bool _isVisible;
    private bool _isObjectWasActive;
    private bool _isAnimatorWasEnabled;
    private Action _onVisible;
    private Action _onInvisible;
    private float _updateDeltaTime;
    private float _lateUpdateDeltaTime;
    private Renderer _renderer;
    private bool _lastVisible;
    private readonly Plane[] _frustumPlanes = new Plane[6];
    private Animator _animator;

    protected DocBehaviour()
    {
        DocBehaviourExecutor.Behaviours.Add(this);
        _onVisible += OnVisible;
        _onInvisible += OnInvisible;
    }

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();

        if (_animator != null)
        {
            _isAnimatorWasEnabled = _animator.enabled;
        }

        _isObjectWasActive = gameObject.activeSelf;
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

    public void LateUpdateTick(float deltaTime)
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
        }
        
        UpdateVisible(IsVisible(_renderer, DocSettingsContainer.Cameras));
        
        InvokeLateUpdate(deltaTime);
    }

    private void InvokeLateUpdate(float deltaTime)
    {
        if(!isUseLateUpdate) return;
        
        _lateUpdateDeltaTime += deltaTime;

        if (!_isVisible)
        {
            if (Time.frameCount % DocSettingsContainer.FrameDivisor == 0)
            {
                OnLateUpdate(_lateUpdateDeltaTime);
                _lateUpdateDeltaTime = 0f;
            }
        }
        else
        {
            OnLateUpdate(_lateUpdateDeltaTime);
            _lateUpdateDeltaTime = 0f;
        }
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

    private bool IsVisible(Renderer renderer, Camera[] cameras)
    {
        if (cameras == null) return false;

        var bounds = renderer.bounds;

        var isVisibleByAnyCamera = false;

        foreach (var cam in cameras)
        {
            GeometryUtility.CalculateFrustumPlanes(cam, _frustumPlanes);
            isVisibleByAnyCamera = GeometryUtility.TestPlanesAABB(_frustumPlanes, bounds);
            // foreach (var plane in _frustumPlanes)
            // {
            //     var distanceToPoint = plane.GetDistanceToPoint(bounds.center);
            //     if (distanceToPoint < 0)
            //     {
            //         isVisibleByAnyCamera = false;
            //         break;
            //     }
            //
            //     isVisibleByAnyCamera = true;
            // }

            if (isVisibleByAnyCamera)
            {
                break;
            }
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
        
        UpdateActive(_isVisible);
        UpdateAnimatorActive(_isVisible);
    }

    private void UpdateAnimatorActive(bool isVisible)
    {
        if(_animator == null) return; 
        
        if (!isVisible)
        {
            _isAnimatorWasEnabled = _animator.enabled;
            _animator.enabled = false;
        }
        else
        {
            _animator.enabled = _isAnimatorWasEnabled;
        }
    }

    private void UpdateActive(bool isVisible)
    {
        if (!ObjectDisabledByDoc)
        {
            _isObjectWasActive = gameObject.activeSelf;
        }

        if (!isVisible)
        {
            if (_isObjectWasActive)
            {
                DisableObject();
            }
        }
        else
        {
            if (!gameObject.activeSelf)
            {
                SetObjectActiveByDoc(_isObjectWasActive);
            }
        }
    }

    private void SetObjectActiveByDoc(bool isActive)
    {
        if (isActive)
        {
            EnableObject();
        }
        else
        {
            DisableObject();
        }
    }

    private void DisableObject()
    {
        ObjectDisabledByDoc = true;
        gameObject.SetActive(false);
    }

    private void EnableObject()
    {
        ObjectDisabledByDoc = false;
        gameObject.SetActive(true);
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