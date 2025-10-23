using UnityEngine;
using UnityEngine.UI;

public class UI_Options : MonoBehaviour
{
    private Player player;
    [SerializeField] private Toggle healthBarToggle;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();


        healthBarToggle.onValueChanged.AddListener(OnHealthBarToggleChanged);
    }

    private void OnHealthBarToggleChanged(bool isOn)
    {
        player.health.EnableHealthBar(isOn);
    }
    public void GoMainMenuBTN()
    {
        GameManager.instance.ChangeScene("MainMenu", RespawnType.NonSpecific);
    }
}
