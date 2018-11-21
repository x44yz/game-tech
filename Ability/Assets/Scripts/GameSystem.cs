using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameSystem : MonoBehaviour 
{
    public Player player;
    public Enemy enemy;

    private int attackTurn = 0;

    private void Start()
    {
        Debug.Assert(player != null, "CHECK");
        Debug.Assert(enemy != null, "CHECK");
    }

    private void Update()
    {
        UpdateInput();
        UpdateBattle();
    }

    private void UpdateInput()
    {
        if (player.activeAbility == null)
            return;

        if (Input.GetMouseButtonDown(0))
        {
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
                    target.Selected(player.activeAbility);
                }
            }
        }        
    }

    private void UpdateBattle()
    {
        // attack turn

    }
}
