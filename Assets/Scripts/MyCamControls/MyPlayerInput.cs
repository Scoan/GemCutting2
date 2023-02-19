using UnityEngine;

namespace PlayerInputUtils
{
    public class MyPlayerInput : MonoBehaviour
    {
        // Orbit the main camera around a target point when the player inputs left/right. Track in/out when the player inputs forward/back.
        static Camera mainCam;
        static Vector3 tgtPos = new Vector3(0, 0, 0);

        static float distance = 25;
        static float minDistance = 10;
        static float maxDistance = 50;
        static float distanceTopSpeed = 2.0f;
        static float distanceSpeed = 0;
        static float distanceAccel = 1.0f;
        static float distanceDecel = 1.0f;

        // Orbit properties
        static float orbit = 0;     //0-1 rotations = 0-360 degrees counterclockwise (from above).
        static float topSpeed = .25f; // Max rotations per second
        private static float orbitSpeed = 0;  // cur rotations/second
        static float orbitAccel = 1.3f;   // Rotations/second^2 speed up if input applied
        static float orbitDecel = .6f;   // Rotations/second^2 speed up if no input applied

        public void Start()
        {
            mainCam = Camera.main;

            mainCam.transform.position = new Vector3(0, 4, -distance);
            mainCam.transform.LookAt(tgtPos);
        }

        public void FixedUpdate()
        {
            float delT = Time.fixedDeltaTime;

            // Orbit by input
            float orbitInput = -1.0f * (Input.GetKey(KeyCode.A) ? 1 : 0) + 1.0f * (Input.GetKey(KeyCode.D) ? 1 : 0);
            if (orbitInput == 0)
            {
                // No net input? Decelerate the orbit
                if (orbitSpeed > 0)
                {
                    orbitSpeed -= orbitDecel * delT;
                    orbitSpeed = Mathf.Clamp(orbitSpeed, 0, topSpeed);
                }
                else
                {
                    orbitSpeed += orbitDecel * delT;
                    orbitSpeed = Mathf.Clamp(orbitSpeed, -topSpeed, 0);
                }
            }
            else
            {
                // Input? Accelerate the orbit
                orbitSpeed += orbitInput * orbitAccel * delT;
            }

            // Distance by input
            float distanceInput = -1.0f * (Input.GetKey(KeyCode.S) ? 1 : 0) + 1.0f * (Input.GetKey(KeyCode.W) ? 1 : 0);

            ////

            // Clamp velocity
            orbitSpeed = Mathf.Clamp(orbitSpeed, -topSpeed, topSpeed);

            // Compute and set position
            orbit += orbitSpeed * delT;
            orbit = orbit % 1000.0f;    // For safety

            float orbitRads = (orbit % 1.0f) * 360 * Mathf.Deg2Rad;
            mainCam.transform.position = new Vector3(-distance * Mathf.Sin(-orbitRads), 4, -distance * Mathf.Cos(-orbitRads));


            mainCam.transform.LookAt(tgtPos);

            //Debug.Log(string.Format("Orbit:{0}, Velocity:{1}", orbit, velocity));
        }
    }
}
