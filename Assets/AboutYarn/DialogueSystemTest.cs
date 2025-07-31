using UnityEngine;
using Yarn.Unity;

public class DialogueSystemTest : MonoBehaviour
{
    public DialogueRunner runner;
    private void Awake()
    {
        runner = GetComponent<DialogueRunner>();
    }
    void Start()
    {
        runner.StartDialogue("Start"); // 指向 yarn 文件的 Start 节点
    }
}
