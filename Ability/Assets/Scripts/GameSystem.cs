using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameSystem : MonoBehaviour 
{
	private const int TURN_PLAYER = 0;

    public Player player;
    public Enemy enemy;

	public enum TurnStatus
	{
		NONE,
		PREPARE,
		SWITCHING,
		RUNNING,
	}

	public TurnStatus turnStatus { get; set; }
	private TurnStatus curTurnStatus = TurnStatus.NONE;
    private int actionTurn = 0;

    private void Start()
    {
        Debug.Assert(player != null, "CHECK");
        Debug.Assert(enemy != null, "CHECK");
    }

    private void Update()
    {
        // UpdateInput();
        UpdateAction();
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

    private void UpdateAction()
    {
		if (curTurnStatus != turnStatus)
		{
			if (turnStatus == TurnStatus.PREPARE)
			{
				turnStatus = TurnStatus.SWITCHING;
			}
			else if (turnStatus == TurnStatus.SWITCHING)
			{
				// TODO
				// 2 = actor nums
				actionTurn = (actionTurn + 1) % 2; 
				if (actionTurn == 0)
				{
				}
			}
			else if (turnStatus == TurnStatus.RUNNING)
			{

			}

			curTurnStatus = turnStatus;
		}
    }
}
