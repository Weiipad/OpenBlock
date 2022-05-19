using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    public Transform visionTransform;
    public float speed;

    private Vector3 forwardOnXoz;
    private static float sqrtOftwo = Mathf.Sqrt(2.0f) / 2.0f;
    

    private void Update()
    {
        forwardOnXoz = new Vector3(visionTransform.forward.x, 0, visionTransform.forward.z);
        forwardOnXoz.Normalize();
    }

    public void MoveForward()
    {
        transform.position += forwardOnXoz * speed * Time.deltaTime;
    }

    public void MoveBack()
    {
        transform.position -= forwardOnXoz * speed * Time.deltaTime;
    }

    public void MoveLeft()
    {
        
        transform.position += new Quaternion(0, -sqrtOftwo, 0, sqrtOftwo) * forwardOnXoz * speed * Time.deltaTime;
    }

    public void MoveRight()
    {
        transform.position += new Quaternion(0, sqrtOftwo, 0, sqrtOftwo) * forwardOnXoz * speed * Time.deltaTime;
    }

    public void MoveRise()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
    }

    public void MoveDown()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
    }
}
