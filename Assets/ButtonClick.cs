using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonClick : MonoBehaviour
{
    public BallPrefab ballPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Touchscreen.current.press.isPressed)
        {
            BallPrefab ball = Instantiate<BallPrefab>(ballPrefab);
            ball.transform.localPosition = transform.position;
            ball.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * UnityEngine.Random.Range(500, 750));
        }
    }
}
