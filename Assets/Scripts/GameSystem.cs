using System.Net.Http.Headers;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    [SerializeField]
    protected Transform spawn_Transform;
    [SerializeField]
    protected float gapSize = 140;
    [SerializeField]
    protected GameData gameData;

    protected bool startGame;

    void Start()
    {
        InitGame();
    }

    public virtual void InitGame()
    {
    }

    public virtual void StartGame()
    { 
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnObject();
        }
    }

    public virtual void SpawnObject()
    {

    }
}
