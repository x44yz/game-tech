using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. choose ability
// 2. choose target
public class Player : Actor 
{
  public Ability activeAbility = null;

  private void Start()
  {
    activeAbility = new Freeze();
  }

  public override void Tick(float dt)
  {
    if (!isActionTurn)
      return;

    UpdateInput();
  }

  public void Selected()
  {
    Debug.Log("Player selected");
  }

  private void UpdateInput()
  {
    if (Input.GetMouseButtonDown(0))
    {
      if (activeAbility == null)
      {
        Debug.Log("first active ability");
        return;
      }

      Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

      RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
      if (hit.collider != null)
      {
        GameObject obj = hit.collider.gameObject;
        Debug.Log("click object: " + obj.name);

        IAbilityTarget target = obj.GetComponent<IAbilityTarget>();
        if (target != null)
        {
          target.Selected(this, activeAbility);
        }
      }
    }
  }
}
