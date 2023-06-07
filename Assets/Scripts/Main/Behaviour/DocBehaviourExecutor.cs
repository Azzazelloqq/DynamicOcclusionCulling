using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Main.Behaviour
{
public class DocBehaviourExecutor : MonoBehaviour
{
    public static List<DocBehaviour> Behaviours = new(1000);

    private void Update()
    {
        Profiler.BeginSample("Doc behaviour executor. Update()");
        foreach (var behaviour in Behaviours)
        {
            if (behaviour != null)
            {
                behaviour.UpdateTick(Time.deltaTime);
            }
        }
        Profiler.EndSample();
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