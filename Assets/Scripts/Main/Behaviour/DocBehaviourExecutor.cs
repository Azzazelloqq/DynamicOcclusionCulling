using System;
using System.Collections.Generic;
using TMPro.EditorUtilities;
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
                var isActive = behaviour.gameObject.activeSelf || behaviour.ObjectDisabledByDoc;

                if (isActive)
                {
                    behaviour.UpdateTick(Time.deltaTime);
                }
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
                var isActive = behaviour.gameObject.activeSelf || behaviour.ObjectDisabledByDoc;
                if (isActive)
                {
                    behaviour.LateUpdateTick(Time.deltaTime);
                }
            }
        }
    }
}
}