using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulFSM : FSM
{
    public enum ActionState
    {
        None, Patrol, Chase, Attack, Dead
    }

    public ActionState currentState;
    [SerializeField] float speed;
    [SerializeField] float rotateSpeed;
    [SerializeField] Enemy enemy;
    [SerializeField] float detectionRadius;
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
        meleeAttackRate = 3.0f;


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
            case ActionState.Patrol: UpdatePatrolState(); SetSpeedParams(1, 0.5f); break;
            case ActionState.Chase: UpdateChaseState(); SetSpeedParams(3, 12); break;
            case ActionState.Attack: UpdateAttackState(); SetSpeedParams(3,12); break;
            case ActionState.Dead: UpdateDeadState(); break;
        }
        elapsedTime += Time.deltaTime;
        if(enemy.Health <= 0)
        {
            currentState = ActionState.Dead;
        }
    }

    void SetSpeedParams(float newSpeed, float newRotateSpeed)
    {
        speed = Mathf.Lerp(speed, newSpeed, Time.deltaTime * 20f);
        rotateSpeed = Mathf.Lerp(rotateSpeed, newRotateSpeed, Time.deltaTime * 5f);
    }
    private void UpdateDeadState()
    {
        if (!isDead)
        {
            isDead = true;
            enemy.Die();
        }
    }

    private void UpdateAttackState()
    {
        destinationPosition = playerTransform.position;
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if(distance >= 0.1f && distance < detectionRadius + 15)
        {
            Quaternion targetRotation = Quaternion.LookRotation(destinationPosition - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);

            transform.Translate(Vector3.forward * Time.deltaTime * speed);

            if(distance >= 0.1f && distance < 2f)
            {
                Attack();
            }
        }
        else if(distance >= detectionRadius + 15)
        {
            currentState = ActionState.Patrol;
        }
        
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

        if(distanceToPlayer <= 1f)
        {
            Debug.Log("Time to attack!");
            currentState = ActionState.Attack;
        }
        else if(distanceToPlayer >= detectionRadius + 5)
        {
            currentState = ActionState.Patrol;
        }
        Quaternion targetRotation = Quaternion.LookRotation(destinationPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
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
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);

        //move forward
        //controller.Move(Vector3.forward * Time.deltaTime * speed);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
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
