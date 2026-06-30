using UnityEngine;
using UnityEngine.AI;

public class SC_Enemy_Movement : MonoBehaviour
{
    // ----- VARIABLES ----- //

    public enum EnemyState { CHASE_PLAYER, MOVE_TO_TARGET, STAND_STILL }

    [SerializeField] private GameObject mTargetPlayer;
    [SerializeField] private NavMeshAgent mAgent;
    [SerializeField] private GameObject mTarget;

    [SerializeField] private EnemyState mCurrentState = EnemyState.STAND_STILL;

    [SerializeField] private float mAcceptanceDistance;

    // ----- FUNCTIONS ----- //

    // If moving, move
    //      If chasing a player, do that
    //      Otherwise, go to target
    // Otherwise, stay put
    private void Update()
    {
        switch(mCurrentState)
        {
            case EnemyState.CHASE_PLAYER:
                Debug.Log("Moving to player: " + mTargetPlayer.transform.position);
                mAgent.SetDestination(mTargetPlayer.transform.position);

                if (Vector3.Distance(transform.position, mTargetPlayer.transform.position) <= mAcceptanceDistance)
                {
                    mCurrentState = EnemyState.STAND_STILL;
                }

                break;

            case EnemyState.MOVE_TO_TARGET:
                Debug.Log("Moving to target: " + mTarget.transform.position);
                mAgent.SetDestination(mTarget.transform.position);

                if (Vector3.Distance(transform.position, mTarget.transform.position) <= mAcceptanceDistance)
                {
                    mCurrentState = EnemyState.STAND_STILL;
                }

                break;

            default:
                Debug.Log("Staying still: " + mAgent.transform.position);
                mAgent.SetDestination(mAgent.transform.position);
                break;
        }
    }
    
    public void SetState(EnemyState state)
    {
        mCurrentState = state;
    }
    
    public void SetTargetPlayer(GameObject target)
    {
        mTargetPlayer = target;
    }

    public GameObject GetTargetPlayer()
    {
        return mTargetPlayer;
    }

    public void SetTargetLocation(Vector3 location)
    { 
        mTarget.transform.position = location;
    }

    public Vector3 GetTargetLocation()
    {
        return mTarget.transform.position;
    }
}
