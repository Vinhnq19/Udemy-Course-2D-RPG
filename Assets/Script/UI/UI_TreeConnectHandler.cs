using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

[Serializable]

public class UI_TreeConnectDetails
{
    public UI_TreeConnectHandler chillNode;
    public NodeDirectionType direction;
    [Range(100f, 350f)] public float length;
    [Range(-25f, 25f)] public float rotation;
}

public class UI_TreeConnectHandler : MonoBehaviour
{
    private RectTransform myRect => GetComponent<RectTransform>();
    [SerializeField] private UI_TreeConnection[] connections;
    [SerializeField] private UI_TreeConnectDetails[] details;

    private Image connectionImage;
    private Color originalColor;

    private void Awake()
    {
        if (connectionImage != null)
        originalColor = connectionImage.color;
    }

    public UI_TreeNode[] GetChildNodes()
    {
        List<UI_TreeNode> childrenToReturn = new List<UI_TreeNode>();
        foreach (var node in details)
        {
            if (node.chillNode != null)
            {
                childrenToReturn.Add(node.chillNode.GetComponent<UI_TreeNode>());
            }
        }
        return childrenToReturn.ToArray();
    }

    private void OnValidate()
    {
        if (details.Length <= 0) return;
        if (details.Length != connections.Length)
        {
            Debug.LogWarning("Connection details count does not match connection count." + gameObject.name);
            return;
        }
        UpdateAllConnections();
    }
    private void UpdateConnection()
    {
        for (int i = 0; i < details.Length; i++)
        {
            var detail = details[i];
            var connection = connections[i];
            Vector2 targetPosition = connection.GetConnectionPoint(myRect);
            Image connectionImage = connection.GetConnectionImage();
            connection.DirectConnection(detail.direction, detail.length, detail.rotation);

            if (detail.chillNode == null) continue;
            detail.chillNode.SetPosition(targetPosition);
            detail.chillNode.SetConnectionImage(connectionImage);
            detail.chillNode.transform.SetAsLastSibling(); // Ensure child nodes are rendered above connections
        }
    }

    public void UpdateAllConnections()
    {
        UpdateConnection();

        foreach (var node in details)
        {
            if (node.chillNode == null) continue;
            node.chillNode?.UpdateAllConnections();
        }
    }

    public void ConnectImageUnlocked(bool unlocked)
    {
        if (connectionImage == null) return;
        connectionImage.color = unlocked ? Color.white : originalColor;
    }

    public void SetConnectionImage(Image image) => connectionImage = image;
    public void SetPosition(Vector2 position) => myRect.anchoredPosition = position;
}
