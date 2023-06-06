using System;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Behaviour
{
public class DocBehaviourExecutor : MonoBehaviour
{
    public static List<DocBehaviour> Behaviours = new(1000);

    private void Update()
    {
        foreach (var behaviour in Behaviours)
        {
            if (behaviour != null)
            {
                behaviour.UpdateTick(Time.deltaTime);
            }
        }
    }

    private void LateUpdate()
    {
        foreach (var behaviour in Behaviours)
        {
            if (behaviour != null)
            {
                behaviour.LateUpdateTick(Time.deltaTime);
            }
        }
    }
}
}