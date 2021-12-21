using System;
using UnityEngine;
using UnityEngine.UI;

namespace MelonModManager.Core.View
{
    public class ModInfo : MonoBehaviour
    {
        public Text Title;
        public Text Details;
        public Text Description;
        
        private void Awake()
        {
            Title = gameObject.transform.Find("Title").GetComponent<Text>();
            Details = gameObject.transform.Find("Details").GetComponent<Text>();
            Description = gameObject.transform.Find("Description").GetComponent<Text>();
        }
    }
}