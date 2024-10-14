using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ProveYouAreSmartEditorWindow : EditorWindow
{
    private Texture2D image; // Texture for the image

    [MenuItem("PlaySchool/Prove You are smart")]
    public static void ShowWindow()
    {
         // Create the editor window with a title
    ProveYouAreSmartEditorWindow window = GetWindow<ProveYouAreSmartEditorWindow>("Prove you are smart");

    // Set a fixed size for the window
    window.minSize = new Vector2(400, 300); // Replace with desired width and height
    window.maxSize = new Vector2(400, 300); // Same as minSize to fix the size

    // Optionally, show the window in the center of the screen
    //window.position = new Rect(0, 0, 400, 300); // Center it
    }

    private void OnEnable()
    {
        // Load the image from the Resources folder
        image = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/PlaySchoolAPI/Images/RecaptchaLogo.png", typeof(Texture2D));
    }


    private void OnGUI()
    {
        // Set the style for the heading (centered and bold)
        GUIStyle headingStyle = new GUIStyle(GUI.skin.label);
        headingStyle.fontSize = 20;  // Set the font size
        headingStyle.alignment = TextAnchor.MiddleCenter; // Center the text
        headingStyle.fontStyle = FontStyle.Bold;  // Make the text bold

        // Add vertical space at the top
        GUILayout.Space(0);

        // Begin vertical layout to place image and heading in the center
        GUILayout.BeginVertical();

        // Flexible space to center the content vertically
        GUILayout.FlexibleSpace();

        // Begin horizontal layout to center the image horizontally
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        // Display the image (centered horizontally)
        if (image != null)
        {
            GUILayout.Label(image, GUILayout.Width(100), GUILayout.Height(100)); // Adjust image size as needed
        }

        // End horizontal layout
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        // Add some vertical space between image and heading
        GUILayout.Space(10);

        // Center the heading below the image
        GUILayout.Label("Prove You are Smart!", headingStyle);

        if(GUILayout.Button("Start Assement"))
        {
            ComparingEditorWindow.ShowWindow();
        }

        // Flexible space to center the content vertically
        GUILayout.FlexibleSpace();

        // End vertical layout
        GUILayout.EndVertical();

    }
}
