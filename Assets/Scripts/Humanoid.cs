using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Humanoid : MonoBehaviour
{
    [SerializeField] Transform[] patrolPoints;
    public NavMeshAgent agent;
    [SerializeField] int index = 0;

    public Transform target;
    
    public float fov = 90;

    public float useFOV
    {
        get { return Mathf.Cos(fov * 0.5f * Mathf.Deg2Rad); }
    }
    
    IAgentStates currentState = new PatrolState();
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        NewPathPoint();
    }

    public void ChangeState(IAgentStates newState)
    {
        currentState.Exit(this);
        
        currentState = newState;
        newState.Enter(this);
    }

    void Update()
    {
        currentState.Update(this);
    }

    public void Patrol()
    {
        if (HasReachedTargetPoint())
        {
            index++;
            index %= patrolPoints.Length;

            NewPathPoint();
        }
    }

    void NewPathPoint()
    {
        Vector3 pointOne = patrolPoints[index].position;
        NavMesh.SamplePosition(pointOne, out var hit, 50, NavMesh.AllAreas);
        agent.SetDestination(hit.position);
    }

    bool HasReachedTargetPoint()
    {
        Vector3 positionFlat = transform.position;
        positionFlat.y = 0;
        
        float distance = Vector3.Distance(agent.pathEndPosition, positionFlat);
        Debug.Log(distance);

        if (distance < 0.75)
        {
            return true;
        }

        return false;
    }

    void OnDrawGizmos()
    {
        Vector3 forward = transform.forward;
        Vector3 up = transform.up;

        float radius = Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad) * 5;

        float linDis = Mathf.Sqrt(Mathf.Pow(5, 2) + Mathf.Pow(radius, 2));

        Vector3 sideRight = Quaternion.AngleAxis(fov * 0.5f, up) * forward;
        Vector3 sideLeft = Quaternion.AngleAxis(-fov * 0.5f, up) * forward;


        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + sideRight * linDis);
        Gizmos.DrawLine(transform.position, transform.position + sideLeft * linDis);
    }
}

class PatrolState : IAgentStates
{
    public void Enter(Humanoid h)
    {
        Debug.Log("Entered Patrol");
    }
    
    public void Update(Humanoid h)
    {
        h.Patrol();

        if (h.target != null)
        {
            if (Physics.Raycast(h.transform.position, h.target.position - h.transform.position, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "Player")
                {
                    if(Vector3.Dot(h.transform.forward, (h.target.position - h.transform.position).normalized) > h.useFOV)
                    {
                        Debug.Log("Found Player");
                        
                        h.ChangeState(new ChasingState());
                    }
                }
            }
        }
    }
    
    public void Exit(Humanoid h)
    {
        Debug.Log("Exited Patrol");
    }
}

class ChasingState : IAgentStates
{
    public void Enter(Humanoid h)
    {
        Debug.Log("Chasing Player");
    }
    
    public void Update(Humanoid h)
    {
        h.agent.SetDestination(h.target.position);
        
        if (Physics.Raycast(h.transform.position, h.target.position - h.transform.position, out RaycastHit hit,
                Mathf.Infinity))
        {
            if (hit.collider.tag != "Player")
            {
                h.ChangeState(new PatrolState());
                return;
            }
        }
        
        if(Vector3.Dot(h.transform.forward, (h.target.position - h.transform.position).normalized) < h.useFOV)
            h.ChangeState(new PatrolState());
    }
    
    public void Exit(Humanoid h)
    {
        Debug.Log("Lost Player");
    }
}

public interface IAgentStates
{
    void Enter(Humanoid h);
    void Update(Humanoid h);
    void Exit(Humanoid h);
}
