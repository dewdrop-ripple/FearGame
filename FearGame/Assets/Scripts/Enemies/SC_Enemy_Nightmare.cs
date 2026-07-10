using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SC_Enemy_Nightmare : MonoBehaviour
{
    // ----- VARIABLES ----- //

    [SerializeField] private GameObject mTargetPlayer;
    [SerializeField] private NavMeshAgent mAgent;
    [SerializeField] private GameObject mTarget;

    [SerializeField] private float mAcceptanceDistance;

    [SerializeField] private float mMinTeleportDistance;
    [SerializeField] private float mMaxTeleportDistance;

    [SerializeField] private GameObject mParent;

    private GameObject[] mRespawnPoints;


    // ----- FUNCTIONS ----- //

    private void Start()
    {
        mRespawnPoints = GameObject.FindGameObjectsWithTag("NightmareRespawn");
    }

    // Update target
    // Move towards target player
    private void Update()
    {
        // Update target
        GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");

        if (playerList.Length > 0)
        {
            GameObject selection = playerList[0];

            for (int i = 1; i < playerList.Length; i++)
            {
                if (playerList[i].GetComponent<SC_PlayerData>().GetAdrenaline() > selection.GetComponent<SC_PlayerData>().GetAdrenaline())
                {
                    selection = playerList[i];
                }
            }

            mTargetPlayer = selection;

            // Movement
            mAgent.SetDestination(mTargetPlayer.transform.position);

            // When player is caught, teleport to a random location
            if (Vector3.Distance(transform.position, mTargetPlayer.transform.position) <= mAcceptanceDistance)
            {
                Vector3 teleportPos = GetRandomRespawnPoint();

                transform.position = teleportPos;
            }
        }
        else
        {
            // If no players, stand still
            mAgent.SetDestination(mAgent.transform.position);
        }
    }
    
    public void SetTargetPlayer(GameObject target)
    {
        mTargetPlayer = target;
    }

    public GameObject GetTargetPlayer()
    {
        return mTargetPlayer;
    }

    // Copied from Unity Documentation Example
    private Vector3 GetRandomNavMeshPoint()
    {
        for (int i = 0; i < 45; i++)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * mMaxTeleportDistance;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas) && 
                Vector3.Distance(transform.position, hit.position) >= mMinTeleportDistance)
            {
                return hit.position;
            }
        }

        return transform.position;
    }

    private Vector3 GetRandomRespawnPoint()
    {
        if (mRespawnPoints.Length == 0)
        {
            return new Vector3(0, 0, 0);
        }

        int randomPointIndex = (int) Mathf.Floor(Random.Range(0.0f, mRespawnPoints.Length - 0.01f));

        return mRespawnPoints[randomPointIndex].transform.position;
    }
}
