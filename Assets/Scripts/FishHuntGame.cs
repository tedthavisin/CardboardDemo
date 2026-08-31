using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Runs a round of spear fishing: you get a thirty second countdown, a spear is
/// thrown for you once a second, and every one that lands on the fish is worth
/// one point. All you do is aim. Rounds restart on their own after a short
/// breather.
/// </summary>
public class FishHuntGame : MonoBehaviour
{
    [Header("Scene references")]
    [Tooltip("The camera the player aims with. Defaults to the camera on this object.")]
    public Camera playerCamera;

    [Tooltip("Where thrown spears appear. Should be a child of the camera.")]
    public Transform spearSpawn;

    public Spear spearPrefab;
    public FishTarget fish;

    [Header("Rules")]
    public float roundSeconds = 30f;

    [Tooltip("Seconds between the end of a round and the start of the next one.")]
    public float restartDelay = 10f;

    [Tooltip("Seconds between automatic throws.")]
    public float fireInterval = 1f;

    public float throwSpeed = 25f;

    private TextMesh _hudText;
    private TextMesh _messageText;
    private float _timeLeft;
    private float _nextShotIn;
    private float _restartIn;
    private int _score;
    private bool _roundActive;

    private void Awake()
    {
        if (playerCamera == null)
        {
            playerCamera = GetComponent<Camera>();
        }

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        // The heads-up display is built in code so it always sits square in
        // front of the camera, in both eyes.
        _hudText = CreateHudText("HUD", new Vector3(0f, 0.42f, 1.4f), 0.0032f);
        _messageText = CreateHudText("Message", new Vector3(0f, -0.32f, 1.4f), 0.0028f);
    }

    private void Start()
    {
        StartRound();
    }

    private void Update()
    {
        if (!_roundActive)
        {
            _restartIn -= Time.deltaTime;

            // The trigger just skips the wait; the next round starts either way.
            if (_restartIn <= 0f || TriggerPressedThisFrame())
            {
                StartRound();
            }
            else
            {
                UpdateIntermission();
            }

            return;
        }

        _timeLeft -= Time.deltaTime;

        if (_timeLeft <= 0f)
        {
            _timeLeft = 0f;
            EndRound();
            return;
        }

        _nextShotIn -= Time.deltaTime;
        if (_nextShotIn <= 0f)
        {
            ThrowSpear();

            // Advance rather than reset so the throws stay on a steady beat.
            _nextShotIn += Mathf.Max(fireInterval, 0.05f);
        }

        UpdateHud();
    }

    /// <summary>Called by a spear that connected with the fish.</summary>
    public void RegisterHit(FishTarget hitFish)
    {
        if (!_roundActive)
        {
            return;
        }

        _score++;
        UpdateHud();
        hitFish.Respawn();
    }

    private void StartRound()
    {
        _score = 0;
        _timeLeft = roundSeconds;
        _nextShotIn = Mathf.Max(fireInterval, 0.05f);
        _roundActive = true;

        if (fish != null)
        {
            fish.Respawn();
        }

        _messageText.text = string.Empty;
        UpdateHud();
    }

    private void EndRound()
    {
        _roundActive = false;
        _restartIn = Mathf.Max(restartDelay, 0f);
        UpdateHud();
        UpdateIntermission();
    }

    private void UpdateIntermission()
    {
        _messageText.text = string.Format(
            "Time's up!\nScore: {0}\n\nNext round in {1:0}",
            _score,
            Mathf.Max(Mathf.Ceil(_restartIn), 0f));
    }

    private void ThrowSpear()
    {
        if (spearPrefab == null || spearSpawn == null || playerCamera == null)
        {
            return;
        }

        Transform eye = playerCamera.transform;

        // Aim at whatever the crosshair is actually over, so the spear goes
        // where the player is looking rather than parallel to it.
        Vector3 aimPoint = eye.position + eye.forward * 100f;

        // Ignore anything closer than the spear itself. A hit in front of the
        // player's face would put the aim point behind the spawn, which throws
        // the spear backwards.
        if (Physics.Raycast(eye.position, eye.forward, out RaycastHit hit, 100f)
            && hit.distance > 1f)
        {
            aimPoint = hit.point;
        }

        Vector3 direction = (aimPoint - spearSpawn.position).normalized;

        Spear spear = Instantiate(spearPrefab, spearSpawn.position, Quaternion.LookRotation(direction));
        spear.Launch(this, direction * throwSpeed);
    }

    private void UpdateHud()
    {
        _hudText.text = string.Format("{0:0.0}s     Score {1}", _timeLeft, _score);

        if (_roundActive)
        {
            _hudText.text += string.Format(
                "\nnext spear {0:0.0}", Mathf.Max(_nextShotIn, 0f));
        }
    }

    /// <summary>
    /// Accepts the Cardboard trigger, plus a tap / click / spacebar so a round
    /// can be restarted in the editor without a headset.
    /// </summary>
    private static bool TriggerPressedThisFrame()
    {
        if (Google.XR.Cardboard.Api.IsTriggerPressed)
        {
            return true;
        }

        Touchscreen touchscreen = Touchscreen.current;
        if (touchscreen != null && touchscreen.primaryTouch.press.wasPressedThisFrame)
        {
            return true;
        }

        Mouse mouse = Mouse.current;
        if (mouse != null && mouse.leftButton.wasPressedThisFrame)
        {
            return true;
        }

        Keyboard keyboard = Keyboard.current;
        return keyboard != null && keyboard.spaceKey.wasPressedThisFrame;
    }

    private TextMesh CreateHudText(string name, Vector3 localPosition, float characterSize)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(playerCamera != null ? playerCamera.transform : transform, false);
        go.transform.localPosition = localPosition;

        TextMesh text = go.AddComponent<TextMesh>();
        text.anchor = TextAnchor.MiddleCenter;
        text.alignment = TextAlignment.Center;
        text.fontSize = 90;
        text.characterSize = characterSize;
        text.color = Color.white;

        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (font != null)
        {
            text.font = font;
            go.GetComponent<MeshRenderer>().sharedMaterial = font.material;
        }

        return text;
    }
}
