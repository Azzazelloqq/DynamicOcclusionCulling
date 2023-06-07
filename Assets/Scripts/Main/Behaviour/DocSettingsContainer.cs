using Main.Settings;
using UnityEngine;

namespace Main.Behaviour
{
public class DocSettingsContainer : MonoBehaviour
{
    [SerializeField]
    private DocSettings _docSettings;

    [SerializeField]
    private Camera[] _cameras;

    public static int FrameDivisor;
    public static Camera[] Cameras;
     
    private void Awake()
    {
        if (_docSettings != null)
        {
            FrameDivisor = _docSettings.FrameDivisor;
            Cameras = _cameras;
        }
    }
}
}