import java.util.LinkedList;
import edu.du.dudraw.*;

public class SnakeListener<T> implements DrawListener {
	private Draw canvas;
	private Food food;
	private LinkedList<Segment> snake;

	private enum Directions {
		NORTH, EAST, WEST, SOUTH
	};

	private Directions direction;

	public SnakeListener(int xSize, int ySize) {
		// create a draw object and set the canvas and scale
		canvas = new Draw();
		canvas.setCanvasSize(xSize, ySize);
		canvas.setXscale(-0.5, 19.5);
		canvas.setYscale(-0.5, 19.5);
		// create a new snake in a linked list and add three elements to the list
		snake = new LinkedList<Segment>();
		Segment head = new Segment(5, 3);
		Segment tail = new Segment(5, 1);
		Segment middle = new Segment(5, 2);
		snake.addFirst(head);
		snake.addLast(middle);
		snake.addLast(tail);
		canvas.setPenColor(canvas.BLACK);
		for (Segment s : snake) {
			canvas.filledSquare(s.xPosition, s.yPosition, 0.5);
		}
		// set the starting direction of the snake
		direction = Directions.NORTH;
		// create a new food object
		food = new Food(15, 15);
		food.draw();
		canvas.addListener(this);
		canvas.enableDoubleBuffering();
		canvas.startUpdate();
		canvas.setFrameTime(300);
	}

	@Override
	public void keyPressed(int arg0) {

	}

	@Override
	public void keyReleased(int arg0) {
		// TODO Auto-generated method stub

	}

	@Override
	public void keyTyped(char c) {
		// change the direction with the key typed
		if (c == 'w') {
			direction = Directions.NORTH;
		}
		if (c == 'a') {
			direction = Directions.WEST;
		}
		if (c == 's') {
			direction = Directions.SOUTH;
		}
		if (c == 'd') {
			direction = Directions.EAST;
		}
	}

	@Override
	public void mouseClicked(double arg0, double arg1) {
		// TODO Auto-generated method stub

	}

	@Override
	public void mouseDragged(double arg0, double arg1) {
		// TODO Auto-generated method stub

	}

	@Override
	public void mousePressed(double arg0, double arg1) {
		// TODO Auto-generated method stub

	}

	@Override
	public void mouseReleased(double arg0, double arg1) {
		// TODO Auto-generated method stub

	}

	@Override
	public void update() {

		canvas.clear();

		// when moving, add a new segment to the snake with an updated position
		if (this.direction == Directions.NORTH) {
			snake.addFirst(new Segment(snake.get(0).xPosition, snake.get(0).yPosition + 1));
		}
		if (direction == Directions.WEST) {
			snake.addFirst(new Segment(snake.get(0).xPosition - 1, snake.get(0).yPosition));
		}
		if (direction == Directions.EAST) {
			snake.addFirst(new Segment(snake.get(0).xPosition + 1, snake.get(0).yPosition));
		}
		if (direction == Directions.SOUTH) {
			snake.addFirst(new Segment(snake.get(0).xPosition, snake.get(0).yPosition - 1));
		}
		// if a food is eaten, then create a new food
		if (food.isEaten() == true) {
			food.draw();
		}
		// if food isn't eaten, remove the added segment from the snake before drawing
		// it
		else {
			snake.removeLast();
			food.draw();
		}
		

		// draw the entire snake with updated segments
		canvas.setPenColor(canvas.BLACK);
		for (int i = 0; i < snake.size(); i++) {
			canvas.filledSquare(snake.get(i).xPosition, snake.get(i).yPosition, 0.5);
		}

		canvas.show();
		
		// check to see if the game has ended
		if (gameOver() == true) {
			canvas.stopUpdate();
		}
	}

	// checks if the game is over
	public boolean gameOver() {
		// if the snake runs into itself, stop the game
		for (int i = 1; i < snake.size(); i++) {
			if (snake.get(0).xPosition == snake.get(i).xPosition && snake.get(0).yPosition == snake.get(i).yPosition) {
				canvas.clear();
				canvas.setPenColor(canvas.BLACK);
				canvas.text(10, 10, "Game Over");
				canvas.show();
				return true;
			}
		}
		// if the snake hits a wall, stop the game
		if (snake.get(0).xPosition == 20 || snake.get(0).yPosition == 20 || snake.get(0).xPosition == -1 || snake.get(0).yPosition == -1 ) {
			canvas.clear();
			canvas.setPenColor(canvas.BLACK);
			canvas.text(10, 10, "Game Over");
			canvas.show();
			return true;
		}
		return false;
	}

	public class Segment {
		private double xPosition;
		private double yPosition;

		// create a new segment object with a set xPosition and yPosition
		public Segment(double xPos, double yPos) {
			xPosition = xPos;
			yPosition = yPos;
		}

	}

	public class Food {
		private int xPos;
		private int yPos;

		// create a new food object with a set xPosition and yPosition
		public Food(int x, int y) {
			this.xPos = x;
			this.yPos = y;
		}

		// draw the new food object
		public void draw() {
			canvas.setPenColor(canvas.YELLOW);
			canvas.filledSquare(xPos, yPos, 0.5);
		}

		// if the food is eaten, update the food object with a new position
		public boolean isEaten() {
			if (snake.get(0).xPosition == xPos && snake.get(0).yPosition == yPos) {
				xPos = (int) (Math.random()*20);
				yPos = (int) (Math.random() * 20);
				return true;
			}
			return false;
		}

	}

}
