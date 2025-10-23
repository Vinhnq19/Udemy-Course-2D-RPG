using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    private FileDataHandler dataHandler;
    private GameData gameData;
    private List<ISaveable> allSaveables;
    [SerializeField] private string fileName = "gameData.json";
    [SerializeField] private bool encryptData = true;

    private void Awake()
    {
        instance = this;
    }


    private IEnumerator Start()
    {
        Debug.Log(Application.persistentDataPath);
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData); //persistentDataPath is a special folder for saving data
        allSaveables = FindISaveables();

        yield return null;
        
        LoadGame();
    }

    private void LoadGame()
    {
        gameData = dataHandler.LoadData();
        if (gameData == null)
        {
            Debug.Log("No data found. Initializing new game data.");
            gameData = new GameData();
            return;
        }
        
        foreach(var saveable in allSaveables)
        {
            saveable.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (ISaveable saveable in allSaveables)
        {
            saveable.SaveData(ref gameData);
        }
        dataHandler.SaveData(gameData);
    }

    public GameData GetGameData()
    {
        return gameData;
    }

    [ContextMenu("Delete Save Data")]

    public void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        dataHandler.Delete();

        LoadGame();
    }

    private void OnApplicationQuit()
    {
       SaveGame(); 
    }

    private List<ISaveable> FindISaveables()
    {
        return
        FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
        .OfType<ISaveable>().ToList(); // Find all MonoBehaviours that implement ISaveable
        
    }
}
