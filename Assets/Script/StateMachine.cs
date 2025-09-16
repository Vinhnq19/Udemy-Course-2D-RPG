using UnityEngine;

public class StateMachine
//This class manages the states of an entity, allowing it to transition between different states and execute state-specific behavior.
{
    public EntityState currentState { get; private set; } //Can only be set within this class
    public void Initialize(EntityState startState) //Sets the initial state of the state machine and calls the Enter method of that state.
    {
        currentState = startState;
        currentState.Enter();
    }

    //Changes the current state to a new state, calling the Exit method of the current state and the Enter method of the new state.
    public void ChangeState(EntityState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void UpdateActiveState() //Calls the Update method of the current state.
    {
        currentState.Update();
    }

}

