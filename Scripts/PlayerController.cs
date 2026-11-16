using UnityEngine;

// Moves the player with WASD or the arrow keys.
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // -1, 0 or 1 for each axis, based on the keys held down.
        float x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        float y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

        Vector2 direction = new Vector2(x, y);

        // Prevent moving faster when pressing two keys diagonally.
        if (direction.magnitude > 1f) direction = direction.normalized;

        body.velocity = direction * moveSpeed;
    }
}
