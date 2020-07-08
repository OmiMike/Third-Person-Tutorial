using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public static ThirdPersonMovement instance;

    [SerializeField] bool isCrouching = false;

    Vector2 inputAxis;

    Rigidbody rb;
    Animator anim;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        inputAxis.x = Input.GetAxisRaw("Horizontal");
        inputAxis.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.C))
            isCrouching = !isCrouching;

        anim.SetBool("isCrouching", isCrouching);
        anim.SetFloat("InputX", inputAxis.x);
        anim.SetFloat("InputY", inputAxis.y);
    }
}
