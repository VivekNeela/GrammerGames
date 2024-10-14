using System.Collections;
using System.Collections.Generic;
using Codice.CM.Common.Replication;
using UnityEngine;

public static class GameSSHPath
{
     #if UNITY_EDITOR_OSX
      public static string sortingClone_SH = System.IO.Path.GetDirectoryName(Application.dataPath) + "/Assets/PlaySchoolAPI/ShellScripts/SortingGame/SortingClone_ios.sh";
    public static string sortingPull_SH = System.IO.Path.GetDirectoryName(Application.dataPath) + "/Assets/PlaySchoolAPI/ShellScripts/SortingGame/SortingPull_ios.sh";
   
    #elif UNITY_EDITOR
    public static string sortingClone_SH = System.IO.Path.GetDirectoryName(Application.dataPath) + "/Assets/PlaySchoolAPI/ShellScripts/SortingGame/SortingClone_android.sh";
    public static string sortingPull_SH = System.IO.Path.GetDirectoryName(Application.dataPath) + "/Assets/PlaySchoolAPI/ShellScripts/SortingGame/SortingPull_android.sh";
    #endif


    

    #if UNITY_EDITOR_OSX
        public static string spotTheDiffClone_SH = System.IO.Path.GetDirectoryName(Application.dataPath) + "/Assets/PlaySchoolAPI/ShellScripts/SpotTheDiff/SpotDifferenceClone_ios.sh";
    public static string spotTheDiffPull_SH = System.IO.Path.GetDirectoryName(Application.dataPath) + "/Assets/PlaySchoolAPI/ShellScripts/SpotTheDiff/SpotDifferencePull_ios.sh";
    
    #elif UNITY_EDITOR
    public static string spotTheDiffClone_SH = System.IO.Path.GetDirectoryName(Application.dataPath) + "/Assets/PlaySchoolAPI/ShellScripts/SpotTheDiff/SpotDifferenceClone_android.sh";
    public static string spotTheDiffPull_SH = System.IO.Path.GetDirectoryName(Application.dataPath) + "/Assets/PlaySchoolAPI/ShellScripts/SpotTheDiff/SpotDifferencePull_android.sh";
    #endif
}
