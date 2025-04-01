using Mono.Cecil.Cil;
using UnityEditor;
using UnityEngine;

public class GOAPPlanWindow : EditorWindow
{
    [MenuItem("GOAP/GOAPPlanWindow")]
    static void OpenWindow()
    {
        GetWindow<GOAPPlanWindow>();
    }

    private GOAPPlan plan;
    private Vector2 scrollPosition;
    private void OnGUI()
    {
        //Test();
        //return;
        if (Selection.gameObjects.Length == 0) return;

        GameObject go = Selection.gameObjects[0];
        if (go == null) return;

        GOAPAgent agent = go.GetComponent<GOAPAgent>();
        if (agent == null) return;

        plan = agent.plan;
        if (plan == null || plan.startNode == null||plan.goalName==null) return;
        EditorGUILayout.LabelField($"�ƻ���{plan.goalName}");

        scrollPosition = GUILayout.BeginScrollView(scrollPosition); //���������

        GOAPPlanNode startNode = plan.startNode;
        Color oldColor = GUI.color;

        PrintNode(startNode);

        GUI.color = oldColor;

        GUILayout.EndScrollView();
    }

    /// <summary>
    /// ���Ƽƻ��ڵ�
    /// </summary>
    /// <param name="node"></param>
    /// <param name="depth"></param>
    private void PrintNode(GOAPPlanNode node, int depth = 0)
    {

        string prefix = new string(' ',depth*6) ;   //����ݹ�ո�ռλ����


        string nodeName = null;
        if (node.action.actionName==null)
        {
            nodeName = $"{prefix}{node.action.GetType().Name}";
        }    //�ڵ�����
        else
        {
            nodeName= $"{prefix}{node.action.actionName}";
        }

        GUI.color = plan.runingNode == node ? Color.red : Color.yellow;  //��Ϊ�Ƿ������У�����Ϊ��ɫ��ֹͣΪ��ɫ
        EditorGUILayout.LabelField(nodeName);

        for (int i = 0; i < node.preconditions.Count; i++)
        {
            PrintNode(node.preconditions[i],depth+1);
        }
    }

    #region Test

    #endregion
}
