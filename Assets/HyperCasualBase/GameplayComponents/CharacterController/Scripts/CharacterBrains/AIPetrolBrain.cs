using UnityEngine;
using UnityEngine.AI;

public class AIPetrolBrain : CharacterBrainBase
{
    private NavMeshAgent navMeshAgent;
    public NavMeshAgent NavMeshAgent
    {
        get
        {
            if(navMeshAgent == null)
            {
                navMeshAgent = GetComponent<NavMeshAgent>();
                if (navMeshAgent == null)
                    navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            }
            return navMeshAgent;
        }
    }

    private NavMeshSurface navMeshSurface;
    public NavMeshSurface NavMeshSurface { get { return (navMeshSurface == null) ? navMeshSurface = FindObjectOfType<NavMeshSurface>() : navMeshSurface; } set { navMeshSurface = value; } }

    private float distToAgent;
    private NavMeshPath navMeshPath;
    private Vector3 targetPos = Vector3.zero;

    private bool IsDestinationReach()
    {
        if (!NavMeshAgent.hasPath)
        {
            targetPos = NavMeshSurface.GetRandomPosition(transform.position);
            NavMeshAgent.SetDestination(targetPos);
            return false;
        }

        float distToTarget = Vector3.Distance(transform.position, NavMeshAgent.destination);

        if (distToTarget < NavMeshAgent.stoppingDistance + 0.5f)
        {
            targetPos = NavMeshSurface.GetRandomPosition(transform.position);
            NavMeshAgent.SetDestination(targetPos);
            return true;
        }
        else return false;
    }


    public override void Initialize()
    {
        base.Initialize();
        NavMeshAgent.updatePosition = false;
        NavMeshAgent.updateRotation = false;
        NavMeshAgent.acceleration = 100f;
        NavMeshAgent.speed = 10f;
    }

    public override void Logic()
    {
        if (!NavMeshAgent.enabled)
            return;

        if (navMeshPath == null)
            navMeshPath = new NavMeshPath();

        distToAgent = Vector3.Distance(transform.position, NavMeshAgent.nextPosition);

        NavMeshAgent.isStopped = (distToAgent > 1.5f) ? true : false;

        IsDestinationReach();

        Vector3 direction = (NavMeshAgent.nextPosition - transform.position).normalized;

        if (distToAgent > 0.2f)
            CharacterController.Move(direction);
    }





    public override void Dispose()
    {
        Utilities.DestroyExtended(NavMeshAgent);
        base.Dispose();
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < NavMeshAgent.path.corners.Length; i++)
        {
            Gizmos.color = Color.white;
            if (i != NavMeshAgent.path.corners.Length - 1)
                Gizmos.DrawLine(NavMeshAgent.path.corners[i], NavMeshAgent.path.corners[i + 1]);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(NavMeshAgent.path.corners[i], 0.2f);
        }
    }
}
