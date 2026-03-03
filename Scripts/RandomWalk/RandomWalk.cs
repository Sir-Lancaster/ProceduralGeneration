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
	private bool Walk(RWRoom currentRoom, int stepCount, bool isBranch = false)
	{
		List<Vector2I> validDirections = GetValidDirections(currentRoom, isBranch);
		if (stepCount >= _minSteps && (stepCount >= _maxSteps || validDirections.Count == 0))
		{
			return true;
		}
		
		// Early exit step chance.
		if (stepCount >= _minSteps && _random.NextDouble() < _stepChance) return true;

		// Dead end before minsteps.
		if (validDirections.Count == 0)
		{
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

			// Add the hallway to the current room based on direction. Horizontal case first.
			if (direction.X != 0)
			{
				currentRoom.Hallways.Add(new Rect2I(
					new Vector2I(currentRoom.Position.X + ROOM_SIZE, currentRoom.Position.Y + ROOM_SIZE / 2 - HALLWAY_WIDTH / 2),
					new Vector2I(ROOM_SIZE, HALLWAY_WIDTH)));
			}

			// Vertical case.
			else if (direction.Y != 0)
			{
				currentRoom.Hallways.Add(new Rect2I(
					new Vector2I(currentRoom.Position.X + ROOM_SIZE / 2 - HALLWAY_WIDTH / 2, currentRoom.Position.Y + ROOM_SIZE),
					new Vector2I(HALLWAY_WIDTH, ROOM_SIZE)));
			}
		}
		
		// If next room is not null, then connect next room and current room as neighbors.
		else if (_allowLoops)
		{
			currentRoom.Neighbors.Add(nextRoom);
			nextRoom.Neighbors.Add(currentRoom);
			return true;
		}

		// Recursion and branch case.
		bool success = Walk(nextRoom, stepCount + 1);
		if (!success)
		{
			// Undo: remove nextRoom, remove neighbors, remove hallway.
			_rooms.Remove(nextRoom);
			currentRoom.Neighbors.Remove(nextRoom);
			nextRoom.Neighbors.Remove(currentRoom);
			currentRoom.Hallways.RemoveAt(currentRoom.Hallways.Count - 1);
			return false; // Propagate failure up.
		}

		if (_allowBranches && _random.NextDouble() < _branchChance)
		{
			Walk(currentRoom, stepCount, true);
		}

		return true;
	}

	// Helper function.
	private List<Vector2I> GetValidDirections(RWRoom currentRoom, bool isBranch)
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

			bool canConnect = _allowLoops || (isBranch && _allowBranchesToConnect);
			if (!roomExists || canConnect)
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
	}
}
