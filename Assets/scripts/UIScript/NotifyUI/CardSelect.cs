using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyLib
{
    public class CardSelect : IUserInterface
    {
        UIGrid grid;
        List<GameObject> cards  = new List<GameObject>();
        GameObject cell;
        int selCard = 0;
        bool sel = false;
        void Awake()
        {
            grid = GetGrid("Grid");
            cell = GetName("Cell");
            var cu = cell.GetComponent<IUserInterface>();
            cu.SetCallback("Info", OnSelect);
        }

        void OnSelect() {
            selCard = 0;
            sel = !sel;
        }
    }
}