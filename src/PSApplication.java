import java.util.ArrayList;
import java.util.Collections;
import java.util.HashSet;

/**
 *
 */
public class PSApplication {

	// list of seen boards - this is used to prevent rechecking of paths
	private static final HashSet<Long> SEEN_BOARDS = new HashSet<>();

	// list of SOLUTIONS boards in ascending order - filled in once the SOLUTIONS is found
	private static final ArrayList<Long> SOLUTIONS = new ArrayList<>();

	// -------

	// goal board (one marble in center)
	private static final long GOAL_BOARD = 16777216L;

	// initial board (one marble free in center)
	private static final long INITIAL_BOARD = 124141717933596L;

	// board that contains a ball in every available slot, i.e. GOAL_BOARD | INITIAL_BOARD
	private static final long VALID_BOARD_CELLS = 124141734710812L;

	// holds all 76 MOVES that are possible
	// the inner array is structures as following:
	// - first entry holds the peg that is added by the move
	// - second entry holds the two pegs that are removed by the move
	// - third entry holds all three involved pegs
	private static final long[][] MOVES = new long[76][];

	// -------


	// print the board
	private static void printBoard(long board) {
		// loop over all cells (the board is 7 x 7)
		for (int i = 0; i < 49; i++) {
			boolean validCell = ((1L << i) & VALID_BOARD_CELLS) != 0L;
			System.out.print(validCell ? (((1L << i) & board) != 0L ? "O " : "· ") : "  ");
			if (i % 7 == 6) System.out.println();
		}
		System.out.println("-------------");
	}


	// create the two possible MOVES for the three added pegs
	// (this function assumes that the pegs are in one continuous line)
	private static void createMoves(int bit1, int bit2, int bit3, ArrayList<long[]> moves) {

		moves.add(new long[]{(1L << bit1), (1L << bit2) | (1L << bit3),
				(1L << bit1) | (1L << bit2) | (1L << bit3)});
		moves.add(new long[]{(1L << bit3), (1L << bit2) | (1L << bit1),
				(1L << bit1) | (1L << bit2) | (1L << bit3)});
	}


	// do the calculation recursively by starting from
	// the "GOAL_BOARD" and doing MOVES in reverse
	private static boolean search(long board) {
		// for all possible MOVES
		for (long[] move : MOVES) {
			// check if the move is valid
			// Note: we place "two ball" check first since it is more
			// likely to fail. This saves about 20% in run time (!)
			if ((move[1] & board) == 0L && (move[0] & board) != 0L) {
				// calculate the board after this move was applied
				long newBoard = board ^ move[2];
				// only continue processing if we have not seen this board before
				if (!SEEN_BOARDS.contains(newBoard)) {
					SEEN_BOARDS.add(newBoard);
					// check if the initial board is reached
					if (newBoard == INITIAL_BOARD || search(newBoard)) {
						SOLUTIONS.add(board);
						return true;
					}
				}
			}
		}
		return false;
	}


	// the main method
	public static void main(String[] args) {
		// to measure the overall runtime of the program
		long time = System.currentTimeMillis();

		// add starting board (as this board is not added by the recursive function)
		SOLUTIONS.add(INITIAL_BOARD);

		// generate all possible MOVES
		ArrayList<long[]> moves = new ArrayList<>();
		// holds all starting positions in west-east direction
		int[] startsX = new int[]{2, 9, 14, 15, 16, 17, 18, 21, 22, 23, 24, 25, 28, 29, 30, 31, 32, 37, 44};
		for (int x : startsX) {
			createMoves(x, x + 1, x + 2, moves);
		}
		// holds all starting positions in north-south direction
		int[] startsY = new int[]{2, 3, 4, 9, 10, 11, 14, 15, 16, 17, 18, 19, 20, 23, 24, 25, 30, 31, 32};
		for (int y : startsY) {
			createMoves(y, y + 7, y + 14, moves);
		}
		// randomize the order of the MOVES (this highly influences the resulting runtime)
		Collections.shuffle(moves);
		// fill in the global MOVES variable that is used by the solver
		moves.toArray(MOVES);

		// start recursively search for the initial board from the goal (reverse direction!)
		search(GOAL_BOARD);

		// print required time
		System.out.println("Completed in " + (System.currentTimeMillis() - time) + " ms.");

		// print the found SOLUTIONS
		for (long step : SOLUTIONS) {
			printBoard(step);
		}
	}
}
