using System;
using System.Collections.Generic;
using Godot;

public partial class RandomWalk
{
	// Constants.
	private const int ROOM_SIZE = 8;
	private const int HALLWAY_WIDTH = 2;

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

	private RWRoom Walk(RWRoom currentRoom, int stepCount)
	{
		
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
