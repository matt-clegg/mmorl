namespace MMORL.Client.Interface
{
    public abstract class UiElement
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool ShouldRemove { get; protected set; }

        public abstract void Update(float delta);
        public abstract void Render();
    }
}
