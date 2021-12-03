using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using System.IO;

public class WindowPanelManager : SingletonGameObject<WindowPanelManager>
{
	internal UnityAction<Lable> OnBeginDragLabelCallEvent;
	internal UnityAction<Lable> OnDragLabelCallEvent;
	internal UnityAction<Lable> OnEndDragLabelCallEvent;
    private Canvas canvas;
    public Canvas UICanvas
    {
        get
        {
            if (canvas==null) {
                canvas = FindObjectOfType<Canvas>();
            }
            
            return canvas;
        }
    }

    public RectTransform CanvasRectTransform
    {
        get
        {

            return UICanvas.GetComponent<RectTransform>();
        }
    }

    public WindowPanel currentWindowPanel { get;  set; }//当前标签所属面板
    private void Awake()
    {
        MatchLayoutFile();
        if (!LoadLayout(Persistence.LastLayoutName))
        {
            LoadLayout(Persistence.DefaultLayoutName);
        }
        


    }

    #region 处理Lable拖动事件

    public void OnBeginDragLabel(Lable lable) {
        Rect rect = new Rect();
     
        float width = lable.windowPanel.GetPanelRect().size.x;
        float height = lable.windowPanel.GetPanelRect().size.y;

        width = Mathf.Max(480, width / 1.5f);
        height = Mathf.Max(270, height / 1.5f);
        rect.size = new Vector2(width, height);

        DefaultWindowPanel.Instance.Show( lable.lableName, rect.size, rect.position);
        currentWindowPanel = lable.windowPanel;

        OnBeginDragLabelCallEvent?.Invoke(lable);

    }
    public void OnDragLabel(Lable lable)
    {
        OnDragLabelCallEvent?.Invoke(lable);
    }
    public void OnEndDragLabel(Lable lable)
    {
        OnEndDragLabelCallEvent?.Invoke(lable);
        DefaultWindowPanel.Instance.Hide();
       
        if (!lable.inLableRect) {
            Debug.LogError("超出范围=="+Input.mousePosition);
             CreateFloatWindow(lable);
             currentWindowPanel.SetDefaultContent();
        }
    }

    #endregion


    #region 创建窗口以及Lable操作


    /// <summary>
    /// 根据标签创建一个浮动窗口
    /// </summary>
    /// <param name="lable"></param>
    /// <returns></returns>

    public WindowPanel CreateFloatWindow(Lable lable)
    {
        GameObject panel = Resources.Load<GameObject>("WindowPanel");
        GameObject newWindowObj = Instantiate(panel, transform);
        newWindowObj.transform.SetAsLastSibling();
        WindowPanel newWindow = newWindowObj.GetComponent<WindowPanel>();
        newWindow.InitData();
        newWindow.SetLableContentByLable(lable);

        Vector2 newSize = Vector2.zero;
        Vector3 newPos = Vector3.zero;
        float width = currentWindowPanel.GetPanelRect().size.x;
        float height = currentWindowPanel.GetPanelRect().size.y;

        width = Mathf.Max(960, width / 1.5f);
        height= Mathf.Max(540, height / 1.5f);
        newSize = new Vector2(width,height);





        Vector3 pos = Input.mousePosition;

        float minX = pos.x - (newSize.x/2);
        float maxX = pos.x + (newSize.x/2);
        float minY = pos.y - (newSize.y/2);
        float maxY = pos.y + (newSize.y/2);

       // Debug.LogError(Screen.width+"   "+ Screen.height+"  "+ pos);

        if (minX<0) {
            pos.x -= minX;
        }

        if (maxY > Screen.height) {
            pos.y -= (maxY - Screen.height);
        }

        if (maxX>Screen.width) {
            pos.x -= (maxX - Screen.width);

        }

        if (minY<0) {
            pos.y -= minY;
        }
       
        newPos = UIUtil.GetScreenPointToWorldPointInRectangle(pos, UIManager.Instance.UICanvas);
       
        newWindow.SetRectTransformSize(newSize,newPos);


     
        GameObject windowObj = Instantiate(Resources.Load<GameObject>("FloatWindow"), newWindow.transform.parent);

        FloatWindow window = windowObj.GetComponent<FloatWindow>();
        windowObj.AddComponent<Node>().isFloatWindow = true;


        UIUtil.SetRectTransformSizeWithCurrentAnchors(windowObj.GetComponent<RectTransform>(), newSize, newPos);


        newWindow.rectTransform.SetParent(windowObj.transform);
        newWindow.transform.SetAsFirstSibling();
        newWindow.rectTransform.localScale = Vector3.one;
        newWindow.rectTransform.localPosition = Vector3.zero;

        return newWindow;
    }
   

    public WindowPanel CreatePanelWindow(WindowPanel stayPanel, DragDirection direction, Lable lable, Vector2 size,Vector3 pos)
    {
        GameObject panel = Resources.Load<GameObject>("WindowPanel");
        GameObject newWindowObj = Instantiate(panel, transform);
        WindowPanel newWindow = newWindowObj.GetComponent<WindowPanel>();
        newWindow.InitData();
        newWindow.SetLableContentByLable(lable);
      
        UIUtil.SetRectTransformSizeWithCurrentAnchors(newWindow.rectTransform, size,pos);

     
            //创建新的节点将面板包围  主要是方便移动面板的时候好计算填充
            RectTransform stayNodeRect = CreateWindowArea(stayPanel.rectTransform.parent);
            RectTransform newNodeRect = CreateWindowArea(stayPanel.rectTransform.parent);



            Node stayNode = stayNodeRect.gameObject.AddComponent<Node>();
            Node newNode = newNodeRect.gameObject.AddComponent<Node>();



            stayNodeRect.position = stayPanel.rectTransform.position;
            UIUtil.SetRectTransformSizeWithCurrentAnchors(stayNodeRect, stayPanel.rectTransform.rect.width, stayPanel.rectTransform.rect.height);

            newNodeRect.position = pos;
            UIUtil.SetRectTransformSizeWithCurrentAnchors(newNodeRect, size.x, size.y);
        newNodeRect.SetAsFirstSibling();
        stayNodeRect.SetAsFirstSibling();

        switch (direction)
            {
                case DragDirection.None:
                    break;
                case DragDirection.Left:


                    newNodeRect.SetAsFirstSibling();

                    newNode.SetDirection(DragDirection.Left);
                    stayNode.SetDirection(DragDirection.Right);

                    break;
                case DragDirection.Right:
                    stayNodeRect.SetAsFirstSibling();

                    newNode.SetDirection(DragDirection.Right);
                    stayNode.SetDirection(DragDirection.Left);
                    break;
                case DragDirection.Top:

                    newNodeRect.SetAsFirstSibling();

                    newNode.SetDirection(DragDirection.Top);
                    stayNode.SetDirection(DragDirection.Bottom);

                    break;
                case DragDirection.Bottom:
                    stayNodeRect.SetAsFirstSibling();
                    newNode.SetDirection(DragDirection.Bottom);
                    stayNode.SetDirection(DragDirection.Top);
                    break;
                default:
                    break;
            }

            stayPanel.rectTransform.SetParent(stayNodeRect);
            newWindow.rectTransform.SetParent(newNodeRect);

            stayPanel.rectTransform.offsetMax = Vector2.zero;
            stayPanel.rectTransform.offsetMin = Vector2.zero;
            newWindow.rectTransform.offsetMax = Vector2.zero;
            newWindow.rectTransform.offsetMin = Vector2.zero;

            
        return newWindow;
    }
    /// <summary>
    /// 创建一个新的节点区域
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    private RectTransform CreateWindowArea(Transform parent)
    {
        GameObject node = new GameObject("Node");
        node.AddComponent<RectTransform>();
        node.transform.SetParent(parent);
        node.transform.localScale = Vector3.one;
        return node.GetComponent<RectTransform>();
    }

    /// <summary>
    /// 根据当前标签生成面板
    /// </summary>
    /// <param name="lableName"></param>
    /// <returns></returns>
    public RectTransform GetContentByLableName(string lableName) {
        GameObject panel = Resources.Load<GameObject>("UI/Panel/" + lableName + "Panel");

        if (panel!=null) { 

           return Instantiate(panel).GetComponent<RectTransform>();

        }
        Debug.LogError("UI/Panel/" + lableName + "Panel");
        return null;

    }



    public void DeleteLable(Lable lable) {
        if (lable.CanInteraction)
        {
            currentWindowPanel = lable.windowPanel;
            DestroyImmediate(lable.gameObject);
            currentWindowPanel.SetDefaultContent();
        }
        else {
            Debug.LogError("原则上这个标签不允许删除");
        }
      
    }

    #endregion
    #region 加载保存布局


    public void  Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SaveLayout("Default",true);
        }

        if (Input.GetKeyDown(KeyCode.A)) {
        
            LoadLayout("Default");
        }
    }
    internal void SaveLayout(string layoutName,bool overwrite)
    {
        List<NodeData> nodeDatas = new List<NodeData>();

        foreach (Transform item in transform)
        {
            Node node = item.GetComponent<Node>();
            if (node!=null) {
                NodeData nodeData = node.ToNodeData();

                if (nodeData!=null) {
                    SetNodeChildNodeData(nodeData,node);
                    nodeDatas.Add(nodeData);
                 
                }
            }
        }

        if (nodeDatas.Count > 0)
        {
            List<string> layoutsName = GetLayoutsName();
            if (layoutsName==null) {
                layoutsName = new List<string>();
            }

            if (!layoutsName.Contains(layoutName)|| overwrite)
            {
                if (!layoutsName.Contains(layoutName)) { 
                layoutsName.Add(layoutName);

                }


                string layoutStr = JsonConvert.SerializeObject(nodeDatas);

                string filePath = Path.Combine(Application.streamingAssetsPath, "Layout/" + layoutName + ".txt");
                FileUtil.SaveFile(layoutStr, filePath);

                string layoutNamePath = Path.Combine(Application.streamingAssetsPath, "LayoutName.txt");
                FileUtil.SaveFile(JsonConvert.SerializeObject(layoutsName), layoutNamePath);

            }
            else {
            
                WindowsTools.ShowMessageBox("已经存在该布局，是否覆盖","已存在",(result)=>{
                    if (result)
                    {
                        layoutsName.Add(layoutName);


                        string layoutStr = JsonConvert.SerializeObject(nodeDatas);

                        string filePath = Path.Combine(Application.streamingAssetsPath, "Layout/" + layoutName + ".txt");
                        FileUtil.SaveFile(layoutStr, filePath);

                        string layoutNamePath = Path.Combine(Application.streamingAssetsPath, "LayoutName.txt");
                        FileUtil.SaveFile(JsonConvert.SerializeObject(layoutsName), layoutNamePath);
                    }
                    else {
                        
                     Debug.LogError("已经存在该布局");

                    }

                });
            }
        }
        else {
            Debug.LogError("循环出问题");
        }
    }
    internal bool LoadLayout(string layoutName)
    {
        if (string.IsNullOrEmpty(layoutName)) {
            return false;
        }
        if (!GetLayoutsName().Contains(layoutName)) {
            Debug.LogError("当前没有这个布局" + layoutName);

            return false;
        }
        string filePath = Path.Combine(Application.streamingAssetsPath, "Layout/" + layoutName + ".txt");
        if (!File.Exists(filePath))
        {
            return false;
        }
        ClearLayout();
        
        string layout = FileUtil.ReadFileToString(filePath);
        List<NodeData> nodeDatas = JsonConvert.DeserializeObject<List<NodeData>>(layout);

        for (int i = 0; i < nodeDatas.Count; i++)
        {
            Node node = null;

            if (nodeDatas[i].isFloatWindow)
            {
                node = CreateFloatWindowNode(nodeDatas[i].nodeName);
            }
            else
            {
                if (nodeDatas[i].isPanel)
                {
                    node = CreatePanelWindowNode(nodeDatas[i].lables, nodeDatas[i].nodeName);
                }
                else
                {
                    node = CreateWindowNode(nodeDatas[i].nodeName);
                }
            }

            node.SetData(nodeDatas[i]);

            CreateChildNode(nodeDatas[i]);
        }

        return true;

    }
    public List<string> GetLayoutsName() {
        string layout = FileUtil.ReadFileToString(Path.Combine(Application.streamingAssetsPath,"LayoutName.txt"));

       return  JsonConvert.DeserializeObject<List<string>>(layout);

    }
    private  void  MatchLayoutFile() {
        List<string> layoutsName = GetLayoutsName();

        for (int i = layoutsName.Count-1; i >=0; i--)
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "Layout/" + layoutsName[i] + ".txt");
            if (!File.Exists(filePath))
            {
                layoutsName.RemoveAt(i);
            }
        }

        string layoutNamePath = Path.Combine(Application.streamingAssetsPath, "LayoutName.txt");
        FileUtil.SaveFile(JsonConvert.SerializeObject(layoutsName), layoutNamePath);
    }

    private void ClearLayout() {

        foreach (Transform item in transform)
        {
            Node node = item.GetComponent<Node>();
            if (node != null)
            {
                DestroyImmediate(node.gameObject);
            }
        }
    }

    private void SetNodeChildNodeData(NodeData nodeData ,Node node) {
        Dictionary<Node, NodeData> nodeDic = new Dictionary<Node, NodeData>();
        foreach (Transform item in node.transform)
        {
            Node childNode = item.GetComponent<Node>();
            if (childNode != null)
            {
                NodeData childNodeData = childNode.ToNodeData();
                nodeData.nodeDatas.Add(childNodeData);
                nodeDic.Add(childNode, childNodeData);
        
            }
        }

        foreach (var item in nodeDic.Keys)
        {
            SetNodeChildNodeData(nodeDic[item],item);
        }
    }



   

    private void CreateChildNode(NodeData nodeData) {

        for (int i = 0; i < nodeData.nodeDatas.Count; i++)
        {
            Node node = null;

            if (nodeData.nodeDatas[i].isFloatWindow)
            {
                node = CreateFloatWindowNode(nodeData.nodeDatas[i].nodeName);
            }
            else
            {
                if (nodeData.nodeDatas[i].isPanel)
                {
                    node = CreatePanelWindowNode(nodeData.nodeDatas[i].lables, nodeData.nodeDatas[i].nodeName);
                }
                else
                {
                    node = CreateWindowNode(nodeData.nodeDatas[i].nodeName);
                }
            }
         
            node.SetData(nodeData.nodeDatas[i]);
        }

        for (int i = 0; i < nodeData.nodeDatas.Count; i++)
        {
            CreateChildNode(nodeData.nodeDatas[i]);

        }

    }

    /// <summary>
    /// 加载布局的时候用
    /// </summary>
    /// <returns></returns>
    private Node CreateFloatWindowNode(string nodeName)
    {

        GameObject windowObj = Instantiate(Resources.Load<GameObject>("FloatWindow"));
        windowObj.name = nodeName;
        FloatWindow floatWindow = windowObj.GetComponent<FloatWindow>();
        return  windowObj.AddComponent<Node>();

    }

    private Node CreatePanelWindowNode(string[] lableName, string nodeName)
    {
        GameObject panel = Resources.Load<GameObject>("WindowPanel");
        GameObject newWindowObj = Instantiate(panel, transform);
        WindowPanel newWindow = newWindowObj.GetComponent<WindowPanel>();
        newWindowObj.name = nodeName;
        newWindow.InitData();
        newWindow.AddLables(lableName);
        newWindow.SetLableContentByLable(newWindow.panelTitle.lableRect.GetLastLable());
       
        return newWindowObj.AddComponent<Node>();
    }

    private Node CreateWindowNode(string nodeName)
    {
        GameObject node = new GameObject("Node");
        node.AddComponent<RectTransform>();
        node.name = nodeName;
        return node.AddComponent<Node>();
    }
    #endregion

    private void OnApplicationQuit()
    {
        //保存最后修改的布局
            SaveLayout(Persistence.LastLayoutName,true);
        


    }
}
