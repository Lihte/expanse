using System.Collections.Generic;
using System;

using UnityEngine.EventSystems;
using UnityEngine;

namespace Assets.Scripts
{
    public class CustomEventSystem : EventSystem
    {
        public List<GameObject> SelectedGameObjects;

        public GameObject CurrentlySelected;

        protected override void Update()
        {
            if (CurrentlySelected != currentSelectedGameObject)
                CurrentlySelected = currentSelectedGameObject;
            base.Update();
        }

        public new void SetSelectedGameObject(GameObject selected, BaseEventData pointer)
        {
            if(IsPointerOverGameObject())
                base.SetSelectedGameObject(selected, pointer);
        }
    }
}
