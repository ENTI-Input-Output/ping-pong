using UnityEngine;
using Photon.Pun;


    public class BallController : MonoBehaviourPun
    {
        private Rigidbody _rb;
        private Vector3 _oldVel;
        public float AdditionalImpulse = 1.5f;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            _oldVel = _rb.velocity;
        }
        public void ChangeOwner()
        {
            photonView.RequestOwnership();
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
                _oldVel += paddleVel * AdditionalImpulse;
            }

            //Calculate with Vector3.Reflect
            _rb.velocity = Vector3.Reflect(_oldVel, cp.normal);

            //_rb.AddForce(collisionForce, ForceMode.Impulse);
        }
}