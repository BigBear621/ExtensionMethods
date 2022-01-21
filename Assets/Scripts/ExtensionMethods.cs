using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    #region Private Static Variables
    private static float volumeIn;
    private static float volumeOut;

    private static float inputX;
    private static float inputZ;
    private static Vector3 previous;
    private static Vector3 towards;
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
        inputX = Input.GetAxisRaw("Horizontal");
        inputZ = Input.GetAxisRaw("Vertical");
        towards.x = inputX;
        towards.z = inputZ;
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


}
