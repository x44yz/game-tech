using System.Collections;
using System.Collections.Generic;

// Pseudocode
// https://www.youtube.com/watch?v=l-hh51ncgDI
//function minmax(position, depth, alpha, beta, maximizingPlayer)
//	if depth == 0 or game over in position
//		return static evaluation of position

//	if maximizingPlayer
//		maxEval = -infinity
//		for each child of postion
//			eval = minimax(child, depth - 1, alpha, beta, false)
//			maxEval = max(maxEval, eval)
//			alpha = max(alpha, eval)
//			if beta <= alpha
//				break
//		return maxEval
//	else
//		minEval = +infinity
//		for each child of position
//			eval = minimax(child, depth - 1, alpha, beta, true)
//			minEval = min(minEval, eval)
//			beta = min(beta, eval)
//			if beta <= alpha
//				break
//		return minEval
using System;

namespace TicTacToe
{
	public class MinMax
	{
		// private int player = 0;
		// private int opp = 0;
		// private int depth = 0;
		private Func<int[,], int, int, bool> funcGameOver;
		private Func<int[,], int, int, int> funcEvaluate;
		private Func<int[,], int, List<int[]>> funcMoves;
		private Func<int[,], int, int[], int[,]> funcBoardgen;

		private int[] bestMove = new int[2];

		public MinMax(Func<int[,], int, int, bool> gameoverFunc, 
				Func<int[,], int, int, int> evaluateFunc, 
				Func<int[,], int, List<int[]>> movesFunc,
				Func<int[,], int, int[], int[,]> boardgenFunc)
		{
			// this.player = player;
			// this.opp = opp;
			// this.depth = depth;
			funcGameOver = gameoverFunc;
			funcEvaluate = evaluateFunc;
			funcMoves = movesFunc;
			funcBoardgen = boardgenFunc;
		}

		public int[] Think(int[,] board, int depth, int player, int opp)
		{
			NextMove(board, depth, player, opp, player, int.MinValue, int.MaxValue);
			return bestMove;
		}

		private int NextMove(int[,] board, int depth, int player, 
										int opp, int curr, int alpha, int beta)
		{
			if (depth < 0 || funcGameOver(board, player, opp))
			{
				int score = funcEvaluate(board, player, opp);
				return score;
			}

			List<int[]> moves = funcMoves(board, curr);

			foreach(int[] move in moves)
			{
				int[,] newBoard = funcBoardgen(board, curr, move);
				int score = NextMove(newBoard, depth - 1, player, opp, 
													curr == player ? opp : player, alpha, beta);
				
				// alpha beta pruning
				if (curr == player)
				{
					if (score > alpha)
					{
						alpha = score;
						bestMove = move;
						if (alpha >= beta)
							break;
					}
				}
				else // if (curr == opp)
				{
					if (score < beta)
					{
						beta = score;
						if (alpha >= beta)
							break;
					}
				}
			}

			if (curr == player)
				return alpha;
			else // if (curr == opp)
				return beta;
		}
	}
}

