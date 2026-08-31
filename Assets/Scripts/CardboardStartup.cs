using Google.XR.Cardboard;
using UnityEngine;

/// <summary>
/// Boots the Cardboard XR plugin: keeps the screen awake, prompts for the
/// viewer QR code the first time, and wires up the gear / close / recenter
/// buttons drawn over the stereo view.
/// </summary>
public class CardboardStartup : MonoBehaviour
{
    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.brightness = 1.0f;

        if (!Api.HasDeviceParams())
        {
            Api.ScanDeviceParams();
        }
    }

    private void Update()
    {
        if (Api.IsGearButtonPressed)
        {
            Api.ScanDeviceParams();
        }

        if (Api.IsCloseButtonPressed)
        {
            Application.Quit();
        }

        if (Api.IsTriggerHeldPressed)
        {
            Api.Recenter();
        }

        if (Api.HasNewDeviceParams())
        {
            Api.ReloadDeviceParams();
        }

        Api.UpdateScreenParams();
    }
}
