using System.Collections.Generic;
using UnityEngine;

// The "Echo Twin": replays the movement the player made in the previous round.
// It follows the exact recorded path, one physics step at a time, so it moves
// at exactly the same speed the player did.
public class GhostReplay : MonoBehaviour
{
    List<Vector2> path;
    int index;

    // Called by the GameManager when a round ends.
    public void StartReplay(List<Vector2> recordedPath)
    {
        path = recordedPath;
        index = 0;
        transform.position = path[0];
        gameObject.SetActive(true);
    }

    void FixedUpdate()
    {
        if (path == null) return;

        transform.position = path[index];
        index++;

        // Finished replaying -> hide until the next round.
        if (index >= path.Count)
        {
            path = null;
            gameObject.SetActive(false);
        }
    }
}
