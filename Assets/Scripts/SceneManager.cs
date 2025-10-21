using System.Collections;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private Vector3 lastDeathPosition;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetLastDeathPosition(Vector3 position)
    {
        lastDeathPosition = position;
    }

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();

        string sceneName = SceneManager.GetActiveScene().name;
        ChangeScene(sceneName, RespawnType.None);
    }
    public void ChangeScene(string sceneName, RespawnType respawnType)
    {
        StartCoroutine(ChangeSceneCo(sceneName, respawnType));
    }

    private IEnumerator ChangeSceneCo(string sceneName, RespawnType respawnType)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
        yield return new WaitForSeconds(.2f);
        Vector3 position = GetNewPlayerPosition(respawnType);
        if (position != Vector3.zero)
            Player.instance.TeleportPlayer(position);
    }

    private Vector3 GetNewPlayerPosition(RespawnType type)
    {
        if (type == RespawnType.None)
        {
            var data = SaveManager.instance.GetGameData();
            var checkpoints = FindObjectsByType<Object_Checkpoint>(FindObjectsSortMode.None);
            var unlockedCheckpoints = checkpoints.Where(cp => data.unlockedCheckpoints.TryGetValue(cp.GetCheckpointId(), out bool unlocked) && unlocked)
            .Select(cp => cp.GetPosition()).ToList();

            var waypoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None)
            .Where(wp => wp.GetWaypointType() == RespawnType.Enter)
            .Select(wp => wp.GetPositionAndSetTriggerFalse()).ToList();

            var selectedPositions = unlockedCheckpoints.Concat(waypoints).ToList();

            if (selectedPositions.Count == 0)
                return Vector3.zero;

            return selectedPositions.
            OrderBy(pos => Vector3.Distance(pos, lastDeathPosition)).First(); //arrange from closest to furthest and take the first one
        }
        
        return GetWaypointPosition(type);
    }

    private Vector3 GetWaypointPosition(RespawnType type)
    {
        var waypoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None);
        foreach (var point in waypoints)
        {
            if (point.GetWaypointType() == type)
            {
                return point.GetPositionAndSetTriggerFalse();
            }
        }
        return Vector3.zero;
    }
}
