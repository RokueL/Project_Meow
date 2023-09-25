using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        static GameManager s_instance; // 유일성이 보장된다.
        static GameManager Instance { get { Init(); return s_instance; } } // 유일한 매니저를 갖고온다.

        InputManager _inputManager = new InputManager();
        public static InputManager Inputs { get { return Instance._inputManager; } }

        void Start()
        {
            Init();
        }

        void Update()
        {
            _inputManager.keyUpdate();
        }

        static void Init()
        {
            if (s_instance == null)
            {
                GameObject go = GameObject.Find("@Managers");
                if (go == null)
                {
                    //빈오브젝트를 생성한다.
                    go = new GameObject { name = "@Managers" };
                    go.AddComponent<GameManager>();
                }

                // 마음대로 추가하고 삭제할 수 없도록 함.
                DontDestroyOnLoad(go);
                s_instance = go.GetComponent<GameManager>();
            }
        }
    }
}

