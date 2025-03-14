using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PointSpawn
{
    public static List<Vector3> GeneratePoints(float radius, Vector3 sampleRegionSize, int numSamplesBeforeRejection = 30)
    {
        if (sampleRegionSize.x <= 0 || sampleRegionSize.y <= 0 || sampleRegionSize.z <= 0)
        {
            Debug.LogError("Invalid sampleRegionSize: " + sampleRegionSize);
            return new List<Vector3>();
        }

        float cellSize = radius / Mathf.Sqrt(3);

        int gridSizeX = Mathf.CeilToInt(sampleRegionSize.x / cellSize);
        int gridSizeY = Mathf.CeilToInt(sampleRegionSize.y / cellSize);
        int gridSizeZ = Mathf.CeilToInt(sampleRegionSize.z / cellSize);

        Debug.Log($"Grid Size: {gridSizeX}, {gridSizeY}, {gridSizeZ}");

        if (gridSizeX <= 0 || gridSizeY <= 0 || gridSizeZ <= 0)
        {
            Debug.LogError("Grid size is invalid!");
            return new List<Vector3>();
        }

        int[,,] grid = new int[gridSizeX, gridSizeY, gridSizeZ];
        List<Vector3> points = new List<Vector3>();
        List<Vector3> spawnPoints = new List<Vector3>();

        spawnPoints.Add(sampleRegionSize / 2);
        Debug.Log("Initial spawnPoints count: " + spawnPoints.Count);

        while (spawnPoints.Count > 0)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector3 spawnCentre = spawnPoints[spawnIndex];
            bool candidateAccepted = false;

            for (int i = 0; i < numSamplesBeforeRejection; i++)
            {
                float angle1 = Random.value * Mathf.PI * 2;
                float angle2 = Random.value * Mathf.PI;
                Vector3 dir = new Vector3(
                    Mathf.Sin(angle1) * Mathf.Cos(angle2),
                    Mathf.Sin(angle1) * Mathf.Sin(angle2),
                    Mathf.Cos(angle1)
                );
                Vector3 candidate = spawnCentre + dir * Random.Range(radius, 2 * radius);

                if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid))
                {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);

                    int cellX = (int)(candidate.x / cellSize);
                    int cellY = (int)(candidate.y / cellSize);
                    int cellZ = (int)(candidate.z / cellSize);

                    if (cellX < gridSizeX && cellY < gridSizeY && cellZ < gridSizeZ)
                    {
                        grid[cellX, cellY, cellZ] = points.Count;
                    }

                    candidateAccepted = true;
                    break;
                }
                else
                {
                    Debug.LogWarning($"Candidate {candidate} rejected.");
                }
            }

            if (!candidateAccepted)
            {
                spawnPoints.RemoveAt(spawnIndex);
            }
        }

        Debug.Log($"Final points count: {points.Count}");
        return points;
    }


    static bool IsValid(Vector3 candidate, Vector3 sampleRegionSize, float cellSize, float radius, List<Vector3> points, int[,,] grid)
    {
        if (candidate.x >= 0 && candidate.x < sampleRegionSize.x &&
            candidate.y >= 0 && candidate.y < sampleRegionSize.y &&
            candidate.z >= 0 && candidate.z < sampleRegionSize.z)
        {
            int cellX = (int)(candidate.x / cellSize);
            int cellY = (int)(candidate.y / cellSize);
            int cellZ = (int)(candidate.z / cellSize);

            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
            int searchStartY = Mathf.Max(0, cellY - 2);
            int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);
            int searchStartZ = Mathf.Max(0, cellZ - 2);
            int searchEndZ = Mathf.Min(cellZ + 2, grid.GetLength(2) - 1);

            for (int x = searchStartX; x <= searchEndX; x++)
            {
                for (int y = searchStartY; y <= searchEndY; y++)
                {
                    for (int z = searchStartZ; z <= searchEndZ; z++)
                    {
                        int pointIndex = grid[x, y, z] - 1;
                        if (pointIndex != -1)
                        {
                            float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
                            if (sqrDst < radius * radius)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        return false;
    }
}
