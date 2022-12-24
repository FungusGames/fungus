﻿using UnityEngine;
using System.Collections.Generic;
using Object = System.Object;
using Type = System.Type;

namespace Fungus.LionManeSaveSys
{
    /// <summary>
    /// For classes that handle making some particular type of save unit to be stored in RAM as opposed
    /// to saves stored on disk.
    /// </summary>
    public abstract class DataSaver: MonoBehaviour, ISaveCreator
    {
        /// <summary>
        /// The types of objects this can accept as input
        /// </summary>
        public virtual IList<Type> ValidInputTypes
        {
            get { return validInputTypes; }
        }

        protected Type[] validInputTypes = new Type[]
        {
            typeof(Object)
        };

        public virtual IList<ISaveUnit> CreateSavesFrom(IList<Object> inputs)
        {
            IList<ISaveUnit> result = new ISaveUnit[inputs.Count];

            for (int i = 0; i < inputs.Count; i++)
            {
                Object currentInput = inputs[i];
                ISaveUnit saveCreated = CreateSaveFrom(currentInput);
                result[i] = saveCreated;
            }

            return result;
        }

        public abstract ISaveUnit CreateSaveFrom(Object input);

        protected virtual void Validate(Object input)
        {
            if (!IsValid(input))
            {
                AlertFor(input);
            }
        }

        protected virtual bool IsValid(Object input)
        {
            Type inputType = input.GetType();

            foreach (Type validType in ValidInputTypes)
            {
                bool sameOrSubtype = inputType == validType || inputType.IsSubclassOf(validType);

                if (sameOrSubtype)
                    return true;
            }

            return false;
        }

        protected virtual void AlertFor(Object invalidInput)
        {
            string messageFormat = "{0} is invalid input for {1} on GameObject {2}.";
            string errorMessage = string.Format(messageFormat, invalidInput, GetType().Name, this.gameObject.name);
            throw new System.InvalidOperationException(errorMessage);
        }
    }

    public abstract class DataSaver<TSaveUnit, TInput> : DataSaver, ISaveCreator<TSaveUnit, TInput>
        where TSaveUnit: ISaveUnit
    {

        public virtual TSaveUnit CreateSaveFrom(TInput input)
        {
            return (TSaveUnit)CreateSaveFrom(input as Object);
        }

        public virtual IList<TSaveUnit> CreateSavesFrom(IList<TInput> inputs)
        {
            IList<TSaveUnit> saves = new TSaveUnit[inputs.Count];

            for (int i = 0; i < inputs.Count; i++)
            {
                var currentInput = inputs[i];
                TSaveUnit newSave = (TSaveUnit) CreateSaveFrom(currentInput);
                saves[i] = newSave;
            }

            return saves;
        }
    }
}