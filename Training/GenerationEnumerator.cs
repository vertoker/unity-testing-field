using System.Collections;
using System.Collections.Generic;

namespace NN.Training
{
    public class GenerationEnumerator : IEnumerator<int>
    {
        private int generationCapacity;
        private int currentGeneration;
        private int counterInGeneration;

        public int GenerationCapacity => generationCapacity;
        public int CurrentGeneration => currentGeneration;
        public int CounterInGeneration => counterInGeneration;
        
        public int Current => counterInGeneration;
        object IEnumerator.Current => Current;

        public GenerationEnumerator(int generationCapacity)
        {
            this.generationCapacity = generationCapacity;
            currentGeneration = 1;
            Reset();
        }

        public bool MoveNext()
        {
            counterInGeneration++;
            if (generationCapacity > counterInGeneration)
                return true;
            
            currentGeneration++;
            return false;
        }
        public void Reset()
        {
            counterInGeneration = -1;
        }
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}