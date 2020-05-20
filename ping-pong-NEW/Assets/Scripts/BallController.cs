using UnityEngine;
using Photon.Pun;


public class BallController : MonoBehaviourPun
{
    private Rigidbody _rb;
    private Vector3 _oldVel;
    public float AdditionalImpulse = 1.5f;
    public bool IsLocked = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _oldVel = _rb.velocity;
    }
    public void ChangeOwner(int photonID)
    {
        photonView.TransferOwnership(photonID);
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
            //ChangeOwner();
            //_oldVel *= Bounciness;
            Vector3 paddleVel = col.rigidbody.velocity;
            //Debug.Log("Paddle velocity = " + paddleVel);
            _oldVel += paddleVel * AdditionalImpulse;
        }

        //Calculate with Vector3.Reflect
        _rb.velocity = Vector3.Reflect(_oldVel, cp.normal);

        Surface surface = col.transform.GetComponent<Surface>();
        if (surface != null)
        {
            GameLogic.Instance.OnBallCollision(surface);
        }
        else
        {
            Debug.Log("The ball didn't hit a controlled surface");
        }

        //_rb.AddForce(collisionForce, ForceMode.Impulse);
    }
}