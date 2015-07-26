using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ChuMeng
{
    public class PackageUI : IUserInterface
    {
        UIGrid  leftGrid;
        GameObject leftCell;

        List<GameObject> leftCells = new List<GameObject>();

        void Awake()
        {
            SetCallback("closeButton", Hide);
        }
        // Use this for initialization
        void Start()
        {
    
        }
    
        // Update is called once per frame
        void Update()
        {
    
        }
    }

}