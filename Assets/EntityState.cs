using UnityEngine;

public abstract class EntityState
{
    protected Player player;
    protected StateMachine stateMachine;
    protected string stateName;

    public EntityState(Player player, StateMachine stateMachine, string stateName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.stateName = stateName;
    }

    protected EntityState(StateMachine stateMachine, string stateName)
    {
        this.stateMachine = stateMachine;
        this.stateName = stateName;
    }

    public virtual void Enter()
    {
        // Code to execute when entering the state, every time state will be changed, enter will be called
        Debug.Log($"Entering state: " + stateName);
    }

    public virtual void Update()
    {
        // Code to execute every frame while in this state
        Debug.Log($"Updating state: " + stateName);
    }

    public virtual void Exit()
    {
        // Code to execute when exiting the state
        Debug.Log($"Exiting state: " + stateName);
    }
}
