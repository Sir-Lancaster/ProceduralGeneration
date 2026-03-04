using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class RandomWalk
{
	// Constants.
	public const int ROOM_SIZE = 8;
	public const int HALLWAY_WIDTH = 2;

	// Private variables. Ordered: classes, Lists, int, float, bool.
	private Random _random;
	private List<RWRoom> _rooms;
	private int _minSteps;
	private int _maxSteps;
	private int _seed; 
	private float _stepChance;
	private float _branchChance;
	private bool _allowLoops;
	private bool _allowBranches;
	private bool _allowBranchesToConnect;

	// Rooms allows the renderer to access the list.
	public List<RWRoom> Rooms => _rooms;

	// The algorithm.
	private bool Walk(RWRoom currentRoom, int stepCount, RWRoom cameFrom = null, bool isBranch = false)
	{
		List<Vector2I> validDirections = GetValidDirections(currentRoom, cameFrom, isBranch);
		GD.Print($"Step {stepCount}, room {currentRoom.Position}, validDirs: {validDirections.Count}");
		
		if (stepCount >= _minSteps && (stepCount >= _maxSteps || validDirections.Count == 0))
		{
			GD.Print("Returning true - stopping condition met");
			return true;
		}
		
		if (stepCount >= _minSteps && _random.NextDouble() > _stepChance) return true;

		if (validDirections.Count == 0)
		{
			GD.Print("Returning false - dead end before minSteps");
			return false;
		}

		// Choose a random valid direction from available ones.
		Vector2I direction = validDirections[_random.Next(0, validDirections.Count)];

		// Check if room always exists (in case of _allowLoops = True).
		Vector2I nextPosition = currentRoom.Position + direction;
		RWRoom nextRoom = _rooms.Find(r => r.Position == nextPosition);
		
		// if nextRoom is null, create the new room. if not and _allowLoops = true, connect as neighbors.
		if (nextRoom == null)
		{
			// Create the room and add it to _rooms.
			nextRoom = new RWRoom(nextPosition);
			_rooms.Add(nextRoom);

			// Add the new room to the current room's neighbors and the current room as a neighbor to the new room.
			currentRoom.Neighbors.Add(nextRoom);
			nextRoom.Neighbors.Add(currentRoom);

			// Add hallway immediately — we'll remove it if we backtrack.
			int cellSize = ROOM_SIZE + HALLWAY_WIDTH;
			int pixelX = currentRoom.Position.X * cellSize;
			int pixelY = currentRoom.Position.Y * cellSize;
			Rect2I hallway;

			if (direction.X != 0)
			{
				// If going right, hallway starts at right edge of room.
				// If going left, hallway starts at left edge of nextRoom (which is left edge of current room - hallway width).
				int hallwayX = direction.X > 0 
					? pixelX + ROOM_SIZE 
					: pixelX - (cellSize - ROOM_SIZE);
				hallway = new Rect2I(
					new Vector2I(hallwayX, pixelY + ROOM_SIZE / 2 - HALLWAY_WIDTH / 2),
					new Vector2I(cellSize - ROOM_SIZE, HALLWAY_WIDTH));
			}
			else
			{
				// If going down, hallway starts at bottom edge of room.
				// If going up, hallway starts at top edge of nextRoom.
				int hallwayY = direction.Y > 0 
					? pixelY + ROOM_SIZE 
					: pixelY - (cellSize - ROOM_SIZE);
				hallway = new Rect2I(
					new Vector2I(pixelX + ROOM_SIZE / 2 - HALLWAY_WIDTH / 2, hallwayY),
					new Vector2I(HALLWAY_WIDTH, cellSize - ROOM_SIZE));
			}
			currentRoom.Hallways.Add(hallway);
			nextRoom.Hallways.Add(hallway); // mirror hallway on nextRoom too

			bool success = Walk(nextRoom, stepCount + 1, currentRoom, isBranch);
			if (!success)
			{
				GD.Print($"Backtracking from step {stepCount}");
				_rooms.Remove(nextRoom);
				currentRoom.Neighbors.Remove(nextRoom);
				nextRoom.Neighbors.Remove(currentRoom);
				currentRoom.Hallways.Remove(hallway);
				nextRoom.Hallways.Remove(hallway);
				return false;
			}

			// Branch only after walk fully succeeds.
			if (_allowBranches && _random.NextDouble() > _branchChance)
			{
				int hallwayCountBefore = currentRoom.Hallways.Count;
				Walk(currentRoom, stepCount + 1, nextRoom, true);
				while (currentRoom.Hallways.Count > hallwayCountBefore)
					currentRoom.Hallways.RemoveAt(currentRoom.Hallways.Count - 1);
			}
		}
		else if (_allowLoops || (isBranch && _allowBranchesToConnect))
		{
			if (!currentRoom.Neighbors.Contains(nextRoom))
			{
				currentRoom.Neighbors.Add(nextRoom);
				nextRoom.Neighbors.Add(currentRoom);
			}
			return true;
		}

		return true;
	}

	// Helper function.
	private List<Vector2I> GetValidDirections(RWRoom currentRoom, RWRoom cameFrom, bool isBranch)
	{
		List<Vector2I> validDirections = new List<Vector2I>();
		Vector2I[] directions =
        [
            new Vector2I(0, -1),
			new Vector2I(0, 1),
			new Vector2I(-1, 0),
			new Vector2I(1, 0)
		];
		foreach (var direction in directions)
		{
			Vector2I neighborPosition = currentRoom.Position + direction;
			bool roomExists = _rooms.Any(r => r.Position == neighborPosition);
			bool isBacktrack = cameFrom != null && neighborPosition == cameFrom.Position;

			bool canConnect = _allowLoops || (isBranch && _allowBranchesToConnect);
			if (!isBacktrack && (!roomExists || canConnect))
			{
				validDirections.Add(direction);
			}
		}
		return validDirections;
	}

	public void Generate(int minSteps, int maxSteps, int seed, float stepChance, float branchChance, bool allowLoops, bool allowBranches, bool allowBranchesToConnect)
	{
		// Assign parameters to private variables.
		_minSteps = minSteps;
		_maxSteps = maxSteps;
		_seed = seed;
		_stepChance = stepChance;
		_branchChance = branchChance;
		_allowLoops = allowLoops;
		_allowBranches = allowBranches;
		_allowBranchesToConnect = allowBranchesToConnect;

		// Initialize seed and rooms.
		_random = new Random(_seed);
		_rooms = new List<RWRoom>();

		// create start room and call Walk algorithm.
		RWRoom startRoom = new RWRoom(new Vector2I(0, 0));
		_rooms.Add(startRoom);
		Walk(startRoom, 0);
		GD.Print($"Rooms generated: {_rooms.Count}");
	}
}
