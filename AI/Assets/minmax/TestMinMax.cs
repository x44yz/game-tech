using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TicTacToe
{
	public class ChessType
	{
		public const int None = -1;
		public const int O = 0;
		public const int X = 1;
	}

	public class TestMinMax : MonoBehaviour
	{
		public const int BOARD_CELLS = 9;
		public const int PLAYER_NUM = 2;
		public const int REALPLAYER_INDEX = 0;

		private System.Random rnd = new System.Random();

		public Transform chessParent;
		public Transform[] boardCells;

		public GameObject objO = null;
		public GameObject objX = null;

		public GameObject[] playerHeads = new GameObject[PLAYER_NUM];

		private int[] chesss = new int[BOARD_CELLS];

		private int playerIdx = -1;
		private bool playerThinking = false;

		private void Start()
		{
			for (int i = 0; i < chesss.Length; ++i)
			{
				chesss[i] = ChessType.None;
			}
		}

		private void Update()
		{
			if (playerThinking)
			{
				if (playerIdx != REALPLAYER_INDEX)
				{
					// Update AI

				}
			}
			else
			{
				if (playerIdx >= 0 && playerIdx < PLAYER_NUM)
				{
					var curp = playerHeads[playerIdx].transform.localPosition;
					playerHeads[playerIdx].transform.localPosition = new Vector3(curp.x, -54f, curp.z);
				}

				playerIdx += 1;
				playerIdx = playerIdx % PLAYER_NUM;
				Debug.Assert(playerIdx >= 0 && playerIdx < PLAYER_NUM, "CHECK");

				var p = playerHeads[playerIdx].transform.localPosition;
				playerHeads[playerIdx].transform.localPosition = new Vector3(p.x, 0, p.z);

				playerThinking = true;
			}
		}

		public void OnClickBoardCell(int cidx)
		{
			Debug.Log("xx-- OnClickBoardCell > " + cidx);
			Debug.Assert(cidx >= 0 && cidx < BOARD_CELLS, "CHECK");

			if (playerIdx != REALPLAYER_INDEX)
			{
				Debug.Log("xx-- now it's not your turn.");
				return;
			}

			if (chesss[cidx] != ChessType.None)
			{
				Debug.Log("xx-- cell is not empty.");
				return;
			}

			Debug.Assert(REALPLAYER_INDEX == ChessType.O, "CHECK");
			GameObject objChess = Instantiate(objO);

			objChess.transform.SetParent(chessParent, false);
			objChess.transform.position = boardCells[cidx].position;
			chesss[cidx] = ChessType.O;

			PlayerFinishTurn();
		}

		public void PlayerFinishTurn()
		{
			playerThinking = false;

			// check end
		}
	}
}

