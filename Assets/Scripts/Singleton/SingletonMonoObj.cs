using UnityEngine;

namespace GameFrameWork
{
    /// <summary>
    /// MonoBehaviour单例模式
    /// </summary>
    public class SingletonMonoObj<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static object _lock = new object();

        public static T Instance
        {
            get
            {
                //if (applicationIsQuitting)
                //{
                //    Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                //        "' already destroyed on application quit." +
                //        " Won't create again - returning null.");
                //    return null;
                //}

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        if (GameObject.FindObjectOfType<T>() != null)
                        {
                            _instance = GameObject.FindObjectOfType<T>();
                        }
                        else
                        {
                            string fullName = typeof(T).ToString();
                            string[] bufName = fullName.Split('.');
                            string name = bufName[bufName.Length - 1];
                            GameObject singleton = PublicFunc.CreateObjFromRes(name);
                            _instance = singleton.GetComponent<T>();
                            singleton.name = string.Format("{0}", typeof(T));
                            singleton.transform.position = Vector3.zero;
                            singleton.transform.eulerAngles = Vector3.zero;
                        }
                        //DontDestroyOnLoad(singleton);
                    }

                    return _instance;
                }
            }
            set
            {
                if (_instance != null)
                {
                    Destroy(value);
                    return;
                }
                _instance = value;
                //DontDestroyOnLoad(_instance);
            }
        }

        public static bool applicationIsQuitting = false;


        /// <summary>  
        /// When Unity quits, it destroys objects in a random order.  
        /// In principle, a Singleton is only destroyed when application quits.  
        /// If any script calls Instance after it have been destroyed,  
        ///   it will create a buggy ghost object that will stay on the Editor scene  
        ///   even after stopping playing the Application. Really bad!  
        /// So, this was made to be sure we're not creating that buggy ghost object.  
        /// </summary>  
        void OnDestroy()
        {
            
            _instance = null;
            WhenDestroy();
        }

        public virtual void WhenDestroy()
        {

        }
        public virtual void Startup()
        {

        }
    }
}