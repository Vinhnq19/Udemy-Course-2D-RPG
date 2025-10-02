using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Experimental.GraphView;

public class UI_TreeConnection : MonoBehaviour
{
    [SerializeField] private RectTransform rotationPoint;
    [SerializeField] private RectTransform connectionLength;
    [SerializeField] private RectTransform childNodeConnectPoint;

    public void DirectConnection(NodeDirectionType direction, float length, float offset)
    {
        bool shoudBeActive = direction != NodeDirectionType.None;
        float finalLength = shoudBeActive ? length : 0f;
        float angle = GetDirectionAngle(direction);

        rotationPoint.localRotation = Quaternion.Euler(0f, 0f, angle + offset);
        connectionLength.sizeDelta = new Vector2(finalLength, connectionLength.sizeDelta.y);
    }

    public Image GetConnectionImage() => connectionLength.GetComponent<Image>();
    public Vector2 GetConnectionPoint(RectTransform rect)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle
        (
            rect.parent as RectTransform,
            childNodeConnectPoint.position,
            null,
            out var localPoint
        );
        return localPoint;
    }
    private float GetDirectionAngle(NodeDirectionType direction)
    {
        switch (direction)
        {
            case NodeDirectionType.Left:
                return 180f;
            case NodeDirectionType.Right:
                return 0f;
            case NodeDirectionType.Up:
                return 90f;
            case NodeDirectionType.Down:
                return -90f;
            case NodeDirectionType.UpLeft:
                return 135f;
            case NodeDirectionType.UpRight:
                return 45f;
            case NodeDirectionType.DownLeft:
                return -135f;
            case NodeDirectionType.DownRight:
                return -45f;
            default:
                return 0f;
        }
    }
}
public enum NodeDirectionType
{
    None,
    Left,
    Right,
    Up,
    Down,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight
}
