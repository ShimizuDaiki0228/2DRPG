using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;

    private Transform player;

    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private string closestCheckpointId;

    [SerializeField] private DescriptionPoint[] descriptionPoints;
    [SerializeField] private string descriptionPointId;

    //チュートリアルのアイテム取得時のテキスト表示
    //チュートリアルとして、最初のアイテムを二つ取得したかどうかを確認する
    private bool isFirstTutorial = false;
    public bool IsFirstTutorial => isFirstTutorial;
    private IDisposable isFirstTutorialSubscription;
    //UIスクリプトで購読して発火されたときに、特殊テキストを表示するように
    public Subject<int> firstTutorialSubject = new Subject<int>(); 
    public IObservable<int> OnFisrtTutorialAsObservable => firstTutorialSubject.AsObservable();

    [Header("Lost currency")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    //最後に死んだプレイヤーの位置
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;

    private void Start()
    {
        checkpoints = FindObjectsOfType<Checkpoint>();
        descriptionPoints = FindObjectsOfType<DescriptionPoint>();

        player = PlayerManager.instance.player.transform;
    }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;
    }
    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data) => StartCoroutine(LoadWithDelay(_data));

    private IEnumerator LoadWithDelay(GameData _data)
    {
        yield return new WaitForSeconds(.1f);

        PlacePlayerClosestCheckpoint(_data);
        LoadLostCurrency(_data);
        LoadClosestCheckPoints(_data);
        LoadDescriptionPoints(_data);
        LoadFirstTutorial(_data);
    }

    private void LoadFirstTutorial(GameData _data)
    {
        isFirstTutorial = _data.isFirstTutorial;

        if (!isFirstTutorial)
        {
            isFirstTutorialSubscription = Inventory.instance.firstItemDrop
                .Where(itemNum => itemNum >= 2)
                .Subscribe(itemNum =>
                {
                    //特殊バージョンのテキスト2番目を表示するようにする
                    firstTutorialSubject.OnNext(1);
                    isFirstTutorialSubscription.Dispose();

                }).AddTo(this);
        }
    }

    private void LoadDescriptionPoints(GameData _data)
    {
        foreach(KeyValuePair<string, bool> pair in _data.descriptionPoints)
        {
            foreach(DescriptionPoint descriptionPoint in descriptionPoints)
            {
                if (descriptionPoint.id == pair.Key && pair.Value == true)
                    descriptionPoint.InActiveDescriptionPoint();
            }
        }
    }

    private void LoadClosestCheckPoints(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
        {
            foreach (Checkpoint checkpoint in checkpoints)
            {
                if (checkpoint.id == pair.Key && pair.Value == true)
                    checkpoint.ActiveCheckpoint();
            }
        }
    }

    private void LoadLostCurrency(GameData _data)
    {
        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostCurrencyX = _data.lostCurrencyX;
        lostCurrencyY = _data.lostCurrencyY;

        if(lostCurrencyAmount > 0)
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX, lostCurrencyY), Quaternion.identity);
            newLostCurrency.GetComponent<LostCurrencyController>().currency = lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;
    }

    public void SaveData(ref GameData _data)
    {
        _data.isFirstTutorial = isFirstTutorial;

        _data.lostCurrencyAmount = lostCurrencyAmount;
        _data.lostCurrencyX = player.position.x;
        _data.lostCurrencyY = player.position.y;

        if(FindClosestCheckpoint() != null)
            _data.closestCheckpointId = FindClosestCheckpoint().id;

        _data.checkpoints.Clear();

        foreach(Checkpoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.id, checkpoint.activationStatus);
        }

        _data.descriptionPoints.Clear();

        foreach(DescriptionPoint descriptionPoint in descriptionPoints)
        {
            _data.descriptionPoints.Add(descriptionPoint.id, descriptionPoint.activeStatus);
        }
    }
    private void PlacePlayerClosestCheckpoint(GameData _data)
    {
        if (_data.closestCheckpointId == null)
            return;

        closestCheckpointId = _data.closestCheckpointId;

        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (closestCheckpointId == checkpoint.id)
                player.position = checkpoint.transform.position;
        }
    }

    private Checkpoint FindClosestCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;

        foreach(var checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(player.position, checkpoint.transform.position);

            if(distanceToCheckpoint < closestDistance && checkpoint.activationStatus == true)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }
        return closestCheckpoint;
    }

    public void PauseGame(bool _pause)
    {
        if (_pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void SetIsFirstTutorial()
    {
        isFirstTutorial = true;
    }
}
