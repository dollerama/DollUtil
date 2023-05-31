using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DollUtil
{
    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public string name;

        private Stack<GameObject> contents;
        public GameObject container;
        private MonoBehaviour manager;
        private int capacity;

        public void Init(MonoBehaviour _manager, int _capacity = 4)
        {
            capacity = _capacity;
            manager = _manager;
            contents = new Stack<GameObject>();
            container = new GameObject(name);

            for(int i=0; i < capacity; i++)
            {
                var go = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, container.transform);
                go.SetActive(false);
                contents.Push(go);
            }
        }

        public void Resize()
        {
            for (int i = 0; i < capacity; i++)
            {
                var go = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, container.transform);
                go.SetActive(false);
                contents.Push(go);
            }
            capacity *= 2;
        }

        public GameObject Grab(Vector3 position, Quaternion rotation, float give_in = -1)
        {
            if(contents.Count < capacity/2)
            {
                Resize();
            }

            var c = contents.Peek();

            c.SetActive(true);
            c.transform.parent = null;
            c.transform.position = position;
            c.transform.rotation = rotation;

            if(give_in > 0)
            {
                manager.StartCoroutine(Give_In(c, give_in));
            }

            return contents.Pop();
        }

        public IEnumerator Give_In(GameObject content, float time)
        {
            yield return new WaitForSeconds(time);

            content.transform.parent = container.transform;
            content.transform.localPosition = Vector3.zero;
            content.transform.rotation = Quaternion.identity;
            content.SetActive(false);

            contents.Push(content);
        }

        public void Give(GameObject content)
        {
            
            content.transform.parent = container.transform;
            content.transform.localPosition = Vector3.zero;
            content.transform.rotation = Quaternion.identity;
            content.SetActive(false);

            contents.Push(content);
        }
    }
}
