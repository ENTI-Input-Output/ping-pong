using UnityEngine;


public class BallController : MonoBehaviour
{
    private Rigidbody _rb;
    private Vector3 _oldVel;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _oldVel = _rb.velocity;
    }

    void OnCollisionEnter(Collision col)
    {
        //Get contact point
        ContactPoint cp = col.GetContact(0);

        //Calculate impact force
        //Vector3 collisionForce = col.impulse / Time.fixedDeltaTime;

        //Debug.Log(collisionForce);

        if (col.transform.name == Constants.Paddle)
        {
            //_oldVel *= Bounciness;
            Vector3 paddleVel = col.rigidbody.velocity;
            Debug.Log("Paddle velocity = " + paddleVel);
            _oldVel += paddleVel;
        }

        //Calculate with Vector3.Reflect
        _rb.velocity = Vector3.Reflect(_oldVel, cp.normal);

        //_rb.AddForce(collisionForce, ForceMode.Impulse);
    }
}
