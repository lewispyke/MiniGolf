using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float radius = 8;

    private bool ballMoving = false;

    private void FixedUpdate()
    {
        ballMoving = target.GetComponent<BallController>().BallIsHit();

        float x;
        float z;

        if (ballMoving)
        {
            float angle = Mathf.Atan2(target.GetComponent<Rigidbody>().velocity.x, target.GetComponent<Rigidbody>().velocity.z);

            x = Mathf.Sin(angle) * -radius;
            z = Mathf.Cos(angle) * -radius;

        }
        else
        {

            x = Mathf.Sin(target.transform.eulerAngles.y * Mathf.Deg2Rad) * -radius;
            z = Mathf.Cos(target.transform.eulerAngles.y * Mathf.Deg2Rad) * -radius;
        }


        offset = new Vector3(x, 5.2f, z);

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
          transform.position = smoothedPosition;

        
        transform.LookAt(target.position + new Vector3(0f, 2f, 0f));
    }
}



