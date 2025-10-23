using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Portal Scroll", fileName = "Item effect data - Portal Scroll")]

public class ItemEffect_PortalScroll : ItemEffect_DataSO
{
    public override void ExecuteEffect()
    {
        if (SceneManager.GetActiveScene().name == "Level 0")
        {
            Debug.Log("You are already in town!");
            return;
        }

        Player player = Player.instance;
        Vector3 portalPosition = player.transform.position + new Vector3(2f * player.facingDir * 1, 5f, 0);

        Object_Portal.instance.ActivatePortal(portalPosition, player.facingDir);
    }
}
