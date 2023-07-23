using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DollUtil.Attributes;
using UnityEngine;

namespace DollUtil.Buggy
{
    /// <summary>
    /// Console for logging messages and running commands.
    /// </summary>
    public class Console : MonoBehaviour
    {
        static Console mInstance;

        /// <summary>
        /// static instance for Console.
        /// </summary>
        /// <remarks>Initializes on first use.</remarks>
        public static Console Get
        {
            get
            {
                if (mInstance == null)
                {
                    GameObject go = new GameObject();
                    go.name = "Console";
                    mInstance = go.AddComponent<Console>();
                    #if UNITY_EDITOR
                    mInstance.Init();
                    #endif
                }
                return mInstance;
            }

            set
            {
                if (mInstance == null)
                {
                    GameObject go = new GameObject();
                    go.name = "Console";
                    mInstance = go.AddComponent<Console>();
                    #if UNITY_EDITOR
                    mInstance.Init();
                    #endif
                }
                mInstance = value;
            }
        }

        public Stack<string> Messages = new Stack<string>();
        private delegate void consoleEvent(object[] args);
        public Dictionary<string, Delegate> events =
        new Dictionary<string, Delegate>();

        private Vector2 scrollPos;
        private string input;
        private bool show = false;

        /// <summary>
        /// Register a MonoBehaviours methods that are marked [ConsoleCallback].
        /// </summary>
        /// <param name="obj">MonoBehaviour to register</param>
        public void RegisterMonoBehaviour(MonoBehaviour obj)
        {
            InitObj<MonoBehaviour>(obj.name, obj);
        }

        /// <summary>
        /// Wrapper on RegisterMonoBehaviour.
        /// </summary>
        /// <example>
        /// <code>
        /// Console.instance += this;
        /// </code>
        /// </example>
        public static Console operator +(Console b, MonoBehaviour c)
        {
            b.RegisterMonoBehaviour(c);
            return b;
        }

        /// <summary>
        /// Log message to console.
        /// </summary>
        /// <param name="m">Message to log.</param>
        public void Log(string m)
        {
            #if UNITY_EDITOR
            scrollPos.y += 55;
            Messages.Push(m);
            show = true;
            #endif
        }

        private void Update()
        {
            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                show = !show;
            }
            #endif
        }

        private void OnGUI()
        {
            #if UNITY_EDITOR
            if (!show) return;

            GUI.Box(new Rect(0, 0, Screen.width, 150), "");

            GUI.skin.box.fontSize = 24;
            GUI.skin.box.alignment = TextAnchor.MiddleLeft;
            GUI.skin.button.fontSize = 24;
            GUI.skin.textField.fontSize = 36;

            scrollPos = GUI.BeginScrollView(new Rect(0, 0, Screen.width, 150), scrollPos, new Rect(0, 0, Screen.width, Messages.Count*50), false, true);
            int i = 0;
            foreach(var m in Messages.Reverse())
            {
                GUI.Box(new Rect(0, i * 55, Screen.width, 50), new GUIContent(m));
                i++;
            }
            GUI.EndScrollView();
            
            input = GUI.TextField(new Rect(0, 160, Screen.width-200, 60), input);

            if (GUI.Button(new Rect(Screen.width-200, 160, 100, 60), new GUIContent("clear")))
            {
                Messages.Clear();
            }

            if (GUI.Button(new Rect(Screen.width - 100, 160, 100, 60), new GUIContent("enter")))
            {
                Execute(input);
                input = "";
            }

            Event e = Event.current;
            if (e.keyCode == KeyCode.Return && input != "")
            {
                Execute(input);
                input = "";
            }
            #endif
        }

        private void Init()
        {
            MonoBehaviour[] sceneActive = FindObjectsOfType<MonoBehaviour>();
            foreach (MonoBehaviour mono in sceneActive)
            {
                RegisterMonoBehaviour(mono);
            }
        }

        private void InitObj<T>(string N, T obj)
        {
            MethodInfo[] mFields =
            obj.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public);

            for (int i = 0; i < mFields.Length; i++)
            {
                if (mFields[i].MemberType != MemberTypes.Method) continue;
                if (events.ContainsKey(N + "." + mFields[i].Name)) continue;

                ConsoleCallback attribute =
                Attribute.GetCustomAttribute(
                    mFields[i], typeof(ConsoleCallback)
                ) as ConsoleCallback;

                if (attribute != null)
                {
                    ParameterInfo[] pars = mFields[i].GetParameters();
                    List<Type> ts = new List<Type>();
                    foreach (ParameterInfo p in pars)
                    {
                        ts.Add(p.ParameterType);
                    }
                    
                    events.Add(
                        N + "." + mFields[i].Name,
                        CreateDelegate(mFields[i], obj)
                    );
                }
            }
        }

        private void Execute(string command)
        {
            if(command == "help")
            {
                foreach(KeyValuePair<string, Delegate> pair in events)
                {
                    Log(pair.Key);
                }
                return;
            }

            string cmd = command;

            string parsed = "";
            int id = 0;

            foreach (char c in cmd.ToCharArray())
            {
                id++;
                if (c == '(') break;
            }

            object[] argsCast = null;

            parsed = cmd.Substring(0, id - 1);

            if (command.Length-id-2 > 0 && command[command.IndexOf('(') + 1] != ')')
            {
                object[] args = cmd.Substring(id, command.Length - id - 1).Split(',');
                argsCast = new object[args.Length];

                for (int i = 0; i < args.Length; i++)
                {
                    int number;
                    bool successInt = int.TryParse((string)args[i], out number);
                    bool flag;
                    bool successBool = Boolean.TryParse((string)args[i], out flag);
                    float floating;
                    bool successFloat = Single.TryParse((string)args[i], out floating);

                    if (successInt)
                    {
                        argsCast[i] = (object)(number);
                    }
                    else if (successBool)
                    {
                        argsCast[i] = (object)(flag);
                    }
                    else if (successFloat)
                    {
                        argsCast[i] = (object)(floating);
                    }
                    else
                    {
                        string tmp = (string)(args[i]);
                        tmp = tmp.Substring(tmp.IndexOf("\"")+1, tmp.LastIndexOf('\"')-1);
                        argsCast[i] = (object)(tmp);
                    }
                }
            }

            try
            {
                if (argsCast != null)
                    events[parsed].DynamicInvoke(argsCast);
                else
                    events[parsed].DynamicInvoke();
            }
            catch (Exception err)
            {
                UnityEngine.Debug.LogError("DollUtil Err:"+err);
            }
        }

        private Delegate CreateDelegate(MethodInfo method, object target)
        {
            return method.CreateDelegate(Expression.GetDelegateType(
                (from parameter in method.GetParameters() select parameter.ParameterType)
                .Concat(new[] { method.ReturnType })
                .ToArray()), target);
        }
    }
}
