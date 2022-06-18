﻿using UnityEngine;
using Fungus.LionManeSaveSys;

namespace SaveSystemTests
{
    public class FlowchartTransformVarSavingTests : FlowchartVarSavingTests<TransformVarSaver>
    {
        protected override string VariableHolderName => "TransformFlowchart";
        protected override VarSaver.ContentType SaveContentAs
        {
            get { return VarSaver.ContentType.jsonString; }
        }

        protected override void PrepareExpectedResults()
        {
            // Need to save the results as a special type, since simply json-stringifying a Transform won't quite work
            ExpectedResults.Clear();

            for (int i = 0; i < variablesToEncode.Count; i++)
            {
                var toEncode = variablesToEncode[i];
                Transform transformVal = (Transform) toEncode.GetValue();
                TransformSaveUnit state = new TransformSaveUnit(transformVal);
                var weWantPrettyPrint = true;
                var result = JsonUtility.ToJson(state, weWantPrettyPrint);
                
                ExpectedResults.Add(result);
            }
        }

    }
}

