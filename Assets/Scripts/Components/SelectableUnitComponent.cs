using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Components
{
    public class SelectableUnitComponent : MonoBehaviour
    {
        public GameObject selectionRing;
        public bool IsSelected;

        private void Update()
        {
            if(this.IsSelected)
            {
                if(!selectionRing.activeSelf)
                    selectionRing.SetActive(true);

                selectionRing.transform.LookAt(Camera.main.transform);
            }
            else
            {
                if (selectionRing.activeSelf)
                    selectionRing.SetActive(false);
            }
        }
    }
}