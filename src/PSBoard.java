import java.util.Collection;

/**
 * Represents a memento of a Peg Solitaire Board. Its state is represented by a
 * Long to make the operation faster and the moves are encapsulate in there own
 * class to make it easier to understand for the programmer.
 */
public class PSBoard {

	/**
	 * Represents a move on the Peg Solitaire Board.
	 */
	public class PSMove {

		public final Long pegToMove;
		public final Long pegsToRemove;
		public final Long pegsInvolved;


		public PSMove(final Long pegToMove,
		              final Long pegsToRemove) {

			this.pegToMove = pegToMove;
			this.pegsToRemove = pegsToRemove;
			this.pegsInvolved = pegToMove | pegsToRemove;
		}
	}

	private final Collection<PSMove> moves;
	private final Long               state;


	public final Collection<PSMove> moves() {

		return this.moves();
	}


	/**
	 * Optimization.
	 *
	 * @param board
	 *
	 * @return
	 */
	public final boolean isCongruentTo(final PSBoard board) {

		//TODO
		return false;
	}


	public final Long state() {

		return this.state;
	}


	/**
	 * Constructs a board at a specific state.
	 *
	 * @param moves Moves allowed on the board.
	 * @param state Current state of the board.
	 */
	public PSBoard(final Collection<PSMove> moves,
	               final Long state) {

		this.moves = moves;
		this.state = state;
	}


	@Override
	public final String toString() {

		//TODO
		return null;
	}
}
