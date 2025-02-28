using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TouchTriggerEntity : MonoBehaviour
{
    [Serializable]
    public struct iterationHolder {
        public MonoBehaviour owner;
        public string methodName;

        public void Call(){
            if(!owner || methodName == "") return;

            MethodInfo info = owner.GetType().GetMethod(methodName);
            if(info.Name == "") return;

            info.Invoke(owner, null);
        }
    }

    public iterationHolder[] objectsToCall;


    public void ProduceAction(){
        foreach(iterationHolder obj in objectsToCall) obj.Call();
    }
}
