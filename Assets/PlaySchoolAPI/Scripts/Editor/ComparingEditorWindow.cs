using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions; // Add this to use Path methods

public class ComparingEditorWindow : EditorWindow
{
    private Texture2D image1; // First random image (from folder 1)
    private Texture2D image2; // Second random image (from folder 2)
    private string imageName1; // Name of the first random image
    private string imageName2; // Name of the second random image

    [MenuItem("PlaySchool/Who is smart")]
    public static void ShowWindow()
    {
        // Create the editor window with a title
        ComparingEditorWindow window = GetWindow<ComparingEditorWindow>("Prove you are smart");

        // Set a fixed size for the window
        window.minSize = new Vector2(600, 500); // Set minimum window size to accommodate bigger images
        window.maxSize = new Vector2(600, 500); // Fix the window size
    }

    private void OnEnable()
    {
        // Define the folder paths
        string folderPath1 = "Assets/PlaySchoolAPI/Images/Yash/";   // Folder 1
        string folderPath2 = "Assets/PlaySchoolAPI/Images/Others/";   // Folder 2

        // Load a random image from folder 1
        image1 = LoadRandomImageFromFolder(folderPath1, out imageName1);
        

        // Load a random image from folder 2
        image2 = LoadRandomImageFromFolder(folderPath2, out imageName2);
    }

    // Helper method to load a random image from a specified folder and return its name
    private Texture2D LoadRandomImageFromFolder(string folderPath, out string imageName)
    {
        // Find all assets of type Texture2D in the folder
        string[] imageGuids = AssetDatabase.FindAssets("t:Texture2D", new[] { folderPath });

        // If there are images found
        if (imageGuids.Length > 0)
        {
            // Select a random index
            int randomIndex = Random.Range(0, imageGuids.Length);

            // Load the image at the random index
            string imagePath = AssetDatabase.GUIDToAssetPath(imageGuids[randomIndex]);
            Texture2D image = (Texture2D)AssetDatabase.LoadAssetAtPath(imagePath, typeof(Texture2D));

            // Extract the image name (without extension) using Path
            imageName = Path.GetFileNameWithoutExtension(imagePath);
            imageName = Regex.Replace(imageName, @"\d", "");

            // Log to check which image was chosen (for debugging purposes)
            Debug.Log("Loaded random image from " + folderPath + ": " + imagePath);

            return image;
        }
        else
        {
            Debug.LogWarning("No images found in the folder: " + folderPath);
            imageName = null;
            return null;
        }
    }

    private void OnGUI()
    {
        // Set the style for the heading (centered and bold)
        GUIStyle headingStyle = new GUIStyle(GUI.skin.label);
        headingStyle.fontSize = 20;  // Set the font size
        headingStyle.alignment = TextAnchor.MiddleCenter; // Center the text
        headingStyle.fontStyle = FontStyle.Bold;  // Make the text bold

        // Add some vertical space at the top
        GUILayout.Space(20);

        // Display the heading centered
        GUILayout.Label("Who is Smart?", headingStyle);

        // Add some vertical space between heading and images
        GUILayout.Space(20);

        // Begin horizontal layout for the two images
        GUILayout.BeginHorizontal();

        // Add flexible space to center the image pair horizontally
        GUILayout.FlexibleSpace();

        // Display the first image (from folder 1)
        if (image1 != null)
        {
            GUILayout.BeginVertical(); // Stack the image and button vertically
            GUILayout.Label(image1, GUILayout.Width(300), GUILayout.Height(300)); // Set the image size
            if (GUILayout.Button(imageName1, GUILayout.Width(150))) // Button below the first image shows only the image name
            {
                Debug.Log(imageName1 + " selected!");
                PlayschoolEditorWindow.PullPlaySchoolAPI();
            }
            GUILayout.EndVertical();
        }

        // Add some horizontal space between the two images
        GUILayout.Space(20);

        // Display the second image (from folder 2)
        if (image2 != null)
        {
            GUILayout.BeginVertical(); // Stack the image and button vertically
            GUILayout.Label(image2, GUILayout.Width(300), GUILayout.Height(300)); // Set the image size
            if (GUILayout.Button(imageName2, GUILayout.Width(150))) // Button below the second image shows only the image name
            {
                Debug.Log(imageName2 + " selected!");
                EditorApplication.Exit(0);
            }
            GUILayout.EndVertical();
        }

        // Add flexible space to center the image pair horizontally
        GUILayout.FlexibleSpace();

        // End horizontal layout
        GUILayout.EndHorizontal();
    }
}
