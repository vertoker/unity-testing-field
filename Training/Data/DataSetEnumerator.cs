using System.Collections;
using System.Collections.Generic;

namespace NeuralNetworkPipeline.Training
{
    public class DataSetEnumerator : IEnumerator<IDataSet>
    {
        private List<IDataSet> datasets;
        private bool isEmpty;
        private int length;
        
        private int counterInlist;
        private int counterAll;
        
        public int CounterInlist => counterInlist;
        public int CounterAll => counterAll;
        
        public IDataSet Current => datasets[counterInlist];
        object IEnumerator.Current => Current;
        
        public DataSetEnumerator()
        {
            datasets = new List<IDataSet>();
            
            length = 0;
            counterInlist = 0;
            counterAll = 0;
            isEmpty = true;
        }
        public DataSetEnumerator(IEnumerable<IDataSet> datasets)
        {
            this.datasets = new List<IDataSet>(datasets);
            
            length = this.datasets.Count;
            counterInlist = 0;
            counterAll = 0;
            isEmpty = false;
        }

        public void Add(IDataSet dataset)
        {
            length++;
            datasets.Add(dataset);
            isEmpty = true;
        }
        public void AddRange(IEnumerable<IDataSet> datasets)
        {
            this.datasets.AddRange(datasets);
            length = this.datasets.Count;
            isEmpty = true;
        }
        public void Remove(IDataSet dataset)
        {
            datasets.Remove(dataset);
            length--;

            if (length == counterInlist)
                counterInlist = 0;
            isEmpty = length == 0;
        }
        public void Clear()
        {
            datasets.Clear();
            
            isEmpty = false;
            counterInlist = 0;
            counterAll = 0;
            length = 0;
        }

        public bool MoveNext()
        {
            if (isEmpty)
                return false;
            
            counterAll++;
            counterInlist++;
            return length > counterInlist;
        }
        public void Reset()
        {
            counterInlist = -1;
        }
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}