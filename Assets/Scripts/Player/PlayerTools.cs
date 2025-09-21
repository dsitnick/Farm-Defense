using UnityEngine;
using UnityEngine.Events;

public class PlayerTools : MonoBehaviour {
    public Tool[] tools = new Tool[8];
    public HandTool handTool;

    public Transform cameraRoot, toolRoot;

    public int currentToolIndex { get; private set; } = 0;
    public Tool currentTool { get; private set; }

    public UnityEvent<int> onSetActiveTool;

    void Start() {
        // Activate the first tool by default
        if (tools.Length > 0) {
            SetActiveTool(0);
        }
    }

    void Update() {
        HandleToolbarInput();
        currentTool?.UpdateTool();
    }

    private void HandleToolbarInput() {
        // Check for number keys 1-9
        for (int i = 1; i <= tools.Length; i++) {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i)) {
                int toolIndex = i - 1; // Convert to 0-based index
                if (toolIndex < tools.Length) {
                    SetActiveTool(toolIndex);
                    break;
                }
            }
        }
    }

    private void SetActiveTool(int toolIndex) {
        // Deactivate current tool
        currentTool?.SetActive(false);

        // Set new tool
        currentToolIndex = toolIndex;
        currentTool = tools[toolIndex];

        // If tool is null, use hand tool
        if (currentTool == null) {
            currentTool = handTool;
        }

        // Activate new tool
        currentTool?.SetActive(true);

        onSetActiveTool.Invoke(toolIndex);
    }

    public void RemoveTool(int toolIndex) {
        if (tools[toolIndex] == null) return;
        Destroy(tools[toolIndex]);

        tools[toolIndex] = null;
    }

    public void SetTool(int toolIndex, GameObject prefab) {
        if (tools[toolIndex] != null) {
            RemoveTool(toolIndex);
        }

        tools[toolIndex] = prefab == null ? null : Instantiate(prefab, toolRoot).GetComponent<Tool>();

        if (toolIndex == currentToolIndex) {
            SetActiveTool(toolIndex);
        }
    }

    public int GetAvailableSlot() {
        for (int i = 0; i < tools.Length; i++) {
            if (tools[i] == null) return i;
        }
        return -1;
    }
}
