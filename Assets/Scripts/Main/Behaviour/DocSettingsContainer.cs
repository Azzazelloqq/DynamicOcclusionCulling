using Main.Settings;
using UnityEngine;

namespace Main.Behaviour
{
public class DocSettingsContainer : MonoBehaviour
{
    [SerializeField]
    private DocSettings _docSettings;

    public static int FrameDivisor;
    
    private void Awake()
    {
        if (_docSettings != null)
        {
            FrameDivisor = _docSettings.FrameDivisor;
        }
    }
}
}