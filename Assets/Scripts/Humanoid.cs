using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Humanoid : MonoBehaviour
{
    [SerializeField] Transform[] patrolPoints;
    NavMeshAgent agent;
    [SerializeField] int index = 0;
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
}
