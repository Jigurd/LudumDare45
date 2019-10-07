using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    // Start is called before the first frame update

    
    [SerializeField]
    private GameObject _ghostPrefab;
    [SerializeField]
    float ghostMinInterval = 15;
    [SerializeField]
    float ghostMaxInterval = 25;
    float timeToGhost=10;
    private float _lastSpawn=0;

    private GameObject _player;
    void Start()
    {
        _player = GameObject.Find("Player");
    }

 
    void Update()
    {
        if(GameState.Paused)
        {
            return;
        }

        //if it's time for a ghost to spawn
        if(Time.time > _lastSpawn+timeToGhost)
        {
            _lastSpawn = Time.time; //update last spawn
            timeToGhost = Random.Range(ghostMinInterval, ghostMaxInterval); //set a time for next ghost

            //spawn a ghost
            Instantiate(_ghostPrefab, randomDirection(), Quaternion.identity);
        }
    }

    Vector3 randomDirection()
    {
        float angle = Random.Range(0, Mathf.PI * 2);
        Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)).normalized * 40;
        return _player.transform.position + direction;
    }
}
