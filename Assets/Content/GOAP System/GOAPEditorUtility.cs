
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

/// <summary>
/// ������ֻ�����ڱ༭���У������û�и�����
/// </summary>
public static class GOAPEditorUtility
{
    public static GOAPAgent agent;
    public static GOAPGlobal global { get; private set; }


    [InitializeOnLoadMethod]
    public static void Init()
    {
        TryGetGlobal();
        EditorSceneManager.sceneOpened += EditorSceneManager_sceneOpend;
    }

    private static void EditorSceneManager_sceneOpend(Scene scene, OpenSceneMode mode)
    {
        GetGlobal();
    }

    private static void TryGetGlobal()
    {
        if (global == null) GetGlobal();
    }

    private static void GetGlobal()
    {
        global= GameObject.FindAnyObjectByType<GOAPGlobal>();
    }
}

#endif
