using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewTreeArgs 
{
  

    /// <summary>
    /// item初始化的时候需要用用户实现父类去处理
    /// </summary>
    public class InitDataArgs : EventArgs
    {

        public IEnumerable ItemData
        {
            get;
            set;
        }

        public InitDataArgs()
        {

        }
    }
    /// <summary>
    /// item初始化的时候需要用用户实现父类去处理
    /// </summary>
    public class ItemBindDataArgs : EventArgs
    {
        /// <summary>
        /// item绑定的数据项
        /// </summary>
        public object Item
        {
            get;
            private set;
        }
        //是否有子项
        public bool canExpand
        {
            get;
            set;
        }
        //显示的内容
        public string showContent
        {
            get;
            set;
        }
        public ItemBindDataArgs(object item)
        {
            Item = item;
        }

    }
    /// <summary>
    /// 当Item展开的时候需要用户自行定义展开的数据
    /// </summary>
    public class ItemExpandingArgs : EventArgs
    {
        /// <summary>
        /// item绑定的数据项
        /// </summary>
        public object Item
        {
            get;
            private set;
        }

        /// <summary>
        /// 使用此属性指定项的子项
        /// </summary>
        public IEnumerable Children
        {
            get;
            set;
        }

        public ItemExpandingArgs(object item)
        {
            Item = item;
        }
    }
    /// <summary>
    /// 结构发生改变  的时候需要子类去处理这个事件
    /// </summary>
    public class StructuralChangeArgs : EventArgs
    {
        /// <summary>
        /// 用户去判断是否需要操是否需要处理这个操作
        /// </summary>
        public bool isHandle
        {
            get;
            set;
        }
        /// <summary>
        /// 当前停靠的方向
        /// </summary>
        public DropSibling dropSibling
        {
            get;
            private set;
        }
        /// <summary>
        /// 当前拖动的目标
        /// </summary>

        public TreeItemBase dragItem
        {
            get;
            private set;
        }
        /// <summary>
        /// 当前停留的目标
        /// </summary>
        public TreeItemBase droupItem
        {
            get;
            private set;
        }

        public StructuralChangeArgs(DropSibling dropSibling, TreeItemBase dragItem, TreeItemBase droupItem)
        {
            this.dropSibling = dropSibling;
            this.dragItem = dragItem;
            this.droupItem = droupItem;

        }
    }

}
