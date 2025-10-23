using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_Waypoint : MonoBehaviour
{
    [SerializeField] private string transferToScene;
    [SerializeField] private RespawnType waypointType;
    [SerializeField] private RespawnType connectedWaypoint;
    [SerializeField] private Transform respawnPosition;
    [SerializeField] private bool canBeTriggered = true;

    public RespawnType GetWaypointType() => waypointType;
public Vector3 GetPositionAndSetTriggerFalse()
    {
        canBeTriggered = false;
        return respawnPosition == null ? transform.position : respawnPosition.position;
    }
    private void OnValidate()
    {
        gameObject.name = "Object_Waypoint - " + waypointType.ToString() + " - " + transferToScene;

        if (waypointType == RespawnType.Enter)
            connectedWaypoint = RespawnType.Exit;
        if (waypointType == RespawnType.Exit)
            connectedWaypoint = RespawnType.Enter;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canBeTriggered) return;
        
        GameManager.instance.ChangeScene(transferToScene, connectedWaypoint);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canBeTriggered = true;
    }
}
