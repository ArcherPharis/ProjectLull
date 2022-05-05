using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhoulFSM : FSM
{
    public enum ActionState
    {
        None, Patrol, Chase, AgressiveChase, Attack, UnderFire, Confused, Dead
    }
    public ActionState currentState;
    [SerializeField] Enemy enemy;
    [SerializeField] float detectionRadius;
    [SerializeField] float confusedWonderRadius;
    [SerializeField] float lookForPlayerTime = 10f;
    float patrolElapseTime;
    NavMeshAgent agent;
    bool isDead;
    Player player;

    protected override void Initialize()
    {
        currentState = ActionState.Patrol;
        isDead = false;
        elapsedTime = 0.0f;
        patrolElapseTime = 0.0f;
        meleeAttackRate = 2.0f;
        agent = GetComponent<NavMeshAgent>();


        FindNextPoint();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<Player>();
        playerTransform = playerObject.transform;

        if (!playerTransform)
        {
            Debug.Log("There's no player in the scene! It needs a Player tag.");
        }
    }

    protected override void FSMUpdate()
    {
        switch (currentState)
        {
            case ActionState.Patrol: UpdatePatrolState(); SetSpeedParams(1.5f, 3f); break;
            case ActionState.Chase: UpdateChaseState(); SetSpeedParams(3f, 12f); break;
            case ActionState.AgressiveChase: UpdateAggressiveChase(); SetSpeedParams(3.5f,12f); break;
            case ActionState.UnderFire: UpdateUnderFireState(); SetSpeedParams(3.5f, 12f); break;
            case ActionState.Attack: UpdateAttackState(); break;
            case ActionState.Confused: UpdateConfusedState(); break;
            case ActionState.Dead: UpdateDeadState(); break;
        }
        elapsedTime += Time.deltaTime;
        
        if(enemy.Health <= 0)
        {
            currentState = ActionState.Dead;
        }
    }

    private void UpdateConfusedState()
    {
        

        if (elapsedTime >= 5)
        {
            
            Vector3 newPosition = InsideSphere(transform.position, confusedWonderRadius);
            destinationPosition = newPosition;
            agent.SetDestination(destinationPosition);
            enemy.SetConfusedState(false);
            elapsedTime = 0;
            

            if (patrolElapseTime >= lookForPlayerTime)
            {
                currentState = ActionState.Patrol;
                patrolElapseTime = 0f;
            }
        }

        else if(agent.remainingDistance <= agent.stoppingDistance)
        {
            patrolElapseTime += Time.deltaTime;
            enemy.SetConfusedState(true);
        }

        if(Vector3.Distance(transform.position, playerTransform.position) <= detectionRadius)
        {
            currentState = ActionState.AgressiveChase;
            enemy.SetConfusedState(false);
        }
        if (enemy.OnHealthChange())
        {
            currentState = ActionState.UnderFire;
            enemy.SetConfusedState(false);
        }

        Quaternion targetRotation = Quaternion.LookRotation(destinationPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * agent.angularSpeed);

    }

     Vector3 InsideSphere(Vector3 originPosition, float distance)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
        randomDirection += originPosition;
        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, -1);
        return navHit.position;
    }

    private void UpdateUnderFireState()
    {
        destinationPosition = playerTransform.position;
        Quaternion targetRotation = Quaternion.LookRotation(destinationPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * agent.angularSpeed);
        agent.destination = destinationPosition;

        if (agent.remainingDistance <= 1.2f)
        {
            currentState = ActionState.AgressiveChase;
        }
        else if (player.playerTeleported)
        {
            currentState = ActionState.Confused;
        }



    }

    private void UpdateAttackState()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        Quaternion targetRotation = Quaternion.LookRotation(destinationPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * agent.angularSpeed);
        if (distance >= 0.1f && distance < 1.5f)
        {
  
            Attack();

        }
        else if (elapsedTime >= meleeAttackRate && distance > 1.6f)
        {
            currentState = ActionState.AgressiveChase;
        }
        else if (player.playerTeleported)
        {
            currentState = ActionState.Confused;
        }



    }

    void SetSpeedParams(float newSpeed, float newRotateSpeed)
    {
        agent.speed = Mathf.Lerp(agent.speed, newSpeed, Time.deltaTime * 20f);
        agent.angularSpeed = Mathf.Lerp(agent.angularSpeed, newRotateSpeed, Time.deltaTime * 5f);
    }
    private void UpdateDeadState()
    {
        if (!isDead)
        {
            isDead = true;
            agent.isStopped = true;
            enemy.Die();
        }
    }

    private void UpdateAggressiveChase()
    {
        
        destinationPosition = playerTransform.position;
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if(distance >= 0.8f && distance < detectionRadius + 10)
        {
            Quaternion targetRotation = Quaternion.LookRotation(destinationPosition - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * agent.angularSpeed);

            
        }

        else if(distance < 1.5f)
        {
            currentState = ActionState.Attack;
        }
        else if(distance >= detectionRadius + 10)
        {
            currentState = ActionState.Confused;
        }
        agent.destination = destinationPosition;

    }

    private void Attack()
    {
        if (elapsedTime >= meleeAttackRate)
        {
            enemy.Attack();
            elapsedTime = 0.0f;
        }
    }

    private void UpdateChaseState()
    {
        //change the current Destination to the 
        destinationPosition = playerTransform.position;
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        
        if (distanceToPlayer <= 2f)
        {
            
            currentState = ActionState.AgressiveChase;
        }
        else if(distanceToPlayer >= detectionRadius + 5)
        {
            currentState = ActionState.Patrol;
        }
        else if (player.playerTeleported)
        {
            currentState = ActionState.Confused;
        }
        Quaternion targetRotation = Quaternion.LookRotation(destinationPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * agent.angularSpeed);
        agent.destination = destinationPosition;
        enemy.ChangeMovementSpeed(1f, 0.3f);

    }

    private void UpdatePatrolState()
    {//finds another point once ghoul gets close to a waypoint
        agent.stoppingDistance = 0f;
        if(Vector3.Distance(transform.position, destinationPosition) <= 0.2f)
        {
            Debug.Log("Reached destination point \n" + "....calculating next point");
            FindNextPoint();


        }
        else if (enemy.OnHealthChange())
        {

            agent.stoppingDistance = 1.3f;
            currentState = ActionState.UnderFire;
        }
        else if(Vector3.Distance(transform.position, playerTransform.position) <= detectionRadius)
        {
            Debug.Log("*sniff* *sniff* I smell the player...switching to Chasing..");
            agent.stoppingDistance = 1.3f;
            currentState = ActionState.Chase;
        }

        //rotates enemy towards their target
        Quaternion targetRotation = Quaternion.LookRotation(destinationPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * agent.angularSpeed);

        //move forward
        agent.destination = destinationPosition;
        enemy.ChangeMovementSpeed(0f, 0.3f);
    }


    private void FindNextPoint()
    {
        //assigns a random point from list of waypoints
        Debug.Log("...finding next point");
        int randomIndex = UnityEngine.Random.Range(0, wayPointList.Length);
        Vector3 randomPosition = Vector3.zero;
        destinationPosition = wayPointList[randomIndex].transform.position + randomPosition;
    }

}
