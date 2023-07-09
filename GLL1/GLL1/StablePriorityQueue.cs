
namespace GLL1
{
    public class StablePriorityQueue<T>
    {
        private Queue<(T Item, int Priority)> queue;
        private Dictionary<int, Queue<T>> insertionOrder;

        public StablePriorityQueue()
        {
            queue = new Queue<(T, int)>();
            insertionOrder = new Dictionary<int, Queue<T>>();
        }

        public void Enqueue(T item, int priority)
        {
            queue.Enqueue((item, priority));

            if (!insertionOrder.ContainsKey(priority))
            {
                insertionOrder[priority] = new Queue<T>();
            }

            insertionOrder[priority].Enqueue(item);
        }

        public T Dequeue()
        {
            if (queue.Count == 0)
            {
                throw new InvalidOperationException("Priority queue is empty");
            }

            var (item, priority) = queue.Dequeue();

            var itemsWithSamePriority = insertionOrder[priority];
            itemsWithSamePriority.Dequeue();

            if (itemsWithSamePriority.Count == 0)
            {
                insertionOrder.Remove(priority);
            }
            return item;
        }
        public int Count => queue.Count;
    }
}
