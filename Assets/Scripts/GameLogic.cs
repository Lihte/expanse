using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Assets.Scripts
{
    public class GameLogic : MonoBehaviour
    {
        public GameObject canvas;


        void Start()
        {
            var collider = gameObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            
        }

        void Update()
        {
            if (canvas != null)
            {
                var h = canvas.GetComponent<RectTransform>().rect.height;
                var w = canvas.GetComponent<RectTransform>().rect.width;
                GetComponent<BoxCollider2D>().size = new Vector2(w, h);
            }
        }

        public void OnClick()
        {
            Debug.Log(Input.mousePosition);
        }
    }
}