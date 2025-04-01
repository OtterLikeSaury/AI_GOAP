using Sirenix.OdinInspector;
using System.Collections.Generic;

public class GOAPGoals
{
    //�ڲ�������
    public class Goal
    {
        [LabelText("Ŀ��״̬"),OnValueChanged("CheckState")]public GOAPStateType targetState;
        [LabelText("Ŀ������")] public GOAPStateComparer targetValue;
        [LabelText("����ϵ��"),HorizontalGroup("1")] public float priorityMultiply;
        [LabelText("ʵʱ���ȼ�"), HorizontalGroup("1")] public float runtimePriority;

        [LabelText("�Ƿ���ж�����Ŀ��")] public bool canBreak;
        [LabelText("�Ƿ�ɱ�����Ŀ���ж�")] public bool canBeBreak;
        [LabelText("���ռ������ȼ�"), ShowInInspector,ReadOnly,HorizontalGroup("1")] public float piority => priorityMultiply * runtimePriority;
        [LabelText("Ŀ������")] public IGOAPGoalChecker checker;

#if UNITY_EDITOR
        public void CheckState()
        {
            if (GOAPEditorUtility.global!=null&&GOAPEditorUtility.global.TryGetGlobalState(targetState,out GOAPStateBase state)
                &&(targetValue==null||targetValue.GetType()!=state.GetComparerType()))
            {
                targetValue = state.GetComparer();
            }
            else if (GOAPEditorUtility.agent!=null&&GOAPEditorUtility.agent.states.TryGetState(targetState,out state)
                && (targetValue == null || targetValue.GetType() != state.GetComparerType()))
            {
                targetValue = state.GetComparer();
            }

        }
#endif
    }

    private class SortedGoalComparer:IComparer<string>
    {
        public Dictionary<string, Goal> dic;

        public SortedGoalComparer(Dictionary<string, Goal> dic)
        {
            this.dic = dic;
        }

        public int Compare(string x, string y)
        {
            if (x == y) return 0;  //��Ŀ��ͬ��

            int com = dic[y].piority.CompareTo(dic[x].piority);
            if (com == 0) return -1;   //ͬ���ȼ�����ϣ�㷨ȥ����
            else return com;
        }
    }
    //


    public Dictionary<string, Goal> dic = new Dictionary<string, Goal>();
    private SortedList<string, Goal> sortedList;

    private GOAPAgent agent;
    private IGOAPOwner owner;
    public void Init(GOAPAgent agent,IGOAPOwner owner)
    {
        this.agent = agent;
        this.owner = owner;
        sortedList = new SortedList<string, Goal>(dic.Count,new SortedGoalComparer(dic));

    }

    /// <summary>
    /// ��������Ŀ���Ŀ�������������ص�ǰʵʱĿ�������Ŀ��List
    /// </summary>
    /// <returns></returns>
    public SortedList<string, Goal> UpdateGoals()
    {
        if (dic==null||dic.Count==0) return null;
        sortedList.Clear();
        foreach (KeyValuePair<string, Goal> item in dic)
        {
            if (item.Value.checker!=null)
            {
                item.Value.checker.Update(item.Value,agent,owner);
            }

            sortedList.Add(item.Key,item.Value);
        }

        return sortedList;
    }

#if UNITY_EDITOR
    [Button("���Ŀ��״̬������")]
    public void CheckGoalsTargetValueType()
    {

        foreach (KeyValuePair<string, Goal> item in dic)
        {
            item.Value.CheckState();
        }

    }
#endif
}
