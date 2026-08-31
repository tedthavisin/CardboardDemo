using UnityEngine;

/// <summary>
/// A fish that swims straight back and forth in front of the player, and
/// teleports to a fresh spot every time it gets speared.
/// </summary>
public class FishTarget : MonoBehaviour
{
    [Header("Swimming")]
    [Tooltip("How far the fish travels left/right of its swim centre, in metres.")]
    public float swimWidth = 3f;

    [Tooltip("How fast the fish sweeps left and right.")]
    public float swimSpeed = 1.2f;

    [Header("Respawn range")]
    public float minDistance = 6f;
    public float maxDistance = 11f;
    public float minHeight = 0.8f;
    public float maxHeight = 2.2f;

    private Vector3 _center;
    private float _phase;

    private void Awake()
    {
        _center = transform.position;
    }

    private void Update()
    {
        float sweep = Time.time * swimSpeed + _phase;

        // Left and right only: height and distance hold steady until the next
        // respawn.
        transform.position = _center + new Vector3(Mathf.Sin(sweep) * swimWidth, 0f, 0f);

        // The model's nose points down +X, so flip it when the sweep turns round.
        transform.rotation = Quaternion.Euler(0f, Mathf.Cos(sweep) >= 0f ? 0f : 180f, 0f);
    }

    /// <summary>Moves the fish to a new random spot in front of the player.</summary>
    public void Respawn()
    {
        _center = new Vector3(
            0f,
            Random.Range(minHeight, maxHeight),
            Random.Range(minDistance, maxDistance));

        _phase = Random.Range(0f, Mathf.PI * 2f);
    }
}
