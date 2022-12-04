using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMove : MonoBehaviour
{
    public float speed = 3;
    public float speed2 = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            transform.RotateAround(transform.position, -Vector3.up, speed * Input.GetAxis("Mouse X"));
            transform.RotateAround(transform.position, transform.right, speed * Input.GetAxis("Mouse Y"));
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward * Time.deltaTime * speed2;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * Time.deltaTime * speed2;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.back * Time.deltaTime * speed2;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * Time.deltaTime * speed2;
        }

        

        if (Input.GetKey(KeyCode.Space))
        {
            transform.position += Vector3.up * Time.deltaTime * speed2;
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.position += Vector3.down * Time.deltaTime * speed2;
        }
    }
}
