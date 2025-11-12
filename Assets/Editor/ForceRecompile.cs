using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;
// had to make this since unity wasnt auto compiling sometimes :P
public class ForceRecompile
{
    [MenuItem("Tools/Random/Recompile")]
    public static void RecompileAllScripts()
    {
        CompilationPipeline.RequestScriptCompilation(RequestScriptCompilationOptions.CleanBuildCache);
        Debug.Log("forced script recomp :P");
    }
}