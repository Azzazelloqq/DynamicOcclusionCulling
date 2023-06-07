using System;
using Main.Behaviour;
using UnityEngine;

namespace Test.MoveTest
{
public class ObjectMoveTest : DocBehaviour
{
    private const float MoveOffset = 10f;
    private const float Speed = 1f;
    private Vector3 _startMovePosition;
    private bool _isMoveUp;

    protected void Awake()
    {
        _startMovePosition = transform.position;
        isUseUpdate = true;
    }

    private void Update()
    {
        MoveCubeTest(Time.deltaTime);
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        
        MoveCubeTest(deltaTime);
    }

    private void MoveCubeTest(float deltaTime)
    {
        if (transform.position.y < _startMovePosition.y - MoveOffset)
        {
            _isMoveUp = true;
        }
        
        if (transform.position.y > _startMovePosition.y + MoveOffset)
        {
            _isMoveUp = false;
        }

        if (_isMoveUp)
        {
            transform.position = new Vector3(_startMovePosition.x, transform.position.y + Speed * deltaTime,
                _startMovePosition.z);
        }
        else
        {
            transform.position = new Vector3(_startMovePosition.x, transform.position.y - Speed * deltaTime,
                _startMovePosition.z);
        }
    }
}
}