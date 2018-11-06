using UnityEngine;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Components
{
    //public abstract class SelectionManager : MonoBehaviour
    //{
    //    private static SelectionManager _instance;

    //    public static ISelectionManager instance { get { return _instance; } }

    //    public static void SetInstance(SelectionManager instance)
    //    {
    //        if (SelectionManager._instance == instance)
    //            return;

    //        if (SelectionManager._instance != null)
    //            SelectionManager._instance.enabled = false;

    //        SelectionManager._instance = instance;
    //    }

    //    private bool _dontDestroyOnLoad = true;

    //    protected virtual void Awake()
    //    {
    //        if (_dontDestroyOnLoad)
    //            DontDestroyOnLoad(this.transform.root.gameObject);
    //    }

    //    public virtual bool isEnabled
    //    {
    //        get
    //        {
    //            return this.isActiveAndEnabled;
    //        }

    //        set
    //        {
    //            this.enabled = value;
    //        }
    //    }
    //}
}