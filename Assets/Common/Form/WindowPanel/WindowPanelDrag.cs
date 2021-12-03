using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowPanelDrag : MonoBehaviour
{
    private bool isDraging = false;//�Ƿ�����ק��
    private DragDirection dragDirection = DragDirection.None;
    private Vector3 lastPos;
    private WindowPanel windowPanel;
    public RectTransform rectTransform { get; private set; }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        windowPanel = GetComponent<WindowPanel>();

    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (windowPanel.isEnter)
            {
                dragDirection = DetectionDragDirection();
                if (dragDirection != DragDirection.None)
                {
                    isDraging = true;
                    lastPos = Input.mousePosition;
                }
            }
        }
        else
        {

            if (isDraging)
            {
                if (Input.GetMouseButton(0))
                {
                    Vector2 offset = Input.mousePosition - lastPos;
                    lastPos = Input.mousePosition;
                    DragPanelWindow(dragDirection, offset);

                }
                else
                {
                    isDraging = false;

                }
            }
            else
            {
                if (!Input.anyKey)
                {
                    DragDirection direction = DetectionDragDirection();
                    if (direction != dragDirection)
                    {
                        dragDirection = direction;
                        WindowsDrag.SetSystemCursor((DragDirection)Enum.Parse(typeof(DragDirection), dragDirection.ToString()));

                    }

                }

            }
        }
    }




    /// <summary>
    /// ��ק���Ŵ���
    /// </summary>
    /// <param name="dragDirection">��ק��巽��</param>
    /// <param name="offsetValue"> �϶���ƫ��ֵ</param>
    private void DragPanelWindow(DragDirection dragDirection, Vector2 offsetValue)
    {
        if (FindRelationNode(dragDirection, out Node leftOrTopNode, out Node rightOrBottomNode))
        {
            Vector2 size = Vector2.zero;
            float newWidth = 0;
            float newHeight = 0;
            float minSize = 0;
            switch (dragDirection)
            {
                case DragDirection.None:
                    break;
                case DragDirection.Left:
                    minSize = Mathf.Min(300, leftOrTopNode.rectTransform.rect.width);
                    newWidth = leftOrTopNode.rectTransform.rect.width + offsetValue.x;

                    //�жϴ����Ƿ����û��趨�ķ�Χ��
                    if (newWidth < minSize)
                    {
                        //Debug.LogError("�µ�ƫ����"+(offsetValue.x - (minSize.x - newWidth)));
                        offsetValue.x += (minSize - newWidth);
                    }
                    else
                    {
                        minSize = Mathf.Min(300, rightOrBottomNode.rectTransform.rect.width);

                        newWidth = rightOrBottomNode.rectTransform.rect.width - offsetValue.x;
                        if (newWidth < minSize)
                        {
                            //Debug.LogError("�µ�ƫ����"+(offsetValue.x - (minSize.x - newWidth)));
                            offsetValue.x -= (minSize - newWidth);
                        }
                    }

                    offsetValue.y = 0;
                    size = leftOrTopNode.rectTransform.rect.size + new Vector2(offsetValue.x, offsetValue.y);//�ƶ����������С

                    SetSizeDeltaValue(leftOrTopNode.rectTransform, new Vector3(offsetValue.x / 2, offsetValue.y / 2));

                    //leftOrTopNode.rectTransform.position = leftOrTopNode.rectTransform.position + new Vector3(offsetValue.x / 2, offsetValue.y / 2);//�ƶ�������λ��;
                    leftOrTopNode.rectTransform.SetRectTransformSizeWithCurrentAnchors(size.x, size.y);


                    size = rightOrBottomNode.rectTransform.rect.size - new Vector2(offsetValue.x, offsetValue.y);//�ƶ����������С
                    SetSizeDeltaValue(rightOrBottomNode.rectTransform, new Vector3(offsetValue.x / 2, offsetValue.y / 2));

                    //rightOrBottomNode.rectTransform.position = rightOrBottomNode.rectTransform.position + new Vector3(offsetValue.x / 2, offsetValue.y / 2);//�ƶ�������λ��;
                    rightOrBottomNode.rectTransform.SetRectTransformSizeWithCurrentAnchors(size.x, size.y);

                    break;
                case DragDirection.Right:

                    minSize = Mathf.Min(300, leftOrTopNode.rectTransform.rect.width);
                    newWidth = leftOrTopNode.rectTransform.rect.width + offsetValue.x;

                    //�жϴ����Ƿ����û��趨�ķ�Χ��
                    if (newWidth < minSize)
                    {
                        //Debug.LogError("�µ�ƫ����"+(offsetValue.x - (minSize.x - newWidth)));
                        offsetValue.x += (minSize - newWidth);
                    }
                    else
                    {
                        minSize = Mathf.Min(300, rightOrBottomNode.rectTransform.rect.width);

                        newWidth = rightOrBottomNode.rectTransform.rect.width - offsetValue.x;
                        if (newWidth < minSize)
                        {
                            //Debug.LogError("�µ�ƫ����"+(offsetValue.x - (minSize.x - newWidth)));
                            offsetValue.x -= (minSize - newWidth);
                        }
                    }

                    offsetValue.y = 0;
                    size = leftOrTopNode.rectTransform.rect.size + new Vector2(offsetValue.x, offsetValue.y);//�ƶ����������С
                    SetSizeDeltaValue(leftOrTopNode.rectTransform, new Vector3(offsetValue.x / 2, offsetValue.y / 2));

                    leftOrTopNode.rectTransform.SetRectTransformSizeWithCurrentAnchors(size.x, size.y);


                    size = rightOrBottomNode.rectTransform.rect.size - new Vector2(offsetValue.x, offsetValue.y);//�ƶ����������С
                    SetSizeDeltaValue(rightOrBottomNode.rectTransform, new Vector3(offsetValue.x / 2, offsetValue.y / 2));

                    rightOrBottomNode.rectTransform.SetRectTransformSizeWithCurrentAnchors(size.x, size.y);

                    break;
                case DragDirection.Top:
                    minSize = Mathf.Min(200, leftOrTopNode.rectTransform.rect.height);
                    newHeight = leftOrTopNode.rectTransform.rect.height - offsetValue.y;

                    //�жϴ����Ƿ����û��趨�ķ�Χ��
                    if (newHeight < minSize)
                    {

                        offsetValue.y -= (minSize - newHeight);
                    }
                    else
                    {
                        minSize = Mathf.Min(200, rightOrBottomNode.rectTransform.rect.height);

                        newHeight = rightOrBottomNode.rectTransform.rect.height + offsetValue.y;
                        if (newHeight < minSize)
                        {

                            offsetValue.y += (minSize - newHeight);
                        }
                    }

                    offsetValue.x = 0;
                    size = leftOrTopNode.rectTransform.rect.size - new Vector2(offsetValue.x, offsetValue.y);//�ƶ����������С
                    SetSizeDeltaValue(leftOrTopNode.rectTransform, new Vector3(offsetValue.x / 2, offsetValue.y / 2));

                    UIUtil.SetRectTransformSizeWithCurrentAnchors(leftOrTopNode.rectTransform, size.x, size.y);


                    size = rightOrBottomNode.rectTransform.rect.size + new Vector2(offsetValue.x, offsetValue.y);//�ƶ����������С
                    SetSizeDeltaValue(rightOrBottomNode.rectTransform, new Vector3(offsetValue.x / 2, offsetValue.y / 2));

                    UIUtil.SetRectTransformSizeWithCurrentAnchors(rightOrBottomNode.rectTransform, size.x, size.y);
                    break;
                case DragDirection.Bottom:
                    minSize = Mathf.Min(200, leftOrTopNode.rectTransform.rect.height);
                    newHeight = leftOrTopNode.rectTransform.rect.height - offsetValue.y;

                    //�жϴ����Ƿ����û��趨�ķ�Χ��
                    if (newHeight < minSize)
                    {
                        offsetValue.y -= (minSize - newHeight);
                    }
                    else
                    {
                        minSize = Mathf.Min(200, rightOrBottomNode.rectTransform.rect.height);

                        newHeight = rightOrBottomNode.rectTransform.rect.height + offsetValue.y;
                        if (newHeight < minSize)
                        {
                            offsetValue.y += (minSize - newHeight);
                        }
                    }
                    offsetValue.x = 0;
                    size = leftOrTopNode.rectTransform.rect.size - new Vector2(offsetValue.x, offsetValue.y);//�ƶ����������С

                    SetSizeDeltaValue(leftOrTopNode.rectTransform, new Vector3(offsetValue.x / 2, offsetValue.y / 2));

                    UIUtil.SetRectTransformSizeWithCurrentAnchors(leftOrTopNode.rectTransform, size.x, size.y);


                    size = rightOrBottomNode.rectTransform.rect.size + new Vector2(offsetValue.x, offsetValue.y);//�ƶ����������С
                    SetSizeDeltaValue(rightOrBottomNode.rectTransform, new Vector3(offsetValue.x / 2, offsetValue.y / 2));
                    UIUtil.SetRectTransformSizeWithCurrentAnchors(rightOrBottomNode.rectTransform, size.x, size.y);
                    break;
                default:
                    break;
            }

            leftOrTopNode.UpdateRectransform();
            rightOrBottomNode.UpdateRectransform();


        }
        else
        {

        }
    }

    /// <summary>
    /// ����ƫ��������transform��λ��
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="deltaValue"></param>
    private void SetSizeDeltaValue(RectTransform rect, Vector3 deltaValue)
    {
        switch (WindowPanelManager.Instance.UICanvas.renderMode)
        {
            case RenderMode.ScreenSpaceOverlay:
                rect.position += deltaValue;
                break;
            case RenderMode.ScreenSpaceCamera:
                Vector3 tranToScreenPos = UIUtil.GetRectTransformToScreenPosition(rect, WindowPanelManager.Instance.UICanvas);

                rect.position = UIUtil.GetScreenPointToWorldPointInRectangle(tranToScreenPos + deltaValue, WindowPanelManager.Instance.UICanvas);//�ƶ�������λ��;

                break;
            case RenderMode.WorldSpace:

                break;
            default:
                break;
        }
    }

    private bool FindRelationNode(DragDirection dragDirection, out Node leftNode, out Node rightNode)
    {


        Node node = transform.parent.GetComponent<Node>();

        if (node == null)
        {
            leftNode = null;
            rightNode = null;

            return false;

        }
        return node.FindRelationNode(dragDirection, node, out leftNode, out rightNode);
    }


    float detectionDis = 10;
    /// <summary>
    /// �϶���ʱ�������λ�����ķ�λ
    /// </summary>
    private DragDirection DetectionDragDirection()
    {
        DragDirection direction = DragDirection.None;
        if (windowPanel.isEnter)
        {
            Vector3[] corners = UIUtil.GetRectTransformToScreenPositionFromCorners(rectTransform, WindowPanelManager.Instance.UICanvas);
            float leftDis = Input.mousePosition.x - corners[0].x;
            float topDis = corners[1].y - Input.mousePosition.y;
            float rightDis = corners[3].x - Input.mousePosition.x;
            float bottomDis = Input.mousePosition.y - corners[0].y;




            if (leftDis < detectionDis && leftDis < topDis && leftDis < rightDis && leftDis < bottomDis)
            {
                direction = DragDirection.Left;

            }
            if (topDis < detectionDis / 2 && topDis < leftDis && topDis < rightDis && topDis < bottomDis)
            {
                direction = DragDirection.Top;


            }
            if (rightDis < detectionDis && rightDis < topDis && rightDis < leftDis && rightDis < bottomDis)
            {
                direction = DragDirection.Right;
            }

            if (bottomDis < detectionDis && bottomDis < topDis && bottomDis < rightDis && bottomDis < leftDis)
            {
                direction = DragDirection.Bottom;


            }


        }

       
        return direction;


    }
 
}
