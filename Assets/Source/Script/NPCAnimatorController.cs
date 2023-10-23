using System.Collections.Generic;
using UnityEngine;

public class NPCAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private  List<Collider> colliders;

    private bool isGrounded = false;

    private void Awake()
    {
        if(!animator) animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for(int index = 0; index < contactPoints.Length; index ++ )
        {
            if (Vector3.Dot(contactPoints[index].normal, Vector3.up) > 0.5)
            {
                if (!colliders.Contains(collision.collider))
                {
                    colliders.Add(collision.collider);
                }

                isGrounded = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true;
                break;
            }
        }

        if (validSurfaceNormal)
        {
            isGrounded = true;
            if (!colliders.Contains(collision.collider))
            {
                colliders.Add(collision.collider);
            }
        }
        else
        {
            if (colliders.Contains(collision.collider))
            {
                colliders.Remove(collision.collider);
            }
            if (colliders.Count == 0) { isGrounded = false; }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(animator)
        {
            animator.SetBool("Grounded", isGrounded);
        }
    }
}
