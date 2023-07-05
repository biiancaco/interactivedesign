using UnityEngine;

public class FlowerScript : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private int currentIndex;
    private float timer;
    private bool isAnimating;

    private void Start()
    {
        // Initialize the sprite renderer and current index
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentIndex = 0;

        // Check if there are sprites assigned
        if (sprites.Length == 0)
        {
            Debug.LogError("No sprites assigned to the FlowerScript.");
            enabled = false; // Disable the script
        }
    }

    private void Update()
    {
        if (isAnimating)
        {
            // Increment the timer
            timer += Time.deltaTime;

            // Check if it's time to change the sprite
            if (timer >= 0.1f)
            {
                // Increment the index and loop back to the beginning if necessary
                currentIndex = (currentIndex + 1) % sprites.Length;

                // Change the sprite
                spriteRenderer.sprite = sprites[currentIndex];

                // Reset the timer
                timer = 0f;
            }
        }
    }

    public void StartAnimation()
    {
        // Start the animation
        isAnimating = true;
    }

    public void StopAnimation()
    {
        // Stop the animation
        isAnimating = false;
    }
}
