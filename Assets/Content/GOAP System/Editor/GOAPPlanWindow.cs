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
        EditorGUILayout.LabelField($"计划：{plan.goalName}");

        scrollPosition = GUILayout.BeginScrollView(scrollPosition); //创造滚动条

        GOAPPlanNode startNode = plan.startNode;
        Color oldColor = GUI.color;

        PrintNode(startNode);

        GUI.color = oldColor;

        GUILayout.EndScrollView();
    }

    /// <summary>
    /// 绘制计划节点
    /// </summary>
    /// <param name="node"></param>
    /// <param name="depth"></param>
    private void PrintNode(GOAPPlanNode node, int depth = 0)
    {

        string prefix = new string(' ',depth*6) ;   //创造递归空格占位符号


        string nodeName = null;
        if (node.action.actionName==null)
        {
            nodeName = $"{prefix}{node.action.GetType().Name}";
        }    //节点命名
        else
        {
            nodeName= $"{prefix}{node.action.actionName}";
        }

        GUI.color = plan.runingNode == node ? Color.red : Color.yellow;  //行为是否在运行，运行为红色，停止为黄色
        EditorGUILayout.LabelField(nodeName);

        for (int i = 0; i < node.preconditions.Count; i++)
        {
            PrintNode(node.preconditions[i],depth+1);
        }
    }

    #region Test

    #endregion
}
