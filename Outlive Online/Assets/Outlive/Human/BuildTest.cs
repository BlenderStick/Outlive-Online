using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlive.Human
{
    public class BuildTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnBuildProgress (Build.CallbackContext ctx)
        {
            Debug.Log(ctx.Progress);
        }
    }
}