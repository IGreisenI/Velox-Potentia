using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour, IGameEventListener<EnemyDeathInfo>
{
    public GameObject obj;
    public float interval;
    public float amount;
    public int enemyLimit;
    public GameObject target;
    public List<Transform> _transforms;
    public TextMeshProUGUI enemyCounter;
    public EnemyDeathEvent enemyDeathEvent;
    public GameEndEvent gameEndEvent;

    private List<GameObject> enemyList = new List<GameObject>();

    private void OnEnable()
    {
        enemyDeathEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        enemyDeathEvent.UnregisterListener(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyCounter.text = amount.ToString();
        StartCoroutine(spawnObject());
    }

    public IEnumerator spawnObject()
    {
        float passedTime = 0;
        float maxTime = interval * amount;

        while (passedTime < maxTime)
        {
            if(enemyLimit < enemyList.Count)
            {
                yield return new WaitForSeconds(interval);
            }
            else { 
                //obj.GetComponent<Enemy>().player = target;
                GameObject enemy = Instantiate(obj, _transforms[Random.Range(0, _transforms.Count)]);
                enemy.transform.parent = null;
                enemy.GetComponent<Enemy>().player = target;
                enemyList.Add(enemy);

                passedTime += interval;

                yield return new WaitForSeconds(interval);
            }
        }
    }

    public void OnEventRaised(EnemyDeathInfo arg)
    {
        enemyList.RemoveAt(0);
        amount -= 1;
        enemyCounter.text = amount.ToString();
        if(amount <= 0)
        {
            gameEndEvent.Raise(new GameEndInfo("Congratz!!!"));
        }
    }
}
