using UnityEngine;

/// <summary>
/// A thrown spear. Flies straight, scores a point if it hits the fish, and
/// cleans itself up either way.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Spear : MonoBehaviour
{
    [Tooltip("Seconds before an unspent spear removes itself.")]
    public float lifeSeconds = 3f;

    private FishHuntGame _game;
    private bool _spent;

    /// <summary>Sends the spear on its way.</summary>
    public void Launch(FishHuntGame game, Vector3 velocity)
    {
        _game = game;
        GetComponent<Rigidbody>().linearVelocity = velocity;
        transform.rotation = Quaternion.LookRotation(velocity);
        Destroy(gameObject, lifeSeconds);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_spent)
        {
            return;
        }

        _spent = true;

        FishTarget fish = collision.collider.GetComponentInParent<FishTarget>();
        if (fish != null && _game != null)
        {
            _game.RegisterHit(fish);
        }

        Destroy(gameObject);
    }
}
