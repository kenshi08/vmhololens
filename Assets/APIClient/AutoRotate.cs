using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class AutoRotate : MonoBehaviour
    {
        public Vector3 rotation;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(rotation * Time.deltaTime, Space.Self);
        }
    }

}
