using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameSystem : MonoBehaviour 
{
	private const int TURN_PLAYER = 1;

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

        player.onTurnStart += OnActorTurnStart;
        player.onTurnFinish += OnActorTurnFinish;
        enemy.onTurnStart += OnActorTurnStart;
        enemy.onTurnFinish += OnActorTurnFinish;
    
        turnStatus = TurnStatus.PREPARE;
    }

    private void Update()
    {
        UpdateAction();
        // UpdateInput();

        float dt = Time.deltaTime;
        if (player != null)
            player.Tick(dt);
        if (enemy != null)
            enemy.Tick(dt);
    }

//    private void UpdateInput()
//    {
//        if (player.isActionTurn == false)
//            return;
//
//        if (Input.GetMouseButtonDown(0))
//        {
//            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
//
//            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
//            if (hit.collider != null)
//            {
//                GameObject obj = hit.collider.gameObject;
//                Debug.Log("click object: " + obj.name);
//
//                IAbilityTarget target = obj.GetComponent<IAbilityTarget>();
//                if (target != null)
//                {
//                    target.Selected(this, player.activeAbility);
//                }
//            }
//        }        
//    }

    private void UpdateAction()
    {
		if (curTurnStatus != turnStatus)
		{
            curTurnStatus = turnStatus;

            if (curTurnStatus == TurnStatus.PREPARE)
			{
				turnStatus = TurnStatus.SWITCHING;
			}
            else if (curTurnStatus == TurnStatus.SWITCHING)
			{
				// TODO
				// 2 = actor nums
				actionTurn = (actionTurn + 1) % 2; 
                if (actionTurn == TURN_PLAYER)
                {
                    Debug.Log("player action turn");
                    player.StartTurn();
                }
                else
                {
                    Debug.Log("enemy action turn");
                    enemy.StartTurn();
                }
			}
		}
    }

    private void OnActorTurnStart(Actor actor)
    {
        
    }

    private void OnActorTurnFinish(Actor actor)
    {
        curTurnStatus = TurnStatus.SWITCHING;
    }
}
