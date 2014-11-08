using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AiEnemy : MonoBehaviour {

    public CharacterSystem mainTarget;
    public CharacterSystem character;
    public AIState aiState = AIState.idle;
    public List<Transform> patrolPoints;
    public int currentPatrolPoint = 0;
    public float AttackRange = 10;
    public bool chargeAtTarget = true;
    public CharacterDetector charDetector;
    float distanceToTarget;

    void Awake()
    {
        if (!character) 
        {
            if (!(character = GetComponent<CharacterSystem>()))
            {
                Debug.LogError("AiEnemy Error: " + name + "Have no CharacterSystem script ");
            }
        }
        if (!charDetector)
        {
            if(!(charDetector = GetComponentInChildren<CharacterDetector>()))
            {
                Debug.LogError("AiEnemy Error: " + name + "Have no CharacterDetector script in his children");
            }
        }
    }
	
	void FixedUpdate () 
    {
        if (character.IsAlive())
        {
            ChangeState();
            switch (aiState)
            {
                case AIState.idle:
                    IdleStateUpdateFunction();
                    break;
                case AIState.attacking:
                    AttackingStateUpdateFunction();
                    break;
                case AIState.runAway:
                    break;
                default:
                    break;
            }
        }
	
	}

    private void IdleStateUpdateFunction()
    {
        if (patrolPoints.Count > 1)
        {
            Vector3 toPointMoveVector = patrolPoints[currentPatrolPoint].position - transform.position;
            //Debug.Log("To Point Vector" + toPointMoveVector);
            if (Vector3.Distance(patrolPoints[currentPatrolPoint].position, transform.position) > 1)
            {
                //toPointMoveVetor.Normalize();
                //character.Move(toPointMoveVetor.x, toPointMoveVetor.y);
                //if (toPointMoveVetor.y > 0.2)
                //{
                //    character.Jump();
                //}
                //Debug.Log("To Point Vector2" + toPointMoveVector);
                toPointMoveVector = AIMove(toPointMoveVector);
                //Debug.Log("To Point Vector3" + toPointMoveVector);
            }
            else
            {
                currentPatrolPoint++;
                if (currentPatrolPoint >= patrolPoints.Count)
                {
                    currentPatrolPoint = 0;
                }
            }
        }
        else
        {
            if (patrolPoints.Count == 1)
            {
                Vector3 toPointMoveVector = patrolPoints[0].position - transform.position;
                if (Vector3.Distance(patrolPoints[0].position, transform.position) > 1)
                {
                    //toPointMoveVector.Normalize();
                    //character.Move(toPointMoveVector.x, toPointMoveVector.y);
                    //if (toPointMoveVector.y > 0.2) 
                    //{
                    //    character.Jump();
                    //}
                    toPointMoveVector = AIMove(toPointMoveVector);
                }
            }
        }
    }

    private void AttackingStateUpdateFunction()
    {
        if (mainTarget.IsAlive())
        {
            distanceToTarget = Vector3.Distance(mainTarget.transform.position, transform.position);
            //MoveTotarget
            //AttackTarget
            Vector3 moveVector = mainTarget.transform.position - transform.position;
            moveVector.Normalize();
            if (distanceToTarget < AttackRange)
            {
                character.TargetWeaponAt(mainTarget.transform.position);
                character.Shoot();
                if (chargeAtTarget)
                {
                    //character.Move(moveVector.x, moveVector.y);
                    //if (moveVector.y > 0.2)
                    //{
                    //    character.Jump();
                    //}
                    moveVector = AIMove(moveVector);
                }
            }
            else
            {
                moveVector = AIMove(moveVector);
            }

            //Attack him;
        }
        else
        {
            charDetector.detectedCharacters.Remove(mainTarget);
            if (charDetector.detectedCharacters.Count > 0)
            {
                mainTarget = charDetector.GetClosestEnemy();
            }
        }
    }

    private Vector3 AIMove(Vector3 moveVector)
    {
        moveVector.Normalize();
        //rigidbody.velocity = Vector3.right * moveVector.x * 5;
        character.Move(moveVector.y, moveVector.x);
        if (moveVector.y > 0.2)
        {
            character.Jump();
        }
        return moveVector;
    }

    private void ChangeState()
    {
        switch (aiState)
        {
            case AIState.idle:
                if (charDetector.detectedCharacters.Count > 0) 
                {
                    aiState = AIState.attacking;
                    mainTarget = charDetector.GetClosestEnemy();
                }
                break;
            case AIState.attacking:
                if (charDetector.detectedCharacters.Count == 0) 
                {
                    aiState = AIState.idle;
                }
                break;
            case AIState.runAway:
                break;
            default:
                break;
        }
        //throw new System.NotImplementedException();
    }

    public enum AIState 
    { 
        idle,
        attacking,
        runAway
    }
}
