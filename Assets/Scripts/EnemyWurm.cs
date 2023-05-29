using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyWurm : Enemy
{
    // Start is called before the first frame update
    [SerializeField] float speed = 2f;
    [SerializeField] float turnspeed = 180f;
    [SerializeField] List<GameObject> bodyparts = new List<GameObject>();
    [SerializeField] List<Vector2> PositionHistory = new List<Vector2>();
    [SerializeField] List<Quaternion> QuaternionsHistory = new List<Quaternion>();
    public GameObject bodypart;
    public GameObject weakbodyPart;
    public int bodyNum = 7;
    public int weakBodyIndex = 3;
    public int maxHistorySize = 250;
    public int Gap = 30;
    public float timeGap = 0.01f;
    [SerializeField] float countup = 0f;

    void Start()
    {
        //GameObject temp = Instantiate(bodyparts[0], transform.position,transform.rotation,transform);
        base.Start();
        IsInVulnerableState = true;
        //spwan weakbody part index randomly
        weakBodyIndex = UnityEngine.Random.Range(0, bodyNum);

        for (int i = 0; i < bodyNum; i++)
        {
            if (i != weakBodyIndex)
            {
                addBody();
            }
            else
            {
                GameObject temp = Instantiate(weakbodyPart);
                Weakspot = temp.GetComponent<CircleCollider2D>();
                temp.GetComponent<wurmBodyCollid>().parent = this;
                bodyparts.Add(temp);
            }
        }

        PositionHistory.Insert(0, this.transform.position);
        QuaternionsHistory.Insert(0, this.transform.rotation);
        countup = 0;
        OnDeath += EnemyWurm_OnDeath;
    }

    private void EnemyWurm_OnDeath(Cell obj)
    {
        foreach (var part in bodyparts)
        {
            Destroy(part);
        }
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        //store position of head
        if (countup >= timeGap)
        {
            PositionHistory.Insert(0, this.transform.position);
            QuaternionsHistory.Insert(0, this.transform.rotation);
            countup = 0f;
        }
        //move forward
        //this.transform.position += transform.up * speed * Time.deltaTime;


        countup += Time.deltaTime;
        //check the length of the list
        // Debug.Log(PositionHistory.Count);

        if (PositionHistory.Count >= maxHistorySize)
        {
            PositionHistory.RemoveAt(PositionHistory.Count - 1);
            QuaternionsHistory.RemoveAt(QuaternionsHistory.Count - 1);
        }

        //move bodyparts
        int index = 1;
        foreach (var part in bodyparts)
        {
            //find position for each body part
            Vector2 pos = PositionHistory[math.min(index * Gap, PositionHistory.Count - 1)];
            Quaternion rot = QuaternionsHistory[math.min(index * Gap, PositionHistory.Count - 1)];

            part.transform.rotation = rot;
            part.transform.position = pos;

            index++;
        }
    }

    private void addBody()
    {
        GameObject temp = Instantiate(bodypart);
        temp.GetComponent<wurmBodyCollid>().parent = this;
        bodyparts.Add(temp);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }
}