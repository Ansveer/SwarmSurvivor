using UnityEngine;

public class SceneResize : MonoBehaviour
{
    public Sprite backgroundSprite;
    public bool fullscreen = false;
    public int screenWidth;
    public int screenHeight;

    void Start()
    {
        if (backgroundSprite != null)
        {
            int spriteWidth = (int)backgroundSprite.rect.width;
            int spriteHeight = (int)backgroundSprite.rect.height;

            Screen.SetResolution(spriteWidth, spriteHeight, fullscreen);
        }
        else
        {
            Screen.SetResolution(screenWidth, screenHeight, fullscreen);
        }
    }
}
