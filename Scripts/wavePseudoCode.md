```
Initialize:
    Create a grid where each cell contains a bitmask of all possible tiles (superposition)
    Define constraint rules: for each tile and direction, which tiles can be adjacent
    Track uncollapsed cells. (cells with PopCount(superpostion) > 1)

Function WaveFunctionCollapse():
    While there are uncollapsed cells:
        // Find the cell with minimum entropy (fewest possibilities)
        cell = findMinimumEntropyCell()

        // Collapse the cell to a single tile
        // Choose randomly from remaining possibilities, weighted by tile weights
        chosenTile = weightedRandomBit(cell.superposition, weights)
        cell.superposition = (1UL << chosenTile) // Set only that bit

        propagationQueue = [cell]
        While propagationQueue is not empty:
            currentCell = propagationQueue.pop()

            For each neighbor of currentCell:
                oldPossibilities = neighbor.superposition

                // Get the constraint bitmask for this direction
                // currentCell has only 1 bit set (collapsed)
                tileId = BitIndex(currentCell.superposition) // which bit is set
                allowedNeighbors = constraintRules[tileId][direction]

                // Keep only tiles that are both possible AND allowed by constraints
                neighbor.superposition &= allowedNeighbors

                // Check if possibilities changed
                if neightbor.superposition != oldPossibilities:
                    if neighbor.superposition == 0: 
                        // Contradiction: handle with backtracking or restart
                        return FAILURE
                    propagationQueue.add(neighbor)

    Return SUCCESS

Function findMinimumEntropyCell():
    minEntropy = infinity
    candidates = []

    For each uncollapsed cell in grid:
        entropy = PopCount(cell.superposition) // Count the 1 bits
        If entropy = 1:
            continue // already collapsed, skip
        if entropy < minEntropy:
            minEntropy = entropy
            candidates = [cell]
        Else if entropy == minEntropy:
            candidates.add(cell)

    // Return random cell from candidates with minimum entropy
    Return randomChoice(candidates)

Function weightedRandomBit(superposition, weights):
    // Extract which bits are set in superposition
    // For each set bit, add its weight to a total.
    // Pick randomly proportional to weights
    // return the bit index (0-15)
```
