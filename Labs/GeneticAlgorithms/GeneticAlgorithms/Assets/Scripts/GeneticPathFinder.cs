using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pathfinding creature that uses a genetic algorithm to find the shortest path
/// to the target
/// </summary>
/// <permission cref="hasFinished">Whether or not the creature has reached the target</permission>
/// <permission cref="hasBeenInitialized">Whether or not the creature has been initialized</permission
public class GeneticPathFinder : MonoBehaviour
{
    public float creatureSpeed;
    public float pathMultiplier;
    int pathIndex = 0;

    public DNA dna;
    public bool hasFinished = false;
    public bool hasBeenInitialized = false;

    Vector2 target;
    Vector2 nextPoint;

    public float Fitness
    {
        get
        {
            float dist = Vector2.Distance(transform.position, target);

            if (dist == 0) dist = 0.0001f;
            return 60 / dist;
        }
    }

    /// <summary>
    /// Initializes the creature
    /// </summary>
    /// <param name="newDna">The DNA object to use</param>
    /// <param name="_target">The target of the creature</param>
    public void InitCreature(DNA newDna, Vector2 _target)
    {
        dna = newDna;
        target = _target;

        nextPoint = transform.position;
        hasBeenInitialized = true;
    }

    /// <summary>
    /// Starts the creature with a random DNA object and a target of (0, 0)
    /// </summary>
    public void Start()
    {
        InitCreature(new DNA(), Vector2.zero);
    }

    /// <summary>
    /// Updates the position of the creature
    /// </summary>
    private void Update()
    {
        if (pathIndex == dna.genes.Count || Vector2.Distance(transform.position, target) < 0.5f)
        {
            hasFinished = true;
        }

        if ((Vector2)transform.position == nextPoint)
        {
            if (pathIndex < dna.genes.Count)
            {
                nextPoint = (Vector2)transform.position + dna.genes[pathIndex];
                pathIndex++;
            }
        }

        else transform.position = Vector2.MoveTowards(
            transform.position, nextPoint, 
            creatureSpeed * Time.deltaTime);
    }
}
