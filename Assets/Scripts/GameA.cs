using System.Collections.Generic;
using UnityEngine;

public class GameA : GameSystem
{
    [SerializeField]
    private Obstacle obstacle_Object;
    [SerializeField]
    private Transform end_Transform;

    private float laneIndex;
    private float laneRand;
    private Obstacle newObstacle;
    private List<Obstacle> obstacleList = new List<Obstacle>();
    private List<Obstacle> obstaclePrevList = new List<Obstacle>();

    public override void SpawnObject()
    {
        if (obstaclePrevList.Count > 0)
        {
            newObstacle = obstaclePrevList[0];
            newObstacle.Show();
            obstaclePrevList.RemoveAt(0);
        }
        else
        {
            newObstacle = Instantiate(obstacle_Object, spawn_Transform);
        }

        laneRand = Random.Range(0f, 30f);

        if (laneRand < 10)
            laneIndex = -gapSize;
        else if (laneRand >= 10 && laneRand < 20)
            laneIndex = 0;
        else
            laneIndex = gapSize;

        newObstacle.endPoint = end_Transform.localPosition;
        newObstacle.transform.localPosition = new Vector3 (laneIndex, 0, 0);
        newObstacle.completeHide_Callback += RemoveObject;
        obstacleList.Add(newObstacle);
    }

    void RemoveObject(Obstacle _target)
    {
        obstaclePrevList.Add(_target);
        obstacleList.Remove(_target);
    }
}
