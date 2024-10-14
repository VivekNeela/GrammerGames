using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public class PlayschoolEditorWindow : EditorWindow
{
    private static string inputText = System.IO.Path.GetDirectoryName(Application.dataPath); // This will store the project root directory
    private Texture2D image; // This will store the image to be displayed
    private Texture2D developerPhoto; // This will store the developer's photo

    private const int ImageSize = 250; // Size for the image and frame

    // Add a menu item to open this window
    [MenuItem("PlaySchool/Update Submodule")]
    private static void ShowWindow()
    {
        // Show the window with a title
        GetWindow<PlayschoolEditorWindow>("Custom Window");
    }



    // Create the GUI for the window
    private void OnEnable()
    {
        // Set inputText to the project's root folder path (without Assets folder)
        inputText = System.IO.Path.GetDirectoryName(Application.dataPath);

        // Load an image from the Resources folder (or any other path)
        image = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/PlaySchoolAPI/Images/myImage.jpg", typeof(Texture2D));

        // Load developer's photo from the Resources folder (or any other path)
        developerPhoto = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Resources/developerPhoto.jpg", typeof(Texture2D));
    }

    private void OnGUI()
    {
        CreditsImage();
        PlaySchoolAPI();
        SortingGameClone();
    }

    void CreditsImage()
    {
        // Add a "Developed by" label
        GUILayout.Label("Developed by:", EditorStyles.boldLabel);

        if (image != null)
        {
            // Define the frame area with fixed size for the main image
            Rect imageRect = GUILayoutUtility.GetRect(ImageSize, ImageSize, GUILayout.Width(ImageSize + 10), GUILayout.Height(ImageSize + 10));

            // Create a custom GUIStyle for the frame around the image
            GUIStyle imageFrameStyle = new GUIStyle();
            imageFrameStyle.normal.background = Texture2D.whiteTexture; // Set frame color (white in this case)
            imageFrameStyle.border = new RectOffset(5, 5, 5, 5); // Set border size

            // Draw the frame around the main image
            GUI.Box(imageRect, GUIContent.none, imageFrameStyle);

            // Draw the image inside the frame, scaled to fit within the frame
            GUI.DrawTexture(imageRect, image, ScaleMode.ScaleToFit, true);
        }
        else
        {
            GUILayout.Label("Main image not found");
        }

        //GUILayout.Space(50);
    }

    void PlaySchoolAPI()
    {
        // Add a label for the project location
        GUILayout.Label("Project Location", EditorStyles.boldLabel);

        // Display the non-editable label with the project root location
        EditorGUILayout.LabelField("Project Path:", inputText);

        // Add a button, and perform action when clicked
        if (GUILayout.Button("Pull PlayShool API"))
        {
            ProveYouAreSmartEditorWindow.ShowWindow();

            //     // Determine the platform and run the appropriate command
            //     #if UNITY_EDITOR_WIN
            //     // Windows
            //     string gitCommand = "git submodule update --remote";
            //     string fullCommand = $"/k cd /d \"{inputText}\" && {gitCommand}";
            //     Process.Start("cmd.exe", fullCommand);
            //     #elif UNITY_EDITOR_OSX
            //     // macOS
            //     string gitCommand = "git submodule update --remote";
            //     string fullCommand = $"cd \"{inputText}\"; {gitCommand}";
            //     string terminalCommand = $"/bin/zsh -c \"{fullCommand}\"";
            //     Process.Start("open", $"-a Terminal \"{terminalCommand}\"");
            //  //   Process.Start("open", "-a Terminal");
            //     #endif

            UnityEngine.Debug.Log("Command executed for platform.");
        }
    }

    public static void PullPlaySchoolAPI()
    {
        // Determine the platform and run the appropriate command
#if UNITY_EDITOR_WIN
            // Windows
            string gitCommand = "git submodule update --remote";
            string fullCommand = $"/k cd /d \"{inputText}\" && {gitCommand}";
            Process.Start("cmd.exe", fullCommand);
#elif UNITY_EDITOR_OSX
        // macOS
         string gitCommand = "git submodule update --remote";
        // string fullCommand = $"cd \"{inputText}\"; {gitCommand}";
        // string terminalCommand = $"/bin/zsh -c \"{fullCommand}\"";
        // Process.Start("open", $"-a Terminal \"{inputText}\" && {gitCommand}");
        WriteToFile(inputText+"/Assets/PlaySchoolAPI/ShellScripts/PlaySchoolAPI/PlaySchoolAPI.sh",gitCommand,()=>
        {
            RunSSH(inputText+"/Assets/PlaySchoolAPI/ShellScripts/PlaySchoolAPI/PlaySchoolAPI.sh");
        });

        //   Process.Start("open", "-a Terminal");
#endif
    }

    static void WriteToFile(string path, string content, Action callback)
    {
        try
        {
            // Create or open the file, write to it, and ensure content is flushed and saved
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.Write(content);
                writer.Flush();  // Ensure the content is written to the file
            }
            UnityEngine.Debug.Log($"File written and saved successfully at {path}");
            callback?.Invoke();
        }
        catch (System.Exception ex)
        {
          UnityEngine.Debug.LogError($"Failed to write and save the file: {ex.Message}");
        }

    }



    private Vector2 scrollPosition = new Vector2(1000, 10);

    void SortingGameClone()
    {
        // Start the outer horizontal layout
        EditorGUILayout.BeginHorizontal();

        // Scroll view on the left (for buttons)
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandWidth(true), GUILayout.Height(position.height));

        // First column for buttons inside the scroll view
        EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true)); // Automatically adjusts to window size

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Clone Sorting Game", GameSSHPath.sortingClone_SH), GUILayout.ExpandWidth(true)))
        {
            RunSSH(GameSSHPath.sortingClone_SH);
        }

        if (GUILayout.Button(new GUIContent("Pull Sorting Game", GameSSHPath.sortingPull_SH), GUILayout.ExpandWidth(true)))
        {
            RunSSH(GameSSHPath.sortingPull_SH);
        }
        EditorGUILayout.EndHorizontal();

        // Add all your "AD" buttons in the scroll view
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Clone SpotTheDiff Game", GameSSHPath.spotTheDiffClone_SH), GUILayout.ExpandWidth(true)))
        {
            RunSSH(GameSSHPath.spotTheDiffClone_SH);


        }
        if (GUILayout.Button(new GUIContent("Pull SpotTheDiff Game", GameSSHPath.spotTheDiffPull_SH), GUILayout.ExpandWidth(true)))
        {
            RunSSH(GameSSHPath.spotTheDiffPull_SH);


        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.EndVertical();

        // End the scroll view
        EditorGUILayout.EndScrollView();

        // End the outer horizontal layout
        EditorGUILayout.EndHorizontal();
    }

    private static void RunSSH(string ssh)
    {
        string scriptssh = ssh;
        Process process = new Process();
        process.StartInfo.FileName = "/bin/bash";
        process.StartInfo.Arguments = $"\"{scriptssh}\"";
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;
        process.Start();
        string result = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();
    }

}
