using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CWR
{
    public class PlayerAgent : Agent
    {
        void Update()
        {
            velocity.x = Input.GetAxis("Horizontal");
            velocity.z = Input.GetAxis("Vertical");
            velocity = Utils.Vector3Truncate(velocity, maxSpeed);

            pos += velocity * Time.deltaTime;
        }
    }
}
