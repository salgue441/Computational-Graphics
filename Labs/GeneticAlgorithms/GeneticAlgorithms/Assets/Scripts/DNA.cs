using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a DNA sequence
/// </summary>
public class DNA : MonoBehaviour
{
    public List<Vector3> genes = new();

    /// <summary>
    /// Constructs a new DNA:: DNA object.
    /// </summary>
    /// <param name="genomeLength">The length of the genome</param>
    public DNA(int genomeLength = 50) : base()
    {
        for (int i = 0; i < genomeLength; i++)
        {
            genes.Add(new(Random.Range(-1.0f, 1.0f),
                Random.Range(-1.0f, 1.0f)));
        }
    }

    /// <summary>
    /// Constructs a new DNA:: DNA object. This is used for reproduction.
    /// </summary>
    /// <param name="parent">The parent DNA object</param>
    /// <param name="partner">The partner DNA object</param>
    /// <param name="mutationRate">The mutation rate, default is 0.01f</param>
    public DNA(DNA parent, DNA partner, float mutationRate = 0.01f) : base()
    {
        for (int i = 0; i < parent.genes.Count; i++)
        {
            if (Random.Range(0.0f, 1.0f) < mutationRate)
                genes.Add(new(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)));

            else
            {
                int chance = Random.Range(0, 2);
                genes.Add(chance == 0 ? parent.genes[i] : partner.genes[i]);
            }
        }
    }
}
