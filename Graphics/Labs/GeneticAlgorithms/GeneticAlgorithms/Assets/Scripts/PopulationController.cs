using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationController : MonoBehaviour
{
    List<GeneticPathFinder> population = new List<GeneticPathFinder>();
    public GameObject creaturePrefab;
    public int populationSize = 100;
    public int genomeLength;
    public float cutoff = 0.3f;
    public Transform spawnPoint;
    public Transform end;

    /// <summary>
    /// Starts the PopulationController
    /// </summary>
    private void Start()
    {
        InitPopulation();
    }

    /// <summary>
    /// Updates the Population
    /// </summary>
    private void Update()
    {
        if (!HasActive()) NextGeneration();
    }

    /// <summary>
    /// Initializes the population of the simulation
    /// </summary>
    void InitPopulation()
    {
        for (int i = 0; i < populationSize; i++)
        {
            GameObject go = Instantiate(creaturePrefab, spawnPoint.position, Quaternion.identity);

            go.GetComponent<GeneticPathFinder>().InitCreature(new DNA(genomeLength), end.position);
            population.Add(go.GetComponent<GeneticPathFinder>());
        }
    }

    /// <summary>
    /// Verifies if the creature has reached the target
    /// </summary>
    /// <returns>Whether or not the creature has reached the target</returns>
    bool HasActive()
    {
        for (int i = 0; i < population.Count; i++)
            if (!population[i].hasFinished)
                return true;

        return false;
    }

    /// <summary>
    /// Gets the fittest creature in the population
    /// </summary>
    /// <returns>The fittest</returns>
    GeneticPathFinder GetFittest()
    {
        float maxFitness = float.MinValue;
        int index = 0;

        for (int i = 0; i < population.Count; i++)
        {
            if (population[i].Fitness > maxFitness)
            {
                maxFitness = population[i].Fitness;
                index = i;
            }
        }

        // Check if population[index] is not null, zero or -1
        if (population[index])
        {
            GeneticPathFinder fittest = population[index];
            population.RemoveAt(index);
            return fittest;
        }

        return null;
    }

    void NextGeneration()
    {
        int survivorCut = Mathf.RoundToInt(populationSize * cutoff);
        List<GeneticPathFinder> survivors = new List<GeneticPathFinder>();

        for (int i = 0; i < survivorCut; i++)
            survivors.Add(GetFittest());

        for (int i = 0; i < population.Count; i++)
            Destroy(population[i].gameObject);

        for (int i = 0; i < survivors.Count; i++)
            Destroy(survivors[i].gameObject);

        population.Clear();
    }
}
