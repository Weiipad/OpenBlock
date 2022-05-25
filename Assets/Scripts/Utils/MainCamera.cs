using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenBlock.Utils
{
    public class MainCamera : MonoBehaviour
    {
        private GameObject obj;
        private void Update()
        {
            if (obj != null)
            {
                transform.position = obj.transform.position;
                transform.rotation = obj.transform.rotation;
            }
        }

        public void Trace(GameObject obj)
        {
            this.obj = obj;
            transform.position = obj.transform.position;
            transform.rotation = obj.transform.rotation;
        }

        public void StopTrace()
        {
            obj = null;
        }
    }
}
