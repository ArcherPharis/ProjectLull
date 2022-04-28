using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhoulFSM : FSM
{
    public enum ActionState
    {
        None, Patrol, Chase, AgressiveChase, Attack, Dead
    }

    public ActionState currentState;
    //[SerializeField] float speed;
    //[SerializeField] float rotateSpeed;
    [SerializeField] Enemy enemy;
    [SerializeField] float detectionRadius;
    NavMeshAgent agent;
    bool isDead;
    CharacterController controller;

    //chase and attack are pretty similar, consider combining the two.
    //or just keep attack and make it harder for the enemy to lose interest in you once they get close enough compared to chase.

    //consider making an idle state in between patrolling states.
    //also need to add if they get shot at, or I guess just take damage *while* in patrol state, to immediately come after the
    //player. They'll give up after..let's say 10 seconds and if they never enter the chase state, they will go back to patrol


    protected override void Initialize()
    {
        currentState = ActionState.Patrol;
        isDead = false;
        elapsedTime = 0.0f;
        meleeAttackRate = 2.0f;
        agent = GetComponent<NavMeshAgent>();


        FindNextPoint();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        controller = GetComponent<CharacterController>();
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
            case ActionState.Patrol: UpdatePatrolState(); SetSpeedParams(1f, 3f); break;
            case ActionState.Chase: UpdateChaseState(); SetSpeedParams(3f, 12f); break;
            case ActionState.AgressiveChase: UpdateAggressiveChase(); SetSpeedParams(3.5f,12f); break;
            case ActionState.Attack: UpdateAttackState(); break;
            case ActionState.Dead: UpdateDeadState(); break;
        }
        elapsedTime += Time.deltaTime;
        if(enemy.Health <= 0)
        {
            currentState = ActionState.Dead;
        }
    }

    private void UpdateAttackState()
    {
        //destinationPosition = playerTransform.position;
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance >= 0.1f && distance < 1.5f)
        {
            agent.isStopped = true;
            Quaternion targetRotation = Quaternion.LookRotation(destinationPosition - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * agent.angularSpeed);
            Attack();

        }
        else if (elapsedTime >= meleeAttackRate && distance > 1.6f)
        {
            agent.isStopped = false;
            currentState = ActionState.AgressiveChase;
        }
        agent.isStopped = false;


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
            enemy.Die();
        }
    }

    private void UpdateAggressiveChase()
    {
        //Vector3 playerPos = new Vector3(playerTransform.position.x, 0, playerTransform.position.z); //prevents enemy from "jumping" with player. (old system)
        destinationPosition = playerTransform.position;
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if(distance >= 0.8f && distance < detectionRadius + 15)
        {
            Quaternion targetRotation = Quaternion.LookRotation(destinationPosition - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * agent.angularSpeed);

            
            //transform.Translate(Vector3.forward * Time.deltaTime * enemy.speed);
        }

        else if(distance < 1.5f)
        {
            currentState = ActionState.Attack;
        }
        else if(distance >= detectionRadius + 15)
        {
            currentState = ActionState.Patrol;
        }
        agent.destination = destinationPosition;

    }

    private void Attack()
    {
        if (elapsedTime >= meleeAttackRate)
        {
            Debug.Log("I would attack here");
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
        Quaternion targetRotation = Quaternion.LookRotation(destinationPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * agent.angularSpeed);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * enemy.rotateSpeed);
        agent.destination = destinationPosition;
        //transform.Translate(Vector3.forward * Time.deltaTime * enemy.speed);
        enemy.ChangeMovementSpeed(1f, 0.3f);

    }

    private void UpdatePatrolState()
    {//finds another point once ghoul gets close to a waypoint
        if(Vector3.Distance(transform.position, destinationPosition) <= 2f)
        {
            Debug.Log("Reached destination point \n" + "....calculating next point");
            FindNextPoint();
        }
        else if(Vector3.Distance(transform.position, playerTransform.position) <= detectionRadius)
        {
            Debug.Log("*sniff* *sniff* I smell the player...switching to Chasing..");
            currentState = ActionState.Chase;
        }

        //rotates enemy towards their target
        Quaternion targetRotation = Quaternion.LookRotation(destinationPosition - transform.position);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * enemy.rotateSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * agent.angularSpeed);

        ////move forward
        //transform.Translate(Vector3.forward * Time.deltaTime * enemy.speed);
        agent.destination = destinationPosition;
        enemy.ChangeMovementSpeed(0f, 0.3f);
    }

    private void FindNextPoint()
    {
        //assigns a random point from list of waypoints
        Debug.Log("...finding next point");
        int randomIndex = UnityEngine.Random.Range(0, wayPointList.Length);
        float randomRadius = 10.0f;
        Vector3 randomPosition = Vector3.zero;
        destinationPosition = wayPointList[randomIndex].transform.position + randomPosition;
        //checks to make sure we don't get the same number twice
        if (IsInCurrentRange(destinationPosition))
        {
            randomPosition = new Vector3(UnityEngine.Random.Range(-randomRadius, randomRadius), 0.0f, UnityEngine.Random.Range(-randomRadius, randomRadius));
            destinationPosition = wayPointList[randomIndex].transform.position + randomPosition;
        }

    }

    private bool IsInCurrentRange(Vector3 position)
    {
        float xPos = Mathf.Abs(position.x - transform.position.x);
        float zPos = Mathf.Abs(position.z - transform.position.z);

        if(xPos <= 2 && zPos <= 2)
        {
            return true;
        }
        return false;
    }
}
