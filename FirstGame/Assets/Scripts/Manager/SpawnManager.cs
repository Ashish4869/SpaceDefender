using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script is for spawning enemies in waves
/// </summary>
public class SpawnManager : MonoBehaviour
{
    //To define the different states in spawning process
    public enum SpawnState  {SPAWNING , WAITING , COUNTING , STOPSPAWNING};


    // A class that holds varialbles for spawning and a function to increase wave difficulty after 5 waves
    [System.Serializable]
    public class Wave
    {
        public string name;
        public GameObject _crabEnemy, _spiderEnemy, _OctoEnemy;
        public float _Enemycount;
        public float _SpawnRate;

        public void WaveUp()
        {
            _Enemycount *= 2f;
            _SpawnRate *= 2f;
        }
    }

    public Wave[] _waves;
    public int _nextWave;
    public float _timeBetweenWaves = 3f;
    public float _waveCountDown;
    public float _timeBetweenWaveEndChecks = 1f;
    bool isBossWave = false;
    SpawnState _state = SpawnState.COUNTING;
    
    [SerializeField]
    GameObject TopLeft;
    [SerializeField]
    GameObject BottomLeft;
    [SerializeField]
    UIManager _wave;

    private void Start()
    {
        _waveCountDown = _timeBetweenWaves;
        Wall.WallFallen += WallhasFallen;
    }
    // Update is called once per frame
    void Update()
    {
        if(_state != SpawnState.STOPSPAWNING)
        {
            if (_state == SpawnState.WAITING)
            {
                if (!IsEnemyAlive())
                {
                    NextWave();
                }
                else
                {
                    return;
                }
            }


            if (_waveCountDown <= 0)
            {
                if (_state != SpawnState.SPAWNING)
                {
                    StartCoroutine(SpawnWave(_waves[_nextWave]));
                }
            }
            else
            {
                _waveCountDown -= Time.deltaTime;
            }

        }
    }

    //spawn the next wave if no more enemies are remaining
    bool IsEnemyAlive()
    {
        _timeBetweenWaveEndChecks -= Time.deltaTime;

        if(_timeBetweenWaveEndChecks <= 0f)
        {
            _timeBetweenWaveEndChecks = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }
    //update the values so that next waves can be spawned
    void NextWave()
    {
        _state = SpawnState.COUNTING;
        _waveCountDown = _timeBetweenWaves;

        if(_nextWave + 1 == 4)
        {
            isBossWave = true;
        }

        if(_nextWave + 1 > _waves.Length - 1)
        {
            for(int i =0; i<_waves.Length; i++)
            {
                _waves[i].WaveUp();
            }

            _nextWave = 0;
        }
        else
        {
            _nextWave++;
        }

        _wave.NewWave();
    }

    //spawn wave
    IEnumerator SpawnWave(Wave CurrentWave)
    {
        _state = SpawnState.SPAWNING;
        for(int i = 0; i< CurrentWave._Enemycount; i++)
        {
            SpawnEnemies(CurrentWave);

            yield return new WaitForSeconds(1 / CurrentWave._SpawnRate);
        }

        _state = SpawnState.WAITING;

        yield break;
    }

    
    //Spawn enemeis in random pos on the y
    void SpawnEnemies(Wave ThisWave)
    {
        Vector2 SpawnPoint = new Vector2(transform.position.x,
              Random.Range(TopLeft.transform.position.y, BottomLeft.transform.position.y));
        int EID = Random.Range(0, 3);

        switch (EID)
        {
            case 0:
                Instantiate(ThisWave._crabEnemy, SpawnPoint, transform.rotation);
                break;

            case 1:
                Instantiate(ThisWave._spiderEnemy, SpawnPoint, transform.rotation);
                break;

            case 2:
                Instantiate(ThisWave._OctoEnemy, SpawnPoint, transform.rotation);
                break;
        }

        if(isBossWave == true)
        {
            FindObjectOfType<Enemy>().GetComponent<Enemy>().Bossify();
            isBossWave = false;
        }
    }


    //stop spawning once walll is broken
    void WallhasFallen()
    {
        _state = SpawnState.STOPSPAWNING;
        Wall.WallFallen -= WallhasFallen;
    }

}

