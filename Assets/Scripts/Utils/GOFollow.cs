using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOFollow : MonoBehaviour
{
    public GameObject obj;
    private void Update()
    {
        transform.position = obj.transform.position;
    }
}
