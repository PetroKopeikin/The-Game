using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private float jumpForce = 200f;

    [SerializeField]
    private float speed = 6f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;
        Move();

        if (Input.GetKeyDown("space"))
        {
            Jump();
        }
    }

    void Move()
    {
        float XAxis = Input.GetAxis("Horizontal");
        float YAxis = Input.GetAxis("Vertical");

        if (XAxis != 0 || YAxis != 0)
        {
            float dirX = transform.localPosition.x + XAxis * Time.deltaTime * speed;
            float dirZ = transform.localPosition.z + YAxis * Time.deltaTime * speed;
            transform.localPosition = new Vector3(dirX, transform.localPosition.y, dirZ);
        }
    }

    void Jump()
    { 
        var rigigbody = GetComponent<Rigidbody>();
        if(!rigigbody)
        {
            Debug.LogError("RigidBody was not found");
            return;
        }

        rigigbody.AddForce(transform.up * jumpForce);

    }
}
