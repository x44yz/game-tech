using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// agent.SetVoidActionDelegate("Sleep", Sleep);
// agent.SetVoidActionDelegate("Shower", Shower);
// agent.SetVoidActionDelegate("Eat at Restaurant", EatAtRestaurant);
// agent.SetVoidActionDelegate("Eat at Home", EatAtHome);
// agent.SetVoidActionDelegate("Watch Movie", WatchMovie);
// agent.SetVoidActionDelegate("Get Groceries", GetGroceries);
// agent.SetVoidActionDelegate("Drink Coffee", DrinkCoffee);
// agent.SetVoidActionDelegate("Work", Work);
// agent.SetVoidActionDelegate("Work at Home", WorkAtHome);

public class Agent : MonoBehaviour, IAIAgent
{
    public UtilityAI utilityAI;
    public AgentContext context;

    private void Update() 
    {
        float dt = Time.deltaTime;
        utilityAI.Tick(dt);
    }

    public IContext Context()
    {
        return context;
    }
}
