﻿using Object = System.Object;

namespace Fungus.LionManeSaveSys
{
    public abstract class SaveUnit: ISaveUnit
    {
        protected Object contents;
        public Object Contents
        {
            get { return contents; }
            set { contents = value; }
        }

        public abstract string TypeName { get; }
    }

    /// <summary>
    /// Has a default implementation for units of save data made to work with 
    /// Fungus's save system.
    /// </summary>
    /// <typeparam name="TContents"></typeparam>
    public abstract class SaveUnit<TContents> : SaveUnit, ISaveUnit<TContents>
    {
        protected new TContents contents;
        public new virtual TContents Contents
        {
            get { return contents; }
            set
            {
                this.contents = value;
                base.Contents = value;
                // ^So that when being treated as a non-specific SaveUnit, the right stuff is passed
                // when calling its Contents property
            }
        }

    }
}