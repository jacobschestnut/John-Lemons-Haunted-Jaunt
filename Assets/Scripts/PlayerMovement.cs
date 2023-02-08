using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Initializes a float for turn speed value
    public float turnSpeed = 20f;

    // Creates an instance of the Animator component to only be used locally with this class AKA a "member component" i.e. "m_"
    Animator m_Animator;

    // Creates a reference to the Rigidbody component the same way
    Rigidbody m_Rigidbody;

    // Creates an instance of the Vector3 component to only be used locally with this class
    Vector3 m_Movement;

    // A way of storing rotations - giving it a value of no rotation
    Quaternion m_Rotation = Quaternion.identity;

    // Start is called before the first frame update
    void Start()
    {
        // "Get a reference to a component of type 'Animator', and assign it to the variable called m_Animator."
        m_Animator = GetComponent<Animator>();

        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // "Sets" the 3 vector values every time the frame updates
        m_Movement.Set(horizontal, 0f, vertical);

        // "Normalizes" the the vector values back to 1 so that speed is the same in every direction
        m_Movement.Normalize();

        // Mathf method that returns True if the values are approximately the same. For naming simplicity we invert the bool with "!" 
        // These methods are used to determine whether or not the horizontal/vertical input has changed when the frame updates
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);

        // Consolidates the bools above and translates to "is the system recieving any horizontal input OR vertical input?"
        bool isWalking = hasHorizontalInput || hasVerticalInput;

        // This is communicating with the animator component in the Unity suite, changing the bool and, thus, playing the movement animation
        m_Animator.SetBool("IsWalking", isWalking);

        // This code creates a Vector3 variable called desiredForward.
        // It sets it to the return of a method called RotateTowards, which is a static method from the Vector3 class. RotateTowards takes four parameters — the first two are Vector3s, and are the vectors that are being rotated from and towards respectively.
        // The code starts with transform.forward, and aims for the m_Movement variable. transform.forward is a shortcut to access the Transform component and get its forward vector.
        // The next two parameters are the amount of change between the starting vector and the target vector: first the change in angle (in radians) and then the change in magnitude.  This code changes the angle by turnSpeed * Time.deltaTime and the magnitude by 0.
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);

        // This line simply calls the LookRotation method and creates a rotation looking in the direction of the given parameter
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    void OnAnimatorMove()
    // This method allows you to apply root motion as you want, which means that movement and rotation can be applied separately.
    // This way the physics engine and the animations don't conflict
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);

        m_Rigidbody.MoveRotation(m_Rotation);
    }
}
