namespace MMORL.Client.Events
{
    public abstract class GameEvent
    {
        public int? Id { get; set; }

        private bool _isFirstUpdate = true;

        public bool Update(float delta)
        {
            if (_isFirstUpdate)
            {
                OnStart();
                _isFirstUpdate = false;
            }

            bool result = Process(delta);

            if (result)
            {
                OnEnd();
            }

            return result;
        }

        protected abstract bool Process(float delta);

        public virtual bool CanStart()
        {
            return true;
        }

        protected virtual void OnStart()
        {

        }

        protected virtual void OnEnd()
        {

        }
    }
}
