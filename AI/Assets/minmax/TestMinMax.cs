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

	public enum GameStatus
	{
		START,
		RUNNING,
		END,
	}

	public class TestMinMax : MonoBehaviour
	{
		public const int BOARD_CELLS = 9;
		public const int BOARD_WIDTH = 3;
		public const int PLAYER_NUM = 2;
		public const int REALPLAYER_INDEX = 0;

		private System.Random rnd = new System.Random();

		public Transform chessParent;
		public Transform[] boardCells;

		public GameObject objO = null;
		public GameObject objX = null;

		public GameObject[] playerHeads = new GameObject[PLAYER_NUM];

		private int[,] chesss = new int[BOARD_WIDTH, BOARD_WIDTH];

		private int playerIdx = -1;
		private bool playerThinking = false;

		private GameStatus status = GameStatus.START;

		private MinMax ai = null;

		private void Start()
		{
			for (int i = 0; i < BOARD_WIDTH; ++i)
			{
				for (int j = 0; j < BOARD_WIDTH; ++j)
					chesss[i,j] = ChessType.None;
			}

			ai = new MinMax(FuncGameOver, FuncEvaluate, FuncMoves, FuncBoardgen);

			status = GameStatus.RUNNING;
		}

		private void Update()
		{
			if (status != GameStatus.RUNNING)
				return;

			if (playerThinking)
			{
				
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

				PlayerEnterTurn();
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

			int cx = cidx % BOARD_WIDTH;
			int cy = cidx / BOARD_WIDTH;

			Debug.Assert(cy >= 0 && cy < BOARD_WIDTH, "CHECK");

			if (chesss[cx, cy] != ChessType.None)
			{
				Debug.Log("xx-- cell is not empty.");
				return;
			}

			Debug.Assert(REALPLAYER_INDEX == ChessType.O, "CHECK");
			GameObject objChess = Instantiate(objO);

			objChess.transform.SetParent(chessParent, false);
			objChess.transform.position = boardCells[cidx].position;
			chesss[cx, cy] = ChessType.O;

			PlayerFinishTurn();
		}

		public void PlayerFinishTurn()
		{
			playerThinking = false;

			// check end
		}

		public void PlayerEnterTurn()
		{
			playerThinking = true;

			// AI
			if (playerIdx != REALPLAYER_INDEX)
			{
				// MinMax.
			}
		}

		public void EndGame()
		{
			status = GameStatus.END;
		}

		// AI Callback
		public bool FuncGameOver(int[,] board, int player, int opp)
		{
			

			return false;
		}

		public int FuncEvaluate(int[,] board, int player, int opp)
		{
			return 0;
		}

		public List<int[]> FuncMoves(int[,] board, int player)
		{
			return null;
		}

		public int[,] FuncBoardgen(int[,] board, int player, int[] move)
		{
			return null;
		}
	}
}

