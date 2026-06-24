using UnityEngine;

public class SC_FearZoneCollisions : MonoBehaviour
{
    // ----- VARIABLES ----- //

    private const string M_TARGET_TAG = "AdrenalineEffectZone";

    // The amount of time it takes for adrenaline to change by one fear point at base
    private const float M_ADRENALINE_DROP_TIME = 20;
    private const float M_SAFE_ZONE_VALUE = -10;
    private const float M_NEAR_PLAYER_ZONE_VALUE = M_SAFE_ZONE_VALUE / 2;

    private SC_PlayerData mPlayerData;

    private SC_GameManager mGameManager;


    // ----- FUNCTIONS ----- //

    private void Start()
    {
        mPlayerData = GetComponent<SC_PlayerData>();

        mGameManager = FindAnyObjectByType<SC_GameManager>();
    }

    private void Update()
    {
        if (!mGameManager.AreStatsLocked())
        {
            float currentAdrenaline = mPlayerData.GetAdrenaline();
            float adrenalineChange = (1 / M_ADRENALINE_DROP_TIME) * Time.deltaTime;

            mPlayerData.SetAdrenaline(currentAdrenaline + adrenalineChange);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == M_TARGET_TAG)
        {
            SC_FearData.FearType zoneType = other.GetComponent<SC_ZoneData>().GetFearType();

            float adrenalineEffect;

            if (zoneType == SC_FearData.FearType.SAFE)
            {
                adrenalineEffect = M_SAFE_ZONE_VALUE;
            }
            else if (zoneType == SC_FearData.FearType.NEAR_PLAYER)
            {
                adrenalineEffect = M_NEAR_PLAYER_ZONE_VALUE;
            }
            else
            {
                adrenalineEffect = mPlayerData.GetFearValue(zoneType);
            }

            float currentAdrenaline = mPlayerData.GetAdrenaline();
            float adrenalineChange = (adrenalineEffect / M_ADRENALINE_DROP_TIME) * Time.deltaTime;

            mPlayerData.SetAdrenaline(currentAdrenaline + adrenalineChange);
        }
    }
}
