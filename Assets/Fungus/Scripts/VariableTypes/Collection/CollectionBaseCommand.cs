﻿using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Base class for all FungusCollection commands
    /// </summary>
    [AddComponentMenu("")]
    public abstract class CollectionBaseCommand : Command
    {
        [SerializeField]
        protected CollectionData collection;

        [SerializeField]
        [VariableProperty()]
        protected Variable variableToUse;

        public Collection Collection
        {
            get
            {
                return collection.Value;
            }
        }

        public override void OnEnter()
        {
            if (collection.Value != null && variableToUse != null)
            {
                OnEnterInner();
            }

            Continue();
        }

        protected abstract void OnEnterInner();

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }

        public override bool HasReference(Variable variable)
        {
            return variable == collection || variable == variableToUse;
        }

        public override string GetSummary()
        {
            if (collection.Value == null)
                return "Error: no collection selected";

            if (variableToUse == null)
                return "Error: no variable selected";

            return variableToUse.Key + " to " + collection.Value.name;
        }
    }
}