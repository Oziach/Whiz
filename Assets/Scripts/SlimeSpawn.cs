using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawn : MonoBehaviour
{
    [SerializeField] GameObject slimeBirthPrefab;
    [SerializeField] GameObject slimePrefab;
    [SerializeField] GameObject currSlime;

    [SerializeField] float slimeBirthSpawnDelay;
    [SerializeField] float slimeSpawnDelay;

    float slimeBirthSpawnCountdown;
    float slimeSpawnCountdown;

    enum SlimeSpawnState {
        Idle, 
        SlimeBirth,
    }

    SlimeSpawnState currState;

    // Start is called before the first frame update
    void Start()
    {
        slimeBirthSpawnCountdown = slimeBirthSpawnDelay;
        slimeSpawnCountdown = slimeSpawnDelay;
        currState = SlimeSpawnState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if(currState == SlimeSpawnState.Idle) {

            if (currSlime != null) { return; }

            slimeSpawnCountdown -= Time.deltaTime;
            if (slimeSpawnCountdown <= 0) {

                Instantiate(slimeBirthPrefab, transform.position, Quaternion.identity, null);
                slimeSpawnCountdown = slimeSpawnDelay;
                currState = SlimeSpawnState.SlimeBirth;

            }
        }   

        else if(currState == SlimeSpawnState.SlimeBirth){

            slimeBirthSpawnCountdown -= Time.deltaTime;
            
            if (slimeBirthSpawnCountdown <= 0) {
                slimeBirthSpawnCountdown = slimeBirthSpawnDelay;
                currSlime = Instantiate(slimePrefab, transform.position, Quaternion.identity, null);
                currState = SlimeSpawnState.Idle;
            }
        }
    }

}
