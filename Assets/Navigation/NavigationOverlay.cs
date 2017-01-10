using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationOverlay : MonoBehaviour
{
    private readonly List<GameObject> _enemyMarkers = new List<GameObject>();

    void Start()
    {
        var enemies = FindObjectsOfType<Enemy>();
       
        foreach (var enemy in enemies)
        {
            var marker = Instantiate(enemy.NavigationMarkerPrefab, transform);
            var script = marker.AddComponent<NavigationMarker>();
            script.target = enemy;

            _enemyMarkers.Add(marker);
        }
    }

}
