namespace Falcon.MtG
{
    using System.Collections.Generic;

    public class AddRemoveTracker
    {
        public AddRemoveTracker()
        {
            ObjectsToRemove = new List<object>();
            ObjectsToAdd = new List<object>();
        }

        public List<object> ObjectsToRemove;
        public List<object> ObjectsToAdd;

        public void Merge(AddRemoveTracker other)
        {
            this.ObjectsToAdd.AddRange(other.ObjectsToAdd);
            this.ObjectsToRemove.AddRange(other.ObjectsToRemove);

            other.ObjectsToAdd.Clear();
            other.ObjectsToRemove.Clear();
        }
    }

    public class UpsertResult<T> : AddRemoveTracker
    {
        public UpsertResult() : base()
        { }

        public T MainObject;
    }
}