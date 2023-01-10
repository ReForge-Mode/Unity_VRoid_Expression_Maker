using UnityEngine;
using System.IO;

public class ScreenshotTaker : MonoBehaviour
{
    public Camera camera;
    public Vector2 cropSize;
    public string screenshotPath;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.F12))
        {
            TakeScreenshot();
        }
    }

    public void TakeScreenshot()
    {
        // Create a new RenderTexture with the desired dimensions
        RenderTexture renderTexture = new RenderTexture((int)cropSize.x, (int)cropSize.y, 24);

        // Set the camera's target texture to the new RenderTexture
        camera.targetTexture = renderTexture;

        // Render the camera's view
        camera.Render();

        // Create a new Texture2D with the same dimensions as the RenderTexture
        Texture2D screenshot = new Texture2D((int)cropSize.x, (int)cropSize.y, TextureFormat.RGB24, false);

        // Read the pixels from the RenderTexture into the Texture2D
        screenshot.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

        // Apply the texture
        screenshot.Apply();

        // Encode the texture into a PNG
        byte[] bytes = screenshot.EncodeToPNG();

        // Write the PNG to a file
        File.WriteAllBytes(screenshotPath, bytes);

        // Reset the camera's target texture
        camera.targetTexture = null;

        // Release the RenderTexture
        RenderTexture.ReleaseTemporary(renderTexture);

    }
}
