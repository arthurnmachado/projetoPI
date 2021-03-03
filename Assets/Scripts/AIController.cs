using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class AIController : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
    
    public Transform target;                                    // target to aim for
    private Animator animator;

    [SerializeField]
    Weapon currentWeapon;
    private bool hasWeapon;

    private void Start()
    {
        // get the components on the object we need ( should not be null due to require component so no need to check )
        agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();

        agent.updateRotation = true;
        agent.updatePosition = true;
        // transform.localScale = new Vector3(1f, 1f, -1f);
        animator = GetComponent<Animator>();

        SetRagdoll(false);

        hasWeapon = currentWeapon != null;
    }

    private void SetRagdoll(bool isRagdoll)
    {
        foreach (Rigidbody bodyPart in GetComponentsInChildren<Rigidbody>())
            bodyPart.isKinematic = !isRagdoll;
    }

    public void MegaDeath()
    {
        agent.enabled = false;
        target = null;
        SetRagdoll(true);
        if(animator)
            animator.enabled = false;
    }

    private void Update()
    {
        if (target != null)
        {
            if (hasWeapon && currentWeapon.CanFire() && Random.value< 0.1)
            {
                currentWeapon.Fire(target);
            }
            agent.SetDestination(target.position);
        }
        
        animator.SetFloat("Walk", agent.velocity.magnitude/agent.speed);
    }


    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
