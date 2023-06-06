using UnityEngine;

namespace Main.Settings
{
[CreateAssetMenu(fileName = "DocSettings", menuName = "ScriptableObjects/DOC/Settings", order = 1)]
public class DocSettings : ScriptableObject
{
    [field: SerializeField]
    public int FrameDivisor { get; private set; } 
}
}
