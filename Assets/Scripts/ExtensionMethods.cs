using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    #region Private Static Variables
    private static float volumeIn;
    private static float volumeOut;

    private static float moveX;
    private static float moveZ;
    private static Vector3 previous;
    private static Vector3 towards;

    private static float lookHor;
    private static float lookVer;
    private static Vector3 bodyAngle;
    private static Vector3 lookAngle;
    #endregion


    /// <summary>
    /// It changes music player's volume smoothly. Select AudioClip you want to play and start this Coroutine. It requires two AudioSources.
    /// </summary>
    /// <param name="clip">Music file component that should be played.</param>
    /// <param name="fadeIn">Music Player that needs to be played.</param>
    /// <param name="fadeOut">Music Player that needs to be stopped.</param>
    public static IEnumerator CrossFade(this AudioClip clip, AudioSource fadeIn, AudioSource fadeOut)
    {
        volumeIn = fadeIn.volume;
        volumeOut = fadeOut.volume;
        
        fadeIn.clip = clip;
        fadeIn.Play();
        while (volumeOut > 0)
        {
            volumeIn += Time.deltaTime/2;
            volumeOut -= Time.deltaTime/2;
            if (volumeIn >= 1)
                volumeIn = 1;
            if (volumeOut <= 0)
                volumeOut = 0;

            fadeIn.volume = volumeIn;
            fadeOut.volume = volumeOut;
            yield return null;
        }
        fadeOut.Stop();
    }
    
    /// <summary>
    /// Same Method as Rigidbody's MovePosition, but it hides long codes for better readability.
    /// </summary>
    /// <param name="body">Rigidbody component which is attached to Character.</param>
    /// <param name="towards">Vector3 direction that Character must go.</param>
    /// <param name="speed">Float number that defines how fast should Character move.</param>
    public static void MoveTowards(this Rigidbody body, Vector3 towards, float speed)
    {
        body.MovePosition(body.position + towards.normalized * Time.deltaTime * speed);
    }

    /// <summary>
    /// Single Method to move Character's position, which takes keyboard's or joystick's inputs.
    /// </summary>
    /// <param name="body">Rigidbody component which is attached to Character.</param>
    /// <param name="speed">Float number that defines how fast should Character move.</param>
    public static void MovementMethod(this Rigidbody body, float speed)
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveZ = Input.GetAxisRaw("Vertical");
        towards.x = moveX;
        towards.z = moveZ;
        towards.Normalize();

        if (Input.GetKey(KeyCode.LeftShift))
            speed *= 1.5f;

        towards = body.transform.TransformDirection(towards);
        towards = Vector3.Lerp(previous, towards, Time.deltaTime * speed);
        body.MovePosition(body.position + towards * Time.deltaTime * speed);

        previous = towards; //Deep Copy

        Debug.DrawRay(body.position, towards * 5, Color.red);

        if (Input.GetKeyDown(KeyCode.Space))
            body.AddForce(Vector3.up * 5, ForceMode.Impulse);

        Debug.DrawRay(body.position, Vector3.up * body.velocity.y, Color.magenta);
    }

    /// <summary>
    /// It handles Camera for 3rd Person View. This requires Transform of Camera's pivot.
    /// </summary>
    /// <param name="body">Rigidbody component which is attached to Character.</param>
    /// <param name="pivot">Transform of Camera's pivot.</param>
    /// <param name="speed">Float number that defines how fast should Camera move.</param>
    public static void ThirdPersonControl(this Rigidbody body, Transform pivot, float speed)
    {
        lookHor = Input.GetAxis("Mouse X");
        lookVer = -Input.GetAxis("Mouse Y");
        lookAngle.x = lookVer;

        if (Input.GetMouseButton(1))
        {
            lookAngle.y = lookHor;
            bodyAngle.y = 0;
        }
        else
        {
            if (Input.GetMouseButtonUp(1))
                pivot.eulerAngles = new Vector3(0, body.transform.eulerAngles.y, 0);
            lookAngle.y = 0;
            bodyAngle.y = lookHor;
        }

        pivot.eulerAngles += lookAngle;
        body.transform.eulerAngles += bodyAngle;

        Debug.DrawRay(pivot.position, pivot.forward * 1f, Color.cyan);
    }

    public static void FirstPersonControl(this Camera cam, Transform pivot, float speed)
    {

    }

    public static Queue<GameObject> QueueDeepCopy(this Queue<GameObject> origin)
    {
        Queue<GameObject> copy = new Queue<GameObject>();
        for (int i = 0; i < origin.Count; i++)
        {
            GameObject content = origin.Dequeue();
            origin.Enqueue(content);
            copy.Enqueue(content);
        }
        return copy;
    }

    public static void Vector3Int(this ref Vector3 vector3)
    {
        if (vector3.x < 0)
            vector3.x = -1;
        else if (vector3.x > 1)
            vector3.x = 1;
        else vector3.x = 0;

        if (vector3.y < 0)
            vector3.y = -1;
        else if (vector3.y > 1)
            vector3.y = 1;
        else vector3.y = 0;

        if (vector3.z < 0)
            vector3.z = -1;
        else if (vector3.z > 1)
            vector3.z = 1;
        else vector3.z = 0;
    }

}
