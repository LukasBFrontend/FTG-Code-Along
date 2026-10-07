using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Humanoid : MonoBehaviour
{
    [SerializeField] Transform[] patrolPoints;
    NavMeshAgent agent;
    [SerializeField] int index = 0;

    public Transform target;
    
    public float fov = 90;

    public float useFOV
    {
        get{return Mathf.Cos(Mathf.Deg2Rad * fov *0.5f);}
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        NewPathPoint();
    }

    // Update is called once per frame
    void Update()
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

class Patrol : IAgentStates
{
    public void Enter(Humanoid h)
    {
        Debug.Log("Entered Patrol");
    }
    
    public void Update(Humanoid h)
    {
        
    }
    
    public void Exit(Humanoid h)
    {
        
    }
}

class Chasing : IAgentStates
{
    public void Enter(Humanoid h)
    {
        Debug.Log("Found Player");
    }
    
    public void Update(Humanoid h)
    {
        
    }
    
    public void Exit(Humanoid h)
    {
        
    }
}

public interface IAgentStates
{
    void Enter(Humanoid h);
    void Update(Humanoid h);
    void Exit(Humanoid h);
}
