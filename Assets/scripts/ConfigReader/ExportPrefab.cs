using UnityEngine;
using System.Collections;
using MyLib;
using SimpleJSON;
using System.Reflection;
using System.IO;

/// <summary>
/// 将Prefab转化为Json对象
/// 
/// {
///     Type: "GameObject";
///     Name: "xxx"
///     Pos: "<vec>x,y,z"
///     Component: [
///         Type: "Monobehavior XXXClass",
///         xxx: 123,
///         xxx: true,
///         xxx: "abc",
///         xxx: "<vec>x,y,z"
///         xxx: 10.2,
///     ]
///     Children: [
///         {
///             Type: "GameObject",
///             Component: [
///             ],
///             Childrent: [
///             ],
///         },
///     ]
/// } 
/// </summary>
public class ExportPrefab : MonoBehaviour
{
    [ButtonCallFunc()]
    public bool Export;

    public void ExportMethod()
    {
        var jc = ExportGameObject(this.gameObject);
        var dirPath = Path.Combine(Application.dataPath, "../ExportJson");
        if(!File.Exists(dirPath)) {
            Directory.CreateDirectory(dirPath);
        }
        var fileName = Path.Combine(dirPath, this.gameObject.name+".json");
        File.WriteAllText(fileName, jc.ToString());
        Debug.LogError("ExportFile: "+fileName);
    }
    private SimpleJSON.JSONClass ExportGameObject(GameObject go) {
        var jobj = new JSONClass();
        jobj.Add("Type", "GameObject");
        jobj.Add("Name", go.name);
        jobj.Add("Pos", SerVec(go.transform.position));
        var arr = new JSONArray();
        jobj.Add("Component", arr);
        foreach(var c in go.GetComponents<MonoBehaviour>()) {
            arr.Add(ExportComponent(c));
        }

        var arr1 = new JSONArray();
        jobj.Add("Children", arr1);
        foreach(Transform t in go.transform) {
            arr1.Add(ExportGameObject(t.gameObject));
        }
        return jobj;
    }

    private JSONClass  ExportComponent(MonoBehaviour com) {
        var jobj = new JSONClass();
        jobj.Add("Type", com.GetType().Name);
        var t = com.GetType();
        var fields = t.GetFields();
        foreach(var f in fields) {
            jobj.Add(f.Name, ExportAttr(f, com));
        }
        return jobj;
    }


    private string SerVec(Vector3 pos) {
        return string.Format("<vec>{0:F},{1:F},{2:F}", pos.x, pos.y, pos.z);
    }
    private JSONData ExportAttr(FieldInfo finfo, object obj) {
        var ftyp = finfo.FieldType;
        if(finfo.FieldType == typeof(bool)) {
            return new JSONData(System.Convert.ToBoolean(finfo.GetValue(obj)));
        }else if(ftyp == typeof(int)){
            return new JSONData(System.Convert.ToInt32(finfo.GetValue(obj)));
        }else if(ftyp == typeof(float)) {
            return new JSONData(System.Convert.ToSingle(finfo.GetValue(obj)));
        }else if(ftyp == typeof(string)) {
            return new JSONData(System.Convert.ToString(finfo.GetValue(obj)));
        }
        else {
            return new JSONData("Unknow: "+finfo.GetValue(obj)+" type: "+finfo.FieldType);
        }
    }
}
