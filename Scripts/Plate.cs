using UnityEngine;

// A floor plate that "latches" (locks in, turns green) when the correct
// character stands on it. Yellow plates only react to the Player,
// blue plates only react to the Ghost echo.
public class Plate : MonoBehaviour
{
    [Header("Tick = YELLOW plate (player). Untick = BLUE plate (ghost).")]
    public bool isPlayerPlate;

    [Header("Drag the GameManager object here")]
    public GameManager gameManager;

    // Once latched, the plate stays pressed for the rest of the level.
    public bool IsLatched { get; private set; }

    void Start()
    {
        gameManager.RegisterPlate(this);
    }

    // Runs automatically when something with a collider enters this trigger.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsLatched) return;

        if (isPlayerPlate && other.CompareTag("Player")) Latch();
        else if (!isPlayerPlate && other.CompareTag("Ghost")) Latch();
    }

    void Latch()
    {
        IsLatched = true;
        GetComponent<SpriteRenderer>().color = new Color(0.35f, 1f, 0.45f); // green = done
        gameManager.PlateLatched();
    }
}
